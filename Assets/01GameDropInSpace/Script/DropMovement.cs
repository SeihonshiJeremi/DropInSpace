using UnityEngine;
using UnityEngine.EventSystems;

public class DropMovement : MonoBehaviour
{
    [Header("Boutons et positions cibles")]
    public RectTransform[] boutons;
    public float speed = 5f;
    public Canvas canvas;
    public GameObject menuLayout;

    private Vector3 targetPosition;
    private bool menuActif = false;

    void Start()
    {
        targetPosition = transform.position; // centre au départ
    }

    void Update()
    {
        // Vérifie si le menu vient de s'activer
        if (!menuActif && menuLayout != null && menuLayout.activeInHierarchy)
        {
            menuActif = true;
        }

        if (menuActif)
        {
            GameObject selected = EventSystem.current?.currentSelectedGameObject;
            if (selected != null)
            {
                RectTransform rt = selected.GetComponent<RectTransform>();
                if (rt != null)
                {
                    Vector3 worldPos = GetWorldPosition(rt);
                    targetPosition = new Vector3(worldPos.x, worldPos.y, transform.position.z);
                }
            }
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
    }

    Vector3 GetWorldPosition(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 center = (corners[0] + corners[2]) / 2f;
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, center);
        Camera cam = Camera.main;
        float depth = Mathf.Abs(cam.transform.position.z - transform.position.z);
        return cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, depth));
    }
}