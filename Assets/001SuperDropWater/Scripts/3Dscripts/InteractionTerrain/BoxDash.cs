using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDash : MonoBehaviour
{
    public Transform endBoxDash;
    public float moveSpeed;
    private float boxDashTime;
    private GameObject player;
    private CharacterController controller;

    void Start()
    {
        player = GameObject.Find("Player");
        controller = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        //boxDash
        boxDashTime -= Time.deltaTime;
        if (boxDashTime > 0)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, endBoxDash.transform.position, moveSpeed * Time.deltaTime);

           /* player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | 
                RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;*/
            controller.enabled = false;
        }
        else controller.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //boxDash
        if (other.gameObject.tag == "Player")
        {
            boxDashTime = 1.0f;
        }
    }
}
