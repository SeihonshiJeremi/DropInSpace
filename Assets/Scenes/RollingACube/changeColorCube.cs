using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColorCube : MonoBehaviour
{
    public Material m_Material02;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            GetComponent<Renderer>().material = m_Material02;
            Debug.Log("changeCubeMaterial");
        }

        if (collision.collider.tag == "destructorPlatform")
        {
            Destroy(gameObject);
        }
    }
}
