using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationRolling : MonoBehaviour
{
    public Transform pointTP;
    public GameObject Player;
    public Transform pointTPMonster;
    public GameObject Monster;
    public Transform pointTPMonster02;
    public GameObject Monster02;

    // CORRIGÉ : cache des GetComponent
    private rollingCube playerRolling;
    private swipeMovementDIS swipe;

    void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) playerRolling = playerObj.GetComponent<rollingCube>();

        GameObject swipeObj = GameObject.Find("swipeMovement");
        if (swipeObj != null) swipe = swipeObj.GetComponent<swipeMovementDIS>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRolling != null) playerRolling.tDesacTouche();
            if (swipe != null) swipe.tDesacTouche();
            Player.transform.position = pointTP.position;
        }

        // CORRIGÉ : Monster et MonsterChangeColor fusionnés
        if (other.CompareTag("Monster") || other.CompareTag("MonsterChangeColor"))
            Monster.transform.position = pointTPMonster.position;

        if (other.CompareTag("Monster02"))
            Monster02.transform.position = pointTPMonster02.position;
    }
}