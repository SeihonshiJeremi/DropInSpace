using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeInfiniManager : MonoBehaviour
{
    public static ModeInfiniManager Instance { get; private set; }

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

    private List<string> stagesRestants = new List<string>();
    private int scoreCumule = 0;
    private int nombreStages = 0;
    private int meilleurScore = 0;

    private const string PREFS_MEILLEUR = "MeilleurScoreInfiniStages";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        meilleurScore = PlayerPrefs.GetInt(PREFS_MEILLEUR, 0);
        PlayerPrefs.SetInt("ModeInfiniActif", 1);
        PlayerPrefs.Save();

        SceneManager.sceneLoaded += OnSceneLoaded;
        NouvelleRotation();
        LancerProchainStage();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (UnityEngine.InputSystem.Keyboard.current.f1Key.wasPressedThisFrame)
        {
            Debug.Log("[ModeInfiniManager] DEBUG F1 — Saut au dernier stage !");
            while (stagesRestants.Count > 1)
                stagesRestants.RemoveAt(0);
            LancerProchainStage();
        }
#endif
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerPrefs.SetInt("ModeInfiniActif", 0);
        PlayerPrefs.Save();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[ModeInfiniManager] Scene chargee : {scene.name}");

        // Dans un stage — surveille la fin via valueFinLevel
        if (!scene.name.StartsWith("SceneTransition"))
            StartCoroutine(SurveillerFinStage());
    }

    private IEnumerator SurveillerFinStage()
    {
        // Attend que zoomManager soit initialise
        yield return new WaitForSeconds(0.5f);
        if (zoomManager.Instance == null) yield break;

        // Attend que valueFinLevel soit rempli
        while (zoomManager.Instance != null && zoomManager.Instance.valueFinLevel <= 0)
            yield return new WaitForSeconds(0.1f);

        Debug.Log($"[ModeInfiniManager] Stage demarre — {zoomManager.Instance.valueFinLevel} dalles");

        // Surveille jusqu'a valueFinLevel == 0
        while (zoomManager.Instance != null && zoomManager.Instance.valueFinLevel > 0)
            yield return new WaitForSeconds(0.1f);

        if (zoomManager.Instance == null) yield break;

        // Recupere le score
        int scoreFinLevel = PlayerPrefs.GetInt("ScoreFinLevel", 0);
        int scoreDepart = PlayerPrefs.GetInt("ScoreDepart", 0);
        int scoreStage = Mathf.Max(0, scoreFinLevel - scoreDepart);
        scoreCumule += scoreStage;
        nombreStages++;

        Debug.Log($"[ModeInfiniManager] Stage {nombreStages} termine ! +{scoreStage} — Total: {scoreCumule}");

        if (scoreCumule > meilleurScore)
        {
            meilleurScore = scoreCumule;
            PlayerPrefs.SetInt(PREFS_MEILLEUR, meilleurScore);
        }

        yield return new WaitForSeconds(0.5f);
        LancerProchainStage();
    }

    private void NouvelleRotation()
    {
        stagesRestants = new List<string>(tousLesStages);
        Melanger(stagesRestants);
        Debug.Log($"[ModeInfiniManager] Nouvelle rotation — {stagesRestants.Count} stages");
    }

    private void LancerProchainStage()
    {
        if (stagesRestants.Count == 0) NouvelleRotation();

        string prochainStage = stagesRestants[0];
        stagesRestants.RemoveAt(0);

        PlayerPrefs.SetInt("ScoreTemporaire", scoreCumule);
        PlayerPrefs.SetInt("ScoreDepart", scoreCumule);
        PlayerPrefs.SetInt("ModeInfiniActif", 1);
        PlayerPrefs.Save();

        Debug.Log($"[ModeInfiniManager] Lancement : {prochainStage} ({stagesRestants.Count} restants)");
        SceneManager.LoadScene(prochainStage);
    }

    public int GetScore() => scoreCumule;
    public int GetNombreStages() => nombreStages;
    public int GetMeilleur() => meilleurScore;

    private void Melanger(List<string> liste)
    {
        for (int i = liste.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (liste[i], liste[j]) = (liste[j], liste[i]);
        }
    }
}