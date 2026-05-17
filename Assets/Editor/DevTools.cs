#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DevTools
{
    // Ctrl+Alt+R — Reset prefs + lance StartDropInSpace
    [MenuItem("DevTools/Reset Prefs + Lancer Menu %&r")]
    static void ResetEtLancer()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("[DevTools] PlayerPrefs effacés !");
        LancerMenu();
    }

    // Ctrl+Alt+L — Lance StartDropInSpace sans reset
    [MenuItem("DevTools/Lancer Menu (sans reset) %&l")]
    static void LancerMenuSansReset()
    {
        LancerMenu();
    }

    static void LancerMenu()
    {
        if (EditorApplication.isPlaying)
            EditorApplication.isPlaying = false;

        EditorApplication.delayCall += () =>
        {
            EditorSceneManager.OpenScene("Assets/01GameDropInSpace/SceneDIP/StartDropInSpace.unity");
            EditorApplication.isPlaying = true;
            Debug.Log("[DevTools] StartDropInSpace lancé !");
        };
    }
}
#endif