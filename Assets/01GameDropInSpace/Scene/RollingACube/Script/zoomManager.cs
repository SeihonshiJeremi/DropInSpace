using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class zoomManager : MonoBehaviour
{
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

    AudioSource m_MyAudioSource;
    public AudioSource m_MyAudioSourceBonbon;
    public AudioClip audioStage;
    public AudioClip clipNoTime;
    public bool chAudio = false;

    public bool itemAudio = false;
    public bool itemAudioBonbon = false;
    public AudioClip ItemTimeDiv2;
    public AudioClip ItemStop;
    public AudioClip ItemInvincibility;
    public AudioClip ItemBonbon;

    private string scoreFinLevel = "ScoreFinLevel";
    private string scoreTemporaire = "ScoreTemporaire";

    public bool transitionExploration;
    public string sceneTransitionExploration;
    public int valeurScene;

    private LifeBarManager lifeBarManager;
    private bool modeZen = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();

        // ─── CHARGEMENT DEPUIS GAMESTATEMANAGER ───────────────────────
        score = GameStateManager.Instance.CurrentScore;
        durationItem = 0.0f;
        modeZen = GameStateManager.Instance.IsZenMode;
        transitionExploration = GameStateManager.Instance.IsExplorationMode;
        valeurScene = PlayerPrefs.GetInt("ValeurScene", 0);

        if (modeZen)
        {
            GameObject lifeBarObj = GameObject.Find("LifeBar");
            if (lifeBarObj != null)
                lifeBarObj.SetActive(false);

            try
            {
                foreach (GameObject monstre in GameObject.FindGameObjectsWithTag("Monster"))
                    monstre.SetActive(false);
            }
            catch { }

            try
            {
                foreach (GameObject monstre in GameObject.FindGameObjectsWithTag("Monster02"))
                    monstre.SetActive(false);
            }
            catch { }

            try
            {
                foreach (GameObject monstre in GameObject.FindGameObjectsWithTag("MonsterChangeColor"))
                    monstre.SetActive(false);
            }
            catch { }

            chAudio = true;
        }
        else
        {
            GameObject lifeBarObj = GameObject.Find("LifeBar");
            if (lifeBarObj != null)
                lifeBarManager = lifeBarObj.GetComponent<LifeBarManager>();
        }
    }

    void Update()
    {
        durationItem -= Time.deltaTime;

        PlayerPrefs.SetInt(scoreFinLevel, score);
        PlayerPrefs.SetInt(scoreTemporaire, score);

        if (itemAudio && durationItem < 0)
        {
            audioOrigine();
            itemAudio = false;
            if (chAudio) changementAudio();
        }

        if (itemAudioBonbon && durationItem < 0)
        {
            m_MyAudioSourceBonbon.Stop();
            itemAudioBonbon = false;
        }

        if (durationItem < 0) durationItem = durationItemOrigine;

        timerNiveau -= Time.deltaTime;
        tempsint = Mathf.RoundToInt(timerNiveau);
        text_timer.text = " " + tempsint;

        if (!modeZen)
        {
            if (timerNiveau < 1 && lifeBarManager != null)
                lifeBarManager.Reset();

            if (timerNiveau <= 20 && !chAudio)
            {
                changementAudio();
                chAudio = true;
            }
        }

        text_finirLevel.text = " " + valueFinLevel;
        text_score.text = " " + FormatScore(score);

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
            if (GameStateManager.Instance.IsInfinityMode)
                return;

            if (GameStateManager.Instance.IsObscurMode)
            {
                SceneManager.LoadScene("SceneTransitionDark");
                return;
            }

            if (modeZen)
            {
                SceneManager.LoadScene("SceneTransitionZen");
                return;
            }

            valeurScene = valeurScene + 1;
            PlayerPrefs.SetInt("ValeurScene", valeurScene);
            GameStateManager.Instance.AdvanceStage();
            GameStateManager.Instance.SaveToPrefs();
            SceneManager.LoadScene(scene);
        }

        if (transitionExploration && valueFinLevel == 0)
        {
            GameStateManager.Instance.AdvanceStage();
            GameStateManager.Instance.SaveToPrefs();
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
        activDoubleScore = 0;
    }

    public void ajoutScoreBonbonRouge() { score += 200; }
    public void ajoutScoreBonbonVert() { score += 100; }

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

    private string FormatScore(int score)
    {
        if (score >= 1000000)
            return (score / 1000000f).ToString("0.#") + "M";
        if (score >= 10000)
            return (score / 1000f).ToString("0.#") + "K";
        return score.ToString();
    }
}