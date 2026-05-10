using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testClignotement : MonoBehaviour
{
    public MeshRenderer ObjetFaireClignoter;

    public float timerClignotement;

    public void Update()
    {
        

        timerClignotement -= Time.deltaTime;

        if(timerClignotement > 0)
        {
            StartCoroutine(Clignotement());
            //ObjetFaireClignoter.GetComponent<MeshRenderer>().enabled = true;

        }

        
        
    }

    IEnumerator Clignotement()
    {
        for (int i = 0; i < 1; i++)
        {
            ObjetFaireClignoter.enabled = false;
            yield return new WaitForSeconds(1f); // or 0.125f if it shall take 0.25 in total
            ObjetFaireClignoter.enabled = true;
            yield return new WaitForSeconds(1f); // or 0.125f if it shall take 0.25 in total
        }
    }
}
