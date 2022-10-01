using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class zoomManager : MonoBehaviour
{
    public int valueFinLevel;
    public int nbFinLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void finLevel(int finLevelToAdd)
    {
        valueFinLevel += finLevelToAdd;
        if(valueFinLevel == nbFinLevel)
        {
            Debug.Log("go fin level");
        }
    }

    public void reduceFinLevel(int finLevelToAdd)
    {
        valueFinLevel -= finLevelToAdd;
    }
}
