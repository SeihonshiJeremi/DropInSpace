using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveDirection;
    private float _directionY;
    private bool _canDoubleJump = false;
    private GameObject player;
    private GameObject playerModel;
    private Transform pivot;

    public float gravityScale;
    public float jumpForce;
    public float rotateSpeed;
    public float moveSpeed;

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
            if (Input.GetButtonDown("Jump") && _canDoubleJump)
            {
                _directionY = jumpForce * 1.5f;
                _canDoubleJump = false;
            }

        }

        //Move The Player
        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = yStore;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);

        //Move the player in different directions based on camera look direction
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {            
            moveSpeed = moveSpeed * 1.0f;

            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //boxJump
        if(other.gameObject.tag == "boxJump")
        {
            _directionY = jumpForce * 2.0f;
            moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}
