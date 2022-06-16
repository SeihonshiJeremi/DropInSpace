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

    void Start()
    {
        if (points.Length > 0)
        {
            current_target = points[0];
        }
        tolerance = speed * Time.deltaTime;
    }

    void Update()
    {
        if (transform.position != current_target) MovePlatform();
        else UpdateTarget();

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Player.GetComponent<DropController>().enabled = true;
        }

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
            Player.GetComponent<DropController>().enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = transform;
            Player.GetComponent<DropController>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = null;
        }

    }



}
