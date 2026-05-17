using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeMenuManager : MonoBehaviour
{
    private MainMenuManager mainMenuManager;

    void Start()
    {
        mainMenuManager = Object.FindFirstObjectByType<MainMenuManager>();
    }

    public void OnArcadeClassicClick()
    {
        PlayerPrefs.SetInt("ModeInfiniActif", 0);
        PlayerPrefs.SetInt("ModeObscurActif", 0);
        PlayerPrefs.SetInt("ModeZenActif", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("SceneTransition01-01");
    }

    public void OnInfiniClick()
    {
        PlayerPrefs.SetInt("ModeObscurActif", 0);
        PlayerPrefs.SetInt("ModeZenActif", 0);
        PlayerPrefs.SetInt("ModeInfiniActif", 1);
        PlayerPrefs.SetInt("ScoreDepart", PlayerPrefs.GetInt("ScoreTemporaire", 0));
        PlayerPrefs.Save();
        SceneManager.LoadScene("SceneTransitionInfini");
    }

    public void OnObscurClick()
    {
        PlayerPrefs.SetInt("ModeInfiniActif", 0);
        PlayerPrefs.SetInt("ModeZenActif", 0);
        PlayerPrefs.SetInt("ModeObscurActif", 1);
        PlayerPrefs.SetInt("ScoreDepart", PlayerPrefs.GetInt("ScoreTemporaire", 0));
        PlayerPrefs.Save();
        SceneManager.LoadScene("SceneTransitionDark");
    }

    public void OnZenClick()
    {
        PlayerPrefs.SetInt("ModeInfiniActif", 0);
        PlayerPrefs.SetInt("ModeObscurActif", 0);
        PlayerPrefs.SetInt("ModeZenActif", 1);
        PlayerPrefs.SetInt("ScoreDepart", PlayerPrefs.GetInt("ScoreTemporaire", 0));
        PlayerPrefs.Save();
        SceneManager.LoadScene("SceneTransitionZen");
    }

    public void OnChronoClick()
    {
        // TODO : mode Chrono
        Debug.Log("Chrono clicked");
    }

    public void OnBackClick()
    {
        mainMenuManager.OnBackClick();
    }
}