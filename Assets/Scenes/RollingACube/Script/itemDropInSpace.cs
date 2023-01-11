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

    //apparition de l'item
    public int choixPlatform;
    public GameObject item;
    public float tAvAppItem;
    public float tAvAppItemOringine;
    public bool pos01, pos02, pos03, pos04, pos05, pos06, pos07, pos08, pos09;
    public bool pos10, pos11, pos12, pos13, pos14, pos15, pos16, pos17, pos18, pos19;
    public bool pos20, pos21, pos22, pos23, pos24, pos25, pos26, pos27, pos28, pos29;
    public bool pos30, pos31, pos32, pos33, pos34, pos35, pos36, pos37, pos38, pos39;
    public bool pos40, pos41, pos42, pos43, pos44, pos45, pos46, pos47, pos48, pos49;
    public bool pos50, pos51, pos52, pos53, pos54, pos55, pos56, pos57, pos58, pos59;
    public bool pos60, pos61, pos62, pos63, pos64, pos65, pos66, pos67, pos68, pos69;
    public bool pos70, pos71, pos72, pos73, pos74, pos75, pos76, pos77, pos78, pos79;
    public bool pos80, pos81;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && itemTimeDiv2 == true)
        {

        }

        if (other.gameObject.tag == "Player" && itemTimeStop == true)
        {

        }

        if (other.gameObject.tag == "Player" && itemInvincibilty == true)
        {

        }

        if (other.gameObject.tag == "Player" && itemBonbonScoreRouge == true)
        {

        }

        if (other.gameObject.tag == "Player" && itemBonbonScoreVert == true)
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tAvAppItem -= Time.deltaTime;
        choixPlatform = Random.Range(1, 82);
        if (tAvAppItem < 0)
        {
            if (pos01 == true && choixPlatform == 1) { item.transform.position = position01; }
            if (pos02 == true && choixPlatform == 2) { item.transform.position = position02; }
            if (pos03 == true && choixPlatform == 3) { item.transform.position = position03; }
            if (pos04 == true && choixPlatform == 4) { item.transform.position = position04; }
            if (pos05 == true && choixPlatform == 5) { item.transform.position = position05; }
            if (pos06 == true && choixPlatform == 6) { item.transform.position = position06; }
            if (pos07 == true && choixPlatform == 7) { item.transform.position = position07; }
            if (pos08 == true && choixPlatform == 8) { item.transform.position = position08; }
            if (pos09 == true && choixPlatform == 9) { item.transform.position = position09; }
            if (pos10 == true && choixPlatform == 10) { item.transform.position = position10; }
            if (pos11 == true && choixPlatform == 11) { item.transform.position = position11; }
            if (pos12 == true && choixPlatform == 12) { item.transform.position = position12; }
            if (pos13 == true && choixPlatform == 13) { item.transform.position = position13; }
            if (pos14 == true && choixPlatform == 14) { item.transform.position = position14; }
            if (pos15 == true && choixPlatform == 15) { item.transform.position = position15; }
            if (pos16 == true && choixPlatform == 16) { item.transform.position = position16; }
            if (pos17 == true && choixPlatform == 17) { item.transform.position = position17; }
            if (pos18 == true && choixPlatform == 18) { item.transform.position = position18; }
            if (pos19 == true && choixPlatform == 19) { item.transform.position = position19; }
            if (pos20 == true && choixPlatform == 20) { item.transform.position = position20; }
            if (pos21 == true && choixPlatform == 21) { item.transform.position = position21; }
            if (pos22 == true && choixPlatform == 22) { item.transform.position = position22; }
            if (pos23 == true && choixPlatform == 23) { item.transform.position = position23; }
            if (pos24 == true && choixPlatform == 24) { item.transform.position = position24; }
            if (pos25 == true && choixPlatform == 25) { item.transform.position = position25; }
            if (pos26 == true && choixPlatform == 26) { item.transform.position = position26; }
            if (pos27 == true && choixPlatform == 27) { item.transform.position = position27; }
            if (pos28 == true && choixPlatform == 28) { item.transform.position = position28; }
            if (pos29 == true && choixPlatform == 29) { item.transform.position = position29; }
            if (pos30 == true && choixPlatform == 30) { item.transform.position = position30; }
            if (pos31 == true && choixPlatform == 31) { item.transform.position = position31; }
            if (pos32 == true && choixPlatform == 32) { item.transform.position = position32; }
            if (pos33 == true && choixPlatform == 33) { item.transform.position = position33; }
            if (pos34 == true && choixPlatform == 34) { item.transform.position = position34; }
            if (pos35 == true && choixPlatform == 35) { item.transform.position = position35; }
            if (pos36 == true && choixPlatform == 36) { item.transform.position = position36; }
            if (pos37 == true && choixPlatform == 37) { item.transform.position = position37; }
            if (pos38 == true && choixPlatform == 38) { item.transform.position = position38; }
            if (pos39 == true && choixPlatform == 39) { item.transform.position = position39; }
            if (pos40 == true && choixPlatform == 40) { item.transform.position = position40; }
            if (pos41 == true && choixPlatform == 41) { item.transform.position = position41; }
            if (pos42 == true && choixPlatform == 42) { item.transform.position = position42; }
            if (pos43 == true && choixPlatform == 43) { item.transform.position = position43; }
            if (pos44 == true && choixPlatform == 44) { item.transform.position = position44; }
            if (pos45 == true && choixPlatform == 45) { item.transform.position = position45; }
            if (pos46 == true && choixPlatform == 46) { item.transform.position = position46; }
            if (pos47 == true && choixPlatform == 47) { item.transform.position = position47; }
            if (pos48 == true && choixPlatform == 48) { item.transform.position = position48; }
            if (pos49 == true && choixPlatform == 49) { item.transform.position = position49; }
            if (pos50 == true && choixPlatform == 50) { item.transform.position = position50; }
            if (pos51 == true && choixPlatform == 51) { item.transform.position = position51; }
            if (pos52 == true && choixPlatform == 52) { item.transform.position = position52; }
            if (pos53 == true && choixPlatform == 53) { item.transform.position = position53; }
            if (pos54 == true && choixPlatform == 54) { item.transform.position = position54; }
            if (pos55 == true && choixPlatform == 55) { item.transform.position = position55; }
            if (pos56 == true && choixPlatform == 56) { item.transform.position = position56; }
            if (pos57 == true && choixPlatform == 57) { item.transform.position = position57; }
            if (pos58 == true && choixPlatform == 58) { item.transform.position = position58; }
            if (pos59 == true && choixPlatform == 59) { item.transform.position = position59; }
            if (pos60 == true && choixPlatform == 60) { item.transform.position = position60; }
            if (pos61 == true && choixPlatform == 61) { item.transform.position = position61; }
            if (pos62 == true && choixPlatform == 62) { item.transform.position = position62; }
            if (pos63 == true && choixPlatform == 63) { item.transform.position = position63; }
            if (pos64 == true && choixPlatform == 64) { item.transform.position = position64; }
            if (pos65 == true && choixPlatform == 65) { item.transform.position = position65; }
            if (pos66 == true && choixPlatform == 66) { item.transform.position = position66; }
            if (pos67 == true && choixPlatform == 67) { item.transform.position = position67; }
            if (pos68 == true && choixPlatform == 68) { item.transform.position = position68; }
            if (pos69 == true && choixPlatform == 69) { item.transform.position = position69; }
            if (pos70 == true && choixPlatform == 70) { item.transform.position = position70; }
            if (pos71 == true && choixPlatform == 71) { item.transform.position = position71; }
            if (pos72 == true && choixPlatform == 72) { item.transform.position = position72; }
            if (pos73 == true && choixPlatform == 73) { item.transform.position = position73; }
            if (pos74 == true && choixPlatform == 74) { item.transform.position = position74; }
            if (pos75 == true && choixPlatform == 75) { item.transform.position = position75; }
            if (pos76 == true && choixPlatform == 76) { item.transform.position = position76; }
            if (pos77 == true && choixPlatform == 77) { item.transform.position = position77; }
            if (pos78 == true && choixPlatform == 78) { item.transform.position = position78; }
            if (pos79 == true && choixPlatform == 79) { item.transform.position = position79; }
            if (pos80 == true && choixPlatform == 80) { item.transform.position = position80; }
            if (pos81 == true && choixPlatform == 81) { item.transform.position = position81; }

            tAvAppItem = tAvAppItemOringine;
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
