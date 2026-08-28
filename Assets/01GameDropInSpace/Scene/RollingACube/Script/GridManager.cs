using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int columnLength, forwardLength;
    public float x_Space, z_Space;
    public GameObject cubeGrid;
    public GameObject cubeDestructGrid;

    // Positions des cubes destructibles � conserv�es pour compatibilit� Inspector
    public bool pos01, pos02, pos03, pos04, pos05, pos06, pos07, pos08, pos09;
    public bool pos10, pos11, pos12, pos13, pos14, pos15, pos16, pos17, pos18, pos19;
    public bool pos20, pos21, pos22, pos23, pos24, pos25, pos26, pos27, pos28, pos29;
    public bool pos30, pos31, pos32, pos33, pos34, pos35, pos36, pos37, pos38, pos39;
    public bool pos40, pos41, pos42, pos43, pos44, pos45, pos46, pos47, pos48, pos49;
    public bool pos50, pos51, pos52, pos53, pos54, pos55, pos56, pos57, pos58, pos59;
    public bool pos60, pos61, pos62, pos63, pos64, pos65, pos66, pos67, pos68, pos69;
    public bool pos70, pos71, pos72, pos73, pos74, pos75, pos76, pos77, pos78, pos79;
    public bool pos80, pos81;

    // CORRIG� : positions en tableau au lieu de 81 variables
    private static readonly Vector3[] positions = new Vector3[]
    {
        new Vector3(0,20,0),  new Vector3(2,20,0),  new Vector3(4,20,0),  new Vector3(6,20,0),  new Vector3(8,20,0),  new Vector3(10,20,0), new Vector3(12,20,0), new Vector3(14,20,0), new Vector3(16,20,0),
        new Vector3(0,20,2),  new Vector3(2,20,2),  new Vector3(4,20,2),  new Vector3(6,20,2),  new Vector3(8,20,2),  new Vector3(10,20,2), new Vector3(12,20,2), new Vector3(14,20,2), new Vector3(16,20,2),
        new Vector3(0,20,4),  new Vector3(2,20,4),  new Vector3(4,20,4),  new Vector3(6,20,4),  new Vector3(8,20,4),  new Vector3(10,20,4), new Vector3(12,20,4), new Vector3(14,20,4), new Vector3(16,20,4),
        new Vector3(0,20,6),  new Vector3(2,20,6),  new Vector3(4,20,6),  new Vector3(6,20,6),  new Vector3(8,20,6),  new Vector3(10,20,6), new Vector3(12,20,6), new Vector3(14,20,6), new Vector3(16,20,6),
        new Vector3(0,20,8),  new Vector3(2,20,8),  new Vector3(4,20,8),  new Vector3(6,20,8),  new Vector3(8,20,8),  new Vector3(10,20,8), new Vector3(12,20,8), new Vector3(14,20,8), new Vector3(16,20,8),
        new Vector3(0,20,10), new Vector3(2,20,10), new Vector3(4,20,10), new Vector3(6,20,10), new Vector3(8,20,10), new Vector3(10,20,10),new Vector3(12,20,10),new Vector3(14,20,10),new Vector3(16,20,10),
        new Vector3(0,20,12), new Vector3(2,20,12), new Vector3(4,20,12), new Vector3(6,20,12), new Vector3(8,20,12), new Vector3(10,20,12),new Vector3(12,20,12),new Vector3(14,20,12),new Vector3(16,20,12),
        new Vector3(0,20,14), new Vector3(2,20,14), new Vector3(4,20,14), new Vector3(6,20,14), new Vector3(8,20,14), new Vector3(10,20,14),new Vector3(12,20,14),new Vector3(14,20,14),new Vector3(16,20,14),
        new Vector3(0,20,16), new Vector3(2,20,16), new Vector3(4,20,16), new Vector3(6,20,16), new Vector3(8,20,16), new Vector3(10,20,16),new Vector3(12,20,16),new Vector3(14,20,16),new Vector3(16,20,16)
    };

    void Start()
    {

    



        // Remet la gravit� � la normale apr�s le hub
        Physics.gravity = new Vector3(0, -9.81f, 0);
        Debug.Log($"[GridManager] Gravité définie : {Physics.gravity}");

        // G�n�re la grille principale
        for (int i = 0; i < columnLength * forwardLength; i++)
            Instantiate(cubeGrid, new Vector3(x_Space * (i % columnLength), 0, z_Space * (i / columnLength)), Quaternion.identity);

        // CORRIG� : tableau de bools pour �viter 81 if s�par�s
        bool[] posArray = new bool[]
        {
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

        for (int i = 0; i < posArray.Length; i++)
        {
            if (posArray[i])
            {
                GameObject obj = Instantiate(cubeDestructGrid);
                obj.transform.position = positions[i];
            }
        }
    }
}