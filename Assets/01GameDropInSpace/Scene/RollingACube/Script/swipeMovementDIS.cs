using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class swipeMovementDIS : MonoBehaviour
{
    float timer = 0.250f;
    bool xMovement;
    bool yMovement;
    bool xMove;
    bool xMoveNegate;
    bool yMove;
    bool yMoveNegate;

    public GameObject movementPlayer;
    public static swipeMovementDIS instance;
    public delegate void Swipe(Vector2 direction);
    public event Swipe swipePerformed;

    [SerializeField] private InputAction position, press;
    [SerializeField] private float swipeResistance = 100;
    private Vector2 initialPos;
    private Vector2 currentPos => position.ReadValue<Vector2>();

    public float timeDesacTouche = 3.0f;

    // CORRIGÉ : cache rollingCube
    private rollingCube playerRolling;

    private void Awake()
    {
        position.Enable();
        press.Enable();
        press.performed += _ => { initialPos = currentPos; };
        press.canceled += _ => DetectSwipe();
        instance = this;
    }

    void Start()
    {
        playerRolling = movementPlayer.GetComponent<rollingCube>();
    }

    private void DetectSwipe()
    {
        Vector2 delta = currentPos - initialPos;
        Vector2 direction = Vector2.zero;

        if (Mathf.Abs(delta.x) > swipeResistance && xMovement)
        {
            direction.x = Mathf.Clamp(delta.x, -1, 1);
            if (direction.x > 0) xMove = true;
            if (direction.x < 0) xMoveNegate = true;
            yMovement = false;
        }
        else yMovement = true;

        if (Mathf.Abs(delta.y) > swipeResistance && yMovement)
        {
            direction.y = Mathf.Clamp(delta.y, -1, 1);
            if (direction.y > 0) yMove = true;
            if (direction.y < 0) yMoveNegate = true;
            xMovement = false;
        }
        else xMovement = true;

        if (direction != Vector2.zero && swipePerformed != null)
            swipePerformed(direction);
    }

    public void FixedUpdate()
    {
        // CORRIGÉ : timer s'arrête à 0
        if (timeDesacTouche > 0) timeDesacTouche -= Time.deltaTime;

        if (Input.touchCount > 0)
        {
            xMove = xMoveNegate = yMove = yMoveNegate = false;
        }

        // CORRIGÉ : un seul timer partagé correctement
        if (timeDesacTouche <= 0)
        {
            if (xMove || xMoveNegate || yMove || yMoveNegate)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    if (xMove) playerRolling.AssembleRight();
                    else if (xMoveNegate) playerRolling.AssembleLeft();
                    else if (yMove) playerRolling.AssembleForward();
                    else if (yMoveNegate) playerRolling.AssembleBack();
                    timer = 0.250f;
                }
            }
        }
    }

    public void tDesacTouche()
    {
        xMove = xMoveNegate = yMove = yMoveNegate = false;
        timeDesacTouche = 3.0f;
    }
}