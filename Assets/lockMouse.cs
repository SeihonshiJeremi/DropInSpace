using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockMouse : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseLock;

    void Awake()
    {
        DontDestroyOnLoad(mouseLock);
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


}
