using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class zoomManager : MonoBehaviour
{
    // CORRIGÉ : singleton pour éviter GameObject.Find dans chaque cube
    public static zoomManager Instance;

    public float durationItem;
    public float durationItemOrigine;
    public int valueFinLevel;
    [SerializeField] TextMeshProUGUI text_finirLevel;
    [SerializeField] TextMeshProUGUI text_timer;
    public float timerNiveau;
    private int tempsint;
    [SerializeField] TextMeshProUGUI text_score;
    public int score;
    public int scoreSetIn;
    public bool doubleScore = false;
    public int activDoubleScore;
    public float timerStartOrigine;
    public float timerStart;
    public GameObject ObjectTimerStart;
    public string scene;

    // Audio
    AudioSource m_MyAudioSource;
    public AudioSource m_MyAudioSourceBonbon;
    public AudioClip audioStage;
    public AudioClip clipNoTime;
    public bool chAudio = false;

    // Pour audio power
    public bool itemAudio = false;
    public bool itemAudioBonbon = false;
    public AudioClip ItemTimeDiv2;
    public AudioClip ItemStop;
    public AudioClip ItemInvincibility;
    public AudioClip ItemBonbon;

    // Clés PlayerPrefs — CORRIGÉ : initialisées avec un nom valide
    private string scoreFinLevel = "ScoreFinLevel";
    private string scoreTemporaire = "ScoreTemporaire";

    // Pour transition exploration
    public bool transitionExploration;
    public string sceneTransitionExploration;
    public int valeurScene;

    // Cache — CORRIGÉ : GetComponent et Find cachés
    private LifeBarManager lifeBarManager;

    void Awake()
    {
        // CORRIGÉ : Awake garantit que Instance est prêt avant tous les Start()
        Instance = this;
    }

    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();
        score = PlayerPrefs.GetInt(scoreTemporaire, 0);
        durationItem = 0.0f;
        valeurScene = PlayerPrefs.GetInt("ValeurScene", valeurScene);

        // AJOUTÉ : détecte automatiquement si on vient du mode Exploration
        if (PlayerPrefs.GetInt("ModeExploration", 0) == 1)
            transitionExploration = true;

        // cache LifeBarManager une fois
        GameObject lifeBarObj = GameObject.Find("LifeBar");
        if (lifeBarObj != null)
            lifeBarManager = lifeBarObj.GetComponent<LifeBarManager>();
    }

    void Update()
    {
        // CORRIGÉ : timer et UI dans Update, pas FixedUpdate
        durationItem -= Time.deltaTime;

        // Sauvegarde score
        PlayerPrefs.SetInt(scoreFinLevel, score);
        PlayerPrefs.SetInt(scoreTemporaire, score);

        // Timer item audio — CORRIGÉ : décrémenté une seule fois
        if (itemAudio && durationItem < 0)
        {
            audioOrigine();
            itemAudio = false;
            if (chAudio) changementAudio();
        }

        // Audio bonbon
        if (itemAudioBonbon && durationItem < 0)
        {
            m_MyAudioSourceBonbon.Stop();
            itemAudioBonbon = false;
        }

        // Reset durationItem
        if (durationItem < 0) durationItem = durationItemOrigine;

        // Timer niveau
        timerNiveau -= Time.deltaTime;
        tempsint = Mathf.RoundToInt(timerNiveau);
        text_timer.text = " " + tempsint;

        // CORRIGÉ : utilise le cache lifeBarManager
        if (timerNiveau < 1 && lifeBarManager != null)
            lifeBarManager.Reset();

        if (timerNiveau <= 20 && !chAudio)
        {
            changementAudio();
            chAudio = true;
        }

        // UI
        text_finirLevel.text = " " + valueFinLevel;
        text_score.text = " " + score;

        if (activDoubleScore >= 1) doubleScore = true;
        else doubleScore = false;
    }

    public void finLevel(int finLevelToAdd)
    {
        valueFinLevel += finLevelToAdd;
    }

    public void reduceFinLevel(int finLevelToAdd)
    {
        valueFinLevel -= finLevelToAdd;

        if (valueFinLevel == 0 && !transitionExploration)
        {
            valeurScene = valeurScene + 1;
            PlayerPrefs.SetInt("ValeurScene", valeurScene);
            SceneManager.LoadScene(scene);
        }

        if (transitionExploration && valueFinLevel == 0)
        {
            // AJOUTÉ : gère l'enchaînement des stages ou retour au SpaceHub
            PlaneteExploration.PasserStageExplorationSuivant(sceneTransitionExploration);
        }
    }

    public void addFinLevel(int finLevelToAdd)
    {
        valueFinLevel += finLevelToAdd;
    }

    public void scoreAdd(int scoreToAdd)
    {
        score += scoreToAdd;
        if (doubleScore) score += scoreToAdd * 2;
    }

    public void scoreDoubleTrue(int activDoubleScoreToAdd)
    {
        activDoubleScore += activDoubleScoreToAdd;
    }

    public void scoreDoubleFalse(int activDoubleScoreToReduce)
    {
        // CORRIGÉ : suppression Debug.Log, écriture claire
        activDoubleScore = 0;
    }

    public void ajoutScoreBonbonRouge()
    {
        score += 200;
    }

    public void ajoutScoreBonbonVert()
    {
        score += 100;
    }

    public void audioOrigine()
    {
        m_MyAudioSource.clip = audioStage;
        m_MyAudioSource.Play();
    }

    public void changementAudio()
    {
        m_MyAudioSource.clip = clipNoTime;
        m_MyAudioSource.Play();
    }

    public void audioItemTimeDiv2()
    {
        m_MyAudioSource.clip = ItemTimeDiv2;
        m_MyAudioSource.Play();
        itemAudio = true;
        durationItem = 10.0f;
    }

    public void audioItemTStop()
    {
        m_MyAudioSource.clip = ItemStop;
        m_MyAudioSource.Play();
        itemAudio = true;
        durationItem = 10.0f;
    }

    public void audioItemTInvincibility()
    {
        m_MyAudioSource.clip = ItemInvincibility;
        m_MyAudioSource.Play();
        itemAudio = true;
        durationItem = 10.0f;
    }

    public void audioItemTBonbon()
    {
        m_MyAudioSourceBonbon.clip = ItemBonbon;
        m_MyAudioSourceBonbon.Play();
        itemAudioBonbon = true;
        durationItem = 0.1f;
    }

    public void TransitionExploration()
    {
        if (valeurScene == 103) transitionExploration = true;
    }
}