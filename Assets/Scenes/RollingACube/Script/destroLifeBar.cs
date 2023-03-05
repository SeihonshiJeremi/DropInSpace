using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroLifeBar : MonoBehaviour
{
    void Awake()
    {
        GameObject.FindWithTag("lifeBar").GetComponent<LifeBarManager>().DetruitLifeBar();
    }
}
