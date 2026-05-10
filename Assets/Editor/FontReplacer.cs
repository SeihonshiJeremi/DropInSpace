using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using TMPro;

public class FontReplacer : EditorWindow
{
    private TMP_FontAsset newFont;
    private Vector2 scrollPos;
    private List<string> log = new List<string>();

    [MenuItem("Tools/Drop In Space/Remplacer Police TMP")]
    public static void ShowWindow()
    {
        GetWindow<FontReplacer>("Remplacer Police");
    }

    void OnGUI()
    {
        GUILayout.Label("Remplacement de police TextMeshPro", EditorStyles.boldLabel);
        GUILayout.Space(10);

        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField(
            "Nouvelle police (TMP Font Asset)", newFont, typeof(TMP_FontAsset), false);

        GUILayout.Space(10);

        if (newFont == null)
        {
            EditorGUILayout.HelpBox("Sélectionne d'abord un TMP Font Asset.", MessageType.Warning);
            return;
        }

        if (GUILayout.Button("Remplacer dans TOUTES les scènes", GUILayout.Height(40)))
        {
            ReplaceInAllScenes();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Remplacer dans la scène ACTIVE seulement", GUILayout.Height(30)))
        {
            ReplaceInCurrentScene();
            log.Add("Scène active traitée.");
        }

        GUILayout.Space(10);
        GUILayout.Label("Log :", EditorStyles.boldLabel);
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));
        foreach (string line in log)
            GUILayout.Label(line);
        GUILayout.EndScrollView();
    }

    private void ReplaceInAllScenes()
    {
        log.Clear();

        // Sauvegarde la scène courante
        string currentScenePath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;

        // Récupère toutes les scènes du Build Settings
        string[] scenePaths = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            scenePaths[i] = EditorBuildSettings.scenes[i].path;

        if (scenePaths.Length == 0)
        {
            log.Add("Aucune scène dans le Build Settings ! Ajoute tes scènes d'abord.");
            return;
        }

        int totalReplaced = 0;

        foreach (string scenePath in scenePaths)
        {
            if (string.IsNullOrEmpty(scenePath)) continue;

            // Ouvre la scène
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            int count = ReplaceInCurrentScene();
            totalReplaced += count;

            if (count > 0)
            {
                EditorSceneManager.SaveScene(scene);
                log.Add($"✓ {scene.name} — {count} texte(s) remplacé(s)");
            }
            else
            {
                log.Add($"- {scene.name} — aucun texte TMP trouvé");
            }
        }

        // Retourne à la scène d'origine
        if (!string.IsNullOrEmpty(currentScenePath))
            EditorSceneManager.OpenScene(currentScenePath, OpenSceneMode.Single);

        log.Add($"--- TERMINÉ : {totalReplaced} texte(s) remplacé(s) au total ---");
        Repaint();
    }

    private int ReplaceInCurrentScene()
    {
        int count = 0;

        // Trouve tous les TextMeshProUGUI (UI)
        TextMeshProUGUI[] uiTexts = FindObjectsOfType<TextMeshProUGUI>(true);
        foreach (var t in uiTexts)
        {
            if (t.font != newFont)
            {
                Undo.RecordObject(t, "Replace Font");
                t.font = newFont;
                EditorUtility.SetDirty(t);
                count++;
            }
        }

        // Trouve tous les TextMeshPro (3D)
        TextMeshPro[] texts3D = FindObjectsOfType<TextMeshPro>(true);
        foreach (var t in texts3D)
        {
            if (t.font != newFont)
            {
                Undo.RecordObject(t, "Replace Font");
                t.font = newFont;
                EditorUtility.SetDirty(t);
                count++;
            }
        }

        return count;
    }
}