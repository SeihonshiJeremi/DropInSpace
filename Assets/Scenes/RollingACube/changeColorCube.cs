using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColorCube : MonoBehaviour
{
    public Material m_Material02;

    public GameObject zoomManager;
    public int value;

    public bool changeColorOK = false;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" && changeColorOK == false)
        {
            GetComponent<Renderer>().material = m_Material02;
            Debug.Log("changeCubeMaterial");


            zoomManager.GetComponent<zoomManager>().finLevel(value);
            changeColorOK = true;
        }

        if (collision.collider.tag == "Player" && changeColorOK == true)
        {

            GetComponent<Renderer>().material = m_Material02;
            Debug.Log("changeCubeMaterial");
            //valeur qui augmente pour finir le niveau
            //zoomManager.GetComponent<zoomManager>().reduceFinLevel(value);
        }

        if (collision.collider.tag == "destructorPlatform")
        {
            Destroy(gameObject);
        }
    }
}
