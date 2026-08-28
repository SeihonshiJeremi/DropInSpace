using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemDropInSpace : MonoBehaviour
{
    public float durationItem;
    public float durationItemOrigine;

    public bool itemTimeDiv2, itemTimeStop, itemInvincibilty, itemBonbonScoreRouge, itemBonbonScoreVert, itemRandom;
    public bool itemTimeDiv202, itemTimeStop02, itemInvincibilty02, itemBonbonScoreRouge02, itemBonbonScoreVert02, itemRandom02;

    public bool itemDetruit = false;
    public bool itemDetruit02 = false;

    public MeshRenderer ObjetFaireClignoter;

    public int choixPlatform;
    public int choixPlatform02;
    public GameObject newItem;
    public GameObject newItem02;

    public GameObject itemTimeDiv2GO, itemTimeStopGO, itemInvincibiltyGO, itemBonbonScoreRougeGO, itemBonbonScoreVertGO, itemRandomGO;
    public GameObject itemTimeDiv2GO02, itemTimeStopGO02, itemInvincibiltyGO02, itemBonbonScoreRougeGO02, itemBonbonScoreVertGO02, itemRandomGO02;

    public float tAvAppItem, tAvAppItemOringine;
    public float tAvAppItem02, tAvAppItemOringine02;

    public bool pos01, pos02, pos03, pos04, pos05, pos06, pos07, pos08, pos09;
    public bool pos10, pos11, pos12, pos13, pos14, pos15, pos16, pos17, pos18, pos19;
    public bool pos20, pos21, pos22, pos23, pos24, pos25, pos26, pos27, pos28, pos29;
    public bool pos30, pos31, pos32, pos33, pos34, pos35, pos36, pos37, pos38, pos39;
    public bool pos40, pos41, pos42, pos43, pos44, pos45, pos46, pos47, pos48, pos49;
    public bool pos50, pos51, pos52, pos53, pos54, pos55, pos56, pos57, pos58, pos59;
    public bool pos60, pos61, pos62, pos63, pos64, pos65, pos66, pos67, pos68, pos69;
    public bool pos70, pos71, pos72, pos73, pos74, pos75, pos76, pos77, pos78, pos79;
    public bool pos80, pos81;

    public GameObject scriptPouvoirItem;
    public GameObject scriptPouvoirItem02;
    public int choixItemRandom;
    public GameObject shieldPlayer;

    // CORRIGÉ : cache GetComponent + guard clignotement
    private monsterZoom monster01;
    private monsterZoom monster02;
    private bool cligno;
    private bool isBlinking;

    // CORRIGÉ : positions en tableau
    private static readonly Vector3[] positions = new Vector3[]
    {
        new Vector3(0,1,0),  new Vector3(2,1,0),  new Vector3(4,1,0),  new Vector3(6,1,0),  new Vector3(8,1,0),  new Vector3(10,1,0), new Vector3(12,1,0), new Vector3(14,1,0), new Vector3(16,1,0),
        new Vector3(0,1,2),  new Vector3(2,1,2),  new Vector3(4,1,2),  new Vector3(6,1,2),  new Vector3(8,1,2),  new Vector3(10,1,2), new Vector3(12,1,2), new Vector3(14,1,2), new Vector3(16,1,2),
        new Vector3(0,1,4),  new Vector3(2,1,4),  new Vector3(4,1,4),  new Vector3(6,1,4),  new Vector3(8,1,4),  new Vector3(10,1,4), new Vector3(12,1,4), new Vector3(14,1,4), new Vector3(16,1,4),
        new Vector3(0,1,6),  new Vector3(2,1,6),  new Vector3(4,1,6),  new Vector3(6,1,6),  new Vector3(8,1,6),  new Vector3(10,1,6), new Vector3(12,1,6), new Vector3(14,1,6), new Vector3(16,1,6),
        new Vector3(0,1,8),  new Vector3(2,1,8),  new Vector3(4,1,8),  new Vector3(6,1,8),  new Vector3(8,1,8),  new Vector3(10,1,8), new Vector3(12,1,8), new Vector3(14,1,8), new Vector3(16,1,8),
        new Vector3(0,1,10), new Vector3(2,1,10), new Vector3(4,1,10), new Vector3(6,1,10), new Vector3(8,1,10), new Vector3(10,1,10),new Vector3(12,1,10),new Vector3(14,1,10),new Vector3(16,1,10),
        new Vector3(0,1,12), new Vector3(2,1,12), new Vector3(4,1,12), new Vector3(6,1,12), new Vector3(8,1,12), new Vector3(10,1,12),new Vector3(12,1,12),new Vector3(14,1,12),new Vector3(16,1,12),
        new Vector3(0,1,14), new Vector3(2,1,14), new Vector3(4,1,14), new Vector3(6,1,14), new Vector3(8,1,14), new Vector3(10,1,14),new Vector3(12,1,14),new Vector3(14,1,14),new Vector3(16,1,14),
        new Vector3(0,1,16), new Vector3(2,1,16), new Vector3(4,1,16), new Vector3(6,1,16), new Vector3(8,1,16), new Vector3(10,1,16),new Vector3(12,1,16),new Vector3(14,1,16),new Vector3(16,1,16)
    };

    void Start()
    {
        // CORRIGÉ : cache des composants monsterZoom
        if (scriptPouvoirItem != null) monster01 = scriptPouvoirItem.GetComponent<monsterZoom>();
        if (scriptPouvoirItem02 != null) monster02 = scriptPouvoirItem02.GetComponent<monsterZoom>();

        newItem = InstancierItem(itemTimeDiv2, itemTimeStop, itemInvincibilty, itemBonbonScoreRouge, itemBonbonScoreVert, itemRandom,
                                 itemTimeDiv2GO, itemTimeStopGO, itemInvincibiltyGO, itemBonbonScoreRougeGO, itemBonbonScoreVertGO, itemRandomGO);

        newItem02 = InstancierItem(itemTimeDiv202, itemTimeStop02, itemInvincibilty02, itemBonbonScoreRouge02, itemBonbonScoreVert02, itemRandom02,
                                   itemTimeDiv2GO02, itemTimeStopGO02, itemInvincibiltyGO02, itemBonbonScoreRougeGO02, itemBonbonScoreVertGO02, itemRandomGO02);

        choixItemRandom = Random.Range(1, 4);
        durationItem = 0.0f;
        if (shieldPlayer != null) shieldPlayer.SetActive(false);
    }

    // CORRIGÉ : fonction helper pour instancier le bon item
    private GameObject InstancierItem(bool div2, bool stop, bool invinc, bool rouge, bool vert, bool random,
                                       GameObject div2GO, GameObject stopGO, GameObject invincGO, GameObject rougeGO, GameObject vertGO, GameObject randomGO)
    {
        if (div2) return Instantiate(div2GO);
        if (stop) return Instantiate(stopGO);
        if (invinc) return Instantiate(invincGO);
        if (rouge) return Instantiate(rougeGO);
        if (vert) return Instantiate(vertGO);
        if (random) return Instantiate(randomGO);
        return null;
    }

    // CORRIGÉ : fonction helper pour placer un item sur la grille
    private void PlacerItem(GameObject item, int choix)
    {
        if (item == null) return;
        bool[] posArray = GetPosArray();
        if (choix >= 1 && choix <= posArray.Length && posArray[choix - 1])
            item.transform.position = positions[choix - 1];
    }

    private bool[] GetPosArray()
    {
        return new bool[] {
            pos01,pos02,pos03,pos04,pos05,pos06,pos07,pos08,pos09,
            pos10,pos11,pos12,pos13,pos14,pos15,pos16,pos17,pos18,pos19,
            pos20,pos21,pos22,pos23,pos24,pos25,pos26,pos27,pos28,pos29,
            pos30,pos31,pos32,pos33,pos34,pos35,pos36,pos37,pos38,pos39,
            pos40,pos41,pos42,pos43,pos44,pos45,pos46,pos47,pos48,pos49,
            pos50,pos51,pos52,pos53,pos54,pos55,pos56,pos57,pos58,pos59,
            pos60,pos61,pos62,pos63,pos64,pos65,pos66,pos67,pos68,pos69,
            pos70,pos71,pos72,pos73,pos74,pos75,pos76,pos77,pos78,pos79,
            pos80,pos81
        };
    }

    void FixedUpdate()
    {
        durationItem -= Time.deltaTime;
        if (durationItem < 0)
        {
            durationItem = durationItemOrigine;
            choixItemRandom = Random.Range(1, 4);
            cligno = false;
            if (shieldPlayer != null) shieldPlayer.SetActive(false);
        }

        // CORRIGÉ : guard isBlinking pour éviter de relancer la coroutine chaque frame
        if (cligno && !isBlinking) StartCoroutine(Clignotement());
    }

    void Update()
    {
        tAvAppItem -= Time.deltaTime;
        tAvAppItem02 -= Time.deltaTime;

        // CORRIGÉ : random calculé seulement quand nécessaire
        if (tAvAppItem < 0 || tAvAppItem02 < 0)
        {
            choixPlatform = Random.Range(1, 82);
            choixPlatform02 = Random.Range(1, 82);
        }

        if (tAvAppItem < 0 && choixPlatform != choixPlatform02)
        {
            if (itemDetruit)
            {
                newItem = InstancierItem(itemTimeDiv2, itemTimeStop, itemInvincibilty, itemBonbonScoreRouge, itemBonbonScoreVert, itemRandom,
                                         itemTimeDiv2GO, itemTimeStopGO, itemInvincibiltyGO, itemBonbonScoreRougeGO, itemBonbonScoreVertGO, itemRandomGO);
                itemDetruit = false;
            }
            PlacerItem(newItem, choixPlatform);
            tAvAppItem = tAvAppItemOringine;
        }

        if (tAvAppItem02 < 0 && choixPlatform != choixPlatform02)
        {
            if (itemDetruit02)
            {
                newItem02 = InstancierItem(itemTimeDiv202, itemTimeStop02, itemInvincibilty02, itemBonbonScoreRouge02, itemBonbonScoreVert02, itemRandom02,
                                           itemTimeDiv2GO02, itemTimeStopGO02, itemInvincibiltyGO02, itemBonbonScoreRougeGO02, itemBonbonScoreVertGO02, itemRandomGO02);
                itemDetruit02 = false;
            }
            PlacerItem(newItem02, choixPlatform02);
            tAvAppItem02 = tAvAppItemOringine02;
        }
    }

    // CORRIGÉ : fonctions item utilisent le cache monster01/monster02
    public void itemTimeDiv2F()
    {
        Destroy(newItem); itemDetruit = true; durationItem = 10.0f;
        monster01?.tAvDepX2(); monster02?.tAvDepX2();
    }

    public void itemTimeStopF()
    {
        Destroy(newItem); itemDetruit = true; durationItem = 10.0f;
        monster01?.powerStop(); monster02?.powerStop();
    }

    public void itemInvincibiltyF()
    {
        Destroy(newItem); itemDetruit = true;
        activClignoOtherScript();
        monster01?.powerInvincibilty(); monster02?.powerInvincibilty();
        durationItem = 10.0f;
    }

    public void itemBonbonScoreRougeF()
    {
        Destroy(newItem); itemDetruit = true;
        monster01?.itemBonbonScoreRouge(); monster02?.itemBonbonScoreRouge();
    }

    public void itemBonbonScoreVertF()
    {
        Destroy(newItem); itemDetruit = true;
        monster01?.itemBonbonScoreVert(); monster02?.itemBonbonScoreVert();
    }

    public void itemRandomF()
    {
        Destroy(newItem); itemDetruit = true;
        powerItemRandom(); durationItem = 10.0f;
    }

    public void itemTimeDiv2F02()
    {
        Destroy(newItem02); itemDetruit02 = true; durationItem = 10.0f;
        monster01?.tAvDepX2(); monster02?.tAvDepX2();
    }

    public void itemTimeStopF02()
    {
        Destroy(newItem02); itemDetruit02 = true; durationItem = 10.0f;
        monster01?.powerStop(); monster02?.powerStop();
    }

    public void itemInvincibiltyF02()
    {
        Destroy(newItem02); itemDetruit02 = true;
        activClignoOtherScript();
        monster01?.powerInvincibilty(); monster02?.powerInvincibilty();
        durationItem = 10.0f;
    }

    public void itemBonbonScoreRougeF02()
    {
        Destroy(newItem02); itemDetruit02 = true;
        monster01?.itemBonbonScoreRouge(); monster02?.itemBonbonScoreRouge();
    }

    public void itemBonbonScoreVertF02()
    {
        Destroy(newItem02); itemDetruit02 = true;
        monster01?.itemBonbonScoreVert(); monster02?.itemBonbonScoreVert();
    }

    public void itemRandomF02()
    {
        Destroy(newItem02); itemDetruit02 = true;
        powerItemRandom();
    }

    public void itemDetruitF()
    {
        Destroy(newItem02); itemDetruit02 = true;
    }

    public void itemDetruitMonsterF()
    {
        Destroy(newItem); itemDetruit = true;
    }

    // CORRIGÉ : boucle inutile supprimée + guard isBlinking
    IEnumerator Clignotement()
    {
        isBlinking = true;
        while (cligno)
        {
            if (ObjetFaireClignoter != null) ObjetFaireClignoter.enabled = false;
            yield return new WaitForSeconds(0.25f);
            if (ObjetFaireClignoter != null) ObjetFaireClignoter.enabled = true;
            yield return new WaitForSeconds(0.25f);
        }
        isBlinking = false;
    }

    public void powerItemRandom()
    {
        switch (choixItemRandom)
        {
            case 1: monster01?.tAvDepX2(); monster02?.tAvDepX2(); break;
            case 2: monster01?.powerStop(); monster02?.powerStop(); break;
            case 3:
                monster01?.powerInvincibilty(); monster02?.powerInvincibilty();
                activClignoOtherScript(); break;
        }
    }

    public void activClignoOtherScript()
    {
        cligno = true;
        if (shieldPlayer != null) shieldPlayer.SetActive(true);
    }

    // Méthodes randomScript — conservées pour compatibilité avec randomItemScript
    public void randomScriptDiv2() { itemTimeDiv2 = true; itemTimeStop = itemInvincibilty = itemBonbonScoreRouge = itemBonbonScoreVert = itemRandom = false; }
    public void randomScriptStop() { itemTimeStop = true; itemTimeDiv2 = itemInvincibilty = itemBonbonScoreRouge = itemBonbonScoreVert = itemRandom = false; }
    public void randomScriptInvincibilty() { itemInvincibilty = true; itemTimeDiv2 = itemTimeStop = itemBonbonScoreRouge = itemBonbonScoreVert = itemRandom = false; }
    public void randomScriptScoreRouge() { itemBonbonScoreRouge = true; itemTimeDiv2 = itemTimeStop = itemInvincibilty = itemBonbonScoreVert = itemRandom = false; }
    public void randomScriptScoreVert() { itemBonbonScoreVert = true; itemTimeDiv2 = itemTimeStop = itemInvincibilty = itemBonbonScoreRouge = itemRandom = false; }
    public void randomScriptRandom() { itemRandom = true; itemTimeDiv2 = itemTimeStop = itemInvincibilty = itemBonbonScoreRouge = itemBonbonScoreVert = false; }
    public void randomScriptDiv202() { itemTimeDiv202 = true; itemTimeStop02 = itemInvincibilty02 = itemBonbonScoreRouge02 = itemBonbonScoreVert02 = itemRandom02 = false; }
    public void randomScriptStop02() { itemTimeStop02 = true; itemTimeDiv202 = itemInvincibilty02 = itemBonbonScoreRouge02 = itemBonbonScoreVert02 = itemRandom02 = false; }
    public void randomScriptInvincibilty02() { itemInvincibilty02 = true; itemTimeDiv202 = itemTimeStop02 = itemBonbonScoreRouge02 = itemBonbonScoreVert02 = itemRandom02 = false; }
    public void randomScriptScoreRouge02() { itemBonbonScoreRouge02 = true; itemTimeDiv202 = itemTimeStop02 = itemInvincibilty02 = itemBonbonScoreVert02 = itemRandom02 = false; }
    public void randomScriptScoreVert02() { itemBonbonScoreVert02 = true; itemTimeDiv202 = itemTimeStop02 = itemInvincibilty02 = itemBonbonScoreRouge02 = itemRandom02 = false; }
    public void randomScriptRandom02() { itemRandom02 = true; itemTimeDiv202 = itemTimeStop02 = itemInvincibilty02 = itemBonbonScoreRouge02 = itemBonbonScoreVert02 = false; }
}