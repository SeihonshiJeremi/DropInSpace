using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeMenuManager : MonoBehaviour
{
    private MainMenuManager mainMenuManager;

    void Start()
    {
        mainMenuManager = FindObjectOfType<MainMenuManager>();
    }

    public void OnArcadeClassicClick()
    {
        SceneManager.LoadScene("SceneTransition01-01");
    }

    public void OnInfiniClick()
    {
        // TODO : scène ou mode Infini Aléatoire
        Debug.Log("Infini Aléatoire clicked");
    }

    public void OnChronoClick()
    {
        // TODO : scène ou mode Chrono
        Debug.Log("Chrono clicked");
    }

    public void OnObscurClick()
    {
        // TODO : scène ou mode Obscur
        Debug.Log("Obscur clicked");
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