using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Gestionnaire des vies du joueur — barres de vie, game over, retour hub
/// </summary>
public class LifeBarManager : MonoBehaviour
{
    public GameObject ImageLife01;
    public GameObject ImageLife03;
    public string sceneGameOver;
    public int reset = 3;

    private Image imageLife01Component;
    private Image imageLife03Component;
    private int lastReset = -1;

    public string sceneSpaceHub = "SpaceHub";

    void Start()
    {
        imageLife01Component = ImageLife01.GetComponent<Image>();
        imageLife03Component = ImageLife03.GetComponent<Image>();
        getLifeBar();
    }

    void Update()
    {
        // Optimisation : ne fait rien si reset n'a pas changé
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

    // ─── GAME OVER / RETOUR HUB ──────────────────────────────────────
    public void reset00()
    {
        resetPrefsLifeBar();

        // ─── MODE EXPLORATION — retour au SpaceHub ───────────────────
        if (GameStateManager.Instance.IsExplorationMode)
        {
            GameStateManager.Instance.SetMode("Arcade");
            GameStateManager.Instance.SetCurrentStage(0);
            SceneManager.LoadScene(sceneSpaceHub);
        }
        // ─── AUTRE MODE — écran game over ────────────────────────────
        else
        {
            SceneManager.LoadScene(sceneGameOver);
        }
    }

public void Reset()
{
    reset = reset - 1;
    Debug.Log($"[LifeBar] Reset() appelé — reset passe à {reset}");
    setLifeBar();
    Scene scene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(scene.name);
}

public void setLifeBar()
{
    PlayerPrefs.SetInt("LifeBar", reset);
    Debug.Log($"[LifeBar] setLifeBar() — PlayerPrefs['LifeBar'] = {reset}");
}

public void getLifeBar()
{
    reset = PlayerPrefs.GetInt("LifeBar", reset);
    Debug.Log($"[LifeBar] getLifeBar() — lu depuis PlayerPrefs : {reset}");
    GameStateManager.Instance.SetLifeBar(reset);
}

    public void resetPrefsLifeBar()
    {
        reset = 3;
        PlayerPrefs.SetInt("LifeBar", reset);
    }
}