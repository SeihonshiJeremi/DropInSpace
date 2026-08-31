using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    // ─── MODES DE JEU ────────────────────────────────────────────────
    public bool IsArcadeMode { get; private set; }
    public bool IsExplorationMode { get; private set; }
    public bool IsInfinityMode { get; private set; }
    public bool IsObscurMode { get; private set; }
    public bool IsZenMode { get; private set; }

    // ─── PROGRESSION SESSION ─────────────────────────────────────────
    public int CurrentScore { get; private set; }
    public int CurrentLifeBar { get; private set; }
    public int CurrentStage { get; private set; }
    public int CurrentPlanet { get; private set; }
    public int ScoreDepart { get; private set; }

    // ─── VICTOIRES SESSION ───────────────────────────────────────────
    public bool VictoryObscur { get; private set; }
    public bool VictoryExploration { get; private set; }

    // ─── CONFIGURATION STAGES ───────────────────────────────────────
    public int NumberOfStages { get; private set; }
    public string[] StageNames { get; private set; }

    void Awake()
    {
        // PROTECTION : si une instance existe déjà, on détruit le NOUVEAU
        if (Instance != null && Instance != this)
        {
            Debug.Log("[GameStateManager] Instance existante détectée — destruction du doublon");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // CRITIQUE : détache du parent AVANT DontDestroyOnLoad
        if (transform.parent != null)
        {
            Debug.Log("[GameStateManager] Détaché du parent pour DontDestroyOnLoad");
            transform.SetParent(null);
        }

        DontDestroyOnLoad(gameObject);

        InitializeStageNames();
        LoadFromPrefs();

        // ─── ABONNEMENT AU CHARGEMENT DE SCÈNE (pour détruire le timer démo) ───
        SceneManager.sceneLoaded += OnSceneLoaded;

        Debug.Log("[GameStateManager] Initialisé et persistant");
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ─── DESTRUCTION DU TIMER DÉMO AU RETOUR MENU ────────────────────
    void OnSceneLoaded(Scene sceneChargee, LoadSceneMode mode)
    {
        if (sceneChargee.name == "StartDropInSpace")
        {
            var loadSceneDemo = FindObjectOfType<loadSceneAutoSpaceHubDemo>();
            if (loadSceneDemo != null)
            {
                Debug.Log("[GameStateManager] Retour à StartDropInSpace — destruction de loadSceneAutoSpaceHubDemo");
                Destroy(loadSceneDemo.gameObject);
            }
        }
    }

    void LoadFromPrefs()
    {
        IsExplorationMode = PlayerPrefs.GetInt("ModeExploration", 0) == 1;
        IsInfinityMode = PlayerPrefs.GetInt("ModeInfiniActif", 0) == 1;
        IsObscurMode = PlayerPrefs.GetInt("ModeObscurActif", 0) == 1;
        IsZenMode = PlayerPrefs.GetInt("ModeZenActif", 0) == 1;
        IsArcadeMode = !IsExplorationMode && !IsInfinityMode && !IsObscurMode && !IsZenMode;

        CurrentScore = PlayerPrefs.GetInt("ScoreTemporaire", 0);
        CurrentLifeBar = PlayerPrefs.GetInt("LifeBar", 3);
        CurrentStage = PlayerPrefs.GetInt("StageEnCours", 0);
        CurrentPlanet = PlayerPrefs.GetInt("PlaneteEnCours", 0);
        ScoreDepart = PlayerPrefs.GetInt("ScoreDepart", 0);

        VictoryObscur = PlayerPrefs.GetInt("VictoireObscur", 0) == 1;
        VictoryExploration = PlayerPrefs.GetInt("VictoireExploration", 0) == 1;

        NumberOfStages = PlayerPrefs.GetInt("NombreStages", 0);
    }

    void InitializeStageNames()
    {
        StageNames = new string[10];
        for (int i = 0; i < 10; i++)
        {
            string key = $"Stage_{i}";
            StageNames[i] = PlayerPrefs.GetString(key, "");
        }
    }

    public string GetCurrentStageName()
    {
        if (CurrentStage >= 0 && CurrentStage < StageNames.Length)
            return StageNames[CurrentStage];
        return "";
    }

    public void SetMode(string mode)
    {
        switch (mode.ToLower())
        {
            case "arcade":
                IsArcadeMode = true;
                IsExplorationMode = IsInfinityMode = IsObscurMode = IsZenMode = false;
                break;
            case "exploration":
                IsExplorationMode = true;
                IsArcadeMode = IsInfinityMode = IsObscurMode = IsZenMode = false;
                break;
            case "infinity":
                IsInfinityMode = true;
                IsArcadeMode = IsExplorationMode = IsObscurMode = IsZenMode = false;
                break;
            case "obscur":
                IsObscurMode = true;
                IsArcadeMode = IsExplorationMode = IsInfinityMode = IsZenMode = false;
                break;
            case "zen":
                IsZenMode = true;
                IsArcadeMode = IsExplorationMode = IsInfinityMode = IsObscurMode = false;
                break;
        }
    }

    public void AddScore(int amount) { CurrentScore += amount; }
    public void SetScore(int score) { CurrentScore = score; }
    public void SetLifeBar(int lives) { CurrentLifeBar = Mathf.Clamp(lives, 0, 3); }
    public void ResetLifeBar() { CurrentLifeBar = 3; }
    public void AdvanceStage() { CurrentStage++; }
    public void SetCurrentStage(int stageIndex) { CurrentStage = stageIndex; }
    public void SetCurrentPlanet(int planetNumber) { CurrentPlanet = planetNumber; }

    public void MarkVictory(string mode)
    {
        switch (mode.ToLower())
        {
            case "obscur": VictoryObscur = true; break;
            case "exploration": VictoryExploration = true; break;
        }
    }

    public void SaveToPrefs()
    {
        PlayerPrefs.SetInt("ScoreTemporaire", CurrentScore);
        PlayerPrefs.SetInt("LifeBar", CurrentLifeBar);
        PlayerPrefs.SetInt("StageEnCours", CurrentStage);
        PlayerPrefs.SetInt("PlaneteEnCours", CurrentPlanet);
        PlayerPrefs.Save();
    }

    public void ResetSession()
    {
        IsArcadeMode = IsExplorationMode = IsInfinityMode = IsObscurMode = IsZenMode = false;
        IsArcadeMode = true;

        CurrentScore = 0;
        CurrentLifeBar = 3;
        CurrentStage = 0;
        CurrentPlanet = 0;
        ScoreDepart = 0;

        VictoryObscur = VictoryExploration = false;

        PlayerPrefs.SetInt("ModeExploration", 0);
        PlayerPrefs.SetInt("ModeInfiniActif", 0);
        PlayerPrefs.SetInt("ModeObscurActif", 0);
        PlayerPrefs.SetInt("ModeZenActif", 0);
        PlayerPrefs.SetInt("ScoreTemporaire", 0);
        PlayerPrefs.SetInt("LifeBar", 3);
        PlayerPrefs.SetInt("StageEnCours", 0);
        PlayerPrefs.SetInt("PlaneteEnCours", 0);
        PlayerPrefs.SetInt("VictoireObscur", 0);
        PlayerPrefs.SetInt("VictoireExploration", 0);
        PlayerPrefs.SetInt("ScoreDepart", 0);
        PlayerPrefs.Save();

        Debug.Log("[GameStateManager] Session réinitialisée");
    }

    public bool IsAnySpecialModeActive()
    {
        return IsInfinityMode || IsObscurMode || IsZenMode;
    }

    public void ClearAllInstances()
    {
        var infinimgr = FindObjectOfType<ModeInfiniManager>();
        var obscurmgr = FindObjectOfType<ModeObscurManager>();
        var zenmgr = FindObjectOfType<ModeZenManager>();

        if (infinimgr != null) Destroy(infinimgr.gameObject);
        if (obscurmgr != null) Destroy(obscurmgr.gameObject);
        if (zenmgr != null) Destroy(zenmgr.gameObject);
    }

    public void CheckVictories()
    {
        if (NumberOfStages > 0 && CurrentStage >= NumberOfStages)
        {
            if (IsExplorationMode)
            {
                VictoryExploration = true;
                PlayerPrefs.SetInt("VictoireExploration", 1);
                PlayerPrefs.Save();
            }
            else if (IsObscurMode)
            {
                VictoryObscur = true;
                PlayerPrefs.SetInt("VictoireObscur", 1);
                PlayerPrefs.Save();
            }
        }
    }
}