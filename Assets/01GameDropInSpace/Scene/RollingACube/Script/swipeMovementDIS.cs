using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class swipeMovementDIS : MonoBehaviour
{
    [Header("Sensibilité")]
    public float swipeResistance = 40f;   // Plus bas = plus réactif

    [Header("Respawn (NE PAS TOUCHER)")]
    public float timeDesacTouche = 3.0f;

    public GameObject movementPlayer;
    public static swipeMovementDIS instance;
    public delegate void Swipe(Vector2 direction);
    public event Swipe swipePerformed;

    [SerializeField] private InputAction position, press;

    private Vector2 initialPos;
    private Vector2 currentPos => position.ReadValue<Vector2>();

    private rollingCube playerRolling;
    private bool isDragging = false;

    private void Awake()
    {
        position.Enable();
        press.Enable();

        press.started += _ =>
        {
            initialPos = currentPos;
            isDragging = true;
        };

        press.canceled += _ =>
        {
            isDragging = false;

            // Event swipe pour compat externe
            Vector2 delta = currentPos - initialPos;
            if (delta.magnitude > swipeResistance && swipePerformed != null)
            {
                Vector2 direction = Vector2.zero;
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    direction.x = Mathf.Sign(delta.x);
                else
                    direction.y = Mathf.Sign(delta.y);
                swipePerformed(direction);
            }
        };

        instance = this;
    }

    void Start()
    {
        playerRolling = movementPlayer.GetComponent<rollingCube>();
    }

    void FixedUpdate()
    {
        // ─── COOLDOWN RESPAWN (INTACT) ───
        if (timeDesacTouche > 0)
        {
            timeDesacTouche -= Time.deltaTime;
            return;
        }

        if (!isDragging) return;

        // ─── STICK VIRTUEL : initialPos NE BOUGE PAS ───
        // Tant que le doigt reste dans une direction, Drop continue d'avancer
        // rollingCube._isMoving empêche naturellement les doubles mouvements
        Vector2 delta = currentPos - initialPos;

        if (delta.magnitude > swipeResistance)
        {
            // Direction dominante (comme un D-pad)
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (delta.x > 0) playerRolling.AssembleRight();
                else playerRolling.AssembleLeft();
            }
            else
            {
                if (delta.y > 0) playerRolling.AssembleForward();
                else playerRolling.AssembleBack();
            }
            // ← PAS DE RECENTRAGE : initialPos reste fixe
            // Le doigt doit juste rester dans la direction voulue
        }
    }

    // ─── BLOCAGE APRÈS RESPAWN (INTACT) ───
    public void tDesacTouche()
    {
        isDragging = false;
        timeDesacTouche = 3.0f;
    }

    // ─── RESET FORCÉ (INTACT) ───
    public void ForceResetPress()
    {
        isDragging = false;
    }
}