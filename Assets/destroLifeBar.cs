using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroLifeBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("LifeBar").GetComponent<LifeBarManager>().DetruitLifeBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
