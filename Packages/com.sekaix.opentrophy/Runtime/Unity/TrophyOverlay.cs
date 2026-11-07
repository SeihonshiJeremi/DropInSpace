using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using OpenTrophy.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace OpenTrophy
{
    /// <summary>
    /// What OpenTrophy draws by itself: the "trophy earned" notification and the panel where the player
    /// links an account. Drawn with Unity's immediate GUI so it needs no canvas, no font asset and no
    /// package, in any render pipeline. A game that wants its own look turns the notification off in the
    /// settings and listens to Trophy.Unlocked.
    /// </summary>
    [AddComponentMenu("")]
    public sealed class TrophyOverlay : MonoBehaviour
    {
        sealed class Toast { public TrophyDef Def; public Texture2D Icon; public float Start = -1f; }

        TrophyClient client;
        OpenTrophySettings settings;
        readonly Queue<Toast> queue = new Queue<Toast>();
        Toast current;
        readonly Dictionary<string, Texture2D> icons = new Dictionary<string, Texture2D>();
        bool panelOpen;
        string panelMessage;
        GUIStyle titleStyle, nameStyle, textStyle, codeStyle, buttonStyle;
        int styledFor;
        AudioSource audioSource;

        static readonly Color Back = new Color(0.07f, 0.08f, 0.12f, 0.96f);
        static readonly Color Line = new Color(1f, 1f, 1f, 0.12f);

        public static Color TierColor(TrophyTier tier)
        {
            switch (tier)
            {
                case TrophyTier.Platinum: return new Color(0.73f, 0.83f, 0.96f);
                case TrophyTier.Gold: return new Color(0.91f, 0.73f, 0.24f);
                case TrophyTier.Silver: return new Color(0.76f, 0.78f, 0.82f);
                default: return new Color(0.79f, 0.52f, 0.32f);
            }
        }

        internal void Init(TrophyClient c, OpenTrophySettings s)
        {
            client = c;
            settings = s;
            client.TrophyUnlocked += OnUnlocked;
            client.LinkFailed += (reason) => { panelMessage = reason == "offline" ? settings.texts.offline : reason == "expired" ? settings.texts.expired : settings.texts.error; };
            client.Linked += (name) => { panelMessage = null; };
        }

        void OnUnlocked(TrophyDef def)
        {
            if (!settings.showToasts) return;
            var toast = new Toast { Def = def };
            queue.Enqueue(toast);
            LoadIcon(def, false, (tex) => toast.Icon = tex);
        }

        public void ShowLinkPanel() { panelOpen = true; panelMessage = null; }
        public void HideLinkPanel() { panelOpen = false; }

        // ---- Icons: downloaded once, then read from the disk ----

        public void LoadIcon(TrophyDef def, bool locked, Action<Texture2D> callback)
        {
            string url = client != null && def != null ? client.AbsoluteUrl(locked && !string.IsNullOrEmpty(def.LockedIconUrl) ? def.LockedIconUrl : def.IconUrl) : null;
            if (string.IsNullOrEmpty(url)) { if (callback != null) callback(null); return; }
            Texture2D known;
            if (icons.TryGetValue(url, out known) && known != null) { if (callback != null) callback(known); return; }
            string file = Path.Combine(Path.Combine(Path.Combine(Application.persistentDataPath, "opentrophy"), "icons"), Hash(url) + ".img");
            try
            {
                if (File.Exists(file))
                {
                    var cached = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                    if (cached.LoadImage(File.ReadAllBytes(file))) { icons[url] = cached; if (callback != null) callback(cached); return; }
                    Destroy(cached);
                }
            }
            catch (Exception) { }
            // The server gives WebP, which Unity cannot read: it is asked for PNG instead.
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url + (url.Contains("?") ? "&" : "?") + "format=png");
            request.timeout = 15;
            request.SendWebRequest().completed += (_) =>
            {
                Texture2D tex = null;
                if (request.result == UnityWebRequest.Result.Success)
                {
                    tex = DownloadHandlerTexture.GetContent(request);
                    if (tex != null)
                    {
                        icons[url] = tex;
                        try { Directory.CreateDirectory(Path.GetDirectoryName(file)); File.WriteAllBytes(file, request.downloadHandler.data); } catch (Exception) { }
                    }
                }
                request.Dispose();
                if (callback != null) callback(tex);
            };
        }

        static string Hash(string s)
        {
            using (var sha = SHA1.Create())
            {
                var sb = new StringBuilder();
                foreach (byte b in sha.ComputeHash(Encoding.UTF8.GetBytes(s))) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        // ---- Drawing ----

        void Update()
        {
            if (current == null && queue.Count > 0)
            {
                current = queue.Dequeue();
                current.Start = Time.unscaledTime;
                if (settings.toastSound != null)
                {
                    if (audioSource == null) { audioSource = gameObject.AddComponent<AudioSource>(); audioSource.playOnAwake = false; audioSource.spatialBlend = 0f; audioSource.ignoreListenerPause = true; }
                    audioSource.PlayOneShot(settings.toastSound, settings.toastVolume);
                }
            }
            if (current != null && Time.unscaledTime - current.Start > settings.toastSeconds) current = null;
        }

        void OnGUI()
        {
            if (current == null && !panelOpen) return;
            // Everything is laid out for a 1080-pixel-high screen and scaled to the real one.
            float scale = Mathf.Max(0.5f, Screen.height / 1080f);
            int size = Mathf.RoundToInt(scale * 100);
            if (titleStyle == null || styledFor != size) BuildStyles(scale, size);
            GUI.depth = -1000;
            if (current != null) DrawToast(scale);
            if (panelOpen) DrawPanel(scale);
            GUI.color = Color.white;
        }

        void BuildStyles(float scale, int size)
        {
            styledFor = size;
            titleStyle = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(17 * scale), fontStyle = FontStyle.Bold, alignment = TextAnchor.UpperLeft, wordWrap = false, clipping = TextClipping.Clip };
            nameStyle = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(24 * scale), fontStyle = FontStyle.Bold, alignment = TextAnchor.UpperLeft, wordWrap = false, clipping = TextClipping.Clip };
            textStyle = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(19 * scale), alignment = TextAnchor.UpperLeft, wordWrap = true };
            codeStyle = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(54 * scale), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
            buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = Mathf.RoundToInt(20 * scale), fontStyle = FontStyle.Bold };
            titleStyle.normal.textColor = nameStyle.normal.textColor = textStyle.normal.textColor = codeStyle.normal.textColor = Color.white;
        }

        static void Fill(Rect r, Color c)
        {
            Color old = GUI.color;
            GUI.color = c;
            GUI.DrawTexture(r, Texture2D.whiteTexture);
            GUI.color = old;
        }

        void DrawToast(float scale)
        {
            float age = Time.unscaledTime - current.Start, life = settings.toastSeconds;
            // Slides in, stays, fades out.
            float show = Mathf.Clamp01(age / 0.35f), hide = Mathf.Clamp01((life - age) / 0.5f);
            float alpha = Mathf.Min(show, hide);
            float w = 470 * scale, h = 104 * scale, margin = 28 * scale;
            ToastCorner corner = settings.toastCorner;
            bool top = corner == ToastCorner.TopLeft || corner == ToastCorner.TopRight || corner == ToastCorner.TopCenter;
            float x = corner == ToastCorner.TopLeft || corner == ToastCorner.BottomLeft ? margin
                : corner == ToastCorner.TopCenter || corner == ToastCorner.BottomCenter ? (Screen.width - w) / 2f : Screen.width - w - margin;
            float slide = (1f - show * show * (3f - 2f * show)) * (h + margin);
            float y = top ? margin - slide : Screen.height - h - margin + slide;
            var box = new Rect(x, y, w, h);
            Color tier = TierColor(current.Def.Tier);
            GUI.color = new Color(1, 1, 1, alpha);
            Fill(box, Back);
            Fill(new Rect(box.x, box.y, 5 * scale, box.height), tier);
            float pad = 14 * scale, icon = h - 2 * pad;
            var iconRect = new Rect(box.x + pad + 5 * scale, box.y + pad, icon, icon);
            if (current.Icon != null) GUI.DrawTexture(iconRect, current.Icon, ScaleMode.ScaleToFit);
            else { Fill(iconRect, new Color(tier.r, tier.g, tier.b, 0.22f)); DrawCup(iconRect, tier); }
            float tx = iconRect.xMax + pad, tw = box.xMax - tx - pad;
            Color old = titleStyle.normal.textColor;
            titleStyle.normal.textColor = tier;
            GUI.Label(new Rect(tx, box.y + pad - 2 * scale, tw, 24 * scale), current.Def.Tier == TrophyTier.Platinum ? settings.texts.platinumUnlocked : settings.texts.trophyUnlocked, titleStyle);
            titleStyle.normal.textColor = old;
            GUI.Label(new Rect(tx, box.y + pad + 22 * scale, tw, 34 * scale), current.Def.Name, nameStyle);
            if (!string.IsNullOrEmpty(current.Def.Description))
            {
                Color c = GUI.color;
                GUI.color = new Color(1, 1, 1, alpha * 0.7f);
                var small = new GUIStyle(textStyle) { fontSize = Mathf.RoundToInt(15 * scale), wordWrap = false, clipping = TextClipping.Clip };
                GUI.Label(new Rect(tx, box.y + pad + 54 * scale, tw, 22 * scale), current.Def.Description, small);
                GUI.color = c;
            }
        }

        // A trophy cup made of rectangles, for trophies without an icon.
        static void DrawCup(Rect r, Color c)
        {
            float u = r.width / 10f;
            Fill(new Rect(r.x + 3 * u, r.y + 2 * u, 4 * u, 3.4f * u), c);
            Fill(new Rect(r.x + 2 * u, r.y + 2.4f * u, u, 1.6f * u), c);
            Fill(new Rect(r.x + 7 * u, r.y + 2.4f * u, u, 1.6f * u), c);
            Fill(new Rect(r.x + 4.5f * u, r.y + 5.4f * u, u, 1.4f * u), c);
            Fill(new Rect(r.x + 3.4f * u, r.y + 6.8f * u, 3.2f * u, 0.9f * u), c);
        }

        void DrawPanel(float scale)
        {
            OpenTrophyTexts t = settings.texts;
            Fill(new Rect(0, 0, Screen.width, Screen.height), new Color(0, 0, 0, 0.6f));
            float w = Mathf.Min(620 * scale, Screen.width - 30 * scale), h = 400 * scale;
            var box = new Rect((Screen.width - w) / 2f, (Screen.height - h) / 2f, w, h);
            Fill(box, Back);
            Fill(new Rect(box.x, box.y, box.width, 4 * scale), TierColor(TrophyTier.Gold));
            float pad = 26 * scale, x = box.x + pad, inner = box.width - 2 * pad, y = box.y + pad;
            GUI.Label(new Rect(x, y, inner, 34 * scale), t.linkTitle, nameStyle);
            y += 46 * scale;
            float bh = 48 * scale, by = box.yMax - pad - bh, half = (inner - 12 * scale) / 2f;
            LinkState state = client.LinkState;
            if (state == LinkState.Linked)
            {
                GUI.Label(new Rect(x, y, inner, 60 * scale), string.Format(t.linkedAs, client.PlayerName), textStyle);
                if (client.PendingCount > 0) GUI.Label(new Rect(x, y + 40 * scale, inner, 30 * scale), string.Format(t.pending, client.PendingCount), textStyle);
                if (GUI.Button(new Rect(x, by, half, bh), t.unlink, buttonStyle)) Trophy.Unlink();
                if (GUI.Button(new Rect(x + half + 12 * scale, by, half, bh), t.close, buttonStyle)) panelOpen = false;
            }
            else if (state == LinkState.WaitingForPlayer)
            {
                GUI.Label(new Rect(x, y, inner, 54 * scale), t.linkCodeHelp, textStyle);
                Fill(new Rect(x, y + 60 * scale, inner, 86 * scale), Line);
                GUI.Label(new Rect(x, y + 60 * scale, inner, 86 * scale), client.LinkCode, codeStyle);
                GUI.Label(new Rect(x, y + 156 * scale, inner, 30 * scale), t.waiting, textStyle);
                if (GUI.Button(new Rect(x, by, half, bh), t.openPage, buttonStyle)) Trophy.OpenLinkPage();
                if (GUI.Button(new Rect(x + half + 12 * scale, by, half, bh), t.cancel, buttonStyle)) { client.CancelLink(); panelOpen = false; }
            }
            else
            {
                GUI.Label(new Rect(x, y, inner, 120 * scale), t.linkIntro, textStyle);
                if (!string.IsNullOrEmpty(panelMessage))
                {
                    Color old = textStyle.normal.textColor;
                    textStyle.normal.textColor = new Color(1f, 0.55f, 0.5f);
                    GUI.Label(new Rect(x, y + 126 * scale, inner, 70 * scale), panelMessage, textStyle);
                    textStyle.normal.textColor = old;
                }
                if (GUI.Button(new Rect(x, by, half, bh), t.linkButton, buttonStyle)) { panelMessage = null; Trophy.BeginLink(); }
                if (GUI.Button(new Rect(x + half + 12 * scale, by, half, bh), t.close, buttonStyle)) panelOpen = false;
            }
        }
    }
}
