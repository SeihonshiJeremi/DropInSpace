using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarManager : MonoBehaviour
{
    public GameObject CanvasLifeBar;
    public bool activationBarLife;
    public GameObject ImageLife01;
    public GameObject ImageLife02;
    public GameObject ImageLife03;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
      

    }

    // Update is called once per frame
    void Update()
    {
        //CanvasLifeBar.SetActive(true);
        if (activationBarLife == true) CanvasLifeBar.GetComponent<Canvas>().enabled = true;

        if (activationBarLife == false) CanvasLifeBar.GetComponent<Canvas>().enabled = false;
    }

    public void desactivationLifeBar()
    {
        activationBarLife = false;        
    }

    public void activationLifeBar()
    {
        activationBarLife = true;        
    }
}
