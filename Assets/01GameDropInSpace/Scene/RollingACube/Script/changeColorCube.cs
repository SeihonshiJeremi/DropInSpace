using UnityEngine;
using System.Collections;

public class changeColorCube : MonoBehaviour
{
    public Material m_Material01;
    public Material m_Material02;
    public int valueDalle;
    private int valueScore = 10;
    private int activeDouble = 1;
    public bool changeColorOK = false;
    private zoomManager zm;
    private Renderer cubeRenderer;
    private AudioSource audioSource;
    private float timer = 5.0f;
    private bool destructorHandled = false;

    void Awake()
    {
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

            if (CubeMusicManager.Instance != null)
                CubeMusicManager.Instance.JouerNoteCube();
        }
        else if (collision.collider.CompareTag("Player"))
        {
            zm.scoreDoubleFalse(activeDouble);

            if (CubeMusicManager.Instance != null)
                CubeMusicManager.Instance.ResetCombo();
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
            GameObject obj = GameObject.FindWithTag("destructorPlatform");
            if (obj != null) Destroy(obj);
            timer = 0;
        }
    }
}