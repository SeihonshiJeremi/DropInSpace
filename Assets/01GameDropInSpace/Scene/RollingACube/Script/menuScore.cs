using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScore : MonoBehaviour
{
    public string sceneRetour;
    public string sceneReset;

    public void retour()
    {
        SceneManager.LoadScene(sceneRetour);
    }

    public void reset()
    {
        // CORRIGÉ : réinitialise les PlayerPrefs avant de recharger
        PlayerPrefs.SetInt("ScoreTemporaire", 0);
        PlayerPrefs.SetInt("LifeBar", 3);
        SceneManager.LoadScene(sceneReset);
    }
}