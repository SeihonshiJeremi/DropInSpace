using UnityEngine;

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

            if (CubeMusicManager.Instance != null)
                CubeMusicManager.Instance.JouerNoteCube();
        }
        else if (collision.collider.CompareTag("Player") && changeColorOK)
        {
            if (CubeMusicManager.Instance != null)
                CubeMusicManager.Instance.ResetCombo();
        }

        if (collision.collider.CompareTag("destructorPlatform"))
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("destructorPlatform"))
            Destroy(gameObject);
    }
}