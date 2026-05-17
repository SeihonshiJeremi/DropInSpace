using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeZenManager : MonoBehaviour
{
    public static ModeZenManager Instance;

    private List<string> stages = new List<string>();
    private int stageActuel = 0;
    private bool premierStage = true;

    public int StageActuel => stageActuel;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitStages();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        StartCoroutine(AttendrePuisLancerStage());
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void InitStages()
    {
        stages.Clear();
        string[] planetes = { "01", "02", "03", "04", "05", "06", "07", "08" };
        string[] niveaux = { "01", "02", "03", "04" };
        foreach (var p in planetes)
            foreach (var n in niveaux)
                stages.Add($"Stage{p}-{n}");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SceneTransitionZen")
        {
            if (premierStage) { premierStage = false; return; }

            stageActuel++;
            Debug.Log($"[ModeZenManager] Transition — prochain stage : {(stageActuel < stages.Count ? stages[stageActuel] : "VICTOIRE")}");

            if (stageActuel >= stages.Count)
            {
                StartCoroutine(AttendrePuisVictoire());
                return;
            }

            StartCoroutine(AttendrePuisLancerStage());
            return;
        }

        if (stages.Contains(scene.name))
            Debug.Log($"[ModeZenManager] Stage chargé : {scene.name}");
    }

    IEnumerator AttendrePuisLancerStage()
    {
        yield return new WaitForSeconds(7.5f);
        Debug.Log($"[ModeZenManager] Lancement : {stages[stageActuel]}");
        SceneManager.LoadScene(stages[stageActuel]);
    }

    IEnumerator AttendrePuisVictoire()
    {
        yield return new WaitForSeconds(7.5f);
        int scoreFinal = PlayerPrefs.GetInt("ScoreTemporaire", 0);
        PlayerPrefs.SetInt("ScoreZenFinal", scoreFinal);
        int meilleur = PlayerPrefs.GetInt("MeilleurScoreZen", 0);
        if (scoreFinal > meilleur)
            PlayerPrefs.SetInt("MeilleurScoreZen", scoreFinal);
        PlayerPrefs.Save();
        Debug.Log("[ModeZenManager] VICTOIRE !");
        SceneManager.LoadScene("SceneVictoireZen");
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            stageActuel = stages.Count - 1;
            SceneManager.LoadScene("SceneTransitionZen");
        }

        // F4 — saute directement à SceneVictoireZen
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Debug.Log("[ModeZenManager] DEBUG F4 — SceneVictoireZen !");
            SceneManager.LoadScene("SceneVictoireZen");
        }
    }
#endif
}