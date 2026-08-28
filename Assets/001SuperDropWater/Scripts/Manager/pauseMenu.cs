using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    void Update()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Input.GetButtonDown("Start") && GameIsPaused)
        {
            Resume();
        }
        else if (Input.GetButtonDown("Start")) Pause();

        if (Input.GetButtonDown("Select"))
        {
            QuitGame();
        }
    }


    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
