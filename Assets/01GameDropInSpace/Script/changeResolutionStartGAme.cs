using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeResolutionStartGAme : MonoBehaviour
{
    public bool changeResoTel;
    void Start()
    {
        if(changeResoTel == true) Screen.SetResolution(1280, 720, true);
    }
}
