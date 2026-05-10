using UnityEngine;
using UnityEditor;
using TMPro;

/// <summary>
/// Script Editor — Menu Tools → Ripple Star → Fix SceneTransition TMP Width
/// Corrige le Width du TextMeshPro "00" dans le prefab sceneTransition
/// pour eviter que le texte "04-01" passe a la ligne.
/// </summary>
public class FixSceneTransitionTMP : EditorWindow
{
    private string prefabPath = "Assets/01GameDropInSpace/PrefabDIP/sceneTranslation.prefab";
    private float newWidth = 800f;
    private bool useAutoSize = false;
    private string log = "";

    [MenuItem("Tools/Ripple Star/Fix SceneTransition TMP Width")]
    public static void ShowWindow()
    {
        GetWindow<FixSceneTransitionTMP>("Fix TMP Width");
    }

    void OnGUI()
    {
        GUILayout.Label("Fix SceneTransition TMP Width", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        GUILayout.Label("Corrige le Width du texte stage dans le prefab\npour eviter que '04-01' passe a la ligne.", EditorStyles.helpBox);
        EditorGUILayout.Space();

        prefabPath = EditorGUILayout.TextField("Chemin du prefab", prefabPath);
        EditorGUILayout.Space();

        useAutoSize = EditorGUILayout.Toggle("Utiliser Auto Size", useAutoSize);

        if (!useAutoSize)
            newWidth = EditorGUILayout.FloatField("Nouvelle Width", newWidth);

        EditorGUILayout.Space();

        if (GUILayout.Button("🔧 Lancer le Fix", GUILayout.Height(40)))
        {
            FixPrefab();
        }

        EditorGUILayout.Space();
        if (!string.IsNullOrEmpty(log))
        {
            GUILayout.Label("Log :");
            EditorGUILayout.HelpBox(log, MessageType.Info);
        }
    }

    void FixPrefab()
    {
        log = "";

        // Charge le prefab
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab == null)
        {
            log = "❌ Prefab introuvable au chemin : " + prefabPath;
            Repaint();
            return;
        }

        // Ouvre le prefab pour edition
        string prefabAssetPath = AssetDatabase.GetAssetPath(prefab);
        GameObject prefabContents = PrefabUtility.LoadPrefabContents(prefabAssetPath);

        if (prefabContents == null)
        {
            log = "❌ Impossible d'ouvrir le prefab en edition.";
            Repaint();
            return;
        }

        int count = 0;

        // Cherche tous les TMP dans le prefab
        TextMeshProUGUI[] allTMP = prefabContents.GetComponentsInChildren<TextMeshProUGUI>(true);

        foreach (var tmp in allTMP)
        {
            RectTransform rt = tmp.GetComponent<RectTransform>();
            if (rt == null) continue;

            if (useAutoSize)
            {
                // Active Auto Size
                tmp.enableAutoSizing = true;
                tmp.fontSizeMin = 10;
                tmp.fontSizeMax = 72;
                log += $"✅ Auto Size active sur : {tmp.gameObject.name}\n";
            }
            else
            {
                // Augmente la Width
                Vector2 size = rt.sizeDelta;
                size.x = newWidth;
                rt.sizeDelta = size;

                // Desactive le wrapping pour eviter le passage a la ligne
                tmp.enableWordWrapping = false;

                log += $"✅ Width = {newWidth} sur : {tmp.gameObject.name}\n";
            }

            count++;
        }

        // Sauvegarde le prefab
        PrefabUtility.SaveAsPrefabAsset(prefabContents, prefabAssetPath);
        PrefabUtility.UnloadPrefabContents(prefabContents);

        log += $"\n✅ Termine ! {count} TMP corriges dans le prefab.";
        AssetDatabase.Refresh();
        Repaint();
    }
}
