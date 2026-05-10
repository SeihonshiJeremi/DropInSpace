using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pushButtonStart : MonoBehaviour
{
    public string scene;

    [Header("Keybinds")]
    public KeyCode startButton = KeyCode.Space;

    void Update()
    {
        // CORRIGÉ : utilise la variable scene au lieu du string hardcodé "rollingAcube"
        if (Input.GetKeyDown(startButton)) SceneManager.LoadScene(scene);
    }
}