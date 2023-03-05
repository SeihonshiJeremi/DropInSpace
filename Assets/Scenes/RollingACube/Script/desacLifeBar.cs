using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class desacLifeBar : MonoBehaviour
{
    public bool DesacLifeBar;

    void Awake()
    {
        if (DesacLifeBar == true) GameObject.FindWithTag("lifeBar").GetComponent<LifeBarManager>().desactivationLifeBar();
    }
}
