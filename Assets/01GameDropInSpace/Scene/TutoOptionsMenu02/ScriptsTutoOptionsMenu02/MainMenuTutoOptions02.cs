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
    public float timer;
    public GameObject EventSystem;

    void Awake()
    {
        timer = 2.0f;
        EventSystem.SetActive(false);
    }

    void Start()
    {
        optionsScreen.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer < 0) EventSystem.SetActive(true);
        else EventSystem.SetActive(false);
    }

    public void StartGame()
    {
        // Mode Arcade — remet ModeExploration a 0
        PlayerPrefs.SetInt("ModeExploration", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(scene);
    }

    public void ExplorationButton()
    {
        SceneManager.LoadScene(sceneExploration);
    }

    public void ScoreButton()
    {
        SceneManager.LoadSceneAsync(sceneScore);
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
