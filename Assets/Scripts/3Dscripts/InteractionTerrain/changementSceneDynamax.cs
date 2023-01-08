using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changementSceneDynamax : MonoBehaviour
{
    public string scene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //SceneManager.LoadScene(scene);
             SceneManager.LoadSceneAsync(scene);
        }
    }
}
