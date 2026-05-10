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

    // CORRIGÉ : cache des composants Image
    private Image imageLife01Component;
    private Image imageLife03Component;

    // CORRIGÉ : évite d'appeler reset02/01/00 chaque frame
    private int lastReset = -1;

    void Start()
    {
        imageLife01Component = ImageLife01.GetComponent<Image>();
        imageLife03Component = ImageLife03.GetComponent<Image>();
        getLifeBar();
    }

    void Update()
    {
        // CORRIGÉ : ne fait rien si reset n'a pas changé
        if (reset == lastReset) return;
        lastReset = reset;

        if (reset == 2) reset02();
        if (reset == 1) reset01();
        if (reset == 0) reset00();
    }

    public void reset02()
    {
        imageLife01Component.color = new Color32(44, 44, 44, 255);
        setLifeBar();
    }

    public void reset01()
    {
        imageLife03Component.color = new Color32(44, 44, 44, 255);
        imageLife01Component.color = new Color32(44, 44, 44, 255);
        setLifeBar();
    }

    public string sceneSpaceHub = "SpaceHub";

    public void reset00()
    {
        resetPrefsLifeBar();

        // AJOUTÉ : si on est en mode Exploration → retour SpaceHub
        if (PlayerPrefs.GetInt("ModeExploration", 0) == 1)
        {
            PlayerPrefs.SetInt("ModeExploration", 0);
            PlayerPrefs.SetInt("StageEnCours", 0);
            SceneManager.LoadScene(sceneSpaceHub);
        }
        else
        {
            SceneManager.LoadScene(sceneGameOver);
        }
    }

    public void Reset()
    {
        reset = reset - 1;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
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