using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// SCRIPT DE DEBUG UNIQUEMENT — Sera exclu du build Steam automatiquement
/// Pour tester rapidement le mode Exploration sans jouer 8 planètes
/// </summary>
public class DebugExploration : MonoBehaviour
{
    private bool dejaDeclanche = false;

#if UNITY_EDITOR
    // ── SEUL CE BLOC SERA COMPILÉ DANS L'EDITOR ─────────────────────────
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) && !dejaDeclanche)
        {
            dejaDeclanche = true;
            SimulerToutesPlanetesCompletes();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            dejaDeclanche = false;
            ResetExploration();
        }

        // F3 → va directement au stage 02-04 pour tester la fin
        if (Input.GetKeyDown(KeyCode.F3))
        {
            PlayerPrefs.SetInt("ModeExploration", 1);
            PlayerPrefs.SetInt("PlaneteEnCours", 2);
            PlayerPrefs.SetInt("StageEnCours", 3);
            PlayerPrefs.SetInt("NombreStages", 4);
            PlayerPrefs.SetString("Stage_0", "Stage02-01");
            PlayerPrefs.SetString("Stage_1", "Stage02-02");
            PlayerPrefs.SetString("Stage_2", "Stage02-03");
            PlayerPrefs.SetString("Stage_3", "Stage02-04");
            PlayerPrefs.SetInt("ScoreTemporaire", 0);
            PlayerPrefs.SetInt("LifeBar", 3);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Stage02-04");
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 280, 100), "DEBUG EXPLORATION");
        if (GUI.Button(new Rect(20, 35, 260, 25), "F1 - Toutes planètes complètes"))
            SimulerToutesPlanetesCompletes();
        if (GUI.Button(new Rect(20, 65, 260, 25), "F2 - Reset exploration"))
            ResetExploration();
    }
    // ── FIN DU BLOC DEBUG ──────────────────────────────────────────────
#endif

    // Ces méthodes restent accessibles via code si nécessaire
    public void SimulerToutesPlanetesCompletes()
    {
        ExplorationManager em = FindObjectOfType<ExplorationManager>();
        if (em == null)
        {
            Debug.LogWarning("[Debug] ExplorationManager introuvable !");
            return;
        }

        for (int i = 1; i <= 8; i++)
        {
            if (i == 2) continue; // Laisse un monde intact pour test
            em.MarquerPlaneteComplete(i);
        }

        ExplorationUIManager ui = FindObjectOfType<ExplorationUIManager>();
        if (ui != null) ui.MettreAJourUI();

        Debug.Log("TEST : Toutes les planètes simulées comme complètes !");
    }

    public void ResetExploration()
    {
        ExplorationManager em = FindObjectOfType<ExplorationManager>();
        if (em != null) em.ResetExploration();
        Debug.Log("TEST : Exploration remise à zéro !");
    }
}