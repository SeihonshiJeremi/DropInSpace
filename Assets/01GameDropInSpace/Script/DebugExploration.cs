using UnityEngine;

/// <summary>
/// Script de TEST uniquement — à supprimer avant Steam !
/// Permet de simuler la fin du mode Exploration en un clic.
/// À placer sur n'importe quel GameObject dans le SpaceHub.
/// </summary>
public class DebugExploration : MonoBehaviour
{

    private bool dejaDeclanche = false;

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

        // F3 — va directement au stage 02-04 pour tester la fin
        if (Input.GetKeyDown(KeyCode.F3))
        {
            PlayerPrefs.SetInt("ModeExploration", 1);
            PlayerPrefs.SetInt("PlaneteEnCours", 2);
            PlayerPrefs.SetInt("StageEnCours", 3); // index 3 = 4ème stage
            PlayerPrefs.SetInt("NombreStages", 4);
            PlayerPrefs.SetString("Stage_0", "Stage02-01");
            PlayerPrefs.SetString("Stage_1", "Stage02-02");
            PlayerPrefs.SetString("Stage_2", "Stage02-03");
            PlayerPrefs.SetString("Stage_3", "Stage02-04");
            PlayerPrefs.SetInt("ScoreTemporaire", 0);
            PlayerPrefs.SetInt("LifeBar", 3);
            PlayerPrefs.Save();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Stage02-04");
        }
    }

    public void SimulerToutesPlanetesCompletes()
    {
        if (ExplorationManager.Instance == null)
        {
            Debug.LogWarning("ExplorationManager introuvable !");
            return;
        }

        for (int i = 1; i <= 8; i++)
        {
            if (i == 2) continue; // Laisse le monde 2 pour tester
            ExplorationManager.Instance.MarquerPlaneteComplete(i);
        }

        // Met à jour l'UI
        if (ExplorationUIManager.Instance != null)
            ExplorationUIManager.Instance.MettreAJourUI();

        Debug.Log("TEST : Toutes les planètes marquées comme complètes !");
    }

    public void SimulerPlaneteComplete(int numero)
    {
        if (ExplorationManager.Instance == null) return;
        ExplorationManager.Instance.MarquerPlaneteComplete(numero);
        if (ExplorationUIManager.Instance != null)
            ExplorationUIManager.Instance.MettreAJourUI();
        Debug.Log($"TEST : Planète {numero} marquée comme complète !");
    }

    public void ResetExploration()
    {
        if (ExplorationManager.Instance == null) return;
        ExplorationManager.Instance.ResetExploration();
        if (ExplorationUIManager.Instance != null)
            ExplorationUIManager.Instance.MettreAJourUI();
        Debug.Log("TEST : Exploration remise à zéro !");
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 280, 100), "DEBUG EXPLORATION");
        if (GUI.Button(new Rect(20, 35, 260, 25), "F1 - Toutes planètes complètes"))
            SimulerToutesPlanetesCompletes();
        if (GUI.Button(new Rect(20, 65, 260, 25), "F2 - Reset exploration"))
            ResetExploration();
    }
}