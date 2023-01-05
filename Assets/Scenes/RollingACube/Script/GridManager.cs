using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class GridManager : MonoBehaviour
{
    public int columnLength, forwardLength;
    public float x_Space, z_Space;
    public GameObject cubeGrid;
    public GameObject cubeDestructGrid;

    public bool pos01, pos02, pos03, pos04, pos05, pos06, pos07, pos08, pos09;
    public bool pos10, pos11, pos12, pos13, pos14, pos15, pos16, pos17, pos18, pos19;
    public bool pos20, pos21, pos22, pos23, pos24, pos25, pos26, pos27, pos28, pos29;
    public bool pos30, pos31, pos32, pos33, pos34, pos35, pos36, pos37, pos38, pos39;
    public bool pos40, pos41, pos42, pos43, pos44, pos45, pos46, pos47, pos48, pos49;
    public bool pos50, pos51, pos52, pos53, pos54, pos55, pos56, pos57, pos58, pos59;
    public bool pos60, pos61, pos62, pos63, pos64, pos65, pos66, pos67, pos68, pos69;
    public bool pos70, pos71, pos72, pos73, pos74, pos75, pos76, pos77, pos78, pos79;
    public bool pos80, pos81;


    void Start()
    {
        //cubeGrid
        for(int i = 0; i < columnLength * forwardLength; i++)
        {
            Instantiate(cubeGrid, new Vector3(x_Space * (i % columnLength), 0, z_Space * (i / columnLength)), Quaternion.identity);
        }

        //cubeDestructGrid
        if (pos01 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position01; }
        if (pos02 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position02; }
        if (pos03 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position03; }
        if (pos04 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position04; }
        if (pos05 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position05; }
        if (pos06 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position06; }
        if (pos07 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position07; }
        if (pos08 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position08; }
        if (pos09 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position09; }
        if (pos10 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position10; }
        if (pos11 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position11; }
        if (pos12 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position12; }
        if (pos13 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position13; }
        if (pos14 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position14; }
        if (pos15 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position15; }
        if (pos16 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position16; }
        if (pos17 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position17; }
        if (pos18 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position18; }
        if (pos19 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position19; }
        if (pos20 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position20; }
        if (pos21 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position21; }
        if (pos22 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position22; }
        if (pos23 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position23; }
        if (pos24 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position24; }
        if (pos25 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position25; }
        if (pos26 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position26; }
        if (pos27 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position27; }
        if (pos28 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position28; }
        if (pos29 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position29; }
        if (pos30 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position30; }
        if (pos31 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position31; }
        if (pos32 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position32; }
        if (pos33 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position33; }
        if (pos34 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position34; }
        if (pos35 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position35; }
        if (pos36 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position36; }
        if (pos37 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position37; }
        if (pos38 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position38; }
        if (pos39 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position39; }
        if (pos40 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position40; }
        if (pos41 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position41; }
        if (pos42 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position42; }
        if (pos43 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position43; }
        if (pos44 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position44; }
        if (pos45 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position45; }
        if (pos46 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position46; }
        if (pos47 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position47; }
        if (pos48 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position48; }
        if (pos49 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position49; }
        if (pos50 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position50; }
        if (pos51 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position51; }
        if (pos52 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position52; }
        if (pos53 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position53; }
        if (pos54 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position54; }
        if (pos55 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position55; }
        if (pos56 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position56; }
        if (pos57 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position57; }
        if (pos58 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position58; }
        if (pos59 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position59; }
        if (pos60 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position60; }
        if (pos61 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position61; }
        if (pos62 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position62; }
        if (pos63 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position63; }
        if (pos64 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position64; }
        if (pos65 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position65; }
        if (pos66 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position66; }
        if (pos67 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position67; }
        if (pos68 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position68; }
        if (pos69 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position69; }
        if (pos70 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position70; }
        if (pos71 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position71; }
        if (pos72 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position72; }
        if (pos73 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position73; }
        if (pos74 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position74; }
        if (pos75 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position75; }
        if (pos76 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position76; }
        if (pos77 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position77; }
        if (pos78 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position78; }
        if (pos79 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position79; }
        if (pos80 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position80; }
        if (pos81 == true) { GameObject newGameObject = Instantiate(cubeDestructGrid); newGameObject.transform.position = position81; }


        for (int i = 0; i < 1; i++)
        {
            //Instantiate(cubeDestructGrid, new Vector3(0, 0, 0), Quaternion.identity);
            
        }
    }

    //Ligne01;
    Vector3 position01 = new Vector3(0, 20, 0);
    Vector3 position02 = new Vector3(2, 20, 0);
    Vector3 position03 = new Vector3(4, 20, 0);
    Vector3 position04 = new Vector3(6, 20, 0);
    Vector3 position05 = new Vector3(8, 20, 0);
    Vector3 position06 = new Vector3(10, 20, 0);
    Vector3 position07 = new Vector3(12, 20, 0);
    Vector3 position08 = new Vector3(14, 20, 0);
    Vector3 position09 = new Vector3(16, 20, 0);

    //Ligne02;
    Vector3 position10 = new Vector3(0, 20, 2);
    Vector3 position11 = new Vector3(2, 20, 2);
    Vector3 position12 = new Vector3(4, 20, 2);
    Vector3 position13 = new Vector3(6, 20, 2);
    Vector3 position14 = new Vector3(8, 20, 2);
    Vector3 position15 = new Vector3(10, 20, 2);
    Vector3 position16 = new Vector3(12, 20, 2);
    Vector3 position17 = new Vector3(14, 20, 2);
    Vector3 position18 = new Vector3(16, 20, 2);

    //Ligne03;
    Vector3 position19 = new Vector3(0, 20, 4);
    Vector3 position20 = new Vector3(2, 20, 4);
    Vector3 position21 = new Vector3(4, 20, 4);
    Vector3 position22 = new Vector3(6, 20, 4);
    Vector3 position23 = new Vector3(8, 20, 4);
    Vector3 position24 = new Vector3(10, 20, 4);
    Vector3 position25 = new Vector3(12, 20, 4);
    Vector3 position26 = new Vector3(14, 20, 4);
    Vector3 position27 = new Vector3(16, 20, 4);

    //Ligne04
    Vector3 position28 = new Vector3(0, 20, 6);
    Vector3 position29 = new Vector3(2, 20, 6);
    Vector3 position30 = new Vector3(4, 20, 6);
    Vector3 position31 = new Vector3(6, 20, 6);
    Vector3 position32 = new Vector3(8, 20, 6);
    Vector3 position33 = new Vector3(10, 20, 6);
    Vector3 position34 = new Vector3(12, 20, 6);
    Vector3 position35 = new Vector3(14, 20, 6);
    Vector3 position36 = new Vector3(16, 20, 6);

    //Ligne05
    Vector3 position37 = new Vector3(0, 20, 8);
    Vector3 position38 = new Vector3(2, 20, 8);
    Vector3 position39 = new Vector3(4, 20, 8);
    Vector3 position40 = new Vector3(6, 20, 8);
    Vector3 position41 = new Vector3(8, 20, 8);
    Vector3 position42 = new Vector3(10, 20, 8);
    Vector3 position43 = new Vector3(12, 20, 8);
    Vector3 position44 = new Vector3(14, 20, 8);
    Vector3 position45 = new Vector3(16, 20, 8);

    //Ligne06
    Vector3 position46 = new Vector3(0, 20, 10);
    Vector3 position47 = new Vector3(2, 20, 10);
    Vector3 position48 = new Vector3(4, 20, 10);
    Vector3 position49 = new Vector3(6, 20, 10);
    Vector3 position50 = new Vector3(8, 20, 10);
    Vector3 position51 = new Vector3(10, 20, 10);
    Vector3 position52 = new Vector3(12, 20, 10);
    Vector3 position53 = new Vector3(14, 20, 10);
    Vector3 position54 = new Vector3(16, 20, 10);

    //Ligne07
    Vector3 position55 = new Vector3(0, 20, 12);
    Vector3 position56 = new Vector3(2, 20, 12);
    Vector3 position57 = new Vector3(4, 20, 12);
    Vector3 position58 = new Vector3(6, 20, 12);
    Vector3 position59 = new Vector3(8, 20, 12);
    Vector3 position60 = new Vector3(10, 20, 12);
    Vector3 position61 = new Vector3(12, 20, 12);
    Vector3 position62 = new Vector3(14, 20, 12);
    Vector3 position63 = new Vector3(16, 20, 12);

    //Ligne08
    Vector3 position64 = new Vector3(0, 20, 14);
    Vector3 position65 = new Vector3(2, 20, 14);
    Vector3 position66 = new Vector3(4, 20, 14);
    Vector3 position67 = new Vector3(6, 20, 14);
    Vector3 position68 = new Vector3(8, 20, 14);
    Vector3 position69 = new Vector3(10, 20, 14);
    Vector3 position70 = new Vector3(12, 20, 14);
    Vector3 position71 = new Vector3(14, 20, 14);
    Vector3 position72 = new Vector3(16, 20, 14);

    //Ligne09
    Vector3 position73 = new Vector3(0, 20, 16);
    Vector3 position74 = new Vector3(2, 20, 16);
    Vector3 position75 = new Vector3(4, 20, 16);
    Vector3 position76 = new Vector3(6, 20, 16);
    Vector3 position77 = new Vector3(8, 20, 16);
    Vector3 position78 = new Vector3(10, 20, 16);
    Vector3 position79 = new Vector3(12, 20, 16);
    Vector3 position80 = new Vector3(14, 20, 16);
    Vector3 position81 = new Vector3(16, 20, 16);
}

[System.Serializable]
public class Coordinate
{

    

   
}




