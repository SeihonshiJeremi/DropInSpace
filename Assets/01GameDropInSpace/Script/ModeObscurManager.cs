using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeObscurManager : MonoBehaviour
{
    public static ModeObscurManager Instance { get; private set; }

    [Header("Scene de victoire")]
    public string sceneVictoire = "SceneVictoireObscur";

    private static readonly string[] tousLesStages = new string[]
    {
        "Stage01-01", "Stage01-02", "Stage01-03", "Stage01-04",
        "Stage02-01", "Stage02-02", "Stage02-03", "Stage02-04",
        "Stage03-01", "Stage03-02", "Stage03-03", "Stage03-04",
        "Stage04-01", "Stage04-02", "Stage04-03", "Stage04-04",
        "Stage05-01", "Stage05-02", "Stage05-03", "Stage05-04",
        "Stage06-01", "Stage06-02", "Stage06-03", "Stage06-04",
        "Stage07-01", "Stage07-02", "Stage07-03", "Stage07-04",
        "Stage08-01", "Stage08-02", "Stage08-03", "Stage08-04"
    };

    private int stageActuel = 0;
    private int scoreCumule = 0;
    private int nombreStages = 0;
    private int meilleurScore = 0;
    private bool premierStage = true;

    private const string PREFS_MEILLEUR = "MeilleurScoreObscur";
    private const string SCENE_GAMEOVER = "SceneTransitionGameOver";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Nettoie les autres modes
        if (ModeInfiniManager.Instance != null)
            Destroy(ModeInfiniManager.Instance.gameObject);

        // Configure l'état global
        GameStateManager.Instance.SetMode("Obscur");
        GameStateManager.Instance.SetScore(0);
        GameStateManager.Instance.SetLifeBar(3);
    }

    void Start()
    {
        meilleurScore = PlayerPrefs.GetInt(PREFS_MEILLEUR, 0);
        PlayerPrefs.SetInt("ModeObscurActif", 1);
        PlayerPrefs.Save();

        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(AttendrePuisLancer());
    }

    private IEnumerator AttendrePuisLancer()
    {
        yield return new WaitForSeconds(7.5f);
        LancerProchainStage();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerPrefs.SetInt("ModeObscurActif", 0);
        PlayerPrefs.Save();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (UnityEngine.InputSystem.Keyboard.current.f2Key.wasPressedThisFrame)
        {
            Debug.Log("[ModeObscurManager] DEBUG F2 — Saut au dernier stage !");
            stageActuel = tousLesStages.Length - 1;
            LancerProchainStage();
        }
#endif
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[ModeObscurManager] Scene chargee : {scene.name} — premierStage={premierStage} stageActuel={stageActuel}");

        if (premierStage) { premierStage = false; return; }

        if (scene.name == "SceneTransitionDark")
        {
            StartCoroutine(AttendrePuisLancerSuivant());
            return;
        }

        if (scene.name == SCENE_GAMEOVER)
            GameOver();
    }

    private IEnumerator AttendrePuisLancerSuivant()
    {
        yield return new WaitForSeconds(7.5f);
        LancerSuivant();
    }

    private void LancerSuivant()
    {
        int scoreFinLevel = PlayerPrefs.GetInt("ScoreFinLevel", 0);
        int scoreDepart = PlayerPrefs.GetInt("ScoreDepart", 0);
        int scoreStage = Mathf.Max(0, scoreFinLevel - scoreDepart);
        scoreCumule += scoreStage;
        nombreStages++;

        Debug.Log($"[ModeObscurManager] LancerSuivant — stageActuel={stageActuel}/{tousLesStages.Length}");

        if (scoreCumule > meilleurScore)
        {
            meilleurScore = scoreCumule;
            PlayerPrefs.SetInt(PREFS_MEILLEUR, meilleurScore);
            PlayerPrefs.Save();
        }

        if (stageActuel >= tousLesStages.Length)
        {
            Debug.Log("[ModeObscurManager] VICTOIRE !");
            Victoire();
            return;
        }

        LancerProchainStage();
    }

    private void LancerProchainStage()
    {
        if (stageActuel >= tousLesStages.Length)
        {
            Victoire();
            return;
        }

        string stage = tousLesStages[stageActuel];
        stageActuel++;

        // Sauvegarde du score cumulé pour la prochaine scène
        GameStateManager.Instance.SetScore(scoreCumule);
        GameStateManager.Instance.SaveToPrefs();
        PlayerPrefs.SetInt("ScoreTemporaire", scoreCumule);
        PlayerPrefs.SetInt("ScoreDepart", scoreCumule);
        PlayerPrefs.SetInt("ModeObscurActif", 1);
        PlayerPrefs.Save();

        Debug.Log($"[ModeObscurManager] Stage {stageActuel}/32 : {stage}");
        SceneManager.LoadScene(stage);
    }

    private void Victoire()
    {
        Debug.Log($"[ModeObscurManager] VICTOIRE ! Score final : {scoreCumule}");
        Time.timeScale = 1f;
        GameStateManager.Instance.MarkVictory("Obscur");
        PlayerPrefs.SetInt("ScoreObscurFinal", scoreCumule);
        PlayerPrefs.SetInt("VictoireObscur", 1);
        PlayerPrefs.SetInt("ModeObscurActif", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(sceneVictoire);
        Destroy(gameObject);
    }

    private void GameOver()
    {
        PlayerPrefs.SetInt("ModeObscurActif", 0);
        GameStateManager.Instance.ResetSession();
        PlayerPrefs.Save();
        Destroy(gameObject);
    }

    public int GetScore() => scoreCumule;
    public int GetStageActuel() => stageActuel;
    public int GetMeilleur() => meilleurScore;
}