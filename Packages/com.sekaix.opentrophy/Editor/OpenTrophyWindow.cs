using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using OpenTrophy.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace OpenTrophy.Editor
{
    /// <summary>
    /// Tools › OpenTrophy: creates the settings, checks the game key, brings the trophy list into the
    /// project, writes the key constants, and (in Play mode) unlocks trophies by hand to try things out.
    /// </summary>
    public sealed class OpenTrophyWindow : EditorWindow
    {
        const string SettingsPath = "Assets/Resources/OpenTrophySettings.asset";
        const string KeysPath = "Assets/OpenTrophy/TrophyKeys.cs";

        OpenTrophySettings settings;
        SerializedObject serialized;
        Vector2 scroll;
        UnityWebRequest request;
        string status;
        MessageType statusType = MessageType.Info;
        GameDef game;
        string gameFrom;
        bool showKey, showAdvanced;

        [MenuItem("Tools/OpenTrophy")]
        public static void Open()
        {
            var w = GetWindow<OpenTrophyWindow>();
            w.titleContent = new GUIContent("OpenTrophy");
            w.minSize = new Vector2(420, 480);
            w.Show();
        }

        void OnEnable()
        {
            Find();
            Trophy.Changed += Repaint;
            EditorApplication.playModeStateChanged += OnPlayMode;
        }

        void OnDisable()
        {
            Trophy.Changed -= Repaint;
            EditorApplication.playModeStateChanged -= OnPlayMode;
            EditorApplication.update -= Poll;
            if (request != null) { request.Dispose(); request = null; }
        }

        void OnPlayMode(PlayModeStateChange change)
        {
            // Entering Play mode clears the static events: subscribe again once the game runs.
            if (change == PlayModeStateChange.EnteredPlayMode) { Trophy.Changed -= Repaint; Trophy.Changed += Repaint; }
            Repaint();
        }

        void Find()
        {
            settings = AssetDatabase.LoadAssetAtPath<OpenTrophySettings>(SettingsPath);
            if (settings == null)
            {
                // The asset may have been moved to another Resources folder.
                foreach (string guid in AssetDatabase.FindAssets("t:OpenTrophySettings"))
                {
                    settings = AssetDatabase.LoadAssetAtPath<OpenTrophySettings>(AssetDatabase.GUIDToAssetPath(guid));
                    if (settings != null) break;
                }
            }
            serialized = settings != null ? new SerializedObject(settings) : null;
            game = null;
        }

        void OnGUI()
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);
            EditorGUILayout.Space();
            if (settings == null) { DrawCreate(); EditorGUILayout.EndScrollView(); return; }
            if (serialized == null || serialized.targetObject == null) Find();
            if (serialized == null) { EditorGUILayout.EndScrollView(); return; }
            serialized.Update();
            DrawConnection();
            EditorGUILayout.Space();
            DrawTrophies();
            EditorGUILayout.Space();
            DrawPlayMode();
            EditorGUILayout.Space();
            DrawOptions();
            if (serialized.ApplyModifiedProperties()) EditorUtility.SetDirty(settings);
            EditorGUILayout.EndScrollView();
        }

        void DrawCreate()
        {
            EditorGUILayout.HelpBox("OpenTrophy n'est pas encore configuré dans ce projet. Le bouton crée le fichier de réglages (" + SettingsPath + ").", MessageType.Info);
            if (GUILayout.Button("Créer les réglages", GUILayout.Height(30)))
            {
                Directory.CreateDirectory("Assets/Resources");
                settings = CreateInstance<OpenTrophySettings>();
                settings.localSecret = NewSecret();
                AssetDatabase.CreateAsset(settings, SettingsPath);
                AssetDatabase.SaveAssets();
                Find();
            }
        }

        static string NewSecret()
        {
            var bytes = new byte[24];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        void DrawConnection()
        {
            EditorGUILayout.LabelField("Connexion", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serialized.FindProperty("serverUrl"), new GUIContent("Adresse du service"));
            SerializedProperty key = serialized.FindProperty("gameKey");
            EditorGUILayout.BeginHorizontal();
            if (showKey) key.stringValue = EditorGUILayout.TextField(new GUIContent("Clé du jeu", "otg_… : créée sur le site, espace développeur, onglet « Clés et intégration »."), key.stringValue);
            else key.stringValue = EditorGUILayout.PasswordField(new GUIContent("Clé du jeu", "otg_… : créée sur le site, espace développeur, onglet « Clés et intégration »."), key.stringValue);
            if (GUILayout.Button(showKey ? "Cacher" : "Voir", GUILayout.Width(60))) showKey = !showKey;
            EditorGUILayout.EndHorizontal();
            if (!string.IsNullOrEmpty(key.stringValue) && !key.stringValue.Trim().StartsWith("otg_"))
                EditorGUILayout.HelpBox("Une clé de jeu commence par « otg_ ». Ne mets ici ni ton mot de passe ni un jeton de synchronisation.", MessageType.Warning);
            EditorGUILayout.BeginHorizontal();
            using (new EditorGUI.DisabledScope(request != null || string.IsNullOrEmpty(key.stringValue)))
                if (GUILayout.Button(request != null ? "Connexion…" : "Vérifier et récupérer les trophées", GUILayout.Height(26))) Fetch();
            if (GUILayout.Button("Ouvrir le site", GUILayout.Height(26), GUILayout.Width(110))) Application.OpenURL(settings.serverUrl.TrimEnd('/') + "/#/dev");
            EditorGUILayout.EndHorizontal();
            if (!string.IsNullOrEmpty(status)) EditorGUILayout.HelpBox(status, statusType);
        }

        void Fetch()
        {
            serialized.ApplyModifiedProperties();
            status = null;
            request = UnityWebRequest.Get(settings.serverUrl.Trim().TrimEnd('/') + "/api/v1/play/game");
            request.SetRequestHeader("X-Game-Key", settings.gameKey.Trim());
            request.timeout = 15;
            request.SendWebRequest();
            EditorApplication.update -= Poll;
            EditorApplication.update += Poll;
        }

        void Poll()
        {
            if (request == null) { EditorApplication.update -= Poll; return; }
            if (!request.isDone) return;
            EditorApplication.update -= Poll;
            string body = request.downloadHandler != null ? request.downloadHandler.text : null;
            long code = request.responseCode;
            bool reached = request.result != UnityWebRequest.Result.ConnectionError && code != 0;
            string error = request.error;
            request.Dispose();
            request = null;
            GameDef fresh = reached && code == 200 ? GameDef.FromJson(body) : null;
            if (fresh != null)
            {
                Undo.RecordObject(settings, "OpenTrophy trophies");
                settings.embeddedDefinitions = body;
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
                serialized.Update();
                game = fresh; gameFrom = body;
                int count = 0;
                foreach (var t in fresh.All) count++;
                status = "« " + fresh.Title + " » : " + count + " trophée(s) récupéré(s) et embarqué(s) dans le jeu." + (fresh.Beta ? " Jeu en bêta : pas de platine avant sa sortie." : "");
                statusType = MessageType.Info;
            }
            else
            {
                status = !reached ? "Serveur injoignable (" + error + "). Vérifie l'adresse." : code == 401 ? "Clé de jeu refusée : elle a été retirée, ou mal copiée." : "Réponse inattendue du serveur (" + code + ").";
                statusType = MessageType.Error;
            }
            Repaint();
        }

        GameDef CurrentGame()
        {
            if (Application.isPlaying && Trophy.Game != null) return Trophy.Game;
            if (game == null || gameFrom != settings.embeddedDefinitions)
            {
                gameFrom = settings.embeddedDefinitions;
                game = string.IsNullOrEmpty(gameFrom) ? null : GameDef.FromJson(gameFrom);
            }
            return game;
        }

        static string TierName(TrophyTier t) { return t == TrophyTier.Platinum ? "Platine" : t == TrophyTier.Gold ? "Or" : t == TrophyTier.Silver ? "Argent" : "Bronze"; }

        void DrawTrophies()
        {
            EditorGUILayout.LabelField("Trophées", EditorStyles.boldLabel);
            GameDef g = CurrentGame();
            if (g == null)
            {
                EditorGUILayout.HelpBox("Aucune liste dans le projet. Récupère-la avec le bouton ci-dessus : elle sera embarquée dans le jeu, pour que les trophées s'affichent même au premier lancement hors ligne.", MessageType.Info);
                return;
            }
            EditorGUILayout.LabelField(g.Title + (g.Beta ? "  (bêta)" : ""), EditorStyles.miniBoldLabel);
            bool playing = Application.isPlaying && Trophy.IsReady;
            foreach (TrophyGroup group in g.Groups)
            {
                EditorGUILayout.LabelField(group.Name + (group.IsBase ? "" : "  — extension"), EditorStyles.miniLabel);
                foreach (TrophyDef t in group.Trophies)
                {
                    EditorGUILayout.BeginHorizontal();
                    Color old = GUI.color;
                    GUI.color = TrophyOverlay.TierColor(t.Tier);
                    GUILayout.Label("●", GUILayout.Width(14));
                    GUI.color = old;
                    string extra = (t.Hidden ? " · caché" : "") + (t.Target > 0 ? " · compteur " + (playing ? Trophy.GetProgress(t.Key) + "/" : "") + t.Target : "");
                    EditorGUILayout.LabelField(new GUIContent(t.Name, t.Description), GUILayout.MinWidth(90));
                    EditorGUILayout.SelectableLabel(t.Key, EditorStyles.miniLabel, GUILayout.Height(16), GUILayout.MinWidth(70));
                    GUILayout.Label(TierName(t.Tier) + extra, EditorStyles.miniLabel, GUILayout.Width(150));
                    if (playing)
                    {
                        if (Trophy.IsUnlocked(t.Key)) GUILayout.Label("✔", GUILayout.Width(64));
                        else if (t.Tier == TrophyTier.Platinum) GUILayout.Label("auto", EditorStyles.miniLabel, GUILayout.Width(64));
                        else if (t.Target > 0) { if (GUILayout.Button("+" + Mathf.Max(1, t.Target / 10), EditorStyles.miniButton, GUILayout.Width(64))) Trophy.AddProgress(t.Key, Mathf.Max(1, t.Target / 10)); }
                        else if (GUILayout.Button("Débloquer", EditorStyles.miniButton, GUILayout.Width(64))) Trophy.Unlock(t.Key);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Générer TrophyKeys.cs", "Écrit " + KeysPath + " : une constante par trophée, pour éviter les fautes de frappe (Trophy.Unlock(TrophyKeys.FirstBoss)).")))
                WriteKeys(g);
            EditorGUILayout.EndHorizontal();
        }

        void DrawPlayMode()
        {
            EditorGUILayout.LabelField("En jeu", EditorStyles.boldLabel);
            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Lance le jeu (Play) : tu pourras débloquer les trophées à la main depuis cette fenêtre, et tester la liaison du compte.", MessageType.None);
                if (GUILayout.Button(new GUIContent("Effacer les trophées locaux de l'éditeur", "Supprime le fichier local écrit par l'éditeur sur cette machine. Les trophées du compte ne sont pas touchés.")))
                {
                    string dir = Path.Combine(Application.persistentDataPath, "opentrophy");
                    if (Directory.Exists(dir)) Directory.Delete(dir, true);
                    status = "Fichier local effacé (" + dir + ").";
                    statusType = MessageType.Info;
                }
                return;
            }
            if (!Trophy.IsReady) { EditorGUILayout.HelpBox("OpenTrophy n'a pas démarré : la clé du jeu manque dans les réglages.", MessageType.Warning); return; }
            LinkState state = Trophy.LinkState;
            EditorGUILayout.LabelField("Compte", state == LinkState.Linked ? "lié : " + Trophy.PlayerName : state == LinkState.WaitingForPlayer ? "code " + Trophy.LinkCode + " en attente" : "non lié (trophées gardés en local)");
            EditorGUILayout.LabelField("En attente d'envoi", Trophy.PendingCount.ToString());
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Panneau de liaison")) Trophy.ShowLinkPanel();
            if (GUILayout.Button("Synchroniser")) Trophy.SyncNow();
            if (GUILayout.Button("Effacer le local") && EditorUtility.DisplayDialog("OpenTrophy", "Effacer les trophées locaux et la liaison sur cette machine ? Les trophées déjà sur le compte ne sont pas touchés.", "Effacer", "Annuler")) Trophy.WipeLocalData();
            EditorGUILayout.EndHorizontal();
        }

        void DrawOptions()
        {
            EditorGUILayout.LabelField("Notification", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serialized.FindProperty("showToasts"), new GUIContent("Notification intégrée"));
            using (new EditorGUI.DisabledScope(!settings.showToasts))
            {
                EditorGUILayout.PropertyField(serialized.FindProperty("toastCorner"), new GUIContent("Position"));
                EditorGUILayout.PropertyField(serialized.FindProperty("toastSeconds"), new GUIContent("Durée (s)"));
                EditorGUILayout.PropertyField(serialized.FindProperty("toastSound"), new GUIContent("Son"));
                EditorGUILayout.PropertyField(serialized.FindProperty("toastVolume"), new GUIContent("Volume"));
            }
            EditorGUILayout.PropertyField(serialized.FindProperty("texts"), new GUIContent("Textes (traduction)"), true);
            showAdvanced = EditorGUILayout.Foldout(showAdvanced, "Avancé");
            if (!showAdvanced) return;
            EditorGUILayout.PropertyField(serialized.FindProperty("verboseLog"), new GUIContent("Journal détaillé"));
            SerializedProperty secret = serialized.FindProperty("localSecret");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(secret, new GUIContent("Secret local"));
            if (GUILayout.Button("Nouveau", GUILayout.Width(70)) && (string.IsNullOrEmpty(secret.stringValue) || EditorUtility.DisplayDialog("OpenTrophy", "Changer le secret rend illisibles les fichiers locaux déjà écrits : les joueurs non liés perdraient leurs trophées locaux. À ne pas faire une fois le jeu sorti.", "Changer", "Annuler")))
                secret.stringValue = NewSecret();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Le secret local entre dans le chiffrement du fichier de trophées, avec l'identifiant de l'appareil. Il arrête la triche facile (éditer ou copier le fichier), pas quelqu'un qui démonte le jeu.", MessageType.None);
        }

        // ---- TrophyKeys.cs ----

        void WriteKeys(GameDef g)
        {
            var sb = new StringBuilder();
            sb.AppendLine("// Écrit par Tools › OpenTrophy. Ne pas modifier à la main : le fichier est réécrit à chaque génération.");
            sb.AppendLine("/// <summary>Les identifiants des trophées de « " + g.Title.Replace("\n", " ") + " ».</summary>");
            sb.AppendLine("public static class TrophyKeys");
            sb.AppendLine("{");
            var used = new HashSet<string>();
            foreach (TrophyDef t in g.All)
            {
                if (t.Tier == TrophyTier.Platinum) continue; // never unlocked by the game
                string name = Identifier(t.Key);
                while (!used.Add(name)) name += "_";
                sb.AppendLine("    /// <summary>" + Xml(t.Name) + " (" + TierName(t.Tier) + (t.Target > 0 ? ", compteur jusqu'à " + t.Target : "") + (t.Group.IsBase ? "" : ", " + Xml(t.Group.Name)) + ")</summary>");
                sb.AppendLine("    public const string " + name + " = \"" + t.Key + "\";");
            }
            sb.AppendLine("}");
            Directory.CreateDirectory(Path.GetDirectoryName(KeysPath));
            File.WriteAllText(KeysPath, sb.ToString(), new UTF8Encoding(false));
            AssetDatabase.ImportAsset(KeysPath);
            status = KeysPath + " écrit. Exemple : Trophy.Unlock(TrophyKeys." + (used.Count > 0 ? First(used) : "MonTrophee") + ");";
            statusType = MessageType.Info;
        }

        static string First(HashSet<string> set) { foreach (string s in set) return s; return ""; }
        static string Xml(string s) { return (s ?? "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", " "); }

        /// <summary>first_boss → FirstBoss ; 100-kills → _100Kills.</summary>
        public static string Identifier(string key)
        {
            var sb = new StringBuilder();
            bool upper = true;
            foreach (char c in key)
            {
                if (!char.IsLetterOrDigit(c) || c > 127) { upper = true; continue; }
                sb.Append(upper ? char.ToUpperInvariant(c) : c);
                upper = false;
            }
            if (sb.Length == 0) sb.Append("Trophy");
            if (char.IsDigit(sb[0])) sb.Insert(0, '_');
            return sb.ToString();
        }
    }
}
