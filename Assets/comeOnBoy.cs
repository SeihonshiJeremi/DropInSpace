using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comeOnBoy : MonoBehaviour
{
    public AudioSource audioData01;
    public AudioSource audioData02;
    int random;
    float timer = 2.0f;

    void Start()
    {
        random = Random.Range(1, 3);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            random = Random.Range(1, 3);
            timer = 2.0f;
        } 
        if (Input.GetButton("Dash") && random == 1) audioData01.Play();
        if (Input.GetButton("Dash") && random == 2) audioData02.Play();
    }
}
