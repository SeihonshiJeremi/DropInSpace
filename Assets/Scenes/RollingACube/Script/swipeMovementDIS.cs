using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class swipeMovementDIS : MonoBehaviour
{
    float timer = 0.250f;
    float timerOrigine = 0.250f;


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

    private void Awake()
    {
        position.Enable();
        press.Enable();
        press.performed += _ => { initialPos = currentPos; };
        press.canceled += _ => DetectSwipe();
        instance = this;
    }

    private void DetectSwipe()
    {
        Vector2 delta = currentPos - initialPos;
        Vector2 direction = Vector2.zero;

        if (Mathf.Abs(delta.x) > swipeResistance && xMovement == true)
        {
            direction.x = Mathf.Clamp(delta.x, -1, 1);
            if (direction.x > 0) xMove = true;
            if (direction.x < 0) xMoveNegate = true;
            yMovement = false;
        } else yMovement = true;
        if (Mathf.Abs(delta.y) > swipeResistance && yMovement == true)
        {
            direction.y = Mathf.Clamp(delta.y, -1, 1);
            if (direction.y > 0) yMove = true;
            if (direction.y < 0) yMoveNegate = true;
            xMovement = false;
        } else xMovement = true;
        if (direction != Vector2.zero & swipePerformed != null)
            swipePerformed(direction);
    }

    public void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            xMove = false;
            xMoveNegate = false;
            yMove = false;
            yMoveNegate = false;
        }

        if (xMove == true)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                movementPlayer.GetComponent<rollingCube>().AssembleRight();
                timer = 0.250f;
            }            
        } 

        if(xMoveNegate == true)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                movementPlayer.GetComponent<rollingCube>().AssembleLeft();
                timer = 0.250f;
            }
        }

        if (yMove == true)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                movementPlayer.GetComponent<rollingCube>().AssembleForward();
                timer = 0.250f;
            }
        }

        if (yMoveNegate == true)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                movementPlayer.GetComponent<rollingCube>().AssembleBack();
                timer = 0.250f;
            }
        }
    }


}
