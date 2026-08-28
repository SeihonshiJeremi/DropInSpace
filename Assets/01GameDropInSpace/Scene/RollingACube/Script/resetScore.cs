using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetScore : MonoBehaviour
{
    public int valeurScene;

    void Awake()
    {
        // CORRIGÉ : clés initialisées avec des noms valides
        PlayerPrefs.SetInt("ScoreTemporaire", 0);
        PlayerPrefs.SetInt("LifeBar", 3);
        PlayerPrefs.SetInt("ValeurScene", valeurScene);
    }
}