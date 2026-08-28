using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deletePlayerPrefs : MonoBehaviour
{
    public void ResetPlayerPrefs()
    {       
        PlayerPrefs.DeleteAll();
        Debug.Log("Reset");
    }
}
