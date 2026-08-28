using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomItemScript : MonoBehaviour
{
    public GameObject itemScript;
    public bool goScript;
    public int random;
    public float tempsAvantChangementItem = 20f;

    // CORRIGÉ : cache GetComponent
    private itemDropInSpace itemDIS;

    void Start()
    {
        itemDIS = itemScript.GetComponent<itemDropInSpace>();
    }

    void FixedUpdate()
    {
        if (!goScript) return;

        // CORRIGÉ : timer simple, random calculé une seule fois au déclenchement
        tempsAvantChangementItem -= Time.deltaTime;
        if (tempsAvantChangementItem <= 0)
        {
            random = Random.Range(1, 13);
            AppliquerItem(random);
            tempsAvantChangementItem = 20f;
        }
    }

    // CORRIGÉ : switch au lieu de 12 if
    private void AppliquerItem(int r)
    {
        switch (r)
        {
            case 1: itemDIS.randomScriptDiv2(); break;
            case 2: itemDIS.randomScriptStop(); break;
            case 3: itemDIS.randomScriptInvincibilty(); break;
            case 4: itemDIS.randomScriptScoreRouge(); break;
            case 5: itemDIS.randomScriptScoreVert(); break;
            case 6: itemDIS.randomScriptRandom(); break;
            case 7: itemDIS.randomScriptDiv202(); break;
            case 8: itemDIS.randomScriptStop02(); break;
            case 9: itemDIS.randomScriptInvincibilty02(); break;
            case 10: itemDIS.randomScriptScoreRouge02(); break;
            case 11: itemDIS.randomScriptScoreVert02(); break;
            case 12: itemDIS.randomScriptRandom02(); break;
        }
    }
}