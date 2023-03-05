using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemCollisionDIP : MonoBehaviour
{
    public GameObject item;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "itemTimeDiv202") item.GetComponent<itemDropInSpace>().itemDetruitF();
        if (other.gameObject.tag == "itemTimeStop02") item.GetComponent<itemDropInSpace>().itemDetruitF();
        if (other.gameObject.tag == "itemInvincibilty02") item.GetComponent<itemDropInSpace>().itemDetruitF();
        if (other.gameObject.tag == "itemBonbonScoreRouge02") item.GetComponent<itemDropInSpace>().itemDetruitF();
        if (other.gameObject.tag == "itemBonbonScoreVert02") item.GetComponent<itemDropInSpace>().itemDetruitF();
        if (other.gameObject.tag == "itemRandom02") item.GetComponent<itemDropInSpace>().itemDetruitF();    
    }
}
