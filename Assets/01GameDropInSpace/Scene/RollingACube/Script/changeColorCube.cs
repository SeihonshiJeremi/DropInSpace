using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColorCube : MonoBehaviour
{
    public Material m_Material01;
    public Material m_Material02;
    public int valueDalle;

    private int valueScore = 10;
    private int activeDouble = 1;
    public bool changeColorOK = false;

    // CORRIGÉ : cache des composants
    private zoomManager zm;
    private Renderer cubeRenderer;
    private AudioSource audioSource;

    private float timer = 5.0f;
    private bool destructorHandled = false;

    void Awake()
    {
        // CORRIGÉ : singleton au lieu de GameObject.Find à chaque cube
        zm = zoomManager.Instance;
        cubeRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !changeColorOK)
        {
            cubeRenderer.material = m_Material02;
            zm.reduceFinLevel(valueDalle);
            zm.scoreAdd(valueScore);
            zm.scoreDoubleTrue(activeDouble);
            changeColorOK = true;
            audioSource.Play();
        }
        else if (collision.collider.CompareTag("Player"))
        {
            zm.scoreDoubleFalse(activeDouble);
        }

        if (collision.collider.CompareTag("destructorPlatform"))
            Destroy(gameObject);
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("MonsterChangeColor") && changeColorOK)
        {
            cubeRenderer.material = m_Material01;
            zm.addFinLevel(valueDalle);
            changeColorOK = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("destructorPlatform"))
            Destroy(gameObject);
    }

    void Update()
    {
        if (destructorHandled) return;

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            destructorHandled = true;
            // CORRIGÉ : vérification null avant Destroy
            GameObject obj = GameObject.FindWithTag("destructorPlatform");
            if (obj != null) Destroy(obj);
            timer = 0;
        }
    }
}