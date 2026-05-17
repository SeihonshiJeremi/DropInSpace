using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Remet tout a zero quand on arrive sur le menu principal.
/// Detruit tous les DontDestroyOnLoad, remet les PlayerPrefs a 0.
/// A placer sur un GameObject dans StartDropInSpace.
/// </summary>
public class ResetJeu : MonoBehaviour
{
    void Awake()
    {
        // Detruit tous les managers de modes speciaux
        if (ModeObscurManager.Instance != null)
            Destroy(ModeObscurManager.Instance.gameObject);
        if (ModeInfiniManager.Instance != null)
            Destroy(ModeInfiniManager.Instance.gameObject);
        if (ModeZenManager.Instance != null)
            Destroy(ModeZenManager.Instance.gameObject);

        // Remet uniquement les flags de session — scores et progression conserves !
        PlayerPrefs.SetInt("ModeObscurActif", 0);
        PlayerPrefs.SetInt("ModeInfiniActif", 0);
        PlayerPrefs.SetInt("ModeZenActif", 0);
        PlayerPrefs.SetInt("ModeExploration", 0);
        PlayerPrefs.SetInt("VictoireObscur", 0);
        PlayerPrefs.SetInt("VictoireExploration", 0);
        PlayerPrefs.SetInt("ScoreDepart", 0);
        PlayerPrefs.SetInt("ScoreTemporaire", 0);
        PlayerPrefs.SetInt("ScoreFinLevel", 0);
        PlayerPrefs.SetInt("ScoreZenFinal", 0);
        PlayerPrefs.SetInt("PlaneteEnCours", 0);
        PlayerPrefs.SetInt("StageEnCours", 0);

        // NE PAS toucher :
        // ExplorationProgress — progression des planetes
        // MeilleurScoreInfiniStages — meilleur score mode Infini
        // MeilleurScoreObscur — meilleur score mode Obscur
        // MeilleurScoreZen — meilleur score mode Zen
        // ScoreArcade (et autres scores) — scores persistants

        PlayerPrefs.Save();

        // Remet le temps a la normale (au cas ou le jeu etait en pause)
        Time.timeScale = 1f;
        Debug.Log("[ResetJeu] Tout remis a zero !");
    }
}