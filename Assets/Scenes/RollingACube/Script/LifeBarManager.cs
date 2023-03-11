using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LifeBarManager : MonoBehaviour
{
    public GameObject ImageLife01;
    public GameObject ImageLife03;
    public string sceneGameOver;
    public int reset = 3;
    string LifeBar;
    GameObject lifeBar;

    void Start()
    {
       // DontDestroyOnLoad(this);
        lifeBar = GameObject.FindWithTag("lifeBar");
        getLifeBar();
    }

    void Update()
    {
        if (reset == 2) reset02();
        if (reset == 1) reset01();
        if (reset == 0) reset00();
    }

    public void reset02()
    {
        ImageLife01.GetComponent<Image>().color = new Color32(44,44,44,255);
        setLifeBar();

    }

    public void reset01()
    {
        ImageLife03.GetComponent<Image>().color = new Color32(44, 44, 44, 255);
        ImageLife01.GetComponent<Image>().color = new Color32(44, 44, 44, 255);
        setLifeBar();
    }

    public void reset00()
    {
        resetPrefsLifeBar();
        SceneManager.LoadScene(sceneGameOver);
    }

    public void Reset()
    {
        reset = reset - 1;
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    public void setLifeBar()
    {
        PlayerPrefs.SetInt("LifeBar", reset);
    }

    public void getLifeBar()
    {
        reset = PlayerPrefs.GetInt("LifeBar", reset);
    }

    public void resetPrefsLifeBar()
    {
        reset = 3;
        PlayerPrefs.SetInt("LifeBar", reset);
    }
}
