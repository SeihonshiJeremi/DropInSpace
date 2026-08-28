using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class piecePickup : MonoBehaviour
{
    public int value;
    public GameObject PiecePickupEffect;
    public GameObject CollectibleManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CollectibleManager.GetComponent<collectibleManager>().AddPiece(value);
            Instantiate(PiecePickupEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
