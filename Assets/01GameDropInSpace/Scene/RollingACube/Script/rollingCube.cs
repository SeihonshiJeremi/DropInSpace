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

    // Camera orbitale — palier de rotation (0=sud, 1=est, 2=nord, 3=ouest)
    public int cameraAnglePalier = 0;

    // Input System
    private DIS_Controls controls;
    private Vector2 moveInput;

    // Cache
    private AudioSource audioSource;
    private itemDropInSpace itemScript;

    void Awake()
    {
        controls = new DIS_Controls();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        itemScript = item.GetComponent<itemDropInSpace>();

        x = transform.position.x;
        z = transform.position.z;
        y = transform.position.y;
    }

    void Update()
    {
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
        if (desacTouche) return;
        if (_isMoving) return;

        if (moveInput.x < -0.5f) AssembleDir(GetDirection(Vector3.left));
        else if (moveInput.x > 0.5f) AssembleDir(GetDirection(Vector3.right));
        else if (moveInput.y > 0.5f) AssembleDir(GetDirection(Vector3.forward));
        else if (moveInput.y < -0.5f) AssembleDir(GetDirection(Vector3.back));
    }

    private void AssembleDir(Vector3 dir)
    {
        var anchor = transform.position + (Vector3.down + dir) * 1.085f;
        var axis = Vector3.Cross(Vector3.up, dir);
        StartCoroutine(Roll(anchor, axis));
    }

    IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        _isMoving = true;
        for (int i = 0; i < (90 / _rollSpeed); i++)
        {
            transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForFixedUpdate();
            audioSource.Play();
        }

        Debug.Log($"[Roll] _collisionOK : {_collisionOK} — VibrationsManager : {VibrationsManager.Instance != null}");

        if (_collisionOK && VibrationsManager.Instance != null)
        {
            try { VibrationsManager.Instance.Vibrate(0.1f, 0.3f, 0.08f); }
            catch (System.Exception e) { Debug.LogError($"[Roll] Erreur vibration : {e.Message}"); }
        }

        _isMoving = false;
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

    // Recalcule la direction selon l'angle de la camera
    private Vector3 GetDirection(Vector3 dir)
    {
        if (cameraAnglePalier == 0) return dir;
        for (int i = 0; i < cameraAnglePalier; i++)
            dir = new Vector3(-dir.z, 0, dir.x); // Rotation 90 deg
        return dir;
    }
}