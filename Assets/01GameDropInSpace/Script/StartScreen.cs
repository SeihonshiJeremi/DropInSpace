using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class StartScreen : MonoBehaviour
{
    [Header("UI")]
    public GameObject startScreenCanvas;
    public TMP_Text startText;
    public GameObject mainMenuCanvas;
    public GameObject demoMenuCanvas;
    public bool demoMode;

    [Header("Drop Warp Roll")]
    public Transform dropModel;
    public ParticleSystem warpParticles;

    [Header("Clignotement")]
    public float vitesseClignotement = 1.2f;

    [Header("Warp Roll")]
    public float tonneauSpeed = 360f;
    public float tonneauDuree = 1.2f;
    public float pauseEntreBoucles = 0.8f;

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
        controls.UI.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
        controls.UI.Disable();
    }

    void Start()
    {
        // Tout éteint sauf le Start Screen
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(false);
        if (demoMenuCanvas != null) demoMenuCanvas.SetActive(false);
        if (startScreenCanvas != null) startScreenCanvas.SetActive(true);

        if (UnityEngine.EventSystems.EventSystem.current != null && startScreenCanvas != null)
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(startScreenCanvas);

        StartCoroutine(Clignoter());
        StartCoroutine(WarpRollBoucle());
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

        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            StartCoroutine(PasserAuMenu());
            return;
        }

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
            float elapsed = 0f;
            while (elapsed < vitesseClignotement)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0.1f, 1f, elapsed / vitesseClignotement);
                if (startText != null) startText.alpha = alpha;
                yield return null;
            }

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

        yield return new WaitForSeconds(0.1f);

        // Éteint le Start Screen
        if (startScreenCanvas != null) startScreenCanvas.SetActive(false);

        if (demoMode)
        {
            // ─── MODE DÉMO ───
            if (mainMenuCanvas != null) mainMenuCanvas.SetActive(false);
            if (demoMenuCanvas != null) demoMenuCanvas.SetActive(true);

            // Initialise le demoLayout via MainMenuManager
            MainMenuManager mgr = Object.FindFirstObjectByType<MainMenuManager>();
            if (mgr != null) mgr.InitDemoMode();
        }
        else
        {
            // ─── MODE NORMAL ───
            if (demoMenuCanvas != null) demoMenuCanvas.SetActive(false);
            if (mainMenuCanvas != null) mainMenuCanvas.SetActive(true);
        }

        yield return null;

        SelectionManager selMgr = Object.FindFirstObjectByType<SelectionManager>();
        if (selMgr != null)
        {
            selMgr.ResetSelection();
            Debug.Log("[StartScreen] SelectionManager.ResetSelection() appelé");
        }
        else
        {
            Debug.LogWarning("[StartScreen] SelectionManager introuvable !");
        }
    }
}