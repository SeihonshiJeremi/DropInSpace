using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadSceneAuto : MonoBehaviour
{
    public string scene;
    public float loadSceneTime;
    public bool okChange = false;

    void Update()
    {
        if(okChange == true)
        {
            loadSceneTime -= Time.deltaTime;
            if (loadSceneTime < 0)
            {
                SceneManager.LoadScene(scene);
                // SceneManager.LoadSceneAsync(scene);
            }
        }

    }

    public void boolScene()
    {
        okChange = true;
    }
}
