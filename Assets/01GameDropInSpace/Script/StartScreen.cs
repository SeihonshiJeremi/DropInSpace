using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// Ecran Start — Drop fait le Warp Roll en boucle
/// Texte qui clignote — appuyer n'importe quelle touche pour passer au menu
/// </summary>
public class StartScreen : MonoBehaviour
{
    [Header("UI")]
    public GameObject startScreenCanvas;   // Le Canvas StartScreen
    public TMP_Text startText;             // Le texte "APPUYEZ SUR N'IMPORTE QUELLE TOUCHE"
    public GameObject mainMenuCanvas;      // Le Canvas du menu principal — a desactiver au debut

    [Header("Drop Warp Roll")]
    public Transform dropModel;            // La Sphere de Drop
    public ParticleSystem warpParticles;   // Particules du Warp Roll

    [Header("Clignotement")]
    public float vitesseClignotement = 1.2f; // Vitesse du clignotement

    [Header("Warp Roll")]
    public float tonneauSpeed = 360f;
    public float tonneauDuree = 1.2f;
    public float pauseEntreBoucles = 0.8f; // Pause entre chaque Warp Roll

    private bool peutPasser = false;
    private bool enTransition = false;
    private DIS_Controls controls;

    void Awake()
    {
        controls = new DIS_Controls();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Start()
    {
        // Cache le menu principal au debut
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(false);
        if (startScreenCanvas != null) startScreenCanvas.SetActive(true);

        // Lance le clignotement et le Warp Roll en boucle
        StartCoroutine(Clignoter());
        StartCoroutine(WarpRollBoucle());

        // Attend un peu avant d'accepter les inputs
        StartCoroutine(ActiverInput());
    }

    IEnumerator ActiverInput()
    {
        yield return new WaitForSeconds(1.0f);
        peutPasser = true;
    }

    void Update()
    {
        if (!peutPasser || enTransition) return;

        // Detecte n'importe quelle touche clavier
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            StartCoroutine(PasserAuMenu());
            return;
        }

        // Detecte n'importe quel bouton manette
        if (Gamepad.current != null)
        {
            foreach (var control in Gamepad.current.allControls)
            {
                if (control is UnityEngine.InputSystem.Controls.ButtonControl btn && btn.wasPressedThisFrame)
                {
                    StartCoroutine(PasserAuMenu());
                    return;
                }
            }
        }
    }

    IEnumerator Clignoter()
    {
        while (true)
        {
            // Fade in
            float elapsed = 0f;
            while (elapsed < vitesseClignotement)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0.1f, 1f, elapsed / vitesseClignotement);
                if (startText != null) startText.alpha = alpha;
                yield return null;
            }

            // Fade out
            elapsed = 0f;
            while (elapsed < vitesseClignotement)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0.1f, elapsed / vitesseClignotement);
                if (startText != null) startText.alpha = alpha;
                yield return null;
            }
        }
    }

    IEnumerator WarpRollBoucle()
    {
        // Attend un peu au debut
        yield return new WaitForSeconds(1.5f);

        while (true)
        {
            yield return StartCoroutine(FaireWarpRoll());
            yield return new WaitForSeconds(pauseEntreBoucles);
        }
    }

    IEnumerator FaireWarpRoll()
    {
        float elapsed = 0f;

        if (warpParticles != null) warpParticles.Play();

        Vector3 centreDeRotation = transform.position + transform.forward * 3f;
        float rayonBase = 3f;
        float hauteurSaut = 2f;
        float angleDepart = -90f;

        float fovDepart = Camera.main != null ? Camera.main.fieldOfView : 60f;
        float fovMax = fovDepart + 20f;

        while (elapsed < tonneauDuree)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / tonneauDuree;

            float angle = angleDepart + (360f * t);
            float angleRad = angle * Mathf.Deg2Rad;
            float saut = Mathf.Sin(t * Mathf.PI) * hauteurSaut;

            Vector3 offset = transform.up * (Mathf.Sin(angleRad) * rayonBase + saut)
                           + transform.forward * (Mathf.Cos(angleRad) * rayonBase);

            transform.position = centreDeRotation + offset;

            if (Camera.main != null)
                Camera.main.fieldOfView = Mathf.Lerp(fovDepart, fovMax, Mathf.Sin(t * Mathf.PI));

            if (dropModel != null)
                dropModel.Rotate(tonneauSpeed * Time.deltaTime, 0f, 0f, Space.Self);

            yield return null;
        }

        if (Camera.main != null) Camera.main.fieldOfView = fovDepart;
        if (warpParticles != null) warpParticles.Stop();
        if (dropModel != null) dropModel.localRotation = Quaternion.identity;
    }

    IEnumerator PasserAuMenu()
    {
        enTransition = true;
        peutPasser = false;

        // Petit flash blanc
        yield return new WaitForSeconds(0.1f);

        // Cache l'ecran Start et affiche le menu
        if (startScreenCanvas != null) startScreenCanvas.SetActive(false);
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(true);
    }
}