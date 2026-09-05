using UnityEngine;

public class FicheDrop : MonoBehaviour
{
    string dropName = "Drop";
    int pdvDrop = 5;
    float vitesseAuSol = 5f;
    bool peutNager = true;

    void Start()
    {
        string message = $"{dropName} | PV: {pdvDrop} | Vitesse: {vitesseAuSol} | Nage: {peutNager}";
        Debug.Log(message);
        Debug.Log($"Vitesse entière : {(int)vitesseAuSol}");
    }
}

/*using UnityEngine;
using System;

public class ConsoleWriteLumo : MonoBehaviour
{
    string DropName = "Drop";
    int pdvDrop = 5;
    float speedOnGround = 5;
    bool canSwimm  = false;
    string message;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string message = $"msg : {DropName}{pdvDrop}{speedOnGround}{canSwimm}";   
        Console.WriteLine(message);
        Debug.Log(message);
    }
}*/
