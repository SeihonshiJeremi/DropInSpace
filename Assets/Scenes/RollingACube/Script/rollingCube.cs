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
    private bool _isMoving;
    public bool _collisionOK = false;

    public float x;
    public float z;

    void Start()
    {

    }

    void Update()
    {
        x = this.transform.position.x;
        z = this.transform.position.z;
        roundValeurX();
        roundValeurZ();     
        
        if(_isMoving == false && _collisionOK == true)
        {
            this.transform.position = new Vector3(x, 1.0f, z);
        }
    }

    void FixedUpdate()
    {
        if (_isMoving) return;

        else if (Input.GetKey(arrowLeft)) Assemble(Vector3.left);
        else if (Input.GetKey(arrowRight)) Assemble(Vector3.right);
        else if (Input.GetKey(arrowUp)) Assemble(Vector3.forward);
        else if (Input.GetKey(arrowDown)) Assemble(Vector3.back);

        void Assemble(Vector3 dir)
        {
            var anchor = transform.position + (Vector3.down + dir) * 1.085f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));            
        }
    }

    IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        _isMoving = true;

        for(int i = 0; i < (90 / _rollSpeed); i++)
        {
            transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01f);

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

}
