using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCamera : MonoBehaviour
{
    [Header("Pivot")]
    public Transform pivot;

    [Header("Rotation")]
    public float distancePivot = 14.58f;
    public float hauteurCamera = 17.08f;
    public float dureeRotation = 0.4f;

    [Header("Tactile Caméra")]
    public float tactileSwipeResistance = 60f;

    [Header("Reference Drop")]
    public rollingCube dropScript;

    private float angleActuel = 0f;
    private bool enRotation = false;

    private DIS_Controls controls;

    // ─── Tactile ───
    private Vector2 twoFingerStartPos;
    private bool twoFingerArmed = false;

    void Awake()
    {
        controls = new DIS_Controls();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.UI.Enable();
        controls.Gameplay.RotateLeft.performed += ctx => TournerGauche();
        controls.Gameplay.RotateRight.performed += ctx => TournerDroite();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
        controls.UI.Disable();
    }

    void Start()
    {
        if (pivot != null)
        {
            Vector3 offset = transform.position - pivot.position;
            distancePivot = new Vector2(offset.x, offset.z).magnitude;
            hauteurCamera = transform.position.y;
        }
    }

    void Update()
    {
        HandleTouchCamera();
    }

    void HandleTouchCamera()
    {
        if (enRotation) return;

        // ─── Détection 2 doigts (Android/iOS) ───
        if (Input.touchCount == 2)
        {
            // Premier contact à 2 doigts → on enregistre la position
            if (!twoFingerArmed)
            {
                twoFingerStartPos = Input.GetTouch(0).position;
                twoFingerArmed = true;
            }
            else
            {
                // Mesurer le déplacement du premier doigt
                Vector2 delta = (Vector2)Input.GetTouch(0).position - twoFingerStartPos;

                if (Mathf.Abs(delta.x) > tactileSwipeResistance)
                {
                    // Un seul trigger = une rotation de 90°
                    if (delta.x > 0)
                        TournerDroite();
                    else
                        TournerGauche();

                    // Désarmer jusqu'à ce que les doigts soient relevés
                    twoFingerArmed = false;
                }
            }
        }
        else
        {
            // Moins de 2 doigts → reset
            twoFingerArmed = false;
        }
    }

    // ══════════════════════════════════════════════════════════════════════
    // ROTATION
    // ══════════════════════════════════════════════════════════════════════

    public void TournerGauche()
    {
        if (enRotation) return;
        StartCoroutine(AnimerRotation(-90f));
    }

    public void TournerDroite()
    {
        if (enRotation) return;
        StartCoroutine(AnimerRotation(90f));
    }

    private IEnumerator AnimerRotation(float deltaAngle)
    {
        enRotation = true;

        float angleDepart = angleActuel;
        float angleCible = angleActuel + deltaAngle;
        float elapsed = 0f;

        while (elapsed < dureeRotation)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / dureeRotation);
            float angle = Mathf.Lerp(angleDepart, angleCible, t);
            AppliquerAngle(angle);
            yield return null;
        }

        angleActuel = angleCible % 360f;
        AppliquerAngle(angleActuel);

        RecalculerDirectionsDrop();

        enRotation = false;
    }

    private void AppliquerAngle(float angle)
    {
        if (pivot == null) return;

        float rad = angle * Mathf.Deg2Rad;
        float x = pivot.position.x + distancePivot * Mathf.Sin(rad);
        float z = pivot.position.z - distancePivot * Mathf.Cos(rad);

        transform.position = new Vector3(x, hauteurCamera, z);
        transform.LookAt(new Vector3(pivot.position.x, pivot.position.y - 2f, pivot.position.z));
    }

    private void RecalculerDirectionsDrop()
    {
        if (dropScript == null) return;

        float angle = ((angleActuel % 360f) + 360f) % 360f;
        int palier = Mathf.RoundToInt(angle / 90f) % 4;

        Debug.Log($"[OrbitCamera] Angle: {angle:F0} — Palier: {palier}");

        dropScript.cameraAnglePalier = palier;
    }
}