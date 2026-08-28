using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxCible : MonoBehaviour
{
    public GameObject Cible;
    public bool _canAttackCible = false;
    private float moveSpeed = 300.0f;
    public float boxCibleTime;
    private GameObject player;
    private CharacterController controller;

    void Start()
    {
        player = GameObject.Find("Player");
        controller = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        //attackCible
        if (_canAttackCible == true && Input.GetButton("Dash"))
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, Cible.transform.position, moveSpeed);
            controller.enabled = false;
        }
        else controller.enabled = true;

        //delaiReActivation
        boxCibleTime -= Time.deltaTime;
        if(boxCibleTime < 0)
        {
            Cible.GetComponent<MeshRenderer>().enabled = true;
            this.GetComponent<SphereCollider>().enabled = true;
            this.GetComponent<BoxCollider>().enabled = true;
        }
        else if (boxCibleTime > 0)
        {
            Cible.GetComponent<MeshRenderer>().enabled = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" )
        {
            _canAttackCible = true;         
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _canAttackCible = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<DropController>().boxCibleJump();
            _canAttackCible = false;
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponent<SphereCollider>().enabled = false;
            boxCibleTime = 10.0f;
        }
        
    }
}
