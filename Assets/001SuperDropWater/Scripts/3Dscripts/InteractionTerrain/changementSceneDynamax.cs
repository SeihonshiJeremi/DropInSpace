using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changementSceneDynamax : MonoBehaviour
{
    public string scene;
    public bool avCollision;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //SceneManager.LoadScene(scene);
             SceneManager.LoadSceneAsync(scene);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(avCollision == true)
        {
            SceneManager.LoadSceneAsync(scene);
        }
    }
}
