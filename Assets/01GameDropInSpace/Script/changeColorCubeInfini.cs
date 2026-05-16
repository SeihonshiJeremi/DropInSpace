using UnityEngine;

// Script sur les cubes du mode Infini
// Remplace changeColorCube sur le prefab cubeDestructGrid de la scène Infini
public class changeColorCubeInfini : MonoBehaviour
{
    public Material m_Material01;
    public Material m_Material02;

    private bool changeColorOK = false;
    private Renderer cubeRenderer;
    private AudioSource audioSource;

    void Awake()
    {
        cubeRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !changeColorOK)
        {
            cubeRenderer.material = m_Material02;
            changeColorOK = true;
            if (audioSource != null) audioSource.Play();
            InfiniteGridManager.Instance?.SignalerCubeColorie();
        }

        // Le DestructorPlatform détruit le cube — crée des niveaux différents
        if (collision.collider.CompareTag("destructorPlatform"))
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("destructorPlatform"))
            Destroy(gameObject);
    }
}