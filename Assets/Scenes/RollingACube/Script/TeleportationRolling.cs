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

    public string tag;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            GameObject.Find("Player").GetComponent<rollingCube>().tDesacTouche();
            Vector3 newPosition;
            newPosition = pointTP.position;
            Player.transform.position = newPosition;            
        }

        if (other.gameObject.tag == "Monster")
        {
            Vector3 newPosition;
            newPosition = pointTPMonster.position;
            Monster.transform.position = newPosition;
        }

        if (other.gameObject.tag == "Monster02")
        {
            Vector3 newPosition;
            newPosition = pointTPMonster02.position;
            Monster02.transform.position = newPosition;
        }

        if (other.gameObject.tag == tag)
        {
            Vector3 newPosition;
            newPosition = pointTPMonster02.position;
            Monster02.transform.position = newPosition;
        }

    }
}
