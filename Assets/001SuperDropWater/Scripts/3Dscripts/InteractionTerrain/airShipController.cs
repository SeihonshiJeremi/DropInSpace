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

    // ═══════════════════════════════════════════════════════
    // ⚠️ AJOUT SUSPENSION : DÉRIVE EN APESANTEUR
    // ═══════════════════════════════════════════════════════

    [Header("Suspension dans le vide")]
    public float vitesseRotationIdle = 15f;      // Degrés/seconde de la dérive lente
    public float vitesseRetourIdle = 4f;          // Vitesse du retour à plat quand on bouge
    public float amplitudeFlottement = 0.3f;      // Amplitude du léger balancement haut/bas
    public float frequenceFlottement = 1.0f;      // Vitesse du balancement

    private bool estEnSuspension = false;         // Est-ce que Drop est en train de dériver ?
    private float tempsSansInput = 0f;            // Chronomètre depuis le dernier input
    public float delaiAvantSuspension = 1.0f;     // Temps d'inactivité avant que la dérive commence

    // ═══════════════════════════════════════════════════════
    // ⚠️ AJOUT AUDIO : SONS DE DÉPLACEMENT
    // ═══════════════════════════════════════════════════════

    [Header("Sons de Déplacement")]
    public AudioClip soundMove;              // Son en boucle pendant que Drop avance
    public AudioClip soundDash;              // Son bref au moment du dash
    public AudioClip soundTonneau;           // Son bref au moment du tonneau

    [Header("Réglages du son de déplacement")]
    public float volumeMin = 0.05f;          // Volume minimum (presque silencieux)
    public float volumeMax = 0.35f;          // Volume maximum à pleine vitesse
    public float pitchMin = 0.8f;            // Pitch minimum (son grave)
    public float pitchMax = 1.4f;            // Pitch maximum (son aigu à fond les gaz !)

    private AudioSource loopAudioSource;     // Source pour le son en boucle
    private AudioSource oneShotAudioSource; // Source pour les sons brefs (dash, tonneau)

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

        // ⚠️ AJOUT AUDIO : Création des deux AudioSources
        loopAudioSource = gameObject.AddComponent<AudioSource>();
        loopAudioSource.clip = soundMove;
        loopAudioSource.loop = true;
        loopAudioSource.playOnAwake = false;
        loopAudioSource.volume = volumeMin;
        if (soundMove != null) loopAudioSource.Play();

        oneShotAudioSource = gameObject.AddComponent<AudioSource>();
        oneShotAudioSource.playOnAwake = false;
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

        // ⚠️ AJOUT AUDIO : Le son suit la vitesse réelle de Drop !
        UpdateMoveSound();

        // ═══════════════════════════════════════════════════
        // ⚠️ AJOUT SUSPENSION : Détection d'inactivité
        // ═══════════════════════════════════════════════════
        UpdateSuspension();

        if (dashPressed)
        {
            dashPressed = false;
            ChangePhysicGravity();
            if (speedParticle != null) speedParticle.Play();

            // ⚠️ AJOUT AUDIO : son de dash
            if (soundDash != null)
                oneShotAudioSource.PlayOneShot(soundDash);
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

            // ⚠️ AJOUT AUDIO : son de tonneau
            if (soundTonneau != null)
                oneShotAudioSource.PlayOneShot(soundTonneau);

            StartCoroutine(FaireTonneau());
        }

        // Relance automatiquement si le bouton est maintenu
        if (tonneauHeld && !estEnTonneau)
            StartCoroutine(FaireTonneau());
    }

    // ═══════════════════════════════════════════════════════
    // ⚠️ AJOUT SUSPENSION : MÉTHODE PRINCIPALE
    // ═══════════════════════════════════════════════════════

    void UpdateSuspension()
    {
        // On ne fait rien tant que Drop fait un tonneau (le tonneau gère déjà sa rotation)
        if (estEnTonneau) return;

        // Est-ce que le joueur touche à quelque chose ?
        bool joueurInactif =
            moveInput == Vector2.zero
            && !dashHeld
            && !tonneauHeld;

        if (joueurInactif)
        {
            tempsSansInput += Time.deltaTime;

            // Après le délai, la dérive commence
            if (tempsSansInput >= delaiAvantSuspension)
            {
                estEnSuspension = true;

                if (dropModel != null)
                {
                    // Rotation lente et irrégulière = impression d'apesanteur
                    // Chaque axe tourne à une vitesse différente, c'est ça qui
                    // donne le côté "objet qui flotte" plutôt que "métronome" !
                    dropModel.Rotate(
                        vitesseRotationIdle * 0.6f * Time.deltaTime,   // tangage doux
                        vitesseRotationIdle * Time.deltaTime,          // lacet
                        vitesseRotationIdle * 0.3f * Time.deltaTime,   // roulis très léger
                        Space.Self
                    );
                }

                // Léger balancement haut/bas, comme une goutte qui flotte 💧
                Vector3 posBase = transform.position;
                float flottement = Mathf.Sin(tempsSansInput * frequenceFlottement * Mathf.PI) * amplitudeFlottement * Time.deltaTime;
                transform.position += transform.up * flottement;
            }
        }
        else
        {
            // Le joueur a repris le contrôle
            tempsSansInput = 0f;

            if (estEnSuspension && dropModel != null)
            {
                // Retour progressif à plat : Slerp = interpolation de rotation,
                // contrairement au Snap instantané de FaireTonneau
                dropModel.localRotation = Quaternion.Slerp(
                    dropModel.localRotation,
                    Quaternion.identity,
                    vitesseRetourIdle * Time.deltaTime
                );

                // Quand on est presque à plat, on termine proprement
                if (Quaternion.Angle(dropModel.localRotation, Quaternion.identity) < 0.5f)
                {
                    dropModel.localRotation = Quaternion.identity;
                    estEnSuspension = false;
                }
            }
            else
            {
                estEnSuspension = false;
            }
        }
    }

    // ⚠️ AJOUT AUDIO : MÉTHODE QUI FAIT VIVRE LE SON
    void UpdateMoveSound()
    {
        if (loopAudioSource == null || soundMove == null) return;

        float vitesseTotale = activeForwardSpeed / 200f + Mathf.Abs(activeStrafeSpeed) / strafeSpeed * 0.3f;
        vitesseTotale = Mathf.Clamp01(vitesseTotale);

        loopAudioSource.volume = Mathf.Lerp(volumeMin, volumeMax, vitesseTotale);
        loopAudioSource.pitch = Mathf.Lerp(pitchMin, pitchMax, vitesseTotale);
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

            Camera.main.fieldOfView = Mathf.Lerp(fovDepart, fovMax, Mathf.Sin(t * Mathf.PI));

            if (dropModel != null)
                dropModel.Rotate(tonneauSpeed * Time.deltaTime, 0f, 0f, Space.Self);

            yield return null;
        }

        Camera.main.fieldOfView = fovDepart;

        if (tonneauParticles != null)
            tonneauParticles.Stop();

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