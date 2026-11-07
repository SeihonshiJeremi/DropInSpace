using System;
using System.Collections.Generic;

namespace OpenTrophy.Core
{
    public enum TrophyTier { Bronze, Silver, Gold, Platinum }

    /// <summary>A trophy as the developer defined it on OpenTrophy.</summary>
    public sealed class TrophyDef
    {
        public string Key;
        /// <summary>In the language chosen (TrophyClient.Language), or the game's own when it has no translation.</summary>
        public string Name;
        public string Description;
        /// <summary>The texts as written by the developer, in the game's own language.</summary>
        public string BaseName;
        public string BaseDescription;
        /// <summary>The other languages: code ("en", "pt-BR") → { name, description }. A missing text is null.</summary>
        public Dictionary<string, string[]> Translations = new Dictionary<string, string[]>();
        public TrophyTier Tier;
        /// <summary>Hidden trophies say nothing of themselves until earned.</summary>
        public bool Hidden;
        /// <summary>For a trophy that counts something: the value to reach. 0 when it has no counter.</summary>
        public int Target;
        public string IconUrl;
        public string LockedIconUrl;
        /// <summary>The list it belongs to: the main one, or an add-on.</summary>
        public TrophyGroup Group;
    }

    public sealed class TrophyGroup
    {
        public string Name;
        public string BaseName;
        public Dictionary<string, string[]> Translations = new Dictionary<string, string[]>();
        /// <summary>The main list: the only one that counts for the platinum and the 100 %.</summary>
        public bool IsBase;
        public List<TrophyDef> Trophies = new List<TrophyDef>();
    }

    public sealed class GameDef
    {
        public string Id;
        public string Title;
        /// <summary>A game still being made: trophies are earned, but no platinum before its release.</summary>
        public bool Beta;
        public string Revision;
        /// <summary>The language the trophies are written in ("fr").</summary>
        public string BaseLanguage = "fr";
        /// <summary>The language Name and Description are in right now. Null: the game's own.</summary>
        public string Language { get; private set; }
        public List<TrophyGroup> Groups = new List<TrophyGroup>();
        readonly Dictionary<string, TrophyDef> byKey = new Dictionary<string, TrophyDef>();

        public IEnumerable<TrophyDef> All { get { foreach (var g in Groups) foreach (var t in g.Trophies) yield return t; } }
        public TrophyDef Find(string key) { TrophyDef t; return key != null && byKey.TryGetValue(key, out t) ? t : null; }
        public TrophyDef Platinum { get { foreach (var t in All) if (t.Tier == TrophyTier.Platinum) return t; return null; } }

        /// <summary>Every language this list exists in, the game's own first.</summary>
        public List<string> Languages
        {
            get
            {
                var all = new List<string> { BaseLanguage };
                foreach (var g in Groups) { Collect(all, g.Translations); foreach (var t in g.Trophies) Collect(all, t.Translations); }
                return all;
            }
        }
        static void Collect(List<string> all, Dictionary<string, string[]> from) { foreach (string c in from.Keys) if (!all.Exists(x => string.Equals(x, c, StringComparison.OrdinalIgnoreCase))) all.Add(c); }

        /// <summary>
        /// Puts every Name and Description in this language ("en", "en-US", "pt-BR"…). A text without a
        /// translation stays in the game's own language. Null or empty: the game's own language.
        /// </summary>
        public void SetLanguage(string language)
        {
            Language = string.IsNullOrEmpty(language) ? null : language;
            foreach (var g in Groups)
            {
                string[] tr = Pick(g.Translations, Language, BaseLanguage);
                g.Name = tr != null && !string.IsNullOrEmpty(tr[0]) ? tr[0] : g.BaseName;
                foreach (var t in g.Trophies)
                {
                    tr = Pick(t.Translations, Language, BaseLanguage);
                    t.Name = tr != null && !string.IsNullOrEmpty(tr[0]) ? tr[0] : t.BaseName;
                    t.Description = tr != null && !string.IsNullOrEmpty(tr[1]) ? tr[1] : t.BaseDescription;
                }
            }
        }

        static string Primary(string code) { int i = code.IndexOf('-'); return (i < 0 ? code : code.Substring(0, i)).ToLowerInvariant(); }
        /// <summary>Exact language first, then the same language of another region. The game's own language means "no translation".</summary>
        static string[] Pick(Dictionary<string, string[]> texts, string language, string baseLanguage)
        {
            if (language == null || texts.Count == 0) return null;
            language = language.Replace('_', '-');
            foreach (var p in texts) if (string.Equals(p.Key, language, StringComparison.OrdinalIgnoreCase)) return p.Value;
            if (Primary(language) == Primary(baseLanguage ?? "")) return null;
            foreach (var p in texts) if (p.Key.ToLowerInvariant() == Primary(language)) return p.Value;
            foreach (var p in texts) if (Primary(p.Key) == Primary(language)) return p.Value;
            return null;
        }
        static Dictionary<string, string[]> ReadTexts(object node)
        {
            var all = new Dictionary<string, string[]>();
            var d = MiniJson.Obj(node);
            if (d != null) foreach (var p in d) if (MiniJson.Obj(p.Value) != null) all[p.Key] = new[] { MiniJson.Str(p.Value, "name"), MiniJson.Str(p.Value, "description") };
            return all;
        }

        /// <summary>Reads what GET /play/game answers. Null when the text is not a game.</summary>
        public static GameDef FromJson(string json)
        {
            object root = MiniJson.TryParse(json);
            if (MiniJson.Obj(root) == null || MiniJson.Get(root, "groups") == null) return null;
            var game = new GameDef { Id = MiniJson.Str(root, "id", ""), Title = MiniJson.Str(root, "title", ""), Beta = MiniJson.Bool(root, "beta"), Revision = MiniJson.Str(root, "revision", ""), BaseLanguage = MiniJson.Str(root, "language", "fr") };
            foreach (object g in MiniJson.Arr(MiniJson.Get(root, "groups")))
            {
                var group = new TrophyGroup { Name = MiniJson.Str(g, "name", ""), BaseName = MiniJson.Str(g, "name", ""), Translations = ReadTexts(MiniJson.Get(g, "i18n")), IsBase = MiniJson.Bool(g, "base") };
                foreach (object t in MiniJson.Arr(MiniJson.Get(g, "trophies")))
                {
                    string key = MiniJson.Str(t, "key");
                    if (string.IsNullOrEmpty(key)) continue;
                    var def = new TrophyDef
                    {
                        Key = key, Name = MiniJson.Str(t, "name", key), Description = MiniJson.Str(t, "description", ""), BaseName = MiniJson.Str(t, "name", key), BaseDescription = MiniJson.Str(t, "description", ""), Translations = ReadTexts(MiniJson.Get(t, "i18n")), Tier = ParseTier(MiniJson.Str(t, "tier")),
                        Hidden = MiniJson.Bool(t, "hidden"), Target = (int)MiniJson.Num(t, "target"), IconUrl = MiniJson.Str(t, "iconUrl"), LockedIconUrl = MiniJson.Str(t, "lockedIconUrl"), Group = group,
                    };
                    group.Trophies.Add(def);
                    game.byKey[key] = def;
                }
                game.Groups.Add(group);
            }
            return game;
        }

        public static TrophyTier ParseTier(string s)
        {
            switch (s) { case "platinum": return TrophyTier.Platinum; case "gold": return TrophyTier.Gold; case "silver": return TrophyTier.Silver; default: return TrophyTier.Bronze; }
        }
    }

    /// <summary>A trophy with what the player has done of it. This is what a game shows in its trophy screen.</summary>
    public sealed class TrophyStatus
    {
        public TrophyDef Def;
        public bool Unlocked;
        public DateTime? UnlockedAt;
        public int Progress;
        /// <summary>True while a hidden trophy must still be shown as "hidden trophy".</summary>
        public bool Secret { get { return Def.Hidden && !Unlocked; } }
    }

    public sealed class HttpResult
    {
        /// <summary>0 when the server could not be reached at all.</summary>
        public int Status;
        public string Body;
        public bool Ok { get { return Status >= 200 && Status < 300; } }
        public bool Unreachable { get { return Status == 0; } }
    }

    /// <summary>How requests leave the game. Unity gives one built on UnityWebRequest.</summary>
    public interface ITransport
    {
        System.Threading.Tasks.Task<HttpResult> Send(string method, string url, Dictionary<string, string> headers, string jsonBody);
    }

    public enum LinkState { NotLinked, WaitingForPlayer, Linked }
}
