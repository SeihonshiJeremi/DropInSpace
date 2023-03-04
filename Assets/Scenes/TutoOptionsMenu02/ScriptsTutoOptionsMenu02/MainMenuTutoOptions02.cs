using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuTutoOptions02 : MonoBehaviour
{
    public string scene;
    public string sceneExploration;
    public string sceneScore;

    public GameObject mainMenuButton;
    public GameObject optionsScreen;

    // Start is called before the first frame update
    void Start()
    {
        optionsScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(scene);
    }

    public void ExplorationButton()
    {
        SceneManager.LoadScene(sceneExploration);
    }

    public void ScoreButton()
    {
        SceneManager.LoadScene(sceneScore);
    }

    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
        mainMenuButton.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
        mainMenuButton.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting The Game");
    }
}
