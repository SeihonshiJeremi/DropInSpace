using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rollingCube : MonoBehaviour
{
    [Header("keybinds")]
    public KeyCode arrowLeft = KeyCode.Space;
    public KeyCode arrowRight = KeyCode.Space;
    public KeyCode arrowUp = KeyCode.Space;
    public KeyCode arrowDown = KeyCode.Space;

    [SerializeField] private float _rollSpeed = 3;
    public bool _isMoving;
    public bool _collisionOK = false;

    public float x;
    public float z;
    public float y;

    public bool desacTouche = false;
    public float timeDesacTouche = 3.0f;

    public float horizontal;
    public float vertical;

    //pour les item
    public GameObject item;

    /*//pour le tactile
    public Vector2 startPos;
    public Vector2 direction;
    public bool directionChosen;
    public float screenWidth = Screen.width;
    public float screenHeight = Screen.height;*/

    /*void Awake()
    {
       QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 10;
    }*/

    void Update()
    {
        x = this.transform.position.x;
        z = this.transform.position.z;
        y = this.transform.position.y;
        roundValeurX();
        roundValeurZ();     
        
        if(_isMoving == false && _collisionOK == true)
        {
            this.transform.position = new Vector3(x, 1.0f, z);
        }

        //activation object start party
        timeDesacTouche -= Time.deltaTime;
        if (timeDesacTouche <= 0.0f) activTouche();
        if (timeDesacTouche >= 0.0f) desactivationTouche(); 

        horizontal = Input.GetAxis("DPAD-Horizontal");
        vertical = Input.GetAxis("DPAD-Vertical");
    }

    void FixedUpdate()
    {
        if(desacTouche == false)
        {
            if (_isMoving) return;

            //touche clavier
            else if (Input.GetKey(arrowLeft)) Assemble(Vector3.left);
            else if (Input.GetKey(arrowRight)) Assemble(Vector3.right);
            else if (Input.GetKey(arrowUp)) Assemble(Vector3.forward);
            else if (Input.GetKey(arrowDown)) Assemble(Vector3.back);

            //touche manette xbox
            else if (horizontal < 0) Assemble(Vector3.left);
            else if (horizontal > 0) Assemble(Vector3.right);
            else if (vertical < 0) Assemble(Vector3.forward);
            else if (vertical > 0) Assemble(Vector3.back);

           /* //tactile
            // Track a single touch as a direction control.
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                startPos = new Vector2(screenWidth / 2, screenHeight / 2);
                direction = touch.position - startPos;
            }
            else if (direction.x < 0) Assemble(Vector3.left); 
            else if (direction.x > 0) Assemble(Vector3.right);
            else if (direction.y > 0 && direction.y > direction.x) Assemble(Vector3.forward);
            else if (direction.y < 0) Assemble(Vector3.back);
             

            if (directionChosen)
            {
                // Something that uses the chosen direction...

            }*/

            void Assemble(Vector3 dir)
            {
                var anchor = transform.position + (Vector3.down + dir) * 1.085f;
                var axis = Vector3.Cross(Vector3.up, dir);
                StartCoroutine(Roll(anchor, axis));

               /* direction = new Vector2(0, 0);
                /*startPos = new Vector2(0, 0);*/
            }
        }

    }

    IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        _isMoving = true;

        for(int i = 0; i < (90 / _rollSpeed); i++)
        {
            transform.RotateAround(anchor, axis, _rollSpeed);
            //yield return new WaitForSeconds(0.01f);
            yield return new WaitForFixedUpdate();

            GetComponent<AudioSource>().Play();
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
        //1er item
        if (other.gameObject.tag == "itemTimeDiv2") item.GetComponent<itemDropInSpace>().itemTimeDiv2F();
        if (other.gameObject.tag == "itemTimeStop") item.GetComponent<itemDropInSpace>().itemTimeStopF();
        if (other.gameObject.tag == "itemInvincibilty") item.GetComponent<itemDropInSpace>().itemInvincibiltyF();
        if (other.gameObject.tag == "itemBonbonScoreRouge") item.GetComponent<itemDropInSpace>().itemBonbonScoreRougeF();
        if (other.gameObject.tag == "itemBonbonScoreVert") item.GetComponent<itemDropInSpace>().itemBonbonScoreVertF();
        if (other.gameObject.tag == "itemRandom") item.GetComponent<itemDropInSpace>().itemRandomF();

        //2eme item
        if (other.gameObject.tag == "itemTimeDiv202") item.GetComponent<itemDropInSpace>().itemTimeDiv2F02();
        if (other.gameObject.tag == "itemTimeStop02") item.GetComponent<itemDropInSpace>().itemTimeStopF02();
        if (other.gameObject.tag == "itemInvincibilty02") item.GetComponent<itemDropInSpace>().itemInvincibiltyF02();
        if (other.gameObject.tag == "itemBonbonScoreRouge02") item.GetComponent<itemDropInSpace>().itemBonbonScoreRougeF02();
        if (other.gameObject.tag == "itemBonbonScoreVert02") item.GetComponent<itemDropInSpace>().itemBonbonScoreVertF02();
        if (other.gameObject.tag == "itemRandom02") item.GetComponent<itemDropInSpace>().itemRandomF02();

        if (other.gameObject.tag == "changeCubeMaterial")
        {
            _collisionOK = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "changeCubeMaterial")
        {
            _collisionOK = false;
        }
    }

    public void desactivationTouche()
    {
        desacTouche = true;
        this.transform.position = new Vector3(8, y, 8);
    }

    public void activTouche()
    {
        desacTouche = false;
    }

    public void tDesacTouche()
    {
        timeDesacTouche = 3.0f;
    }

    public void AssembleLeft()
    {
        Assemble(Vector3.left);

        void Assemble(Vector3 dir)
        {
            var anchor = transform.position + (Vector3.down + dir) * 1.085f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));
        }
    }

    public void AssembleRight()
    {
        Assemble(Vector3.right);

        void Assemble(Vector3 dir)
        {
            var anchor = transform.position + (Vector3.down + dir) * 1.085f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));
        }
    }

    public void AssembleForward()
    {
        Assemble(Vector3.forward);

        void Assemble(Vector3 dir)
        {
            var anchor = transform.position + (Vector3.down + dir) * 1.085f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));
        }
    }

    public void AssembleBack()
    {
        Assemble(Vector3.back);

        void Assemble(Vector3 dir)
        {
            var anchor = transform.position + (Vector3.down + dir) * 1.085f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));
        }
    }
}
