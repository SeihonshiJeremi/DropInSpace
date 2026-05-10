using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplorationManager : MonoBehaviour
{
    public static ExplorationManager Instance;

    private bool[] planeteCompletee = new bool[8];
    private int planetesVisitees = 0;
    private const string PREFS_KEY = "ExplorationProgress";

    public string sceneFinExploration = "SceneTransitionEndGame";

    void Awake()
    {
        Instance = this;
        ChargerProgression();
    }

    public void ChargerProgression()
    {
        string data = PlayerPrefs.GetString(PREFS_KEY, "00000000");
        planetesVisitees = 0;
        for (int i = 0; i < 8; i++)
        {
            planeteCompletee[i] = data.Length > i && data[i] == '1';
            if (planeteCompletee[i]) planetesVisitees++;
        }
    }

    public void SauvegarderProgression()
    {
        string data = "";
        for (int i = 0; i < 8; i++)
            data += planeteCompletee[i] ? "1" : "0";
        PlayerPrefs.SetString(PREFS_KEY, data);
        PlayerPrefs.Save();
    }

    public void MarquerPlaneteComplete(int numeroPlanete)
    {
        int index = numeroPlanete - 1;
        if (index < 0 || index >= 8) return;

        if (!planeteCompletee[index])
        {
            planeteCompletee[index] = true;
            planetesVisitees++;
            SauvegarderProgression();

            if (planetesVisitees >= 8)
                DeclenchemerVictoire();
        }
    }

    public bool EstPlaneteComplete(int numeroPlanete)
    {
        int index = numeroPlanete - 1;
        if (index < 0 || index >= 8) return false;
        return planeteCompletee[index];
    }

    public int GetPlanetesVisitees() => planetesVisitees;
    public bool[] GetProgression() => planeteCompletee;

    private void DeclenchemerVictoire()
    {
        Debug.Log("VICTOIRE ! Chargement de : " + sceneFinExploration);
        PlayerPrefs.SetInt("VictoireExploration", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(sceneFinExploration);
    }

    public void ResetExploration()
    {
        for (int i = 0; i < 8; i++) planeteCompletee[i] = false;
        planetesVisitees = 0;
        SauvegarderProgression();
    }
}