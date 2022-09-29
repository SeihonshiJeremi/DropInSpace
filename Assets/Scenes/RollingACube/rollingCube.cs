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



       


}
