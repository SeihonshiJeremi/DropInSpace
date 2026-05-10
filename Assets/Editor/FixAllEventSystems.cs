using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using System.Collections.Generic;

/// <summary>
/// Script Editor — Menu Tools → Ripple Star → Fix All EventSystems
/// Parcourt toutes les scenes du Build Settings et :
/// 1. Remplace Standalone Input Module par Input System UI Input Module
/// 2. Ajoute SelectionManager si absent
/// </summary>
public class FixAllEventSystems : EditorWindow
{
    private Vector2 scrollPos;
    private List<string> logs = new List<string>();

    [MenuItem("Tools/Ripple Star/Fix All EventSystems")]
    public static void ShowWindow()
    {
        GetWindow<FixAllEventSystems>("Fix EventSystems");
    }

    void OnGUI()
    {
        GUILayout.Label("Fix All EventSystems", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        GUILayout.Label("Remplace Standalone Input Module par\nInput System UI Input Module dans toutes les scenes.", EditorStyles.helpBox);
        EditorGUILayout.Space();

        if (GUILayout.Button("🔧 Lancer le Fix", GUILayout.Height(40)))
        {
            logs.Clear();
            FixAllScenes();
        }

        EditorGUILayout.Space();
        GUILayout.Label("Log :");
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(300));
        foreach (var log in logs)
        {
            Color old = GUI.color;
            GUI.color = log.StartsWith("✅") ? Color.green : log.StartsWith("⚠️") ? Color.yellow : log.StartsWith("❌") ? Color.red : Color.white;
            GUILayout.Label(log);
            GUI.color = old;
        }
        EditorGUILayout.EndScrollView();
    }

    void FixAllScenes()
    {
        // Sauvegarde la scene actuelle
        string sceneActuelle = EditorSceneManager.GetActiveScene().path;

        // Recupere toutes les scenes du Build Settings
        var scenes = EditorBuildSettings.scenes;
        int fixed_count = 0;
        int total = 0;

        logs.Add($"📋 {scenes.Length} scenes trouvees dans le Build Settings");

        foreach (var buildScene in scenes)
        {
            if (!buildScene.enabled) continue;
            if (string.IsNullOrEmpty(buildScene.path)) continue;

            total++;
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(buildScene.path);

            try
            {
                // Ouvre la scene
                var scene = EditorSceneManager.OpenScene(buildScene.path, OpenSceneMode.Single);

                bool sceneModifiee = false;

                // Cherche tous les EventSystem dans la scene
                var eventSystems = GameObject.FindObjectsOfType<EventSystem>();

                if (eventSystems.Length == 0)
                {
                    logs.Add($"⚠️ {sceneName} — Aucun EventSystem trouve");
                    continue;
                }

                foreach (var es in eventSystems)
                {
                    // Verifie si StandaloneInputModule est present
                    var standalone = es.GetComponent<StandaloneInputModule>();
                    if (standalone != null)
                    {
                        // Supprime StandaloneInputModule
                        DestroyImmediate(standalone);

                        // Ajoute InputSystemUIInputModule si absent
                        var inputSystemModule = es.GetComponent<InputSystemUIInputModule>();
                        if (inputSystemModule == null)
                        {
                            es.gameObject.AddComponent<InputSystemUIInputModule>();
                            logs.Add($"✅ {sceneName} — StandaloneInputModule remplace par InputSystemUIInputModule");
                        }
                        else
                        {
                            logs.Add($"✅ {sceneName} — StandaloneInputModule supprime (InputSystemUIInputModule deja present)");
                        }
                        sceneModifiee = true;
                    }
                    else
                    {
                        // Verifie si InputSystemUIInputModule est deja present
                        var inputSystemModule = es.GetComponent<InputSystemUIInputModule>();
                        if (inputSystemModule == null)
                        {
                            es.gameObject.AddComponent<InputSystemUIInputModule>();
                            logs.Add($"✅ {sceneName} — InputSystemUIInputModule ajoute");
                            sceneModifiee = true;
                        }
                        else
                        {
                            logs.Add($"  {sceneName} — Deja correct !");
                        }
                    }

                    // Ajoute SelectionManager si absent
                    var selectionManager = es.GetComponent<SelectionManager>();
                    if (selectionManager == null)
                    {
                        es.gameObject.AddComponent<SelectionManager>();
                        logs.Add($"  {sceneName} — SelectionManager ajoute");
                        sceneModifiee = true;
                    }
                }

                // Sauvegarde la scene si modifiee
                if (sceneModifiee)
                {
                    EditorSceneManager.SaveScene(scene);
                    fixed_count++;
                }
            }
            catch (System.Exception e)
            {
                logs.Add($"❌ {sceneName} — Erreur : {e.Message}");
            }
        }

        logs.Add($"");
        logs.Add($"✅ Termine ! {fixed_count}/{total} scenes modifiees.");

        // Retourne a la scene d'origine
        if (!string.IsNullOrEmpty(sceneActuelle))
            EditorSceneManager.OpenScene(sceneActuelle, OpenSceneMode.Single);

        Repaint();
    }
}
