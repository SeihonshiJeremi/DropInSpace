// Runs the core (no Unity needed) against a real OpenTrophy server. See run.mjs.
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OpenTrophy.Core;

class HttpTransport : ITransport
{
    public bool Offline;
    public int Calls;
    static readonly HttpClient http = new HttpClient();
    public async Task<HttpResult> Send(string method, string url, Dictionary<string, string> headers, string jsonBody)
    {
        Calls++;
        if (Offline) return new HttpResult { Status = 0 };
        var req = new HttpRequestMessage(new HttpMethod(method), url);
        foreach (var h in headers) req.Headers.TryAddWithoutValidation(h.Key, h.Value);
        if (jsonBody != null) req.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        try
        {
            var res = await http.SendAsync(req);
            return new HttpResult { Status = (int)res.StatusCode, Body = await res.Content.ReadAsStringAsync() };
        }
        catch (Exception) { return new HttpResult { Status = 0 }; }
    }
}

static class CoreTest
{
    static int passed, failed;
    static void Ok(bool cond, string label) { if (cond) passed++; else { failed++; Console.WriteLine("  ✗ " + label); } }
    static string Env(string n) { return Environment.GetEnvironmentVariable(n); }
    static readonly HttpTransport net = new HttpTransport();
    static string server, gameKey, playerSession, gameId, file;
    static DateTime? frozen;
    static DateTime Now() { return frozen ?? DateTime.UtcNow; }

    static TrophyClient Make(string deviceId = "device-1", string embedded = null, string path = null, string secret = "local-secret")
    {
        var c = new TrophyClient(new TrophyClientOptions { ServerUrl = server, GameKey = gameKey, LocalSecret = secret, DeviceId = deviceId, DeviceName = "PC de test", StoragePath = path ?? file, EmbeddedDefinitions = embedded }, net, Now);
        c.Log += (m) => { if (Env("VERBOSE") != null) Console.WriteLine("    [log] " + m); };
        return c;
    }
    static async Task<object> Site(string method, string path, string body = null)
    {
        var r = await new HttpTransport().Send(method, server + "/api/v1" + path, new Dictionary<string, string> { { "Authorization", "Bearer " + playerSession } }, body);
        return MiniJson.TryParse(r.Body);
    }
    static async Task<Dictionary<string, object>> ServerTrophies()
    {
        var map = new Dictionary<string, object>();
        object g = await Site("GET", "/games/" + gameId);
        foreach (object l in MiniJson.Arr(MiniJson.Get(g, "groups"))) foreach (object t in MiniJson.Arr(MiniJson.Get(l, "trophies"))) if (MiniJson.Str(t, "key") != null) map[MiniJson.Str(t, "key")] = t;
        return map;
    }
    static async Task Until(TrophyClient c, Func<bool> done, int seconds = 20)
    {
        for (int i = 0; i < seconds * 10 && !done(); i++) { await c.Tick(); await Task.Delay(100); }
    }

    static int Main() { try { Run().GetAwaiter().GetResult(); } catch (Exception e) { failed++; Console.WriteLine("  ✗ crashed: " + e); } Console.WriteLine(passed + " ok, " + failed + " failed (Unity core)"); return failed > 0 ? 1 : 0; }

    static async Task Run()
    {
        server = Env("OT_SERVER"); gameKey = Env("OT_GAME_KEY"); playerSession = Env("OT_PLAYER"); gameId = Env("OT_GAME_ID");
        string dir = Path.Combine(Path.GetTempPath(), "ot-core-test-" + Guid.NewGuid().ToString("N"));
        file = Path.Combine(dir, "trophies.dat");

        // MiniJson
        object j = MiniJson.Parse("{\"a\":[1,2.5,\"x\\n\\u00e9\",true,null],\"b\":{\"c\":-3}}");
        Ok(MiniJson.Arr(MiniJson.Get(j, "a")).Count == 5 && MiniJson.Num(MiniJson.Get(j, "b"), "c") == -3 && (string)MiniJson.Arr(MiniJson.Get(j, "a"))[2] == "x\né", "json read");
        Ok(MiniJson.Write(MiniJson.Parse(MiniJson.Write(j))) == MiniJson.Write(j) && MiniJson.TryParse("{oops") == null, "json write, and bad json is not fatal");

        // 1. No account, no network, no list of trophies: a trophy is still earned and kept
        net.Offline = true;
        var c = Make();
        var seen = new List<string>();
        c.TrophyUnlocked += (d) => seen.Add(d.Key);
        Ok(c.Game == null && !c.IsLinked, "starts empty");
        Ok(c.Unlock("first_step") && !c.Unlock("first_step"), "unlock offline, once");
        Ok(seen.Count == 1 && seen[0] == "first_step" && c.PendingCount == 1 && File.Exists(file), "announced, queued and written at once");
        string raw = Encoding.UTF8.GetString(File.ReadAllBytes(file));
        Ok(!raw.Contains("first_step") && !raw.Contains("unlocked"), "the file is not readable as text");
        DateTime earnedAt = DateTime.UtcNow;

        // 2. The game restarts: everything is still there
        c = Make();
        Ok(c.IsUnlocked("first_step") && c.PendingCount == 1, "survives a restart");

        // 3. Tampering and copying
        byte[] bytes = File.ReadAllBytes(file);
        byte[] good = (byte[])bytes.Clone();
        bytes[bytes.Length / 2] ^= 0x01;
        File.WriteAllBytes(file, bytes);
        if (File.Exists(file + ".bak")) File.Delete(file + ".bak");
        Ok(!Make().IsUnlocked("first_step") && File.Exists(file + ".rejected"), "an edited file is rejected and set aside");
        File.WriteAllBytes(file, good);
        Ok(!Make("another-device").IsUnlocked("first_step"), "a file copied to another machine does not open");
        File.WriteAllBytes(file, good);
        Ok(!Make("device-1", null, null, "another-game-secret").IsUnlocked("first_step"), "nor with another game's secret");
        File.WriteAllBytes(file, good);
        c = Make();
        Ok(c.IsUnlocked("first_step"), "the untouched file still opens");
        // a write cut short leaves the previous version usable
        c.Unlock("typo_key");
        File.WriteAllBytes(file, new byte[] { 1, 2, 3 });
        c = Make();
        Ok(c.IsUnlocked("first_step") && !c.IsUnlocked("typo_key"), "a damaged file falls back on its backup");
        c.Unlock("typo_key");

        // 4. Back online: the list of trophies arrives; nothing is sent without an account
        net.Offline = false;
        seen.Clear();
        c.TrophyUnlocked += (d) => seen.Add(d.Key);
        await c.Start();
        Ok(c.Game != null && c.Game.Title == "Jeu Unity" && c.Game.Find("secret").Hidden && c.Game.Find("hunter").Target == 100 && c.Game.Platinum != null, "trophy list read from the server");
        Ok(c.Game.Groups.Count == 2 && c.Game.Groups[0].IsBase && !c.Game.Find("deep").Group.IsBase, "main list and add-on");
        // Languages: every one is on the device, the game switches without the server
        var fs1 = c.Game.Find("first_step");
        Ok(c.Language == null && fs1.Name == "Premiers pas" && c.Game.BaseLanguage == "fr" && string.Join(",", c.Game.Languages.ToArray()) == "fr,en,pt-BR", "the game's own language by default, and the list of languages");
        int changes = 0; Action onLang = () => changes++; c.Changed += onLang;
        c.Language = "en-GB";
        Ok(c.Game.Find("first_step").Name == "First steps" && c.Game.Find("first_step").Description == "Finir le tutoriel" && c.Game.Find("boss").Name == "Le gardien" && c.Game.Find("deep").Group.Name == "Add-on" && changes == 1, "a translation where there is one, the game's text elsewhere");
        c.Language = "pt";
        Ok(c.Game.Find("first_step").Name == "Primeiros passos" && c.Game.Find("first_step").Description == "Terminar o tutorial" && c.Game.Find("deep").Group.Name == "Extension", "a regional translation serves its language");
        c.Language = "de"; Ok(c.Game.Find("first_step").Name == "Premiers pas", "an unknown language shows the game's text");
        c.Language = "fr-CA"; Ok(c.Game.Find("first_step").Name == "Premiers pas", "so does the game's own language");
        c.Language = "en"; await c.RefreshDefinitions();
        Ok(c.Game.Find("first_step").Name == "First steps" && c.Game.Find("first_step").BaseName == "Premiers pas", "the language survives a new reading of the list");
        c.Language = null; c.Changed -= onLang;
        Ok(c.Game.Find("first_step").Name == "Premiers pas", "and back");
        Ok((await ServerTrophies()).Count == 0 || !MiniJson.Bool((await ServerTrophies())["first_step"], "unlocked"), "without a link the server knows nothing yet");
        await Task.Delay(2500); // so that the offline date and the sync date clearly differ

        // 5. Linking with a code
        string code = null, linkedAs = null;
        c.LinkCodeReady += (cd, url) => code = cd;
        c.Linked += (name) => linkedAs = name;
        await c.BeginLink();
        Ok(code != null && code.Length == 9 && c.LinkState == LinkState.WaitingForPlayer && c.LinkUrl.Contains(code), "link code received");
        var restarted = Make(); // the game is closed while the player is on the site
        Ok(restarted.LinkState == LinkState.WaitingForPlayer && restarted.LinkCode == code, "a pending link survives a restart");
        await Site("POST", "/link/" + code + "/approve");
        await Until(c, () => c.IsLinked);
        Ok(c.IsLinked && linkedAs == "joueur" && c.PlayerName == "joueur" && c.LinkState == LinkState.Linked, "linked once the player accepts");
        await Until(c, () => c.PendingCount == 0);
        var st = await ServerTrophies();
        DateTime serverAt = DateTime.Parse(MiniJson.Str(st["first_step"], "unlockedAt")).ToUniversalTime();
        Ok(MiniJson.Bool(st["first_step"], "unlocked") && Math.Abs((serverAt - earnedAt).TotalSeconds) < 1.5, "what was earned before the account exists arrives with its real date");
        Ok(!c.IsUnlocked("typo_key") && c.PendingCount == 0, "a key the server does not know is dropped");

        // 6. Online play, counters, platinum
        seen.Clear();
        Ok(c.Unlock("boss") && c.Unlock("secret"), "unlock online");
        Ok(!c.Unlock("platinum"), "the game cannot unlock the platinum itself");
        await Until(c, () => c.PendingCount == 0);
        Ok(MiniJson.Bool((await ServerTrophies())["boss"], "unlocked"), "sent at once");
        Ok(c.SetProgress("hunter", 37) == 37 && c.SetProgress("hunter", 12) == 37 && c.AddProgress("hunter", 3) == 40, "a counter only goes up");
        Ok(c.SetProgress("boss", 5) == 0 || c.GetProgress("boss") == 0, "no counter on a plain trophy");
        frozen = DateTime.UtcNow.AddSeconds(11); await c.Tick(); frozen = null; // the delayed send
        await Until(c, () => c.PendingCount == 0);
        Ok(MiniJson.Num((await ServerTrophies())["hunter"], "progress") == 40, "progress reaches the server, a little later");
        Ok(c.AddProgress("hunter", 500) == 100 && c.IsUnlocked("hunter"), "reaching the target unlocks");
        Ok(c.IsUnlocked("platinum") && seen.Contains("platinum"), "the platinum falls at once, even before the server answers");
        await Until(c, () => c.PendingCount == 0);
        st = await ServerTrophies();
        Ok(MiniJson.Bool(st["platinum"], "unlocked") && MiniJson.Bool(st["hunter"], "unlocked"), "and the server agrees");
        Ok(seen.FindAll((k) => k == "platinum").Count == 1, "announced once, not twice");
        var all = c.GetAll();
        Ok(all.Count == 6 && all.Find((t) => t.Def.Key == "deep").Unlocked == false && all.Find((t) => t.Def.Key == "hunter").Progress == 100, "the full list with the player's state");

        // 7. Offline again, then back
        net.Offline = true;
        Ok(c.Unlock("deep") && c.PendingCount == 1, "offline unlock is queued");
        await c.Tick(); await c.Tick();
        Ok(c.PendingCount == 1 && c.IsLinked, "offline does not unlink");
        net.Offline = false;
        await c.SyncNow();
        Ok(c.PendingCount == 0 && MiniJson.Bool((await ServerTrophies())["deep"], "unlocked"), "caught up when the network returns");

        // 8. A second device of the same player gets everything
        string file2 = Path.Combine(dir, "second.dat");
        var d2 = Make("device-2", null, file2);
        string code2 = null; d2.LinkCodeReady += (cd, url) => code2 = cd;
        var toasts2 = new List<string>(); d2.TrophyUnlocked += (d) => toasts2.Add(d.Key);
        await d2.Start(); await d2.BeginLink();
        await Site("POST", "/link/" + code2 + "/approve");
        await Until(d2, () => d2.IsLinked);
        Ok(d2.IsUnlocked("boss") && d2.IsUnlocked("platinum") && d2.GetAll().FindAll((t) => t.Unlocked).Count == 6, "another device receives the account's trophies");
        Ok(toasts2.Count == 0, "without announcing them again");

        // 9. The player withdraws the link on the site
        bool unlinked = false; c.Unlinked += () => unlinked = true;
        foreach (object l in MiniJson.Arr(await Site("GET", "/me/links"))) if (MiniJson.Str(l, "device") == "PC de test") await Site("DELETE", "/me/links/" + MiniJson.Str(l, "id"));
        await c.SyncNow();
        Ok(unlinked && !c.IsLinked && c.IsUnlocked("boss"), "a withdrawn link is noticed; local trophies stay");
        Ok(!Make().IsLinked, "and it is remembered");

        // 10. Beta: no platinum before the release (list shipped with the build, no network)
        net.Offline = true;
        string beta = "{\"id\":\"b\",\"title\":\"Beta\",\"beta\":true,\"revision\":\"1\",\"groups\":[{\"name\":\"Base\",\"base\":true,\"trophies\":[{\"key\":\"platinum\",\"name\":\"P\",\"tier\":\"platinum\"},{\"key\":\"only\",\"name\":\"Seul\",\"tier\":\"bronze\"}]}]}";
        var b = Make("device-1", beta, Path.Combine(dir, "beta.dat"));
        Ok(b.Game != null && b.Game.Beta && b.Unlock("only") && !b.IsUnlocked("platinum"), "in beta, no platinum");
        var r = Make("device-1", beta.Replace("\"beta\":true", "\"beta\":false"), Path.Combine(dir, "released.dat"));
        Ok(r.Unlock("only") && r.IsUnlocked("platinum"), "released, the list shipped with the build is enough to give it offline");
        // Counters: reaching or passing the target unlocks, whenever the target becomes known
        string counted = "{\"id\":\"c\",\"title\":\"C\",\"beta\":false,\"revision\":\"1\",\"groups\":[{\"name\":\"Base\",\"base\":true,\"trophies\":[{\"key\":\"ten\",\"name\":\"Dix\",\"tier\":\"bronze\",\"target\":10},{\"key\":\"other\",\"name\":\"Autre\",\"tier\":\"bronze\"}]}]}";
        string cf = Path.Combine(dir, "counters.dat");
        Action reset = () => { foreach (string f in new[] { cf, cf + ".bak" }) if (File.Exists(f)) File.Delete(f); };
        var kc = Make("device-1", counted, cf);
        Ok(kc.SetProgress("ten", 25) == 10 && kc.IsUnlocked("ten"), "a value above the target unlocks");
        reset();
        kc = Make("device-1", counted, cf);
        Ok(kc.SetProgress("ten", 10) == 10 && kc.IsUnlocked("ten"), "a value equal to the target unlocks");
        reset();
        kc = Make("device-1", null, cf); // the game does not know its trophy list yet
        Ok(kc.SetProgress("ten", 15) == 15 && !kc.IsUnlocked("ten"), "without the list, the counter is kept as it is");
        kc.SaveNow();
        kc = Make("device-1", counted, cf); // next launch, the list is there (target 10, already passed)
        var late = new List<string>(); kc.TrophyUnlocked += (d) => late.Add(d.Key);
        Ok(!kc.IsUnlocked("ten"), "nothing is announced before the game listens");
        await kc.Start();
        Ok(kc.IsUnlocked("ten") && late.Count == 1 && kc.GetProgress("ten") == 10, "a counter that already passed its target unlocks once the list is known");
        reset();
        kc = Make("device-1", null, cf);
        kc.SetProgress("ten", 15); kc.SaveNow();
        kc = Make("device-1", counted, cf);
        Ok(kc.SetProgress("ten", 15) == 10 && kc.IsUnlocked("ten"), "and so does setting it again, even to the same value");
        r.WipeLocal();
        Ok(!r.IsUnlocked("only") && !File.Exists(Path.Combine(dir, "released.dat")), "wipe erases the local file");
        net.Offline = false;
        try { Directory.Delete(dir, true); } catch (Exception) { }
    }
}
