using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomItemScript : MonoBehaviour
{
    public GameObject itemScript;
    public bool goScript;
    public int random;
    public float tempsAvantChangementItem;

    public void FixedUpdate()
    {
        if(goScript == true)
        {
            tempsAvantChangementItem -= Time.deltaTime;
            if (tempsAvantChangementItem <= 120 && tempsAvantChangementItem >= 100) random = Random.Range(1, 13);
            if (tempsAvantChangementItem <= 100 && tempsAvantChangementItem >= 80) random = Random.Range(1, 13);
            if (tempsAvantChangementItem <= 80 && tempsAvantChangementItem >= 60) random = Random.Range(1, 13);
            if (tempsAvantChangementItem <= 60 && tempsAvantChangementItem >= 40) random = Random.Range(1, 13);
            if (tempsAvantChangementItem <= 40 && tempsAvantChangementItem >= 20) random = Random.Range(1, 13);
            if (tempsAvantChangementItem <= 20 && tempsAvantChangementItem >= 00) random = Random.Range(1, 13);

            if (random == 1) itemScript.GetComponent<itemDropInSpace>().randomScriptDiv2();
            if (random == 2) itemScript.GetComponent<itemDropInSpace>().randomScriptStop();
            if (random == 3) itemScript.GetComponent<itemDropInSpace>().randomScriptInvincibilty();
            if (random == 4) itemScript.GetComponent<itemDropInSpace>().randomScriptScoreRouge();
            if (random == 5) itemScript.GetComponent<itemDropInSpace>().randomScriptScoreVert();
            if (random == 6) itemScript.GetComponent<itemDropInSpace>().randomScriptRandom();

            if (random == 7) itemScript.GetComponent<itemDropInSpace>().randomScriptDiv202();
            if (random == 8) itemScript.GetComponent<itemDropInSpace>().randomScriptStop02();
            if (random == 9) itemScript.GetComponent<itemDropInSpace>().randomScriptInvincibilty02();
            if (random == 10) itemScript.GetComponent<itemDropInSpace>().randomScriptScoreRouge02();
            if (random == 11) itemScript.GetComponent<itemDropInSpace>().randomScriptScoreVert02();
            if (random == 12) itemScript.GetComponent<itemDropInSpace>().randomScriptRandom02();
        }
    }
}
