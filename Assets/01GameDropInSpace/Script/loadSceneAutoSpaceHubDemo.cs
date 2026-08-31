using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadSceneAutoSpaceHubDemo : MonoBehaviour
{
    [Header("Scene à charger (bouton/trigger)")]
    public string scene;
    public float loadSceneTime;
    public bool okChange = false;

    [Header("Timer Démo Exploration")]
    public float demoDuration = 180f; // 3 minutes
    private float demoTimer;
    private bool demoTimerActive = false;

    [Header("Reset")]
    public string resetSceneName = "StartDropInSpace";

    private static loadSceneAutoSpaceHubDemo instance;

    void Awake()
    {
        // --- DONT DESTROY ON LOAD ---
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // --- TIMER DÉMARRE TOUT SEUL ---
        demoTimer = demoDuration;
        demoTimerActive = true;
        Debug.Log("[loadSceneAutoSpaceHubDemo] Timer démo démarré : " + demoDuration + " sec");
    }

    void Update()
    {
        // --- TIMER DÉMO ---
        if (demoTimerActive)
        {
            demoTimer -= Time.deltaTime;

            if (demoTimer <= 0f)
            {
                ResetDemo();
            }
        }

        // --- CHARGEMENT DE SCÈNE (existant) ---
        if (okChange == true)
        {
            loadSceneTime -= Time.deltaTime;
            if (loadSceneTime < 0)
            {
                SceneManager.LoadScene(scene);
            }
        }
    }

    // Reset complet : clear des préférences de SESSION uniquement
    // (on préserve les réglages audio du joueur : Volume, Musique, Effets)
    void ResetDemo()
    {
        demoTimerActive = false;

        // --- CLEAR PLAYERPREFS CIBLÉ (session uniquement) ---
        PlayerPrefs.DeleteKey("ModeExploration");
        PlayerPrefs.DeleteKey("ModeInfiniActif");
        PlayerPrefs.DeleteKey("ModeObscurActif");
        PlayerPrefs.DeleteKey("ModeZenActif");
        PlayerPrefs.DeleteKey("ValeurScene");
        PlayerPrefs.Save();

        Debug.Log("[loadSceneAutoSpaceHubDemo] RESET — PlayerPrefs session nettoyés "
                  + "(préférences audio conservées), reload scène");

        // Le GameStateManager détruira cet objet au chargement de StartDropInSpace
        SceneManager.LoadScene(resetSceneName);
    }

    public void boolScene()
    {
        okChange = true;
    }
}