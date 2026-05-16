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
        SceneManager.LoadScene("SceneTransition01-01");
    }

    public void OnInfiniClick()
    {
        SceneManager.LoadScene("SceneTransitionInfini");
    }

    public void OnChronoClick()
    {
        // TODO : scène ou mode Chrono
        Debug.Log("Chrono clicked");
    }

    public void OnObscurClick()
    {
        SceneManager.LoadScene("SceneTransitionDark");
    }

    public void OnZenClick()
    {
        // TODO : scène ou mode Zen
        Debug.Log("Zen clicked");
    }

    public void OnBackClick()
    {
        mainMenuManager.OnBackClick();
    }
}