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
            // MODE INFINI : Game Over sauf si respawn en cours
            if (InfiniteModeManager.Instance != null && InfiniteModeManager.Instance.EstEnJeu())
            {
                if (InfiniteGridManager.Instance != null && InfiniteGridManager.Instance.RespawnEnCours())
                    return;

                InfiniteModeManager.Instance.DeclencherGameOver();
                return;
            }

            // Comportement normal
            if (playerRolling != null) playerRolling.tDesacTouche();
            if (swipe != null) swipe.tDesacTouche();
            Player.transform.position = pointTP.position;
        }

        if (other.CompareTag("Monster") || other.CompareTag("MonsterChangeColor"))
            Monster.transform.position = pointTPMonster.position;
        if (other.CompareTag("Monster02"))
            Monster02.transform.position = pointTPMonster02.position;
    }
}