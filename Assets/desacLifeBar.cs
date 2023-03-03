using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class desacLifeBar : MonoBehaviour
{
    public bool DesacLifeBar;

    // Start is called before the first frame update
    void Start()
    {
        if (DesacLifeBar == true)
        {
           
            GameObject.Find("LifeBar").GetComponent<LifeBarManager>().desactivationLifeBar();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
