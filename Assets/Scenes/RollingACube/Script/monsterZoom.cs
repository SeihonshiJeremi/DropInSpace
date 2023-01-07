using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class monsterZoom : MonoBehaviour
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

    public bool _collisionAvecChangerOrientationDroite = false;
    public bool _collisionAvecChangerOrientationGauche = false;
    public bool _collisionAvecChangerOrientationAv = false;
    public bool _collisionAvecChangerOrientationArr = false;

    public float x;
    public float z;

    public float tempsAvantDeplacement;
    public float tempsAvantDeplacementOrigine;
    public int choixDirection;
    public int choixDirectionHorizontal;
    public int choixDirectionVertical;

    public GameObject Monster01;
    public GameObject ModelMonster01;
    public GameObject Monster02;

    public LayerMask whatIsMonster;

    public bool DirectionAleatoire;
    public bool DirectionHorizontale;
    public bool DirectionHorizontaleSingle;
    public bool DirectionVertical;
    public bool DirectionVerticalSingle;

    public string tagMonster;

    public Transform ignoreCollision;    

    //en eattendan
    int reset;

    void Start()
    {
        Transform ignore = ignoreCollision as Transform;
        Physics.IgnoreCollision(ignore.GetComponent<Collider>(), GetComponent<Collider>());
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

        //en attendant
        if (reset == 1)
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
    }

    void FixedUpdate()
    {

        tempsAvantDeplacement -= Time.deltaTime;

        //direction en fonction du monstre
        if (DirectionAleatoire == true) choixDirection = Random.Range(1, 5);
        if (DirectionHorizontale == true) choixDirection = Random.Range(1, 3);
        if (DirectionVertical == true) choixDirection = Random.Range(3, 5);
        if (DirectionHorizontaleSingle == true) choixDirection = Random.Range(1, 2);
        if (DirectionVerticalSingle == true) choixDirection = Random.Range(3, 4);

        //direction lors de collision avec changer
        if (_collisionAvecChangerOrientationGauche == true) choixDirection = 1;
        if (_collisionAvecChangerOrientationDroite == true) choixDirection = 2;
        if (_collisionAvecChangerOrientationAv == true) choixDirection = 3;
        if (_collisionAvecChangerOrientationArr == true) choixDirection = 4;

        if (_isMoving) return;

        else if (tempsAvantDeplacement < 0 && choixDirection == 1)
        {
            Assemble(Vector3.left);
            tempsAvantDeplacement = tempsAvantDeplacementOrigine;
            ModelMonster01.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        else if (tempsAvantDeplacement < 0 && choixDirection == 2)
         {
             Assemble(Vector3.right);
             tempsAvantDeplacement = tempsAvantDeplacementOrigine;
            ModelMonster01.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
        }

         else if (tempsAvantDeplacement < 0 && choixDirection == 3)
         {
             Assemble(Vector3.forward);
             tempsAvantDeplacement = tempsAvantDeplacementOrigine;
            ModelMonster01.transform.rotation = Quaternion.Euler(new Vector3(0, -270, 0));
        }

         else if (tempsAvantDeplacement < 0 && choixDirection == 4)
         {
             Assemble(Vector3.back);
             tempsAvantDeplacement = tempsAvantDeplacementOrigine;
            ModelMonster01.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
        }

        void Assemble(Vector3 dir)
        {
            var anchor = transform.position + (Vector3.down + dir) * 1.085f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));
        }
        /* //view ray
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
             if (hit.transform.tag == "Monster02")
             {
                 Debug.Log("touche");
                 _collisionAvecMonster = true;

                 if (_collisionAvecMonster == true && choixDirectionHorizontal == 1)
                 {
                     Assemble(Vector3.left);
                 }

                 if (_collisionAvecMonster == true && choixDirectionHorizontal == 2)
                 {
                     Assemble(Vector3.right);
                 }
             }
             else _collisionAvecMonster = false;
         }

         //back
         if (Physics.Raycast(rayBack, out hit))
         {
             if (hit.transform.tag == "Monster02")
             {
                 Debug.Log("touche");
                 _collisionAvecMonster = true;

                 if (_collisionAvecMonster == true && choixDirectionHorizontal == 1)
                 {
                     Assemble(Vector3.left);
                 }

                 if (_collisionAvecMonster == true && choixDirectionHorizontal == 2)
                 {
                     Assemble(Vector3.right);
                 }
             }
             else _collisionAvecMonster = false;
         }

         //right
         if (Physics.Raycast(rayRight, out hit))
         {
             if (hit.transform.tag == "Monster02")
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
             if (hit.transform.tag == "Monster02")
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
         }*///code masquer
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
    }

    void roundValeurZ()
    {
        z = Mathf.Round(z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "changeCubeMaterial") _collisionOK = true;

        if (other.gameObject.tag == "Player") Destroy(Player);
        if (other.gameObject.tag == "Player") reset = 1;

        if (other.gameObject.tag == tagMonster)
        {
            _collisionAvecMonster = true;
            Monster02.GetComponent<monsterZoom>().enabled = false;
        }

        if (other.gameObject.tag == "orientationChangerDroite") _collisionAvecChangerOrientationDroite = true;
        if (other.gameObject.tag == "orientationChangerGauche") _collisionAvecChangerOrientationGauche = true;
        if (other.gameObject.tag == "orientationChangerAv") _collisionAvecChangerOrientationAv = true;
        if (other.gameObject.tag == "orientationChangerArr") _collisionAvecChangerOrientationArr = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "changeCubeMaterial") _collisionOK = false;

        if (other.gameObject.tag == tagMonster)
        {
            _collisionAvecMonster = false;
            Monster02.GetComponent<monsterZoom>().enabled = true;
        }

        if (other.gameObject.tag == "orientationChangerDroite") _collisionAvecChangerOrientationDroite = false;
        if (other.gameObject.tag == "orientationChangerGauche") _collisionAvecChangerOrientationGauche = false;
        if (other.gameObject.tag == "orientationChangerAv") _collisionAvecChangerOrientationAv = false;
        if (other.gameObject.tag == "orientationChangerArr") _collisionAvecChangerOrientationArr = false;
    }
}
