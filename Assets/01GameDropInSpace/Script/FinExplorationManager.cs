using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// À placer sur la scène SceneTransitionEndGame.
/// Gère le retour au SpaceHub après l'écran de fin du mode Exploration.
/// </summary>
public class FinExplorationManager : MonoBehaviour
{
    public string sceneSpaceHub = "SpaceHub";
    public float tempsAvantRetour = 5.0f; // secondes avant retour automatique
    public TMP_Text textCompteur;          // optionnel — affiche le compte à rebours

    private float timer;

    void Start()
    {
        timer = tempsAvantRetour;

        /*// Vérifie qu'on vient bien du mode Exploration
        if (PlayerPrefs.GetInt("VictoireExploration", 0) == 0)
        {
            // Pas une victoire Exploration — retour immédiat
            SceneManager.LoadScene(sceneSpaceHub);
        }*/
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (textCompteur != null)
            textCompteur.text = Mathf.CeilToInt(timer).ToString();

        // Retour automatique après le délai
        if (timer <= 0)
            RetourSpaceHub();

        /*// Retour immédiat si le joueur appuie sur une touche
        if (Input.anyKeyDown)
            RetourSpaceHub();*/
    }

    public void RetourSpaceHub()
    {
        PlayerPrefs.SetInt("VictoireExploration", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(sceneSpaceHub);
    }
}