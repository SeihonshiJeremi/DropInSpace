using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemCollisionDIP : MonoBehaviour
{
    public GameObject item;

    // CORRIGÉ : cache GetComponent
    private itemDropInSpace itemDIS;

    void Start()
    {
        if (item != null) itemDIS = item.GetComponent<itemDropInSpace>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (itemDIS == null) return;

        // CORRIGÉ : CompareTag + appel unique via cache
        if (other.CompareTag("itemTimeDiv202") ||
            other.CompareTag("itemTimeStop02") ||
            other.CompareTag("itemInvincibilty02") ||
            other.CompareTag("itemBonbonScoreRouge02") ||
            other.CompareTag("itemBonbonScoreVert02") ||
            other.CompareTag("itemRandom02"))
        {
            itemDIS.itemDetruitF();
        }
    }
}