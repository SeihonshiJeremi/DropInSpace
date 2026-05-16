using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Camera orbitale — tourne autour du centre de la grille par paliers de 90 degres.
/// LB/Tab = rotation gauche, RB/Q = rotation droite.
/// Recalcule les directions de Drop apres chaque rotation.
/// A placer sur la Main Camera.
/// </summary>
public class OrbitCamera : MonoBehaviour
{
    [Header("Pivot")]
    public Transform pivot; // Centre de la grille — assigne dans l'Inspector

    [Header("Rotation")]
    public float distancePivot = 14.58f; // Distance actuelle camera/pivot
    public float hauteurCamera = 17.08f; // Hauteur Y de la camera
    public float dureeRotation = 0.4f;   // Duree de l'animation de rotation

    [Header("Reference Drop")]
    public rollingCube dropScript; // Script de Drop pour recalculer les directions

    // Etat
    private float angleActuel = 0f;     // Angle actuel en degrees (0 = position initiale)
    private bool enRotation = false;

    // Input
    private DIS_Controls controls;

    void Awake()
    {
        controls = new DIS_Controls();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.RotateLeft.performed += ctx => TournerGauche();
        controls.Gameplay.RotateRight.performed += ctx => TournerDroite();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Start()
    {
        // Calcule la distance et hauteur initiales depuis la position actuelle
        if (pivot != null)
        {
            Vector3 offset = transform.position - pivot.position;
            distancePivot = new Vector2(offset.x, offset.z).magnitude;
            hauteurCamera = transform.position.y;
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

        // Recalcule les directions de Drop
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

    // ══════════════════════════════════════════════════════════════════════
    // RECALCUL DIRECTIONS DROP
    // ══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Recalcule les directions de Drop selon l'angle de la camera.
    /// Garantit que "haut" sur le stick = avancer dans la direction visible.
    /// </summary>
    private void RecalculerDirectionsDrop()
    {
        if (dropScript == null) return;

        // Normalise l'angle entre 0 et 360
        float angle = ((angleActuel % 360f) + 360f) % 360f;

        // Selon l'angle, remapper les directions
        // 0 deg = position initiale (sud de la grille)
        // 90 deg = est, 180 = nord, 270 = ouest
        int palier = Mathf.RoundToInt(angle / 90f) % 4;

        Debug.Log($"[OrbitCamera] Angle: {angle:F0} — Palier: {palier}");

        // Le remapping est gere via les methodes publiques de rollingCube
        // On stocke le palier pour que rollingCube l'utilise
        dropScript.cameraAnglePalier = palier;
    }
}