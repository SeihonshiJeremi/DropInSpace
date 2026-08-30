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

    // Reset complet : clear PlayerPrefs + relance le jeu
    void ResetDemo()
    {
        demoTimerActive = false;

        // --- CLEAR ALL PLAYERPREFS ---
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("[loadSceneAutoSpaceHubDemo] RESET — PlayerPrefs nettoyés, reload scène");

        SceneManager.LoadScene(resetSceneName);

        // Détruit ce GameObject pour qu'un nouveau soit fraîchement créé au prochain lancement
        Destroy(gameObject);
    }

    public void boolScene()
    {
        okChange = true;
    }
}