using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pushButtonStart : MonoBehaviour
{
    public string scene;

    [Header("keybinds")]
    public KeyCode startButton = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   public  void Update()
    {

        if (Input.GetKeyDown(startButton)) SceneManager.LoadScene("rollingAcube");


    }
}
