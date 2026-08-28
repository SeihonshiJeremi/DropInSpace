using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeMenuManager : MonoBehaviour
{
    public string sceneTransition = "SceneTransition01-01";
    public string sceneTransitionDark = "SceneTransitionDark";
    public string sceneSpaceHub = "SpaceHub";

    // ─── ARCADE CLASSIQUE ────────────────────────────────────────────
    public void OnArcadeClassicClick()
    {
        GameStateManager.Instance.ResetSession();
        GameStateManager.Instance.SetMode("Arcade");

        PlayerPrefs.SetInt("ModeExploration", 0);
        PlayerPrefs.SetInt("ModeInfiniActif", 0);
        PlayerPrefs.SetInt("ModeObscurActif", 0);
        PlayerPrefs.SetInt("ModeZenActif", 0);
        PlayerPrefs.SetInt("ValeurScene", 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneTransition);
    }

    // ─── MODE INFINI ─────────────────────────────────────────────────
    public void OnInfinityModeClick()
    {
        GameStateManager.Instance.ResetSession();
        GameStateManager.Instance.SetMode("Infinity");

        PlayerPrefs.SetInt("ModeInfiniActif", 1);
        PlayerPrefs.SetInt("ModeExploration", 0);
        PlayerPrefs.SetInt("ModeObscurActif", 0);
        PlayerPrefs.SetInt("ModeZenActif", 0);
        PlayerPrefs.Save();

        // Créer ModeInfiniManager s'il n'existe pas (pas dans l'Inspector)
        if (ModeInfiniManager.Instance == null)
        {
            GameObject mgr = new GameObject("ModeInfiniManager");
            ModeInfiniManager infiniMgr = mgr.AddComponent<ModeInfiniManager>();
            Debug.Log("[ArcadeMenu] ModeInfiniManager créé dynamiquement");
        }

        SceneManager.LoadScene(sceneTransition);
    }

    // ─── MODE ZEN ────────────────────────────────────────────────────
    public void OnZenModeClick()
    {
        GameStateManager.Instance.ResetSession();
        GameStateManager.Instance.SetMode("Zen");

        PlayerPrefs.SetInt("ModeZenActif", 1);
        PlayerPrefs.SetInt("ModeExploration", 0);
        PlayerPrefs.SetInt("ModeInfiniActif", 0);
        PlayerPrefs.SetInt("ModeObscurActif", 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene("SceneTransitionZen");
    }

    // ─── MODE OBSCUR ─────────────────────────────────────────────────
    public void OnObscurModeClick()
    {
        GameStateManager.Instance.ResetSession();
        GameStateManager.Instance.SetMode("Obscur");

        PlayerPrefs.SetInt("ModeObscurActif", 1);
        PlayerPrefs.SetInt("ModeExploration", 0);
        PlayerPrefs.SetInt("ModeInfiniActif", 0);
        PlayerPrefs.SetInt("ModeZenActif", 0);
        PlayerPrefs.Save();

        // NE PAS créer ModeObscurManager (existe déjà dans SceneTransitionDark)
        Debug.Log("[ArcadeMenu] ModeObscurManager existe déjà, utilisation de l'instance");

        SceneManager.LoadScene(sceneTransitionDark);
    }

    // ─── MODE EXPLORATION ────────────────────────────────────────────
    public void OnExplorationModeClick()
    {
        GameStateManager.Instance.ResetSession();
        GameStateManager.Instance.SetMode("Exploration");

        PlayerPrefs.SetInt("ModeExploration", 1);
        PlayerPrefs.SetInt("ModeInfiniActif", 0);
        PlayerPrefs.SetInt("ModeObscurActif", 0);
        PlayerPrefs.SetInt("ModeZenActif", 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneSpaceHub);
    }

    // ─── RETOUR MENU PRINCIPAL ───────────────────────────────────────
    public void BackToMainMenu()
    {
        GameStateManager.Instance.ResetSession();
        
        // Nettoyage des managers
        GameStateManager.Instance.ClearAllInstances();
        
        // Charge StartDropInSpace
        SceneManager.LoadScene("StartDropInSpace");
        Debug.Log("[ArcadeMenu] Retour au menu principal");
    }

    // ─── QUITTER LE JEU ──────────────────────────────────────────────
    public void QuitGame()
    {
        GameStateManager.Instance.ResetSession();
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}