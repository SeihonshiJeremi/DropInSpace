using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetScore : MonoBehaviour
{
    int score;
    string scoreTemporaire;
    int reset;
    string lifeBar;
    int debug;

    public int valeurScene;
    string ValeurScene;

    void Awake()
    {
        //reset score temporaire
        score = 0;
        PlayerPrefs.SetInt(scoreTemporaire, score);

        //reset LifeBar
        reset = 3;
        PlayerPrefs.SetInt("LifeBar", reset);

        debug = PlayerPrefs.GetInt("LifeBar", reset);
        Debug.Log(debug);

        valeurScene = valeurScene;
        PlayerPrefs.SetInt("ValeurScene", valeurScene);
    }
}
