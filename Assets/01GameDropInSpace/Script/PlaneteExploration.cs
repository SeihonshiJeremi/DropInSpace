using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// À placer sur chaque planète du SpaceHub.
/// Gère l'atterrissage et la détection de complétion.
/// </summary>
public class PlaneteExploration : MonoBehaviour
{
    [Header("Identité")]
    [Range(1, 8)]
    public int numeroPlanete = 1;           // 1 à 8

    [Header("Stages de cette planète")]
    public string[] nomStages = new string[4]; // Les 4 scènes de transition ex: "SceneTransition01-01"

    [Header("Visuel")]
    public GameObject indicateurComplete;   // Objet affiché quand planète complète (étoile, halo...)
    public GameObject indicateurSurvol;     // Affiché quand Drop survole la planète

    private bool estComplete = false;

    void Start()
    {
        MettreAJourVisuel();
        if (indicateurSurvol != null) indicateurSurvol.SetActive(false);
    }

    void MettreAJourVisuel()
    {
        if (ExplorationManager.Instance != null)
            estComplete = ExplorationManager.Instance.EstPlaneteComplete(numeroPlanete);

        if (indicateurComplete != null)
            indicateurComplete.SetActive(estComplete);
    }

    // Appelé quand Drop entre dans la zone de la planète
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (indicateurSurvol != null) indicateurSurvol.SetActive(true);

        PlayerPrefs.SetInt("ModeExploration", 1);
        PlayerPrefs.SetInt("PlaneteEnCours", numeroPlanete);
        PlayerPrefs.SetInt("StageEnCours", 0);
        PlayerPrefs.SetInt("NombreStages", nomStages.Length);
        PlayerPrefs.SetInt("ScoreTemporaire", 0);
        PlayerPrefs.SetInt("LifeBar", 3);

        // AJOUTÉ : sauvegarde les noms de tous les stages dans PlayerPrefs
        for (int i = 0; i < nomStages.Length; i++)
            PlayerPrefs.SetString("Stage_" + i, nomStages[i]);

        PlayerPrefs.Save();
    }

    // Appelé depuis zoomManager quand un stage est terminé en mode Exploration
    // Au lieu de retourner au SpaceHub, charge le stage suivant de la planète
    public static void PasserStageExplorationSuivant(string sceneTransitionExploHub)
    {
        int stageIndex = PlayerPrefs.GetInt("StageEnCours", 0) + 1;
        int nombreStages = PlayerPrefs.GetInt("NombreStages", 4);

        PlayerPrefs.SetInt("StageEnCours", stageIndex);

        // Il reste des stages sur cette planète
        if (stageIndex < nombreStages)
        {
            string prochainStage = PlayerPrefs.GetString("Stage_" + stageIndex, "");
            if (!string.IsNullOrEmpty(prochainStage))
            {
                SceneManager.LoadScene(prochainStage);
                return;
            }
        }

        // Tous les stages sont finis
        PlayerPrefs.SetInt("ModeExploration", 0);

        // CORRIGÉ : NotifierPlaneteComplete décide elle-même quelle scène charger
        // Si c'est la dernière planète → SceneTransitionEndGame
        // Sinon → SpaceHub
        bool victoireDeclenche = NotifierPlaneteCompleteAvecRetour(sceneTransitionExploHub);

        if (!victoireDeclenche)
        {
            PlayerPrefs.SetInt("ValeurScene", 200);
            PlayerPrefs.Save();
            SceneManager.LoadScene(sceneTransitionExploHub);
        }
    }

    private static bool NotifierPlaneteCompleteAvecRetour(string sceneSpaceHub)
    {
        int planete = PlayerPrefs.GetInt("PlaneteEnCours", 0);
        if (planete <= 0) return false;

        string data = PlayerPrefs.GetString("ExplorationProgress", "00000000");
        char[] chars = data.ToCharArray();
        int index = planete - 1;

        if (index >= 0 && index < 8 && chars[index] == '0')
        {
            chars[index] = '1';
            string newData = new string(chars);
            PlayerPrefs.SetString("ExplorationProgress", newData);
            PlayerPrefs.Save();

            int total = 0;
            foreach (char c in chars) if (c == '1') total++;

            Debug.Log("Planète " + planete + " complète ! Total : " + total + "/8");

            if (total >= 8)
            {
                PlayerPrefs.SetInt("VictoireExploration", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("SceneTransitionEndGame");
                return true; // victoire déclenchée — ne pas charger le SpaceHub
            }
        }
        return false;
    }

    // Appelé quand Drop quitte la zone
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (indicateurSurvol != null) indicateurSurvol.SetActive(false);
    }

    // Appelé pour atterrir sur la planète (lier à un bouton ou double-trigger)
    public void Atterrir()
    {
        if (nomStages.Length == 0 || string.IsNullOrEmpty(nomStages[0])) return;

        PlayerPrefs.SetInt("PlaneteEnCours", numeroPlanete);
        PlayerPrefs.SetInt("ScoreTemporaire", 0);
        PlayerPrefs.SetInt("LifeBar", 3);
        // AJOUTÉ : marque qu'on est en mode Exploration
        PlayerPrefs.SetInt("ModeExploration", 1);
        SceneManager.LoadScene(nomStages[0]);
    }

    public static void NotifierPlaneteComplete()
    {
        int planete = PlayerPrefs.GetInt("PlaneteEnCours", 0);
        if (planete <= 0) return;

        // Charge la progression depuis PlayerPrefs
        string data = PlayerPrefs.GetString("ExplorationProgress", "00000000");
        char[] chars = data.ToCharArray();
        int index = planete - 1;

        if (index >= 0 && index < 8 && chars[index] == '0')
        {
            chars[index] = '1';
            string newData = new string(chars);
            PlayerPrefs.SetString("ExplorationProgress", newData);
            PlayerPrefs.Save();

            // Compte les planètes complètes
            int total = 0;
            foreach (char c in chars) if (c == '1') total++;

            Debug.Log("Planète " + planete + " complète ! Total : " + total + "/8");

            // Toutes complètes → écran de fin
            if (total >= 8)
            {
                PlayerPrefs.SetInt("VictoireExploration", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("SceneTransitionEndGame");
            }
        }
    }
}