using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LifeBarManager : MonoBehaviour
{
    public GameObject CanvasLifeBar;
    public bool activationBarLife;
    public GameObject ImageLife01;
    public GameObject ImageLife03;
    public string sceneGameOver;
    public int reset = 3;
    GameObject lifeBar;

    void Awake()
    {
        DontDestroyOnLoad(this);
        lifeBar = GameObject.FindWithTag("lifeBar");
    }

    void Update()
    {
        if (activationBarLife == true) CanvasLifeBar.GetComponent<Canvas>().enabled = true;
        if (activationBarLife == false) CanvasLifeBar.GetComponent<Canvas>().enabled = false;
    }

    public void desactivationLifeBar()
    {
        activationBarLife = false;        
    }

    public void activationLifeBar()
    {
        activationBarLife = true;        
    }
    public void ResetLifeBar()
    {
        reset = 3;
    }

    public void reset02()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        ImageLife01.GetComponent<Image>().color = new Color32(44,44,44,255);
    }

    public void reset01()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        ImageLife03.GetComponent<Image>().color = new Color32(44, 44, 44, 255);
    }

    public void reset00()
    {
        SceneManager.LoadScene(sceneGameOver);
        Destroy(lifeBar);
    }

    public void Reset()
    {
        reset = reset - 1;
        if (reset == 2) reset02();
        if (reset == 1) reset01();
        if (reset == 0) reset00();
    }

    public void DetruitLifeBar()
    {
        Destroy(lifeBar);
    }
}
