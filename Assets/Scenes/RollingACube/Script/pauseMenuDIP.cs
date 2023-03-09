using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenuDIP : MonoBehaviour
{
    public bool GameIsPaused;
    public GameObject pauseMenuObject;
    public bool okPause = false;
    public float timer;
    public GameObject EventSystem;
    public AudioSource Audio;
    public Animator anim;

    void Start()
    {
        timer = 2.0f;
        EventSystem.SetActive(false);
        GameIsPaused = false;
        //anim = anim.GetComponent<Animator>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            EventSystem.SetActive(true); okPause = true;
        }
        else
        {
            EventSystem.SetActive(false); okPause = false;
        } 
        if (Input.GetButtonDown("Start") && GameIsPaused == false) Pause();        
        else if (Input.GetButtonDown("Start") && GameIsPaused == true) Resume();
        if(GameIsPaused == true) anim.Play("pauseMenuDIPAnim");
    }

    public void Pause()
    {
        if (okPause == true)
        {
            Time.timeScale = 0f;
            GameIsPaused = true;
            pauseMenuObject.SetActive(true);
            if (timer < 0) Audio.Pause();           
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        pauseMenuObject.SetActive(false);
        if (timer < 0) Audio.Play();
    }



    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
