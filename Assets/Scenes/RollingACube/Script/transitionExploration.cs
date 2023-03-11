using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transitionExploration : MonoBehaviour
{
    public int valeurScene;
    public string ValeurScene;
    public GameObject TransiExplo;

    void Update()
    {
        valeurScene = PlayerPrefs.GetInt("ValeurScene", valeurScene);
        Debug.Log(valeurScene);
        if (valeurScene == 200) Destroy(TransiExplo);
        if(valeurScene == 103) GameObject.Find("ZoomManager").GetComponent<zoomManager>().TransitionExploration();
    }
}
