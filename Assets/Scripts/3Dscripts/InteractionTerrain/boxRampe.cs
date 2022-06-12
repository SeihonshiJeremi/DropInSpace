using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxRampe : MonoBehaviour
{
    public GameObject Cible;
    public GameObject BoxRampe;
    private bool _canAttackCible = false;
    private float moveSpeed = 2.0f;
    private float boxDashTime;
    private GameObject player;
    private CharacterController controller;

    void Start()
    {
        player = GameObject.Find("Player");
        controller = player.GetComponent<CharacterController>();
        BoxRampe.SetActive(false);
    }

    void Update()
    {
        //attackCible
        if (_canAttackCible == true)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, Cible.transform.position, moveSpeed);
            controller.enabled = false;
        }
        else controller.enabled = true;

        boxDashTime -= Time.deltaTime;
        if(boxDashTime > 0)
        {
            BoxRampe.SetActive(true);
        }
        else BoxRampe.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _canAttackCible = true;
            boxDashTime = 1.0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _canAttackCible = false;
        }
    }
}
