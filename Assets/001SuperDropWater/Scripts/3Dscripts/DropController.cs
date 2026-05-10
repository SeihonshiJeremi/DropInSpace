using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveDirection;
    private float _directionY;
    public float currentSpeed = 0.0f;
    public bool _canDoubleJump = false;
    private GameObject player;
    private GameObject playerModel;
    private Transform pivot;

    public float knockBackForce;
    public float knockBackTime;
    private float knockBackCounter;

    public float gravityScale;
    public float jumpForce;
    public float rotateSpeed;
    public float moveSpeed;
    public float velocitySpeed;

    const float Acceleration = 10.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        player = GameObject.Find("Player");
        playerModel = GameObject.Find("playerModel");
        pivot = GameObject.Find("pivot").GetComponent<Transform>();        
    }

    void Update()
    {
        //Gravité
        moveDirection.y = _directionY;
        _directionY -= gravityScale * Time.deltaTime;
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);       

        //Saut
        if (controller.isGrounded)
        {
            _canDoubleJump = true;

            if (Input.GetButtonDown("Jump"))
            {
                _directionY = jumpForce * 1.0f;
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && _canDoubleJump == true)
            {
                _directionY = jumpForce * 1.2f;
                _canDoubleJump = false;
            }

        }

        if (knockBackCounter <= 0)
        {
            //Move The Player
            float yStore = moveDirection.y;
            moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
            moveDirection = moveDirection.normalized * currentSpeed;
            moveDirection.y = yStore;

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);

            //Move the player in different directions based on camera look direction
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                //acceleration
                currentSpeed += Acceleration * Time.deltaTime;
                if (currentSpeed > moveSpeed) currentSpeed = moveSpeed;

                transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            } 
            //decceleration
            else currentSpeed -= Acceleration * Time.deltaTime;
            if (currentSpeed < 0) currentSpeed = 0;
        }
        else knockBackCounter -= Time.deltaTime; 

        //Velocity
        if(!controller.isGrounded)
        {
            if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                Vector3 horizontalVelocity = controller.velocity;
                horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);

                // The speed on the x-z plane ignoring any speed
                float horizontalSpeed = horizontalVelocity.magnitude;
                // The speed from gravity or jumping
                float verticalSpeed = controller.velocity.y;
                // The overall speed
                float overallSpeed = controller.velocity.magnitude;

                moveDirection = horizontalVelocity;
                moveDirection = moveDirection.normalized * velocitySpeed;

                Debug.Log("Velocity");
            }

        }
    }

    //Quand je reçois un dégat 
    public void Knockback(Vector3 direction)
    {        
        knockBackCounter = knockBackTime;
        moveDirection = direction * knockBackForce;
        moveDirection.y = knockBackForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        //boxJump
        if(other.gameObject.tag == "boxJump")
        {
            _directionY = jumpForce * 1.2f;
            moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
            controller.Move(moveDirection * Time.deltaTime);
        }

        //moveSpeed20 movingPlatform
        if (other.gameObject.tag == "movingPlatform")
        {
            currentSpeed = 20.0f;
        }
    }

    public void boxCibleJump()
    {
        _directionY = jumpForce * 1f;
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);
    }

    public void movingPlatform()
    {
        _directionY = jumpForce * 0.5f;
       /* moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);*/
    }
}
