using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroy : MonoBehaviour
{
    void Awake()
    {
        // CORRIGÉ : détache du parent si nécessaire pour que DontDestroyOnLoad fonctionne
        transform.SetParent(null);
        DontDestroyOnLoad(this);
    }
}