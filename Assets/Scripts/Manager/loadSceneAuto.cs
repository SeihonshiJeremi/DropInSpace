using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadSceneAuto : MonoBehaviour
{
    public string scene;
    public float loadSceneTime;

    void Update()
    {
        loadSceneTime -= Time.deltaTime;
        if (loadSceneTime < 0)
        {
            SceneManager.LoadScene(scene);
            // SceneManager.LoadSceneAsync(scene);
        }
    }
}
