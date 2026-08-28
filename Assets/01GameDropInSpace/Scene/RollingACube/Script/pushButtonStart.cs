using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class pushButtonStart : MonoBehaviour
{
    public string scene;

    void Update()
    {
        // Nouveau Input System — remplace Input.GetKeyDown
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            SceneManager.LoadScene(scene);

        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
            SceneManager.LoadScene(scene);
    }
}