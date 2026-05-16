using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Singleton — cerveau du Mode Infini Aleatoire.
/// Gere le numero de vague, la difficulte croissante, le score cumule et le Game Over.
/// A placer sur un GameObject "InfiniteModeManager" dans Stage01-01Infini.
/// </summary>
public class InfiniteModeManager : MonoBehaviour
{
    public static InfiniteModeManager Instance { get; private set; }

    private bool enJeu = false;
    private bool gameOverDeclenche = false;

    [Header("Vagues")]
    public int numeroVague = 1;

    [Header("Score")]
    public int scoreCumule = 0;
    public int meilleurScore = 0;

    private const string PREFS_MEILLEUR_SCORE = "MeilleurScoreInfini";
    private const string PREFS_VAGUE_MAX = "VagueMaxInfini";
    private const string PREFS_MODE_INFINI = "ModeInfini";

    [Header("Difficulte")]
    public float monsterSpeedBase = 3f;
    public float monsterSpeedParVague = 0.3f;
    public float monsterSpeedMax = 7f;
    public float monsterDelaiBase = 2f;
    public float monsterDelaiParVague = 0.1f;
    public float monsterDelaiMin = 0.5f;

    [Header("References")]
    public GameObject monster01;
    public GameObject monster02;
    public GameObject playerGroupe;

    [Header("UI Game Over")]
    public GameObject panelGameOver;
    public TMP_Text textScoreGameOver;
    public TMP_Text textVagueGameOver;
    public TMP_Text textMeilleurScoreGameOver;
    public string sceneMenu = "StartDropInSpace";

    private monsterZoom monster01Script;
    private monsterZoom monster02Script;
    private InfiniteGridManager gridManager;

    // ══════════════════════════════════════════════════════════════════════
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        meilleurScore = PlayerPrefs.GetInt(PREFS_MEILLEUR_SCORE, 0);
        PlayerPrefs.SetInt(PREFS_MODE_INFINI, 1);

        if (panelGameOver != null) panelGameOver.SetActive(false);

        if (monster01 != null) monster01Script = monster01.GetComponent<monsterZoom>();
        if (monster02 != null) monster02Script = monster02.GetComponent<monsterZoom>();

        gridManager = InfiniteGridManager.Instance;

        // Recupere la vague et le score si on vient d'un rechargement de scene
        int vagueDepart = 1;
        if (PlayerPrefs.GetInt("InfiniEnCours", 0) == 1)
        {
            vagueDepart = PlayerPrefs.GetInt("InfiniVague", 1);
            scoreCumule = PlayerPrefs.GetInt("InfiniScore", 0);
            // Remet le score dans zoomManager
            if (zoomManager.Instance != null)
                zoomManager.Instance.score = scoreCumule;
            PlayerPrefs.SetInt("InfiniEnCours", 0);
            Debug.Log($"[InfiniteModeManager] Reprise vague {vagueDepart} score {scoreCumule}");
        }

        DemarrerVague(vagueDepart);
    }

    // ══════════════════════════════════════════════════════════════════════
    // ETAT
    // ══════════════════════════════════════════════════════════════════════

    public bool EstEnJeu() => enJeu;
    public bool RespawnEnCours() => gridManager != null && gridManager.RespawnEnCours();

    // ══════════════════════════════════════════════════════════════════════
    // VAGUES
    // ══════════════════════════════════════════════════════════════════════

    public void DemarrerVague(int vague)
    {
        numeroVague = vague;
        enJeu = true;
        gameOverDeclenche = false;

        if (monster02 != null)
            monster02.SetActive(numeroVague >= 2);

        AppliquerDifficulte();
        Debug.Log($"[InfiniteModeManager] Vague {numeroVague} demarre !");
    }

    /// <summary>Appelee par InfiniteGridManager quand toutes les dalles sont coloriees.</summary>
    public void NouvelleVague()
    {
        enJeu = false;
        Debug.Log($"[InfiniteModeManager] Vague {numeroVague} terminee !");
        StartCoroutine(TransitionVague());
    }

    private IEnumerator TransitionVague()
    {
        yield return new WaitForSeconds(1.5f);
        if (gridManager != null)
            yield return StartCoroutine(gridManager.RegenerarGrille());
        DemarrerVague(numeroVague + 1);
    }

    // ══════════════════════════════════════════════════════════════════════
    // DIFFICULTE
    // ══════════════════════════════════════════════════════════════════════

    private void AppliquerDifficulte()
    {
        float speed = Mathf.Min(monsterSpeedBase + (numeroVague - 1) * monsterSpeedParVague, monsterSpeedMax);
        float delai = Mathf.Max(monsterDelaiBase - (numeroVague - 1) * monsterDelaiParVague, monsterDelaiMin);

        AppliquerAuMonstre(monster01Script, speed, delai);
        if (numeroVague >= 2 && monster02Script != null)
            AppliquerAuMonstre(monster02Script, speed + 0.5f, delai);

        Debug.Log($"[InfiniteModeManager] Speed: {speed:F1} — Delai: {delai:F1}s");
    }

    private void AppliquerAuMonstre(monsterZoom script, float speed, float delai)
    {
        if (script == null) return;
        var f = typeof(monsterZoom).GetField("_rollSpeed",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (f != null) f.SetValue(script, speed);
        script.tempsAvantDeplacementOrigine = delai;
    }

    // ══════════════════════════════════════════════════════════════════════
    // RESPAWN DROP (tombe dans le vide)
    // ══════════════════════════════════════════════════════════════════════

    /// <summary>Appelee par TeleportationRolling — en mode Infini, tomber = respawn.</summary>
    public void DeclencherGameOver()
    {
        if (gridManager != null)
            StartCoroutine(gridManager.RespawnDrop());
    }

    // ══════════════════════════════════════════════════════════════════════
    // GAME OVER (0 vies)
    // ══════════════════════════════════════════════════════════════════════

    /// <summary>Appelee par LifeBarManager quand reset == 0.</summary>
    public void GameOver()
    {
        if (gameOverDeclenche) return;
        gameOverDeclenche = true;
        enJeu = false;

        if (zoomManager.Instance != null)
            scoreCumule = zoomManager.Instance.score;

        int vagueMax = PlayerPrefs.GetInt(PREFS_VAGUE_MAX, 0);
        if (numeroVague > vagueMax)
        {
            vagueMax = numeroVague;
            PlayerPrefs.SetInt(PREFS_VAGUE_MAX, vagueMax);
        }

        if (scoreCumule > meilleurScore)
        {
            meilleurScore = scoreCumule;
            PlayerPrefs.SetInt(PREFS_MEILLEUR_SCORE, meilleurScore);
        }

        PlayerPrefs.SetInt(PREFS_MODE_INFINI, 0);
        PlayerPrefs.Save();

        Debug.Log($"[InfiniteModeManager] GAME OVER — Vague {numeroVague} — Score {scoreCumule}");
        StartCoroutine(AfficherGameOver(vagueMax));
    }

    private IEnumerator AfficherGameOver(int vagueMax)
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
            if (textScoreGameOver != null) textScoreGameOver.text = "Score : " + scoreCumule;
            if (textVagueGameOver != null) textVagueGameOver.text = "Vague atteinte : " + numeroVague;
            if (textMeilleurScoreGameOver != null) textMeilleurScoreGameOver.text = "Meilleur : " + meilleurScore;
        }
    }

    public void BoutonRejouer()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("LifeBar", 3);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BoutonMenu()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("LifeBar", 3);
        PlayerPrefs.SetInt(PREFS_MODE_INFINI, 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(sceneMenu);
    }
}