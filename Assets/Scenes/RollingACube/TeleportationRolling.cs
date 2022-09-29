using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationRolling : MonoBehaviour
{
    public Transform pointTP;
    public GameObject Player;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            Vector3 newPosition;
            newPosition = pointTP.position;
            Player.transform.position = newPosition;
        }

    }
}
