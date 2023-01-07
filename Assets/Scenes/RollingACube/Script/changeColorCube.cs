using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColorCube : MonoBehaviour
{
    public Material m_Material01;
    public Material m_Material02;

    public GameObject zoomManager;
    public int valueDalle;
    private int valueScore = 10;
    private int valueScoreDouble = 20;

    private int activeDouble = 1;

    public bool changeColorOK = false;

    void Awake()
    {
        zoomManager = GameObject.Find("ZoomManager");
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" && changeColorOK == false)
        {
            GetComponent<Renderer>().material = m_Material02;
            zoomManager.GetComponent<zoomManager>().reduceFinLevel(valueDalle);
            zoomManager.GetComponent<zoomManager>().scoreAdd(valueScore);
            zoomManager.GetComponent<zoomManager>().scoreDoubleTrue(activeDouble);
            changeColorOK = true;
            GetComponent<AudioSource>().Play();
        }
        else zoomManager.GetComponent<zoomManager>().scoreDoubleFalse(activeDouble);



        /*if (collision.collider.tag == "Player" && changeColorOK == true)
        {
            //zoomManager.GetComponent<zoomManager>().scoreDoubleFalse(activeDouble);
            Debug.Log("salut");
        }*/

        if (collision.collider.tag == "destructorPlatform")
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "MonsterChangeColor" && changeColorOK == true)
        {
            GetComponent<Renderer>().material = m_Material01;
            zoomManager.GetComponent<zoomManager>().addFinLevel(valueDalle);
            changeColorOK = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "destructorPlatform")
        {
            Destroy(gameObject);
        }
    }
}
