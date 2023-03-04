using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetLifeBar : MonoBehaviour
{
    int score;
    string scoreTemporaire;
    // Start is called before the first frame update
    void Start()
    {
        //reset score temporaire
        score = 0;
        PlayerPrefs.SetInt(scoreTemporaire, score);

        //
        GameObject.Find("LifeBar").GetComponent<LifeBarManager>().ResetLifeBar();

        //pour destroy transition exploration
      
    }


}
