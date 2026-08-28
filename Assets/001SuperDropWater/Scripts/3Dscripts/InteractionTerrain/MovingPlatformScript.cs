using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    public Vector3[] points;
    public int point_number = 0;
    private Vector3 current_target;

    public float tolerance;
    public float speed;
    public float delay_time;

    private float delay_start;

    public bool automatic;

    public GameObject Player;

    public bool _desacTrigger = false;
    public bool _desacMoving = false;

    void Start()
    {
        if (points.Length > 0)
        {
            current_target = points[0];
        }
        tolerance = speed * Time.deltaTime;
    }

    void LateUpdate()
    {
        if (transform.position != current_target) MovePlatform();
        else UpdateTarget();

        if (_desacMoving == true && Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetButtonDown("Jump"))
        {
            this.GetComponent<BoxCollider>().isTrigger = false;
            Player.GetComponent<DropController>().enabled = true;
           // Player.transform.parent = null;
            _desacMoving = false;
        }
        else this.GetComponent<BoxCollider>().isTrigger = true;

        if (_desacTrigger == true && Input.GetButtonDown("Jump"))
        {
            this.GetComponent<BoxCollider>().enabled = false;
            Player.GetComponent<DropController>().boxCibleJump();
            _desacTrigger = false;
           // Player.transform.parent = null;
        }
        else this.GetComponent<BoxCollider>().enabled = true; // mettre un timer de reactuivation

    }

    void MovePlatform()
    {
        Vector3 heading = current_target - transform.position;
        transform.position += (heading / heading.magnitude) * speed * Time.deltaTime;
        if (heading.magnitude < tolerance) transform.position = current_target; delay_start = Time.time;
    }

    void UpdateTarget()
    {
        if(automatic)
        {
            if (Time.time - delay_start > delay_time) NextPlatform();
        }
    }

    public void NextPlatform()
    {
        point_number++;
        if (point_number >= points.Length) point_number = 0;
        current_target = points[point_number];
    }

    private void OnTriggerEnter(Collider other)
	{
        if(other.gameObject == Player)
        {
            Player.transform.parent = transform;
            //Player.GetComponent<DropController>().enabled = false;            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = transform;
            Player.GetComponent<DropController>().enabled = false;
            _desacTrigger = true;
            _desacMoving = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = null;
            
        }

    }

    void jumpExit()
    {

    }



}
