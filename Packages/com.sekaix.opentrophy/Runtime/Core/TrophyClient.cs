using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace OpenTrophy.Core
{
    public sealed class TrophyClientOptions
    {
        /// <summary>https://opentrophy.example, without a trailing slash.</summary>
        public string ServerUrl;
        /// <summary>The game key (otg_…) created in the developer area. It cannot unlock anything by itself.</summary>
        public string GameKey;
        /// <summary>A random text unique to this game, shipped in the build: part of the key of the local file.</summary>
        public string LocalSecret;
        /// <summary>Identifies this machine: the local file only opens on the device that wrote it.</summary>
        public string DeviceId;
        /// <summary>Shown to the player on the site when linking ("PC du salon").</summary>
        public string DeviceName;
        /// <summary>Full path of the local trophy file.</summary>
        public string StoragePath;
        /// <summary>The trophy list shipped with the build (what GET /play/game answered), used until the server answers.</summary>
        public string EmbeddedDefinitions;
        /// <summary>The language trophies are shown in ("en", "pt-BR"…). Null: the language they were written in.</summary>
        public string Language;
    }

    /// <summary>
    /// Trophies for one game and one player, local first: a trophy is earned and saved on the spot, with
    /// or without network, with or without an account. Whatever was earned offline, or before the player
    /// linked an account, is sent as soon as possible with its real date.
    /// Everything here runs on the caller's thread; Unity calls it from the main thread.
    /// </summary>
    public sealed class TrophyClient
    {
        const int MaxPerSync = 500;
        readonly TrophyClientOptions options;
        readonly ITransport transport;
        readonly SecureStore store;
        readonly Func<DateTime> utcNow;

        // What is saved in the local file
        string token, username, definitionsJson;
        readonly Dictionary<string, DateTime> unlocked = new Dictionary<string, DateTime>();
        readonly Dictionary<string, int> progress = new Dictionary<string, int>();
        readonly HashSet<string> pendingUnlocks = new HashSet<string>();
        readonly HashSet<string> pendingProgress = new HashSet<string>();
        string linkSecret, linkCode, linkUrl;
        DateTime linkExpires;

        GameDef game;
        string language;
        bool dirty, flushing, polling, pullNeeded;
        DateTime saveAt = DateTime.MaxValue, flushAt = DateTime.MaxValue, pollAt = DateTime.MaxValue;
        double retrySeconds = 5, pollSeconds = 3;

        /// <summary>A trophy has just been earned (this is when to show it on screen).</summary>
        public event Action<TrophyDef> TrophyUnlocked;
        /// <summary>A counter moved: trophy, value, target.</summary>
        public event Action<TrophyDef, int, int> ProgressChanged;
        /// <summary>A link was asked: show this code (and this address) to the player.</summary>
        public event Action<string, string> LinkCodeReady;
        /// <summary>The player accepted the link on the site: their name.</summary>
        public event Action<string> Linked;
        /// <summary>The link did not happen: "offline", "expired", "refused" (bad game key) or "error".</summary>
        public event Action<string> LinkFailed;
        /// <summary>The account is no longer linked (asked by the game, or withdrawn by the player on the site).</summary>
        public event Action Unlinked;
        /// <summary>Anything changed: lists, progress, link. A trophy screen redraws on this.</summary>
        public event Action Changed;
        public event Action<string> Log;

        public TrophyClient(TrophyClientOptions options, ITransport transport, Func<DateTime> utcNow = null)
        {
            if (options == null) throw new ArgumentNullException("options");
            this.options = options;
            this.transport = transport;
            this.utcNow = utcNow ?? (() => DateTime.UtcNow);
            store = new SecureStore(options.StoragePath, (options.LocalSecret ?? "") + "|" + (options.GameKey ?? ""), options.DeviceId);
            LoadFile();
            if (game == null && !string.IsNullOrEmpty(options.EmbeddedDefinitions)) game = GameDef.FromJson(options.EmbeddedDefinitions);
            language = options.Language;
            if (game != null) game.SetLanguage(language);
        }

        /// <summary>At launch: refreshes the trophy list, then exchanges with the server what each side is missing.</summary>
        public async Task Start()
        {
            UnlockReachedCounters(); // now that the game listens to the events
            await RefreshDefinitions();
            if (token != null) { pullNeeded = true; await Flush(); }
            else if (linkSecret != null) pollAt = utcNow(); // a link was being made when the game closed
        }

        // ---- Reading ----
        public GameDef Game { get { return game; } }
        /// <summary>
        /// The language trophies are shown in. Change it when the player changes the game's language:
        /// every language is already on the device, nothing is asked of the server. Raises Changed.
        /// </summary>
        public string Language
        {
            get { return language; }
            set { language = value; if (game != null) game.SetLanguage(value); Raise(Changed); }
        }
        public bool IsLinked { get { return token != null; } }
        public string PlayerName { get { return username; } }
        public LinkState LinkState { get { return token != null ? LinkState.Linked : linkSecret != null ? LinkState.WaitingForPlayer : LinkState.NotLinked; } }
        public string LinkCode { get { return linkCode; } }
        public string LinkUrl { get { return linkUrl; } }
        /// <summary>Trophies earned here that the server does not have yet.</summary>
        public int PendingCount { get { return pendingUnlocks.Count + pendingProgress.Count; } }
        public bool IsUnlocked(string key) { return key != null && unlocked.ContainsKey(key); }
        public int GetProgress(string key) { int v; return key != null && progress.TryGetValue(key, out v) ? v : 0; }

        /// <summary>Every trophy with the player's state, list by list, in the developer's order.</summary>
        public List<TrophyStatus> GetAll()
        {
            var list = new List<TrophyStatus>();
            if (game == null) return list;
            foreach (var def in game.All)
            {
                DateTime at;
                bool has = unlocked.TryGetValue(def.Key, out at);
                list.Add(new TrophyStatus { Def = def, Unlocked = has, UnlockedAt = has ? (DateTime?)at : null, Progress = has && def.Target > 0 ? def.Target : GetProgress(def.Key) });
            }
            return list;
        }

        // ---- Earning ----

        /// <summary>
        /// Unlocks a trophy now. Works offline and without an account; doing it again changes nothing.
        /// Returns true when the trophy has just been earned.
        /// </summary>
        public bool Unlock(string key)
        {
            if (string.IsNullOrEmpty(key) || unlocked.ContainsKey(key)) return false;
            TrophyDef def = game != null ? game.Find(key) : null;
            if (def != null && def.Tier == TrophyTier.Platinum) { Say("The platinum is never unlocked by the game: it falls by itself when the main list is complete."); return false; }
            if (def == null) Say("Trophy '" + key + "' is not in the known list (old list, or a typo?). It is kept and the server will decide.");
            unlocked[key] = utcNow();
            pendingUnlocks.Add(key);
            if (def != null && def.Target > 0) progress[key] = def.Target;
            Raise(TrophyUnlocked, def ?? new TrophyDef { Key = key, Name = key, Description = "" });
            GrantLocalPlatinum();
            SaveNow(); // an earned trophy is written at once
            flushAt = utcNow();
            Raise(Changed);
            return true;
        }

        /// <summary>Sets a counter (it never goes down). Reaching the target unlocks the trophy.</summary>
        public int SetProgress(string key, int value)
        {
            if (string.IsNullOrEmpty(key) || unlocked.ContainsKey(key)) return GetProgress(key);
            TrophyDef def = game != null ? game.Find(key) : null;
            int target = def != null ? def.Target : 0;
            if (def != null && target <= 0) { Say("Trophy '" + key + "' has no counter."); return 0; }
            int current = GetProgress(key);
            // Reaching the target unlocks, whatever was stored before: the counter may have been filled while the
            // trophy list was not known yet, or the developer may have lowered the target since.
            if (target > 0 && Math.Max(value, current) >= target) { progress[key] = target; Unlock(key); return target; }
            if (value <= current) return current;
            progress[key] = value;
            pendingProgress.Add(key);
            dirty = true;
            DateTime now = utcNow();
            if (saveAt == DateTime.MaxValue) saveAt = now.AddSeconds(3);          // counters move often: the file
            if (flushAt == DateTime.MaxValue) flushAt = now.AddSeconds(10);        // and the network are spared
            if (def != null && ProgressChanged != null) try { ProgressChanged(def, value, target); } catch (Exception e) { Say("ProgressChanged handler: " + e.Message); }
            Raise(Changed);
            return value;
        }

        public int AddProgress(string key, int amount = 1) { return amount <= 0 ? GetProgress(key) : SetProgress(key, GetProgress(key) + amount); }

        /// <summary>
        /// Counters already at (or past) their target unlock their trophy. Needed when the list of trophies
        /// arrives or changes after the counting: the target was unknown, or has been lowered.
        /// </summary>
        void UnlockReachedCounters()
        {
            if (game == null) return;
            var reached = new List<string>();
            foreach (var def in game.All)
                if (def.Target > 0 && def.Tier != TrophyTier.Platinum && !unlocked.ContainsKey(def.Key) && GetProgress(def.Key) >= def.Target) reached.Add(def.Key);
            foreach (string key in reached) Unlock(key);
        }

        // The platinum is given by the server; offline, the game gives it the same way so the player sees it at once.
        void GrantLocalPlatinum()
        {
            if (game == null || game.Beta) return;
            TrophyDef plat = game.Platinum;
            if (plat == null || unlocked.ContainsKey(plat.Key)) return;
            int count = 0;
            foreach (var g in game.Groups)
            {
                if (!g.IsBase) continue;
                foreach (var t in g.Trophies)
                {
                    if (t.Tier == TrophyTier.Platinum) continue;
                    if (!unlocked.ContainsKey(t.Key)) return;
                    count++;
                }
            }
            if (count == 0) return;
            unlocked[plat.Key] = utcNow();
            Raise(TrophyUnlocked, plat);
        }

        // ---- Life cycle ----

        /// <summary>Call regularly (a few times per second is plenty): retries, link polling, delayed saving.</summary>
        public async Task Tick()
        {
            DateTime now = utcNow();
            if (dirty && now >= saveAt) SaveNow();
            if (linkSecret != null && token == null && now >= pollAt) await PollLink();
            if (token != null && now >= flushAt && (PendingCount > 0 || pullNeeded)) await Flush();
        }

        /// <summary>Writes the local file now. Call it when the game is paused or closed.</summary>
        public void SaveNow()
        {
            dirty = false;
            saveAt = DateTime.MaxValue;
            if (!store.Save(MiniJson.Write(ToState()))) Say("The local trophy file could not be written: " + store.FilePath);
        }

        /// <summary>Tries to send right now what the server does not have (the game came back online, for instance).</summary>
        public Task SyncNow() { retrySeconds = 5; pullNeeded = true; return token != null ? Flush() : Task.FromResult(false); }

        public async Task<bool> RefreshDefinitions()
        {
            if (string.IsNullOrEmpty(options.GameKey)) { Say("No game key: the trophy list cannot be read from the server."); return false; }
            HttpResult r = await Request("GET", "/play/game", null, false);
            if (r.Status == 401) { Say("The game key was refused by the server (removed, or mistyped)."); return false; }
            if (!r.Ok) return false;
            GameDef fresh = GameDef.FromJson(r.Body);
            if (fresh == null) return false;
            bool changed = definitionsJson != r.Body;
            game = fresh;
            game.SetLanguage(language);
            definitionsJson = r.Body;
            UnlockReachedCounters();
            if (changed) { GrantLocalPlatinum(); SaveNow(); Raise(Changed); }
            return true;
        }

        // ---- Linking the player's account ----

        /// <summary>Asks the server for a code. LinkCodeReady gives it; the player validates it on the site.</summary>
        public async Task BeginLink()
        {
            if (token != null) return;
            HttpResult r = await Request("POST", "/play/link", MiniJson.Write(new Dictionary<string, object> { { "device", Clip(options.DeviceName, 60) } }), false);
            if (!r.Ok) { RaiseLinkFailed(r.Unreachable ? "offline" : r.Status == 401 ? "refused" : "error"); return; }
            object body = MiniJson.TryParse(r.Body);
            linkSecret = MiniJson.Str(body, "secret");
            linkCode = MiniJson.Str(body, "code");
            linkUrl = MiniJson.Str(body, "url");
            if (linkSecret == null || linkCode == null) { linkSecret = null; RaiseLinkFailed("error"); return; }
            linkExpires = utcNow().AddSeconds(MiniJson.Num(body, "expiresIn", 600));
            pollSeconds = Math.Max(2, MiniJson.Num(body, "interval", 3));
            pollAt = utcNow().AddSeconds(pollSeconds);
            SaveNow();
            if (LinkCodeReady != null) try { LinkCodeReady(linkCode, linkUrl); } catch (Exception e) { Say("LinkCodeReady handler: " + e.Message); }
            Raise(Changed);
        }

        /// <summary>Gives up a link that is waiting for the player.</summary>
        public void CancelLink()
        {
            if (linkSecret == null) return;
            ClearLinkRequest();
            SaveNow();
            Raise(Changed);
        }

        async Task PollLink()
        {
            if (polling) return;
            polling = true;
            try
            {
                if (utcNow() > linkExpires) { ClearLinkRequest(); SaveNow(); RaiseLinkFailed("expired"); Raise(Changed); return; }
                pollAt = utcNow().AddSeconds(pollSeconds);
                HttpResult r = await Request("POST", "/play/link/poll", MiniJson.Write(new Dictionary<string, object> { { "secret", linkSecret } }), false);
                if (!r.Ok || linkSecret == null) return; // offline: try again at the next tick
                object body = MiniJson.TryParse(r.Body);
                string status = MiniJson.Str(body, "status");
                if (status == "expired") { ClearLinkRequest(); SaveNow(); RaiseLinkFailed("expired"); Raise(Changed); return; }
                if (status != "linked") return;
                token = MiniJson.Str(body, "token");
                username = MiniJson.Str(MiniJson.Get(body, "player"), "username", "");
                ClearLinkRequest();
                // Everything earned on this machine so far goes to the account, with its real dates.
                foreach (var k in unlocked.Keys) pendingUnlocks.Add(k);
                foreach (var k in progress.Keys) if (!unlocked.ContainsKey(k)) pendingProgress.Add(k);
                Merge(body, null);
                SaveNow();
                if (Linked != null) try { Linked(username); } catch (Exception e) { Say("Linked handler: " + e.Message); }
                Raise(Changed);
                flushAt = utcNow();
                pullNeeded = true;
            }
            finally { polling = false; }
            if (token != null) await Flush();
        }

        /// <summary>Unlinks the account from this game on this device. Trophies stay on the account and in the local file.</summary>
        public async Task Unlink()
        {
            if (token == null) { CancelLink(); return; }
            string old = token;
            DropToken();
            await transport.Send("DELETE", Url("/play/link"), new Dictionary<string, string> { { "Authorization", "Bearer " + old } }, null);
        }

        void DropToken()
        {
            token = null;
            username = null;
            pullNeeded = false;
            SaveNow();
            Raise(Unlinked);
            Raise(Changed);
        }

        void ClearLinkRequest() { linkSecret = null; linkCode = null; linkUrl = null; pollAt = DateTime.MaxValue; }

        // ---- Exchanging with the server ----

        async Task<bool> Flush()
        {
            if (token == null || flushing) return false;
            flushing = true;
            try
            {
                while (true)
                {
                    var sentUnlocks = new List<string>();
                    var sentProgress = new Dictionary<string, int>();
                    var unlockItems = new List<object>();
                    var progressItems = new List<object>();
                    TrophyDef plat = game != null ? game.Platinum : null;
                    foreach (string key in pendingUnlocks)
                    {
                        if (sentUnlocks.Count >= MaxPerSync) break;
                        if (plat != null && key == plat.Key) continue; // the server gives the platinum itself
                        sentUnlocks.Add(key);
                        unlockItems.Add(new Dictionary<string, object> { { "key", key }, { "at", Iso(unlocked[key]) } });
                    }
                    foreach (string key in pendingProgress)
                    {
                        if (sentProgress.Count >= MaxPerSync) break;
                        sentProgress[key] = GetProgress(key);
                        progressItems.Add(new Dictionary<string, object> { { "key", key }, { "value", sentProgress[key] } });
                    }
                    string body = MiniJson.Write(new Dictionary<string, object> { { "unlocks", unlockItems }, { "progress", progressItems } });
                    HttpResult r = await Request("POST", "/play/sync", body, true);
                    if (token == null) return false; // unlinked while the request was away
                    if (r.Status == 401)
                    {
                        // The player withdrew the link on the site: back to local only, nothing is lost.
                        Say("The account link was removed on the site. Trophies are kept on this device.");
                        DropToken();
                        return false;
                    }
                    if (!r.Ok)
                    {
                        if (r.Status == 400)
                        {
                            // The server will never take this as it is: do not send it forever.
                            Say("The server refused the sync (" + r.Body + ").");
                            foreach (string k in sentUnlocks) pendingUnlocks.Remove(k);
                            foreach (string k in sentProgress.Keys) pendingProgress.Remove(k);
                            SaveNow();
                            return false;
                        }
                        flushAt = utcNow().AddSeconds(retrySeconds); // offline or busy server: later, less and less often
                        retrySeconds = Math.Min(300, retrySeconds * 2);
                        return false;
                    }
                    retrySeconds = 5;
                    pullNeeded = false;
                    object answer = MiniJson.TryParse(r.Body);
                    foreach (string k in sentUnlocks) pendingUnlocks.Remove(k);
                    foreach (var kv in sentProgress) if (GetProgress(kv.Key) == kv.Value) pendingProgress.Remove(kv.Key);
                    if (plat != null) pendingUnlocks.Remove(plat.Key);
                    foreach (object u in MiniJson.Arr(MiniJson.Get(answer, "unknown")))
                    {
                        // A trophy the developer removed: forget it.
                        string k = u as string;
                        if (k == null) continue;
                        unlocked.Remove(k); progress.Remove(k); pendingUnlocks.Remove(k); pendingProgress.Remove(k);
                    }
                    Merge(answer, MiniJson.Arr(MiniJson.Get(answer, "newlyUnlocked")));
                    SaveNow();
                    Raise(Changed);
                    if (pendingUnlocks.Count == 0 && pendingProgress.Count == 0) { flushAt = DateTime.MaxValue; return true; }
                    bool onlyFresh = true; // what is left was earned while the request was away, or did not fit: go again
                    foreach (string k in pendingUnlocks) if (sentUnlocks.Contains(k)) onlyFresh = false;
                    if (!onlyFresh) { flushAt = utcNow().AddSeconds(30); return false; }
                }
            }
            finally { flushing = false; }
        }

        /// <summary>
        /// Takes the account's state as the truth, keeping what this device has not sent yet.
        /// Trophies the server has just given (the platinum, mostly) are announced.
        /// </summary>
        void Merge(object answer, List<object> newly)
        {
            if (MiniJson.Get(answer, "unlocked") == null) return;
            var before = new HashSet<string>(unlocked.Keys);
            var mine = new Dictionary<string, DateTime>();
            foreach (string k in pendingUnlocks) { DateTime at; if (unlocked.TryGetValue(k, out at)) mine[k] = at; }
            unlocked.Clear();
            foreach (object u in MiniJson.Arr(MiniJson.Get(answer, "unlocked")))
            {
                string k = MiniJson.Str(u, "key");
                DateTime at;
                if (k != null && TryIso(MiniJson.Str(u, "at"), out at)) unlocked[k] = at;
            }
            foreach (var kv in mine) if (!unlocked.ContainsKey(kv.Key)) unlocked[kv.Key] = kv.Value;
            foreach (object p in MiniJson.Arr(MiniJson.Get(answer, "progress")))
            {
                string k = MiniJson.Str(p, "key");
                int v = (int)MiniJson.Num(p, "value");
                if (k != null && v > GetProgress(k)) progress[k] = v;
            }
            GrantLocalPlatinum();
            if (newly == null) return;
            foreach (object n in newly)
            {
                string k = MiniJson.Str(n, "key");
                if (k == null || before.Contains(k) || !unlocked.ContainsKey(k)) continue;
                TrophyDef def = game != null ? game.Find(k) : null;
                Raise(TrophyUnlocked, def ?? new TrophyDef { Key = k, Name = MiniJson.Str(n, "name", k), Description = "", Tier = GameDef.ParseTier(MiniJson.Str(n, "tier")) });
            }
        }

        async Task<HttpResult> Request(string method, string path, string body, bool asPlayer)
        {
            var headers = new Dictionary<string, string>();
            if (asPlayer) headers["Authorization"] = "Bearer " + token;
            else headers["X-Game-Key"] = options.GameKey ?? "";
            try { return await transport.Send(method, Url(path), headers, body) ?? new HttpResult(); }
            catch (Exception e) { Say("Network: " + e.Message); return new HttpResult(); }
        }

        string Url(string path) { return (options.ServerUrl ?? "").TrimEnd('/') + "/api/v1" + path; }

        /// <summary>Turns an address given by the server ("/api/v1/trophies/12/icon") into a full one.</summary>
        public string AbsoluteUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;
            return url.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? url : (options.ServerUrl ?? "").TrimEnd('/') + url;
        }

        // ---- The local file ----

        Dictionary<string, object> ToState()
        {
            var u = new Dictionary<string, object>();
            foreach (var kv in unlocked) u[kv.Key] = Iso(kv.Value);
            var p = new Dictionary<string, object>();
            foreach (var kv in progress) p[kv.Key] = (long)kv.Value;
            return new Dictionary<string, object>
            {
                { "v", 1L }, { "token", token }, { "username", username }, { "unlocked", u }, { "progress", p },
                { "pendingUnlocks", new List<string>(pendingUnlocks) }, { "pendingProgress", new List<string>(pendingProgress) },
                { "defs", definitionsJson }, { "linkSecret", linkSecret }, { "linkCode", linkCode }, { "linkUrl", linkUrl },
                { "linkExpires", linkSecret != null ? Iso(linkExpires) : null },
            };
        }

        void LoadFile()
        {
            string text = store.Load();
            object s = text != null ? MiniJson.TryParse(text) : null;
            if (MiniJson.Obj(s) == null) return;
            token = MiniJson.Str(s, "token");
            username = MiniJson.Str(s, "username");
            var u = MiniJson.Obj(MiniJson.Get(s, "unlocked"));
            if (u != null) foreach (var kv in u) { DateTime at; if (TryIso(kv.Value as string, out at)) unlocked[kv.Key] = at; }
            var p = MiniJson.Obj(MiniJson.Get(s, "progress"));
            if (p != null) foreach (var kv in p) if (kv.Value is long) progress[kv.Key] = (int)(long)kv.Value;
            foreach (object k in MiniJson.Arr(MiniJson.Get(s, "pendingUnlocks"))) if (k is string && unlocked.ContainsKey((string)k)) pendingUnlocks.Add((string)k);
            foreach (object k in MiniJson.Arr(MiniJson.Get(s, "pendingProgress"))) if (k is string) pendingProgress.Add((string)k);
            definitionsJson = MiniJson.Str(s, "defs");
            if (definitionsJson != null) game = GameDef.FromJson(definitionsJson);
            if (game != null) game.SetLanguage(language);
            linkSecret = MiniJson.Str(s, "linkSecret");
            linkCode = MiniJson.Str(s, "linkCode");
            linkUrl = MiniJson.Str(s, "linkUrl");
            if (linkSecret != null && !TryIso(MiniJson.Str(s, "linkExpires"), out linkExpires)) ClearLinkRequest();
            if (PendingCount > 0) flushAt = utcNow();
        }

        /// <summary>Erases everything this device knows: local trophies, the link, the cached list. For tests and "reset" menus.</summary>
        public void WipeLocal()
        {
            store.Delete();
            token = null; username = null; definitionsJson = null;
            unlocked.Clear(); progress.Clear(); pendingUnlocks.Clear(); pendingProgress.Clear();
            ClearLinkRequest();
            game = !string.IsNullOrEmpty(options.EmbeddedDefinitions) ? GameDef.FromJson(options.EmbeddedDefinitions) : null;
            if (game != null) game.SetLanguage(language);
            Raise(Changed);
        }

        // ---- Small things ----
        static string Iso(DateTime utc) { return utc.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture); }
        static bool TryIso(string s, out DateTime utc)
        {
            return DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out utc);
        }
        static string Clip(string s, int max) { return string.IsNullOrEmpty(s) ? "" : s.Length <= max ? s : s.Substring(0, max); }
        void Say(string message) { if (Log != null) try { Log(message); } catch (Exception) { } }
        void Raise(Action a) { if (a != null) try { a(); } catch (Exception e) { Say("Event handler: " + e.Message); } }
        void Raise(Action<TrophyDef> a, TrophyDef d) { if (a != null) try { a(d); } catch (Exception e) { Say("Event handler: " + e.Message); } }
        void RaiseLinkFailed(string reason) { if (LinkFailed != null) try { LinkFailed(reason); } catch (Exception e) { Say("LinkFailed handler: " + e.Message); } }
    }
}
