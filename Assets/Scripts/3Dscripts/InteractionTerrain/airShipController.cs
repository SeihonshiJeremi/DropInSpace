using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airShipController : MonoBehaviour
{
    public float forwardSpeed = 25f, strafeSpeed = 7.5f, hoverSpeed = 5f;
    private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;
    private float forwardAcceleration = 2.5f, strafeAcceleration = 2f, hoverAcceleration = 2f;

    public float lookRateSpeed = 90f;   

    private float rollInput;
    public float rollSpeed = 90f, rollAcceleration = 3.5f;

    Vector3 newPosition;

    //pour effet de vitesse
    public Camera Camera;
    public float fov = 60.0f;
    //pour effet particule
    public GameObject particle;

    void Start()
    {
        Physics.gravity = new Vector3(0, 0, 0);
        particle.SetActive(false);

    }

    void Update()
    {
            rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Horizontal"), rollAcceleration * Time.deltaTime);

            transform.Rotate(-Input.GetAxisRaw("Vertical") * lookRateSpeed * Time.deltaTime, Input.GetAxisRaw("Horizontal") * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);

            activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Jump") * forwardSpeed, forwardAcceleration * Time.deltaTime);
            activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, Input.GetAxisRaw("Horizontal") * strafeSpeed, strafeAcceleration * Time.deltaTime);
            activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, hoverAcceleration * Time.deltaTime);

            transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
            transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {
            ChangePhysicGravity();
            
        }

        if (Input.GetButtonUp("Jump"))
        {
            ChangePhysicGravityExit();            
        }

        if (Input.GetButton("Dash"))
        {
            Camera.main.fieldOfView += Time.deltaTime * 5;
            forwardSpeed += Time.deltaTime * 5;
            if (Camera.main.fieldOfView > 130) Camera.main.fieldOfView = 130;
            if (forwardSpeed > 200) forwardSpeed = 200;
            particle.SetActive(true);
        }

        /*if (Input.GetButtonDown("Dash"))
        {
            forwardSpeed = forwardSpeed * 2;
        }*/

        if (Input.GetButtonUp("Dash"))
        {
            //forwardSpeed = forwardSpeed / 2;
            forwardSpeed = 100;
            Camera.main.fieldOfView = 60.0f;
            particle.SetActive(false);
        }
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
