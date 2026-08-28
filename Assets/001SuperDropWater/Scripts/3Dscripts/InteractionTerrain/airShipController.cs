using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class airShipController : MonoBehaviour
{
    public float forwardSpeed = 25f, strafeSpeed = 7.5f;
    private float activeForwardSpeed, activeStrafeSpeed;
    private float forwardAcceleration = 2.5f, strafeAcceleration = 2f;
    public float lookRateSpeed = 90f;
    private float rollInput;
    public float rollSpeed = 90f, rollAcceleration = 3.5f;

    // Effet de vitesse
    public Camera Camera;
    public float fov = 60.0f;

    // Effet particule
    public ParticleSystem speedParticle;
    bool camFOVDiminution;
    bool forwardParticle;

    // Tonneau
    public float tonneauSpeed = 360f;
    public float tonneauDuree = 1.2f;
    private bool estEnTonneau = false;
    public Transform dropModel;
    public ParticleSystem tonneauParticles; // Assigner dans l'Inspector

    // Nouveau Input System
    private DIS_Controls controls;
    private Vector2 moveInput;
    private bool dashHeld;
    private bool dashPressed;
    private bool dashReleased;
    private bool tonneauPressed;
    private bool tonneauHeld;

    void Awake()
    {
        controls = new DIS_Controls();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();

        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Gameplay.Dash.started += ctx => { dashPressed = true; dashHeld = true; };
        controls.Gameplay.Dash.canceled += ctx => { dashReleased = true; dashHeld = false; };

        controls.Gameplay.Tonneau.started += ctx => { tonneauPressed = true; tonneauHeld = true; };
        controls.Gameplay.Tonneau.canceled += ctx => tonneauHeld = false;
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Start()
    {
        Physics.gravity = new Vector3(0, 0, 0);
        if (speedParticle != null) speedParticle.Stop();
    }

    void Update()
    {
        rollInput = Mathf.Lerp(rollInput, moveInput.x, rollAcceleration * Time.deltaTime);
        transform.Rotate(
            -moveInput.y * lookRateSpeed * Time.deltaTime,
            moveInput.x * lookRateSpeed * Time.deltaTime,
            rollInput * rollSpeed * Time.deltaTime,
            Space.Self
        );

        float dashValue = dashHeld ? 1f : moveInput.y > 0.1f ? moveInput.y : 0f;
        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, dashValue * forwardSpeed, forwardAcceleration * Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, moveInput.x * strafeSpeed, strafeAcceleration * Time.deltaTime);

        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        transform.position += transform.right * activeStrafeSpeed * Time.deltaTime;

        if (dashPressed)
        {
            dashPressed = false;
            ChangePhysicGravity();
            if (speedParticle != null) speedParticle.Play();
        }

        if (dashReleased)
        {
            dashReleased = false;
            ChangePhysicGravityExit();
            forwardParticle = true;
            if (forwardSpeed <= 100) forwardSpeed = 100;
            camFOVDiminution = true;
        }

        if (dashHeld)
        {
            Camera.main.fieldOfView += Time.deltaTime * 5;
            forwardSpeed += Time.deltaTime * 5;
            if (Camera.main.fieldOfView > 130) Camera.main.fieldOfView = 130;
            if (forwardSpeed > 200) forwardSpeed = 200;
        }

        if (forwardParticle) forwardSpeed -= Time.deltaTime * 20;
        if (forwardSpeed > 100)
        {
            if (speedParticle != null && !speedParticle.isPlaying) speedParticle.Play();
        }
        else if (forwardSpeed <= 100)
        {
            if (speedParticle != null) speedParticle.Stop();
            forwardParticle = false;
        }

        if (Camera.main.fieldOfView >= 60 && camFOVDiminution)
            Camera.main.fieldOfView -= Time.deltaTime * 20;
        if (Camera.main.fieldOfView < 60)
        {
            Camera.main.fieldOfView = 60.0f;
            camFOVDiminution = false;
        }

        if (tonneauPressed && !estEnTonneau)
        {
            tonneauPressed = false;
            StartCoroutine(FaireTonneau());
        }

        // Relance automatiquement si le bouton est maintenu
        if (tonneauHeld && !estEnTonneau)
            StartCoroutine(FaireTonneau());
    }

    IEnumerator FaireTonneau()
    {
        estEnTonneau = true;
        float elapsed = 0f;

        // Lance les particules
        if (tonneauParticles != null)
            tonneauParticles.Play();

        // Position et rotation de depart
        Vector3 centreDeRotation = transform.position + transform.forward * 3f;
        float rayonBase = 3f;
        float hauteurSaut = 2f;
        float angleDepart = -90f;

        // FOV de depart
        float fovDepart = Camera.main.fieldOfView;
        float fovMax = fovDepart + 20f; // Augmente de 20 pendant la boucle

        while (elapsed < tonneauDuree)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / tonneauDuree;

            // Angle qui fait le tour complet de 0 a 360
            float angle = angleDepart + (360f * t);
            float angleRad = angle * Mathf.Deg2Rad;

            // Petit saut
            float saut = Mathf.Sin(t * Mathf.PI) * hauteurSaut;

            // Position en cercle + saut
            Vector3 offset = transform.up * (Mathf.Sin(angleRad) * rayonBase + saut)
                           + transform.forward * (Mathf.Cos(angleRad) * rayonBase);

            transform.position = centreDeRotation + offset;

            // FOV — monte puis redescend avec la boucle
            Camera.main.fieldOfView = Mathf.Lerp(fovDepart, fovMax, Mathf.Sin(t * Mathf.PI));

            // Rotation visuelle de Drop
            if (dropModel != null)
                dropModel.Rotate(tonneauSpeed * Time.deltaTime, 0f, 0f, Space.Self);

            yield return null;
        }

        // Remet le FOV a sa valeur de depart
        Camera.main.fieldOfView = fovDepart;

        // Arrete les particules
        if (tonneauParticles != null)
            tonneauParticles.Stop();

        // Remet la rotation du modele a zero
        if (dropModel != null)
            dropModel.localRotation = Quaternion.identity;

        estEnTonneau = false;
    }

    public void ChangePhysicGravity()
    {
        Physics.gravity = new Vector3(0, 0, 0);
    }

    public void ChangePhysicGravityExit()
    {
        Physics.gravity = new Vector3(0, 0, 0);
    }
}