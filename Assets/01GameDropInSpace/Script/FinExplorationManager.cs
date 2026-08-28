using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinExplorationManager : MonoBehaviour
{
    public string sceneSpaceHub = "SpaceHub";
    public float tempsAvantRetour = 5.0f;
    public TMP_Text textCompteur;

    private float timer;

    void Start()
    {
        timer = tempsAvantRetour;

        if (GameStateManager.Instance.VictoryExploration == false)
        {
            Debug.LogWarning("[FinExploration] Victoire Exploration non enregistrée !");
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (textCompteur != null)
            textCompteur.text = Mathf.CeilToInt(timer).ToString();

        if (timer <= 0)
            RetourSpaceHub();
    }

    public void RetourSpaceHub()
    {
        // Reset propre de la session Exploration
        GameStateManager.Instance.SetMode("Arcade");
        GameStateManager.Instance.SetCurrentStage(0);
        GameStateManager.Instance.SetCurrentPlanet(0);
        GameStateManager.Instance.SaveToPrefs();

        PlayerPrefs.SetInt("VictoireExploration", 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneSpaceHub);
    }
}