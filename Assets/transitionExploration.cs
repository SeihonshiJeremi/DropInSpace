using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transitionExploration : MonoBehaviour
{
    public bool transiExploration;

    void Start()
    {
        if (transiExploration == true) GameObject.Find("ZoomManager").GetComponent<zoomManager>().TransitionExploration();
    }
}
