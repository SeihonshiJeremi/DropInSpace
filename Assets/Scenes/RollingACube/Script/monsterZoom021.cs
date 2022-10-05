using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterZoom021 : MonoBehaviour
{
   /* [Header("keybinds")]
    public KeyCode arrowLeft = KeyCode.Space;
    public KeyCode arrowRight = KeyCode.Space;
    public KeyCode arrowUp = KeyCode.Space;
    public KeyCode arrowDown = KeyCode.Space;*/

    public GameObject Player;

    [SerializeField] private float _rollSpeed = 3;
    private bool _isMoving;
    public bool _collisionOK = false;
    public bool _collisionAvecMonster = false;

    public float x;
    public float z;


    public float tempsAvantDeplacement;
    public float tempsAvantDeplacementOrigine;
    public int choixDirection;
    public int choixDirectionHorizontal;
    public int choixDirectionVertical;



    void Start()
    {

    }

    void Update()
    {
        x = this.transform.position.x;
        z = this.transform.position.z;
        roundValeurX();
        roundValeurZ();

        if (_isMoving == false && _collisionOK == true)
        {
            this.transform.position = new Vector3(x, 1.0f, z);
        }
    }

    void FixedUpdate()
    {
        tempsAvantDeplacement -= Time.deltaTime;
        choixDirection = Random.Range(1,5);
        choixDirectionHorizontal = Random.Range(1, 2);
        choixDirectionVertical = Random.Range(1, 2);

        if (_isMoving) return;

        else if ( _collisionAvecMonster == false && tempsAvantDeplacement < 0 && choixDirection == 1)
        {
            Assemble(Vector3.left);
            tempsAvantDeplacement = tempsAvantDeplacementOrigine;
        }

        else if ( _collisionAvecMonster == false && tempsAvantDeplacement < 0 && choixDirection == 2)
        {
            Assemble(Vector3.right);
            tempsAvantDeplacement = tempsAvantDeplacementOrigine;
        }

        else if (_collisionAvecMonster == false && tempsAvantDeplacement < 0 && choixDirection == 3)
        {
            Assemble(Vector3.forward);
            tempsAvantDeplacement = tempsAvantDeplacementOrigine;
        }

        else if (_collisionAvecMonster == false && tempsAvantDeplacement < 0 && choixDirection == 4)
        {
            Assemble(Vector3.back);
            tempsAvantDeplacement = tempsAvantDeplacementOrigine;
        }

        void Assemble(Vector3 dir)
        {
            var anchor = transform.position + (Vector3.down + dir) * 1.085f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));
        }

        /*//view ray
        Debug.DrawRay(transform.position, Vector3.forward * 10, Color.red);
        Debug.DrawRay(transform.position, Vector3.back * 10, Color.blue);
        Debug.DrawRay(transform.position, Vector3.right * 10, Color.green);
        Debug.DrawRay(transform.position, Vector3.left * 10, Color.yellow);

        //rayon touche quelque chose
        RaycastHit hit;
        Ray rayForward = new Ray(transform.position, Vector3.forward);
        Ray rayBack = new Ray(transform.position, Vector3.back);
        Ray rayRight = new Ray(transform.position, Vector3.right);
        Ray rayLeft = new Ray(transform.position, Vector3.left);

        //forward
        if (Physics.Raycast(rayForward, out hit))
        {
            if(hit.transform.tag == "Monster")
            {
                Debug.Log("touche");
                _collisionAvecMonster = true;

                if (_collisionAvecMonster == true &&  choixDirectionHorizontal == 1)
                {
                    Assemble(Vector3.left);                  
                } 

                if (_collisionAvecMonster == true &&  choixDirectionHorizontal == 2)
                {
                    Assemble(Vector3.right);                
                } 
            }
            else _collisionAvecMonster = false;
        } 

        //back
        if (Physics.Raycast(rayBack, out hit))
        {
            if (hit.transform.tag == "Monster")
            {
                Debug.Log("touche");
                _collisionAvecMonster = true;

                if (_collisionAvecMonster == true &&  choixDirectionHorizontal == 1)
                {
                    Assemble(Vector3.left);                   
                } 

                if (_collisionAvecMonster == true &&  choixDirectionHorizontal == 2)
                {
                    Assemble(Vector3.right);                 
                } 
            }
            else _collisionAvecMonster = false;
        }

        //right
        if (Physics.Raycast(rayRight, out hit))
        {
            if (hit.transform.tag == "Monster")
            {
                Debug.Log("touche");
                _collisionAvecMonster = true;

                if (_collisionAvecMonster == true && choixDirectionVertical == 1)
                {
                    Assemble(Vector3.forward);
                }

                if (_collisionAvecMonster == true && choixDirectionVertical == 2)
                {
                    Assemble(Vector3.back);
                }
            }
            else _collisionAvecMonster = false;
        }

        //left
        if (Physics.Raycast(rayLeft, out hit))
        {
            if (hit.transform.tag == "Monster")
            {
                Debug.Log("touche");
                _collisionAvecMonster = true;

                if (_collisionAvecMonster == true && choixDirectionVertical == 1)
                {
                    Assemble(Vector3.forward);
                }

                if (_collisionAvecMonster == true && choixDirectionVertical == 2)
                {
                    Assemble(Vector3.back);
                }
            }
            else _collisionAvecMonster = false;
        }*/

    }

    IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        _isMoving = true;

        for (int i = 0; i < (90 / _rollSpeed); i++)
        {
            transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }

        _isMoving = false;
    }

    void roundValeurX()
    {
        x = Mathf.Round(x);
        //Debug.Log(Mathf.Round(x));
    }

    void roundValeurZ()
    {
        z = Mathf.Round(z);
        //Debug.Log(Mathf.Round(z));
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "changeCubeMaterial")
        {
            _collisionOK = true;
        }

        if (other.gameObject.tag == "Player")
        {
            Destroy(Player);
        }

        if (other.gameObject.tag == "Monster")
        {
            //Destroy(Monster01);
            //  Instantiate(Monster01);
           // this.GetComponent<BoxCollider>().isTrigger = true;
            //this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }

        if (other.gameObject.tag == "Monster02")
        {
            // Destroy(Monster02);
            // Instantiate(Monster02);
           // this.GetComponent<BoxCollider>().isTrigger = true;
           // this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "changeCubeMaterial")
        {
            _collisionOK = false;
        }

       // this.GetComponent<BoxCollider>().isTrigger = false;
       // this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    public void OnCollisionEnter(Collision collision)
    {
        /*if (collision.collider.tag == "Player")
        {
            Destroy(Player);
        }*/

       /* if (collision.collider.tag == "Monster")
        {
            this.GetComponent<BoxCollider>().isTrigger = true;
        } else this.GetComponent<BoxCollider>().isTrigger = false;

        if (collision.collider.tag == "Monster02")
        {
            this.GetComponent<BoxCollider>().isTrigger = true;
        } else this.GetComponent<BoxCollider>().isTrigger = false;*/
    }
}
