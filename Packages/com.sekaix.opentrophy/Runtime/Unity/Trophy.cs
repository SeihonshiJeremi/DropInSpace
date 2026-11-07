using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using OpenTrophy.Core;
using UnityEngine;

namespace OpenTrophy
{
    /// <summary>
    /// The whole API a game needs:
    ///   Trophy.Unlock("first_boss");
    ///   Trophy.AddProgress("hunter");          // counters
    ///   Trophy.ShowLinkPanel();                // let the player link an account
    /// Trophies are earned and saved on the device at once, with or without network or account;
    /// they are sent to OpenTrophy as soon as the player has linked an account and the server answers.
    /// </summary>
    public static class Trophy
    {
        static TrophyClient client;
        static OpenTrophySettings settings;
        static TrophyRunner runner;

        /// <summary>A trophy has just been earned.</summary>
        public static event Action<TrophyDef> Unlocked;
        /// <summary>A counter moved: trophy, value, target.</summary>
        public static event Action<TrophyDef, int, int> ProgressChanged;
        /// <summary>Lists, progress or link changed: redraw a trophy screen on this.</summary>
        public static event Action Changed;
        /// <summary>A link code is ready: code, address of the page.</summary>
        public static event Action<string, string> LinkCodeReady;
        /// <summary>The player's account is linked: their name.</summary>
        public static event Action<string> Linked;
        /// <summary>The link did not happen: "offline", "expired", "refused" or "error".</summary>
        public static event Action<string> LinkFailed;
        public static event Action Unlinked;

        /// <summary>False when the settings asset is missing or empty: every call is then ignored, without error.</summary>
        public static bool IsReady { get { return client != null; } }
        public static OpenTrophySettings Settings { get { return settings; } }
        /// <summary>The underlying client, for advanced uses.</summary>
        public static TrophyClient Client { get { return client; } }
        public static GameDef Game { get { return client != null ? client.Game : null; } }
        public static bool IsLinked { get { return client != null && client.IsLinked; } }
        public static string PlayerName { get { return client != null ? client.PlayerName : null; } }
        public static LinkState LinkState { get { return client != null ? client.LinkState : LinkState.NotLinked; } }
        public static string LinkCode { get { return client != null ? client.LinkCode : null; } }
        public static string LinkUrl { get { return client != null ? client.LinkUrl : null; } }
        /// <summary>What was earned here and is not on the server yet.</summary>
        public static int PendingCount { get { return client != null ? client.PendingCount : 0; } }

        /// <summary>Unlocks a trophy. Returns true when it has just been earned (false if already owned).</summary>
        public static bool Unlock(string key) { return client != null && client.Unlock(key); }
        /// <summary>Adds to a counter trophy; reaching its target unlocks it. Returns the new value.</summary>
        public static int AddProgress(string key, int amount = 1) { return client != null ? client.AddProgress(key, amount) : 0; }
        /// <summary>Sets a counter trophy (it never goes down). Returns the value kept.</summary>
        public static int SetProgress(string key, int value) { return client != null ? client.SetProgress(key, value) : 0; }
        public static bool IsUnlocked(string key) { return client != null && client.IsUnlocked(key); }
        public static int GetProgress(string key) { return client != null ? client.GetProgress(key) : 0; }
        /// <summary>
        /// The language trophies are shown in ("en", "pt-BR"…). Set it when the player changes the game's
        /// language: names and descriptions change at once, without asking the server. Changed is raised.
        /// </summary>
        public static string Language
        {
            get { return client != null ? client.Language : null; }
            set { if (client != null) client.Language = value; }
        }
        /// <summary>The languages the trophy list exists in, the game's own first.</summary>
        public static List<string> Languages { get { return client != null && client.Game != null ? client.Game.Languages : new List<string>(); } }

        /// <summary>The player's system language as a code. Null when Unity does not know it.</summary>
        public static string SystemLanguageCode()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.French: return "fr"; case SystemLanguage.English: return "en"; case SystemLanguage.Spanish: return "es";
                case SystemLanguage.German: return "de"; case SystemLanguage.Italian: return "it"; case SystemLanguage.Portuguese: return "pt";
                case SystemLanguage.Dutch: return "nl"; case SystemLanguage.Polish: return "pl"; case SystemLanguage.Russian: return "ru";
                case SystemLanguage.Turkish: return "tr"; case SystemLanguage.Japanese: return "ja"; case SystemLanguage.Korean: return "ko";
                case SystemLanguage.ChineseSimplified: return "zh-Hans"; case SystemLanguage.ChineseTraditional: return "zh-Hant"; case SystemLanguage.Chinese: return "zh";
                case SystemLanguage.Arabic: return "ar"; case SystemLanguage.Czech: return "cs"; case SystemLanguage.Danish: return "da";
                case SystemLanguage.Finnish: return "fi"; case SystemLanguage.Greek: return "el"; case SystemLanguage.Hungarian: return "hu";
                case SystemLanguage.Indonesian: return "id"; case SystemLanguage.Norwegian: return "no"; case SystemLanguage.Romanian: return "ro";
                case SystemLanguage.Swedish: return "sv"; case SystemLanguage.Thai: return "th"; case SystemLanguage.Ukrainian: return "uk";
                case SystemLanguage.Vietnamese: return "vi"; case SystemLanguage.Bulgarian: return "bg"; case SystemLanguage.Catalan: return "ca";
                default: return null;
            }
        }

        /// <summary>Every trophy with the player's state, for a trophy screen.</summary>
        public static List<TrophyStatus> GetAll() { return client != null ? client.GetAll() : new List<TrophyStatus>(); }

        /// <summary>Asks for a link code (LinkCodeReady gives it). The built-in panel does it for you: ShowLinkPanel().</summary>
        public static void BeginLink() { if (client != null) Run(client.BeginLink()); }
        public static void CancelLink() { if (client != null) client.CancelLink(); }
        public static void Unlink() { if (client != null) Run(client.Unlink()); }
        /// <summary>Opens the link page in the player's browser.</summary>
        public static void OpenLinkPage() { if (!string.IsNullOrEmpty(LinkUrl)) Application.OpenURL(LinkUrl); }
        /// <summary>Tries to send now what the server does not have yet.</summary>
        public static void SyncNow() { if (client != null) Run(client.SyncNow()); }
        /// <summary>Shows the built-in panel where the player links (or unlinks) an account.</summary>
        public static void ShowLinkPanel() { if (runner != null) runner.Overlay.ShowLinkPanel(); }
        public static void HideLinkPanel() { if (runner != null) runner.Overlay.HideLinkPanel(); }
        /// <summary>Loads a trophy's icon (cached on disk). The callback gets null when there is none.</summary>
        public static void LoadIcon(TrophyDef def, bool locked, Action<Texture2D> callback) { if (runner != null) runner.Overlay.LoadIcon(def, locked, callback); else if (callback != null) callback(null); }
        /// <summary>Erases the local trophy file and the link on this device (not the account's trophies). For test menus.</summary>
        public static void WipeLocalData() { if (client != null) client.WipeLocal(); }

        // Without a domain reload (Enter Play Mode options), statics survive from one play session to the next.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            client = null; settings = null; runner = null;
            Unlocked = null; ProgressChanged = null; Changed = null; LinkCodeReady = null; Linked = null; LinkFailed = null; Unlinked = null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Boot()
        {
            settings = OpenTrophySettings.Load();
            if (settings == null || !settings.IsConfigured)
            {
                Debug.LogWarning("[OpenTrophy] Not configured: open Tools › OpenTrophy to create the settings and enter the game key. Trophy calls are ignored until then.");
                return;
            }
            if (string.IsNullOrEmpty(settings.localSecret)) Debug.LogWarning("[OpenTrophy] No local secret in the settings: generate one in Tools › OpenTrophy, or the local file is weakly protected.");
            var options = new TrophyClientOptions
            {
                ServerUrl = settings.serverUrl.Trim().TrimEnd('/'),
                GameKey = settings.gameKey.Trim(),
                LocalSecret = settings.localSecret,
                DeviceId = SystemInfo.deviceUniqueIdentifier,
                DeviceName = SystemInfo.deviceName,
                StoragePath = Path.Combine(Path.Combine(Application.persistentDataPath, "opentrophy"), "trophies.dat"),
                EmbeddedDefinitions = settings.embeddedDefinitions,
                Language = string.IsNullOrEmpty(settings.language) ? SystemLanguageCode() : settings.language.Trim(),
            };
            try { client = new TrophyClient(options, new UnityTransport()); }
            catch (Exception e) { Debug.LogException(e); client = null; return; }
            client.Log += (m) => { if (settings.verboseLog) Debug.Log("[OpenTrophy] " + m); };
            client.TrophyUnlocked += (d) => Fire(() => { if (Unlocked != null) Unlocked(d); });
            client.ProgressChanged += (d, v, t) => Fire(() => { if (ProgressChanged != null) ProgressChanged(d, v, t); });
            client.Changed += () => Fire(() => { if (Changed != null) Changed(); });
            client.LinkCodeReady += (c, u) => Fire(() => { if (LinkCodeReady != null) LinkCodeReady(c, u); });
            client.Linked += (n) => Fire(() => { if (Linked != null) Linked(n); });
            client.LinkFailed += (r) => Fire(() => { if (LinkFailed != null) LinkFailed(r); });
            client.Unlinked += () => Fire(() => { if (Unlinked != null) Unlinked(); });

            var go = new GameObject("OpenTrophy");
            UnityEngine.Object.DontDestroyOnLoad(go);
            go.hideFlags = HideFlags.HideInHierarchy;
            runner = go.AddComponent<TrophyRunner>();
            runner.Init(client, settings);
            Run(client.Start());
        }

        static void Fire(Action a) { try { a(); } catch (Exception e) { Debug.LogException(e); } }

        /// <summary>Lets a task run without waiting for it; a failure goes to the console instead of being lost.</summary>
        internal static async void Run(Task task)
        {
            try { await task; } catch (Exception e) { Debug.LogException(e); }
        }
    }

    /// <summary>Lives for the whole game: gives the client its regular tick and saves when the game pauses or closes.</summary>
    [AddComponentMenu("")]
    public sealed class TrophyRunner : MonoBehaviour
    {
        TrophyClient client;
        float nextTick;
        bool ticking;
        public TrophyOverlay Overlay { get; private set; }

        internal void Init(TrophyClient c, OpenTrophySettings settings)
        {
            client = c;
            Overlay = gameObject.AddComponent<TrophyOverlay>();
            Overlay.Init(c, settings);
        }

        void Update()
        {
            if (client == null || ticking || Time.unscaledTime < nextTick) return;
            nextTick = Time.unscaledTime + 0.25f;
            Tick();
        }

        async void Tick()
        {
            ticking = true;
            try { await client.Tick(); } catch (Exception e) { Debug.LogException(e); } finally { ticking = false; }
        }

        void OnApplicationPause(bool paused) { if (client == null) return; if (paused) client.SaveNow(); else Trophy.SyncNow(); }
        void OnApplicationFocus(bool focused) { if (client != null && !focused) client.SaveNow(); }
        void OnApplicationQuit() { if (client != null) client.SaveNow(); }
    }
}
