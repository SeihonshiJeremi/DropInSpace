using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class rollingCube : MonoBehaviour
{
    [SerializeField] private float _rollSpeed = 5;
    public bool _isMoving;
    public bool _collisionOK = false;

    public float x, z, y;
    public float xDesac, zDesac, yDesac;

    public bool desacTouche = false;
    public float timeDesacTouche = 3.0f;

    public GameObject item;

    public int cameraAnglePalier = 0;

    private DIS_Controls controls;
    private Vector2 moveInput;

    private AudioSource audioSource;
    private itemDropInSpace itemScript;

    [Header("Respawn")]
    public float respawnCooldownDuration = 0.5f;
    private float cooldownRespawn = 0f;

    private Coroutine rollCoroutine;

    // ← NOUVEAU : sauvegarder la position/rotation d'origine
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    void Awake()
    {
        controls = new DIS_Controls();

        // ← SAUVEGARDER les coordonnées d'origine au démarrage
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.UI.Enable();
        controls.Gameplay.Move.performed += ctx =>
        {
            if (cooldownRespawn <= 0f)
                moveInput = ctx.ReadValue<Vector2>();
        };
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
        controls.UI.Disable();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (item != null)
            itemScript = item.GetComponent<itemDropInSpace>();

        x = transform.position.x;
        z = transform.position.z;
        y = transform.position.y;
    }

    void Update()
    {
        if (cooldownRespawn > 0f)
        {
            cooldownRespawn -= Time.deltaTime;
            moveInput = Vector2.zero;
            return;
        }

        x = transform.position.x;
        z = transform.position.z;
        y = transform.position.y;
        x = Mathf.Round(x);
        z = Mathf.Round(z);

        if (!_isMoving && _collisionOK)
            transform.position = new Vector3(x, 1.0f, z);

        if (timeDesacTouche > 0)
            timeDesacTouche -= Time.deltaTime;

        if (timeDesacTouche <= 0) activTouche();
        else desactivationTouche();
    }

    void FixedUpdate()
    {
        if (cooldownRespawn > 0f) return;
        if (desacTouche) return;
        if (_isMoving) return;

        if (moveInput.x < -0.5f) AssembleDir(GetDirection(Vector3.left));
        else if (moveInput.x > 0.5f) AssembleDir(GetDirection(Vector3.right));
        else if (moveInput.y > 0.5f) AssembleDir(GetDirection(Vector3.forward));
        else if (moveInput.y < -0.5f) AssembleDir(GetDirection(Vector3.back));
    }

    private void AssembleDir(Vector3 dir)
    {
        if (cooldownRespawn > 0f) return;
        if (desacTouche) return;  // ← CRITIQUE : bloquer pendant le cooldown respawn
        if (_isMoving) return;

        _isMoving = true;

        var anchor = transform.position + (Vector3.down + dir) * 1.085f;
        var axis = Vector3.Cross(Vector3.up, dir);
        rollCoroutine = StartCoroutine(Roll(anchor, axis));
    }

    IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        if (audioSource != null && audioSource.clip != null)
            audioSource.PlayOneShot(audioSource.clip);

        float remainingAngle = 90f;

        while (remainingAngle > 0f)
        {
            float rotationStep = Mathf.Min(_rollSpeed, remainingAngle);
            transform.RotateAround(anchor, axis, rotationStep);
            remainingAngle -= rotationStep;

            yield return new WaitForFixedUpdate();
        }

        float snapX = Mathf.Round(transform.position.x);
        float snapZ = Mathf.Round(transform.position.z);
        transform.position = new Vector3(snapX, 1.0f, snapZ);

        if (_collisionOK && VibrationsManager.Instance != null)
        {
            try { VibrationsManager.Instance.Vibrate(0.1f, 0.3f, 0.08f); }
            catch (System.Exception e) { Debug.LogWarning($"[Roll] Erreur vibration: {e.Message}"); }
        }

        _isMoving = false;
        rollCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("itemTimeDiv2")) itemScript.itemTimeDiv2F();
        if (other.CompareTag("itemTimeStop")) itemScript.itemTimeStopF();
        if (other.CompareTag("itemInvincibilty")) itemScript.itemInvincibiltyF();
        if (other.CompareTag("itemBonbonScoreRouge")) itemScript.itemBonbonScoreRougeF();
        if (other.CompareTag("itemBonbonScoreVert")) itemScript.itemBonbonScoreVertF();
        if (other.CompareTag("itemRandom")) itemScript.itemRandomF();

        if (other.CompareTag("itemTimeDiv202")) itemScript.itemTimeDiv2F02();
        if (other.CompareTag("itemTimeStop02")) itemScript.itemTimeStopF02();
        if (other.CompareTag("itemInvincibilty02")) itemScript.itemInvincibiltyF02();
        if (other.CompareTag("itemBonbonScoreRouge02")) itemScript.itemBonbonScoreRougeF02();
        if (other.CompareTag("itemBonbonScoreVert02")) itemScript.itemBonbonScoreVertF02();
        if (other.CompareTag("itemRandom02")) itemScript.itemRandomF02();

        if (other.CompareTag("changeCubeMaterial"))
        {
            _collisionOK = true;
            VibrationsManager.Instance?.Vibrate(0.1f, 0.3f, 0.08f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("changeCubeMaterial")) _collisionOK = false;
    }

    public void desactivationTouche()
    {
        desacTouche = true;
        transform.position = new Vector3(xDesac, y, zDesac);
    }

    public void activTouche()
    {
        desacTouche = false;
    }

    public void tDesacTouche()
    {
        timeDesacTouche = 3.0f;
    }

    public void AssembleLeft() => AssembleDir(GetDirection(Vector3.left));
    public void AssembleRight() => AssembleDir(GetDirection(Vector3.right));
    public void AssembleForward() => AssembleDir(GetDirection(Vector3.forward));
    public void AssembleBack() => AssembleDir(GetDirection(Vector3.back));

    private Vector3 GetDirection(Vector3 dir)
    {
        if (cameraAnglePalier == 0) return dir;
        for (int i = 0; i < cameraAnglePalier; i++)
            dir = new Vector3(-dir.z, 0, dir.x);
        return dir;
    }

    // ══════════════════════════════════════════════════════════════════════
    // RESET COMPLET (appelé par TeleportationRolling)
    // ══════════════════════════════════════════════════════════════════════

    public void ResetMovementState()
    {
        // Arrêter la coroutine Roll en cours
        if (rollCoroutine != null)
        {
            StopCoroutine(rollCoroutine);
            rollCoroutine = null;
        }

        _isMoving = false;
        desacTouche = true;       // ← BLOQUER le mouvement
        timeDesacTouche = 3.0f;   // ← BLOCAGE de 3 secondes
        moveInput = Vector2.zero;

        // ← REMISE à la position ET rotation d'origine
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;

        // ← Reset _collisionOK (sera remis à true quand Drop touche un cube)
        _collisionOK = false;

        Debug.Log($"[rollingCube] ResetMovementState — Position: {spawnPosition}, Rotation: {spawnRotation.eulerAngles}");
    }

    public void TriggerRespawnCooldown()
    {
        cooldownRespawn = respawnCooldownDuration;
    }
}