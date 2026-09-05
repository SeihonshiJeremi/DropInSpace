using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideController : MonoBehaviour
{
    [SerializeField]private GameObject glideModel;
    [SerializeField]private Rigidbody _rb;
    [SerializeField]private float _horizontal;
    [SerializeField]private float _vertical;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       _rb = GetComponent<Rigidbody>();
       Debug.Log(_rb);
    }

    void Update()
    {
            Debug.Log(_horizontal + " " + _vertical);
    }        

    // Update le jeu accelere en fonction de la machine
    //FixedUpdate le même pour tout le monde
    void FixedUpdate()
    {
        
    }
}
