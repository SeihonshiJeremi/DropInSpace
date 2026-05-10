using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class comeOnBoy : MonoBehaviour
{
    public AudioSource audioData01;
    public AudioSource audioData02;
    private int random;
    private float timer = 2.0f;

    private DIS_Controls controls;

    void Awake()
    {
        controls = new DIS_Controls();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Dash.started += ctx => JouerSon();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

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
    }

    private void JouerSon()
    {
        if (random == 1) audioData01.Play();
        if (random == 2) audioData02.Play();
    }
}