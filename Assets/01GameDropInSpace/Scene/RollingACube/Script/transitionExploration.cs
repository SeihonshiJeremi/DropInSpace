using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transitionExploration : MonoBehaviour
{
    public int valeurScene;
    public GameObject TransiExplo;

    // CORRIGÉ : cache zoomManager
    private zoomManager zm;
    private int lastValeurScene = -1;

    void Start()
    {
        zm = zoomManager.Instance ?? FindObjectOfType<zoomManager>();
    }

    void Update()
    {
        valeurScene = PlayerPrefs.GetInt("ValeurScene", valeurScene);

        // CORRIGÉ : ne fait rien si valeur n'a pas changé + suppression Debug.Log
        if (valeurScene == lastValeurScene) return;
        lastValeurScene = valeurScene;

        if (valeurScene == 200 && TransiExplo != null)
            Destroy(TransiExplo);

        if (valeurScene == 103 && zm != null)
            zm.TransitionExploration();
    }
}