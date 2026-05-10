using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class monsterZoom021 : MonoBehaviour
{
    public GameObject Player;

    [SerializeField] private float _rollSpeed = 3;
    private bool _isMoving;
    public bool _collisionOK = false;
    public bool _collisionAvecMonster = false;

    public float x, z;
    public float tempsAvantDeplacement;
    public float tempsAvantDeplacementOrigine;
    public int choixDirection;

    // CORRIGÉ : LifeBarManager caché
    private LifeBarManager lifeBarManager;

    void Start()
    {
        // CORRIGÉ : cache LifeBarManager
        GameObject lifeBarObj = GameObject.FindWithTag("lifeBar");
        if (lifeBarObj != null) lifeBarManager = lifeBarObj.GetComponent<LifeBarManager>();
    }

    void Update()
    {
        x = Mathf.Round(transform.position.x);
        z = Mathf.Round(transform.position.z);

        if (!_isMoving && _collisionOK)
            transform.position = new Vector3(x, 1.0f, z);
    }

    void FixedUpdate()
    {
        tempsAvantDeplacement -= Time.deltaTime;
        // CORRIGÉ : direction calculée une seule fois par cycle
        choixDirection = Random.Range(1, 5);

        if (_isMoving) return;

        if (!_collisionAvecMonster && tempsAvantDeplacement < 0)
        {
            Vector3 dir = Vector3.zero;
            if (choixDirection == 1) dir = Vector3.left;
            else if (choixDirection == 2) dir = Vector3.right;
            else if (choixDirection == 3) dir = Vector3.forward;
            else if (choixDirection == 4) dir = Vector3.back;

            if (dir != Vector3.zero)
            {
                var anchor = transform.position + (Vector3.down + dir) * 1.085f;
                var axis = Vector3.Cross(Vector3.up, dir);
                StartCoroutine(Roll(anchor, axis));
                tempsAvantDeplacement = tempsAvantDeplacementOrigine;
            }
        }
    }

    IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        _isMoving = true;
        for (int i = 0; i < (90 / _rollSpeed); i++)
        {
            transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForFixedUpdate();
        }
        _isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("changeCubeMaterial")) _collisionOK = true;

        if (other.CompareTag("Player"))
        {
            // CORRIGÉ : Reset via LifeBarManager au lieu de Destroy + reload manuel
            if (lifeBarManager != null) lifeBarManager.Reset();
            else
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("changeCubeMaterial")) _collisionOK = false;
    }
}