using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Affiche la progression en mode Exploration directement dans le SpaceHub.
/// Tout est visible en permanence — pas de bouton nécessaire.
/// </summary>
public class ExplorationUIManager : MonoBehaviour
{
    public static ExplorationUIManager Instance;

    [Header("Compteur")]
    public TMP_Text textCompteur;           // "3 / 8 planètes explorées"

    [Header("Carte des planètes (8 images)")]
    public Image[] iconesPlanetes;
    public Color couleurComplete = Color.white;
    public Color couleurInComplete = new Color(0.3f, 0.3f, 0.3f, 0.8f);

    [Header("Panneau victoire")]
    public GameObject panneauVictoire;
    public TMP_Text textVictoire;

    [Header("Navigation")]
    public string sceneMenu = "StartDropInSpace";

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (panneauVictoire != null) panneauVictoire.SetActive(false);
        // CORRIGÉ : affichage immédiat au chargement
        MettreAJourUI();
    }

    public void MettreAJourUI()
    {
        if (ExplorationManager.Instance == null) return;

        int visitees = ExplorationManager.Instance.GetPlanetesVisitees();
        bool[] progression = ExplorationManager.Instance.GetProgression();

        if (textCompteur != null)
            textCompteur.text = visitees + " / 8 planètes explorées";

        for (int i = 0; i < iconesPlanetes.Length && i < 8; i++)
        {
            if (iconesPlanetes[i] == null) continue;
            bool complete = i < progression.Length && progression[i];
            iconesPlanetes[i].color = complete ? couleurComplete : couleurInComplete;
        }
    }

    public void AfficherVictoire()
    {
        if (panneauVictoire != null)
        {
            panneauVictoire.SetActive(true);
            if (textVictoire != null)
                textVictoire.text = "Toutes les planètes ont été explorées !\nDrop a découvert l'univers entier.";
        }
    }

    public void MasquerVictoire()
    {
        if (panneauVictoire != null) panneauVictoire.SetActive(false);
    }

    public void BoutonRetourMenu()
    {
        SceneManager.LoadScene(sceneMenu);
    }

    public void BoutonResetExploration()
    {
        if (ExplorationManager.Instance != null)
            ExplorationManager.Instance.ResetExploration();
        MettreAJourUI();
        MasquerVictoire();
    }
}