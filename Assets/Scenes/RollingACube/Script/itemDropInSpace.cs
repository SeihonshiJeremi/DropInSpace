using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class itemDropInSpace : MonoBehaviour
{
    //les items
    public bool itemTimeDiv2;
    public bool itemTimeStop;
    public bool itemInvincibilty;
    public bool itemBonbonScoreRouge;
    public bool itemBonbonScoreVert;
    public bool itemRandom;

    public bool itemTimeDiv202;
    public bool itemTimeStop02;
    public bool itemInvincibilty02;
    public bool itemBonbonScoreRouge02;
    public bool itemBonbonScoreVert02;
    public bool itemRandom02;

    //recreation de l'item
    public bool itemDetruit = false;
    public bool itemDetruit02 = false;

    //apparition de l'item
    public int choixPlatform;
    public int choixPlatform02;
    public GameObject newItem;
    public GameObject newItem02;
    public GameObject itemTimeDiv2GO;
    public GameObject itemTimeStopGO;
    public GameObject itemInvincibiltyGO;
    public GameObject itemBonbonScoreRougeGO;
    public GameObject itemBonbonScoreVertGO;
    public GameObject itemRandomGO;
    public GameObject itemTimeDiv2GO02;
    public GameObject itemTimeStopGO02;
    public GameObject itemInvincibiltyGO02;
    public GameObject itemBonbonScoreRougeGO02;
    public GameObject itemBonbonScoreVertGO02;
    public GameObject itemRandomGO02;
    public float tAvAppItem;
    public float tAvAppItemOringine;
    public float tAvAppItem02;
    public float tAvAppItemOringine02;
    public bool pos01, pos02, pos03, pos04, pos05, pos06, pos07, pos08, pos09;
    public bool pos10, pos11, pos12, pos13, pos14, pos15, pos16, pos17, pos18, pos19;
    public bool pos20, pos21, pos22, pos23, pos24, pos25, pos26, pos27, pos28, pos29;
    public bool pos30, pos31, pos32, pos33, pos34, pos35, pos36, pos37, pos38, pos39;
    public bool pos40, pos41, pos42, pos43, pos44, pos45, pos46, pos47, pos48, pos49;
    public bool pos50, pos51, pos52, pos53, pos54, pos55, pos56, pos57, pos58, pos59;
    public bool pos60, pos61, pos62, pos63, pos64, pos65, pos66, pos67, pos68, pos69;
    public bool pos70, pos71, pos72, pos73, pos74, pos75, pos76, pos77, pos78, pos79;
    public bool pos80, pos81;




    // Start is called before the first frame update
    void Start()
    {
        //premier item
        if (itemTimeDiv2 == true) { newItem = Instantiate(itemTimeDiv2GO); }
        if (itemTimeStop == true) { newItem = Instantiate(itemTimeStopGO); }
        if (itemInvincibilty == true) { newItem = Instantiate(itemInvincibiltyGO); }
        if (itemBonbonScoreRouge == true) { newItem = Instantiate(itemBonbonScoreRougeGO); }
        if (itemBonbonScoreVert == true) { newItem = Instantiate(itemBonbonScoreVertGO); }
        if (itemRandom == true) { newItem = Instantiate(itemRandomGO); }

        //deuxième item
        if (itemTimeDiv202 == true) { newItem02 = Instantiate(itemTimeDiv2GO02); }
        if (itemTimeStop02 == true) { newItem02 = Instantiate(itemTimeStopGO02); }
        if (itemInvincibilty02 == true) { newItem02 = Instantiate(itemInvincibiltyGO02); }
        if (itemBonbonScoreRouge02 == true) { newItem02 = Instantiate(itemBonbonScoreRougeGO02); }
        if (itemBonbonScoreVert02 == true) { newItem02 = Instantiate(itemBonbonScoreVertGO02); }
        if (itemRandom02 == true) { newItem02 = Instantiate(itemRandomGO02); }
    }

    //fonction 1er item
    public void itemTimeDiv2F()
    {
        Debug.Log("go function div2");
        Destroy(newItem);
        itemDetruit = true;
    }

    public void itemTimeStopF()
    {
        Debug.Log("go function stop");
        Destroy(newItem);
        itemDetruit = true;
    }

    public void itemInvincibiltyF()
    {
        Debug.Log("go function invincibility");
        Destroy(newItem);
        itemDetruit = true;

    }

    public void itemBonbonScoreRougeF()
    {
        Debug.Log("go function score rouge");
        Destroy(newItem);
        itemDetruit = true;

    }

    public void itemBonbonScoreVertF()
    {
        Debug.Log("go function score vert");
        Destroy(newItem);
        itemDetruit = true;

    }

    public void itemRandomF()
    {
        Debug.Log("go function random");
        Destroy(newItem);
        itemDetruit = true;

    }

    //fonction 2éme item
    public void itemTimeDiv2F02()
    {
        Debug.Log("go function div202");
        Destroy(newItem02);
        itemDetruit02 = true;
    }

    public void itemTimeStopF02()
    {
        Debug.Log("go function stop02");
        Destroy(newItem02);
        itemDetruit02 = true;
    }

    public void itemInvincibiltyF02()
    {
        Debug.Log("go function invincibility02");
        Destroy(newItem02);
        itemDetruit02 = true;

    }

    public void itemBonbonScoreRougeF02()
    {
        Debug.Log("go function score rouge02");
        Destroy(newItem02);
        itemDetruit02 = true;

    }

    public void itemBonbonScoreVertF02()
    {
        Debug.Log("go function score vert02");
        Destroy(newItem02);
        itemDetruit02 = true;

    }

    public void itemRandomF02()
    {
        Debug.Log("go function random02");
        Destroy(newItem02);
        itemDetruit02 = true;

    }

    //si jamais collision entre item
    public void itemDetruitF()
    {
        Debug.Log("colli entre item");
        Destroy(newItem02);
        itemDetruit02 = true;
    }

    public void itemDetruitMonsterF()
    {
        Debug.Log("colli entre item");
        Destroy(newItem);
        itemDetruit = true;
    }

    // Update is called once per frame
    void Update()
    {
        //pour apparition
        tAvAppItem -= Time.deltaTime;
        tAvAppItem02 -= Time.deltaTime;
        choixPlatform = Random.Range(1, 82);
        choixPlatform02 = Random.Range(1, 82);
        if (tAvAppItem < 0 && choixPlatform != choixPlatform02)
        {
            if(itemDetruit == true)
            {
                //premier item
                if (itemTimeDiv2 == true) { newItem = Instantiate(itemTimeDiv2GO); }
                if (itemTimeStop == true) { newItem = Instantiate(itemTimeStopGO); }
                if (itemInvincibilty == true) { newItem = Instantiate(itemInvincibiltyGO); }
                if (itemBonbonScoreRouge == true) { newItem = Instantiate(itemBonbonScoreRougeGO); }
                if (itemBonbonScoreVert == true) { newItem = Instantiate(itemBonbonScoreVertGO); }
                if (itemRandom == true) { newItem = Instantiate(itemRandomGO); }

                itemDetruit = false;
            }


            if (pos01 == true && choixPlatform == 1) { newItem.transform.position = position01; }
            if (pos02 == true && choixPlatform == 2) { newItem.transform.position = position02; }
            if (pos03 == true && choixPlatform == 3) { newItem.transform.position = position03; }
            if (pos04 == true && choixPlatform == 4) { newItem.transform.position = position04; }
            if (pos05 == true && choixPlatform == 5) { newItem.transform.position = position05; }
            if (pos06 == true && choixPlatform == 6) { newItem.transform.position = position06; }
            if (pos07 == true && choixPlatform == 7) { newItem.transform.position = position07; }
            if (pos08 == true && choixPlatform == 8) { newItem.transform.position = position08; }
            if (pos09 == true && choixPlatform == 9) { newItem.transform.position = position09; }
            if (pos10 == true && choixPlatform == 10) { newItem.transform.position = position10; }
            if (pos11 == true && choixPlatform == 11) { newItem.transform.position = position11; }
            if (pos12 == true && choixPlatform == 12) { newItem.transform.position = position12; }
            if (pos13 == true && choixPlatform == 13) { newItem.transform.position = position13; }
            if (pos14 == true && choixPlatform == 14) { newItem.transform.position = position14; }
            if (pos15 == true && choixPlatform == 15) { newItem.transform.position = position15; }
            if (pos16 == true && choixPlatform == 16) { newItem.transform.position = position16; }
            if (pos17 == true && choixPlatform == 17) { newItem.transform.position = position17; }
            if (pos18 == true && choixPlatform == 18) { newItem.transform.position = position18; }
            if (pos19 == true && choixPlatform == 19) { newItem.transform.position = position19; }
            if (pos20 == true && choixPlatform == 20) { newItem.transform.position = position20; }
            if (pos21 == true && choixPlatform == 21) { newItem.transform.position = position21; }
            if (pos22 == true && choixPlatform == 22) { newItem.transform.position = position22; }
            if (pos23 == true && choixPlatform == 23) { newItem.transform.position = position23; }
            if (pos24 == true && choixPlatform == 24) { newItem.transform.position = position24; }
            if (pos25 == true && choixPlatform == 25) { newItem.transform.position = position25; }
            if (pos26 == true && choixPlatform == 26) { newItem.transform.position = position26; }
            if (pos27 == true && choixPlatform == 27) { newItem.transform.position = position27; }
            if (pos28 == true && choixPlatform == 28) { newItem.transform.position = position28; }
            if (pos29 == true && choixPlatform == 29) { newItem.transform.position = position29; }
            if (pos30 == true && choixPlatform == 30) { newItem.transform.position = position30; }
            if (pos31 == true && choixPlatform == 31) { newItem.transform.position = position31; }
            if (pos32 == true && choixPlatform == 32) { newItem.transform.position = position32; }
            if (pos33 == true && choixPlatform == 33) { newItem.transform.position = position33; }
            if (pos34 == true && choixPlatform == 34) { newItem.transform.position = position34; }
            if (pos35 == true && choixPlatform == 35) { newItem.transform.position = position35; }
            if (pos36 == true && choixPlatform == 36) { newItem.transform.position = position36; }
            if (pos37 == true && choixPlatform == 37) { newItem.transform.position = position37; }
            if (pos38 == true && choixPlatform == 38) { newItem.transform.position = position38; }
            if (pos39 == true && choixPlatform == 39) { newItem.transform.position = position39; }
            if (pos40 == true && choixPlatform == 40) { newItem.transform.position = position40; }
            if (pos41 == true && choixPlatform == 41) { newItem.transform.position = position41; }
            if (pos42 == true && choixPlatform == 42) { newItem.transform.position = position42; }
            if (pos43 == true && choixPlatform == 43) { newItem.transform.position = position43; }
            if (pos44 == true && choixPlatform == 44) { newItem.transform.position = position44; }
            if (pos45 == true && choixPlatform == 45) { newItem.transform.position = position45; }
            if (pos46 == true && choixPlatform == 46) { newItem.transform.position = position46; }
            if (pos47 == true && choixPlatform == 47) { newItem.transform.position = position47; }
            if (pos48 == true && choixPlatform == 48) { newItem.transform.position = position48; }
            if (pos49 == true && choixPlatform == 49) { newItem.transform.position = position49; }
            if (pos50 == true && choixPlatform == 50) { newItem.transform.position = position50; }
            if (pos51 == true && choixPlatform == 51) { newItem.transform.position = position51; }
            if (pos52 == true && choixPlatform == 52) { newItem.transform.position = position52; }
            if (pos53 == true && choixPlatform == 53) { newItem.transform.position = position53; }
            if (pos54 == true && choixPlatform == 54) { newItem.transform.position = position54; }
            if (pos55 == true && choixPlatform == 55) { newItem.transform.position = position55; }
            if (pos56 == true && choixPlatform == 56) { newItem.transform.position = position56; }
            if (pos57 == true && choixPlatform == 57) { newItem.transform.position = position57; }
            if (pos58 == true && choixPlatform == 58) { newItem.transform.position = position58; }
            if (pos59 == true && choixPlatform == 59) { newItem.transform.position = position59; }
            if (pos60 == true && choixPlatform == 60) { newItem.transform.position = position60; }
            if (pos61 == true && choixPlatform == 61) { newItem.transform.position = position61; }
            if (pos62 == true && choixPlatform == 62) { newItem.transform.position = position62; }
            if (pos63 == true && choixPlatform == 63) { newItem.transform.position = position63; }
            if (pos64 == true && choixPlatform == 64) { newItem.transform.position = position64; }
            if (pos65 == true && choixPlatform == 65) { newItem.transform.position = position65; }
            if (pos66 == true && choixPlatform == 66) { newItem.transform.position = position66; }
            if (pos67 == true && choixPlatform == 67) { newItem.transform.position = position67; }
            if (pos68 == true && choixPlatform == 68) { newItem.transform.position = position68; }
            if (pos69 == true && choixPlatform == 69) { newItem.transform.position = position69; }
            if (pos70 == true && choixPlatform == 70) { newItem.transform.position = position70; }
            if (pos71 == true && choixPlatform == 71) { newItem.transform.position = position71; }
            if (pos72 == true && choixPlatform == 72) { newItem.transform.position = position72; }
            if (pos73 == true && choixPlatform == 73) { newItem.transform.position = position73; }
            if (pos74 == true && choixPlatform == 74) { newItem.transform.position = position74; }
            if (pos75 == true && choixPlatform == 75) { newItem.transform.position = position75; }
            if (pos76 == true && choixPlatform == 76) { newItem.transform.position = position76; }
            if (pos77 == true && choixPlatform == 77) { newItem.transform.position = position77; }
            if (pos78 == true && choixPlatform == 78) { newItem.transform.position = position78; }
            if (pos79 == true && choixPlatform == 79) { newItem.transform.position = position79; }
            if (pos80 == true && choixPlatform == 80) { newItem.transform.position = position80; }
            if (pos81 == true && choixPlatform == 81) { newItem.transform.position = position81; }

            tAvAppItem = tAvAppItemOringine;
        }

        if (tAvAppItem02 < 0 && choixPlatform != choixPlatform02)
        {
            if (itemDetruit02 == true)
            {
                //deuxième item
                if (itemTimeDiv202 == true) { newItem02 = Instantiate(itemTimeDiv2GO02); }
                if (itemTimeStop02 == true) { newItem02 = Instantiate(itemTimeStopGO02); }
                if (itemInvincibilty02 == true) { newItem02 = Instantiate(itemInvincibiltyGO02); }
                if (itemBonbonScoreRouge02 == true) { newItem02 = Instantiate(itemBonbonScoreRougeGO02); }
                if (itemBonbonScoreVert02 == true) { newItem02 = Instantiate(itemBonbonScoreVertGO02); }
                if (itemRandom02 == true) { newItem02 = Instantiate(itemRandomGO02); }

                itemDetruit02 = false;
            }

            if (pos01 == true && choixPlatform02 == 1) { newItem02.transform.position = position01; }
            if (pos02 == true && choixPlatform02 == 2) { newItem02.transform.position = position02; }
            if (pos03 == true && choixPlatform02 == 3) { newItem02.transform.position = position03; }
            if (pos04 == true && choixPlatform02 == 4) { newItem02.transform.position = position04; }
            if (pos05 == true && choixPlatform02 == 5) { newItem02.transform.position = position05; }
            if (pos06 == true && choixPlatform02 == 6) { newItem02.transform.position = position06; }
            if (pos07 == true && choixPlatform02 == 7) { newItem02.transform.position = position07; }
            if (pos08 == true && choixPlatform02 == 8) { newItem02.transform.position = position08; }
            if (pos09 == true && choixPlatform02 == 9) { newItem02.transform.position = position09; }
            if (pos10 == true && choixPlatform02 == 10) { newItem02.transform.position = position10; }
            if (pos11 == true && choixPlatform02 == 11) { newItem02.transform.position = position11; }
            if (pos12 == true && choixPlatform02 == 12) { newItem02.transform.position = position12; }
            if (pos13 == true && choixPlatform02 == 13) { newItem02.transform.position = position13; }
            if (pos14 == true && choixPlatform02 == 14) { newItem02.transform.position = position14; }
            if (pos15 == true && choixPlatform02 == 15) { newItem02.transform.position = position15; }
            if (pos16 == true && choixPlatform02 == 16) { newItem02.transform.position = position16; }
            if (pos17 == true && choixPlatform02 == 17) { newItem02.transform.position = position17; }
            if (pos18 == true && choixPlatform02 == 18) { newItem02.transform.position = position18; }
            if (pos19 == true && choixPlatform02 == 19) { newItem02.transform.position = position19; }
            if (pos20 == true && choixPlatform02 == 20) { newItem02.transform.position = position20; }
            if (pos21 == true && choixPlatform02 == 21) { newItem02.transform.position = position21; }
            if (pos22 == true && choixPlatform02 == 22) { newItem02.transform.position = position22; }
            if (pos23 == true && choixPlatform02 == 23) { newItem02.transform.position = position23; }
            if (pos24 == true && choixPlatform02 == 24) { newItem02.transform.position = position24; }
            if (pos25 == true && choixPlatform02 == 25) { newItem02.transform.position = position25; }
            if (pos26 == true && choixPlatform02 == 26) { newItem02.transform.position = position26; }
            if (pos27 == true && choixPlatform02 == 27) { newItem02.transform.position = position27; }
            if (pos28 == true && choixPlatform02 == 28) { newItem02.transform.position = position28; }
            if (pos29 == true && choixPlatform02 == 29) { newItem02.transform.position = position29; }
            if (pos30 == true && choixPlatform02 == 30) { newItem02.transform.position = position30; }
            if (pos31 == true && choixPlatform02 == 31) { newItem02.transform.position = position31; }
            if (pos32 == true && choixPlatform02 == 32) { newItem02.transform.position = position32; }
            if (pos33 == true && choixPlatform02 == 33) { newItem02.transform.position = position33; }
            if (pos34 == true && choixPlatform02 == 34) { newItem02.transform.position = position34; }
            if (pos35 == true && choixPlatform02 == 35) { newItem02.transform.position = position35; }
            if (pos36 == true && choixPlatform02 == 36) { newItem02.transform.position = position36; }
            if (pos37 == true && choixPlatform02 == 37) { newItem02.transform.position = position37; }
            if (pos38 == true && choixPlatform02 == 38) { newItem02.transform.position = position38; }
            if (pos39 == true && choixPlatform02 == 39) { newItem02.transform.position = position39; }
            if (pos40 == true && choixPlatform02 == 40) { newItem02.transform.position = position40; }
            if (pos41 == true && choixPlatform02 == 41) { newItem02.transform.position = position41; }
            if (pos42 == true && choixPlatform02 == 42) { newItem02.transform.position = position42; }
            if (pos43 == true && choixPlatform02 == 43) { newItem02.transform.position = position43; }
            if (pos44 == true && choixPlatform02 == 44) { newItem02.transform.position = position44; }
            if (pos45 == true && choixPlatform02 == 45) { newItem02.transform.position = position45; }
            if (pos46 == true && choixPlatform02 == 46) { newItem02.transform.position = position46; }
            if (pos47 == true && choixPlatform02 == 47) { newItem02.transform.position = position47; }
            if (pos48 == true && choixPlatform02 == 48) { newItem02.transform.position = position48; }
            if (pos49 == true && choixPlatform02 == 49) { newItem02.transform.position = position49; }
            if (pos50 == true && choixPlatform02 == 50) { newItem02.transform.position = position50; }
            if (pos51 == true && choixPlatform02 == 51) { newItem02.transform.position = position51; }
            if (pos52 == true && choixPlatform02 == 52) { newItem02.transform.position = position52; }
            if (pos53 == true && choixPlatform02 == 53) { newItem02.transform.position = position53; }
            if (pos54 == true && choixPlatform02 == 54) { newItem02.transform.position = position54; }
            if (pos55 == true && choixPlatform02 == 55) { newItem02.transform.position = position55; }
            if (pos56 == true && choixPlatform02 == 56) { newItem02.transform.position = position56; }
            if (pos57 == true && choixPlatform02 == 57) { newItem02.transform.position = position57; }
            if (pos58 == true && choixPlatform02 == 58) { newItem02.transform.position = position58; }
            if (pos59 == true && choixPlatform02 == 59) { newItem02.transform.position = position59; }
            if (pos60 == true && choixPlatform02 == 60) { newItem02.transform.position = position60; }
            if (pos61 == true && choixPlatform02 == 61) { newItem02.transform.position = position61; }
            if (pos62 == true && choixPlatform02 == 62) { newItem02.transform.position = position62; }
            if (pos63 == true && choixPlatform02 == 63) { newItem02.transform.position = position63; }
            if (pos64 == true && choixPlatform02 == 64) { newItem02.transform.position = position64; }
            if (pos65 == true && choixPlatform02 == 65) { newItem02.transform.position = position65; }
            if (pos66 == true && choixPlatform02 == 66) { newItem02.transform.position = position66; }
            if (pos67 == true && choixPlatform02 == 67) { newItem02.transform.position = position67; }
            if (pos68 == true && choixPlatform02 == 68) { newItem02.transform.position = position68; }
            if (pos69 == true && choixPlatform02 == 69) { newItem02.transform.position = position69; }
            if (pos70 == true && choixPlatform02 == 70) { newItem02.transform.position = position70; }
            if (pos71 == true && choixPlatform02 == 71) { newItem02.transform.position = position71; }
            if (pos72 == true && choixPlatform02 == 72) { newItem02.transform.position = position72; }
            if (pos73 == true && choixPlatform02 == 73) { newItem02.transform.position = position73; }
            if (pos74 == true && choixPlatform02 == 74) { newItem02.transform.position = position74; }
            if (pos75 == true && choixPlatform02 == 75) { newItem02.transform.position = position75; }
            if (pos76 == true && choixPlatform02 == 76) { newItem02.transform.position = position76; }
            if (pos77 == true && choixPlatform02 == 77) { newItem02.transform.position = position77; }
            if (pos78 == true && choixPlatform02 == 78) { newItem02.transform.position = position78; }
            if (pos79 == true && choixPlatform02 == 79) { newItem02.transform.position = position79; }
            if (pos80 == true && choixPlatform02 == 80) { newItem02.transform.position = position80; }
            if (pos81 == true && choixPlatform02 == 81) { newItem02.transform.position = position81; }

            tAvAppItem02 = tAvAppItemOringine02;
        }
    }


        //Ligne01;
        Vector3 position01 = new Vector3(0, 1, 0);
        Vector3 position02 = new Vector3(2, 1, 0);
        Vector3 position03 = new Vector3(4, 1, 0);
        Vector3 position04 = new Vector3(6, 1, 0);
        Vector3 position05 = new Vector3(8, 1, 0);
        Vector3 position06 = new Vector3(10, 1, 0);
        Vector3 position07 = new Vector3(12, 1, 0);
        Vector3 position08 = new Vector3(14, 1, 0);
        Vector3 position09 = new Vector3(16, 1, 0);

        //Ligne02;
        Vector3 position10 = new Vector3(0, 1, 2);
        Vector3 position11 = new Vector3(2, 1, 2);
        Vector3 position12 = new Vector3(4, 1, 2);
        Vector3 position13 = new Vector3(6, 1, 2);
        Vector3 position14 = new Vector3(8, 1, 2);
        Vector3 position15 = new Vector3(10, 1, 2);
        Vector3 position16 = new Vector3(12, 1, 2);
        Vector3 position17 = new Vector3(14, 1, 2);
        Vector3 position18 = new Vector3(16, 1, 2);

        //Ligne03;
        Vector3 position19 = new Vector3(0, 1, 4);
        Vector3 position20 = new Vector3(2, 1, 4);
        Vector3 position21 = new Vector3(4, 1, 4);
        Vector3 position22 = new Vector3(6, 1, 4);
        Vector3 position23 = new Vector3(8, 1, 4);
        Vector3 position24 = new Vector3(10, 1, 4);
        Vector3 position25 = new Vector3(12, 1, 4);
        Vector3 position26 = new Vector3(14, 1, 4);
        Vector3 position27 = new Vector3(16, 1, 4);

        //Ligne04
        Vector3 position28 = new Vector3(0, 1, 6);
        Vector3 position29 = new Vector3(2, 1, 6);
        Vector3 position30 = new Vector3(4, 1, 6);
        Vector3 position31 = new Vector3(6, 1, 6);
        Vector3 position32 = new Vector3(8, 1, 6);
        Vector3 position33 = new Vector3(10, 1, 6);
        Vector3 position34 = new Vector3(12, 1, 6);
        Vector3 position35 = new Vector3(14, 1, 6);
        Vector3 position36 = new Vector3(16, 1, 6);

        //Ligne05
        Vector3 position37 = new Vector3(0, 1, 8);
        Vector3 position38 = new Vector3(2, 1, 8);
        Vector3 position39 = new Vector3(4, 1, 8);
        Vector3 position40 = new Vector3(6, 1, 8);
        Vector3 position41 = new Vector3(8, 1, 8);
        Vector3 position42 = new Vector3(10, 1, 8);
        Vector3 position43 = new Vector3(12, 1, 8);
        Vector3 position44 = new Vector3(14, 1, 8);
        Vector3 position45 = new Vector3(16, 1, 8);

        //Ligne06
        Vector3 position46 = new Vector3(0, 1, 10);
        Vector3 position47 = new Vector3(2, 1, 10);
        Vector3 position48 = new Vector3(4, 1, 10);
        Vector3 position49 = new Vector3(6, 1, 10);
        Vector3 position50 = new Vector3(8, 1, 10);
        Vector3 position51 = new Vector3(10, 1, 10);
        Vector3 position52 = new Vector3(12, 1, 10);
        Vector3 position53 = new Vector3(14, 1, 10);
        Vector3 position54 = new Vector3(16, 1, 10);

        //Ligne07
        Vector3 position55 = new Vector3(0, 1, 12);
        Vector3 position56 = new Vector3(2, 1, 12);
        Vector3 position57 = new Vector3(4, 1, 12);
        Vector3 position58 = new Vector3(6, 1, 12);
        Vector3 position59 = new Vector3(8, 1, 12);
        Vector3 position60 = new Vector3(10, 1, 12);
        Vector3 position61 = new Vector3(12, 1, 12);
        Vector3 position62 = new Vector3(14, 1, 12);
        Vector3 position63 = new Vector3(16, 1, 12);

        //Ligne08
        Vector3 position64 = new Vector3(0, 1, 14);
        Vector3 position65 = new Vector3(2, 1, 14);
        Vector3 position66 = new Vector3(4, 1, 14);
        Vector3 position67 = new Vector3(6, 1, 14);
        Vector3 position68 = new Vector3(8, 1, 14);
        Vector3 position69 = new Vector3(10, 1, 14);
        Vector3 position70 = new Vector3(12, 1, 14);
        Vector3 position71 = new Vector3(14, 1, 14);
        Vector3 position72 = new Vector3(16, 1, 14);

        //Ligne09
        Vector3 position73 = new Vector3(0, 1, 16);
        Vector3 position74 = new Vector3(2, 1, 16);
        Vector3 position75 = new Vector3(4, 1, 16);
        Vector3 position76 = new Vector3(6, 1, 16);
        Vector3 position77 = new Vector3(8, 1, 16);
        Vector3 position78 = new Vector3(10, 1, 16);
        Vector3 position79 = new Vector3(12, 1, 16);
        Vector3 position80 = new Vector3(14, 1, 16);
        Vector3 position81 = new Vector3(16, 1, 16);
    
}
