using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetScore : MonoBehaviour
{
    int score;
    string scoreTemporaire;
    void Awake()
    {
        //reset score temporaire
        score = 0;
        PlayerPrefs.SetInt(scoreTemporaire, score);      
    }
}
