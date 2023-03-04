using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScore : MonoBehaviour
{
    public string sceneRetour;
    public string sceneReset;

    public void retour()
    {
        SceneManager.LoadScene(sceneRetour);
    }

    public void reset()
    {
        SceneManager.LoadScene(sceneReset);
    }
}
