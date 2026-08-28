using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class monsterZoom : MonoBehaviour
{
    public float durationItem = 10.0f;
    public float durationItemOrigine = 10.0f;

    public GameObject Player;

    [SerializeField] private float _rollSpeed = 3;
    private bool _isMoving;
    public bool _collisionOK = false;
    public bool _collisionAvecMonster = false;

    public bool _collisionAvecChangerOrientationDroite = false;
    public bool _collisionAvecChangerOrientationGauche = false;
    public bool _collisionAvecChangerOrientationAv = false;
    public bool _collisionAvecChangerOrientationArr = false;

    public float x, z;
    public float tempsAvantDeplacement;
    public float tempsAvantDeplacementOrigine;
    public int choixDirection;

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

    public bool followMonster;
    public float distanceBetweenObjectsNorth;
    public float distanceBetweenObjectsSouth;
    public float distanceBetweenObjectsEast;
    public float distanceBetweenObjectsWest;
    public GameObject North, South, East, West;

    public GameObject item;
    public GameObject scriptZoomManager;

    public bool boolIgnoreCollisionPlayer = false;
    public Transform ignoreCollisionPlayer;

    public int choixItemRandom;
    public bool MonsterTP;
    public GameObject Monster01Vector;
    public int choixPlatform;

    public bool pos01, pos02, pos03, pos04, pos05, pos06, pos07, pos08, pos09;
    public bool pos10, pos11, pos12, pos13, pos14, pos15, pos16, pos17, pos18, pos19;
    public bool pos20, pos21, pos22, pos23, pos24, pos25, pos26, pos27, pos28, pos29;
    public bool pos30, pos31, pos32, pos33, pos34, pos35, pos36, pos37, pos38, pos39;
    public bool pos40, pos41, pos42, pos43, pos44, pos45, pos46, pos47, pos48, pos49;
    public bool pos50, pos51, pos52, pos53, pos54, pos55, pos56, pos57, pos58, pos59;
    public bool pos60, pos61, pos62, pos63, pos64, pos65, pos66, pos67, pos68, pos69;
    public bool pos70, pos71, pos72, pos73, pos74, pos75, pos76, pos77, pos78, pos79;
    public bool pos80, pos81;

    public GameObject lifeBar;
    public int reset = 3;

    // CORRIGÉ : cache des composants
    private zoomManager zm;
    private itemDropInSpace itemDIS;
    private LifeBarManager lifeBarManager;
    private monsterZoom monster02Script;

    void Awake()
    {
        // CORRIGÉ : Awake garantit que zm est prêt avant tous les Start()
        zm = Object.FindFirstObjectByType<zoomManager>();
    }

    void Start()
    {
        if (ignoreCollision != null)
            Physics.IgnoreCollision(ignoreCollision.GetComponent<Collider>(), GetComponent<Collider>());

        choixItemRandom = Random.Range(1, 6);
        durationItem = 10.0f;

        // Fallback si Awake n'a pas trouvé
        if (zm == null) zm = zoomManager.Instance;
        if (zm == null && scriptZoomManager != null)
            zm = scriptZoomManager.GetComponent<zoomManager>();

        itemDIS = item != null ? item.GetComponent<itemDropInSpace>() : null;
        if (lifeBar != null) lifeBarManager = lifeBar.GetComponent<LifeBarManager>();
        if (Monster02 != null) monster02Script = Monster02.GetComponent<monsterZoom>();
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
        durationItem -= Time.deltaTime;

        if (durationItem < 0)
        {
            durationItem = durationItemOrigine;
            tempsAvantDeplacementOrigine = 2.0f;

            if (ignoreCollision != null)
                Physics.IgnoreCollision(ignoreCollision.GetComponent<Collider>(), GetComponent<Collider>());

            boolIgnoreCollisionPlayer = false;
            choixItemRandom = Random.Range(1, 6);
        }

        if (durationItem > 0 && boolIgnoreCollisionPlayer && ignoreCollisionPlayer != null)
            Physics.IgnoreCollision(ignoreCollisionPlayer.GetComponent<Collider>(), GetComponent<Collider>());

        // Direction
        if (DirectionAleatoire) choixDirection = Random.Range(1, 5);
        if (DirectionHorizontale) choixDirection = Random.Range(1, 3);
        if (DirectionVertical) choixDirection = Random.Range(3, 5);
        if (DirectionHorizontaleSingle) choixDirection = Random.Range(1, 2);
        if (DirectionVerticalSingle) choixDirection = Random.Range(3, 4);

        if (_collisionAvecChangerOrientationGauche) choixDirection = 2;
        if (_collisionAvecChangerOrientationDroite) choixDirection = 1;
        if (_collisionAvecChangerOrientationAv) choixDirection = 4;
        if (_collisionAvecChangerOrientationArr) choixDirection = 3;

        if (!_isMoving && tempsAvantDeplacement < 0)
        {
            Vector3 dir = Vector3.zero;
            if (choixDirection == 1) { dir = Vector3.left; ModelMonster01.transform.rotation = Quaternion.Euler(0, 0, 0); }
            else if (choixDirection == 2) { dir = Vector3.right; ModelMonster01.transform.rotation = Quaternion.Euler(0, -180, 0); }
            else if (choixDirection == 3) { dir = Vector3.forward; ModelMonster01.transform.rotation = Quaternion.Euler(0, -270, 0); }
            else if (choixDirection == 4) { dir = Vector3.back; ModelMonster01.transform.rotation = Quaternion.Euler(0, -90, 0); }

            if (dir != Vector3.zero)
            {
                var anchor = transform.position + (Vector3.down + dir) * 1.085f;
                var axis = Vector3.Cross(Vector3.up, dir);
                StartCoroutine(Roll(anchor, axis));
                tempsAvantDeplacement = tempsAvantDeplacementOrigine;
            }
        }

        // CORRIGÉ : followMonster seulement si activé
        if (followMonster)
        {
            distanceBetweenObjectsNorth = Mathf.Round(Vector3.Distance(North.transform.position, Player.transform.position));
            distanceBetweenObjectsSouth = Mathf.Round(Vector3.Distance(South.transform.position, Player.transform.position));
            distanceBetweenObjectsEast = Mathf.Round(Vector3.Distance(East.transform.position, Player.transform.position));
            distanceBetweenObjectsWest = Mathf.Round(Vector3.Distance(West.transform.position, Player.transform.position));

            if (distanceBetweenObjectsNorth < distanceBetweenObjectsSouth && distanceBetweenObjectsNorth < distanceBetweenObjectsEast && distanceBetweenObjectsNorth < distanceBetweenObjectsWest) choixDirection = 3;
            else if (distanceBetweenObjectsSouth < distanceBetweenObjectsNorth && distanceBetweenObjectsSouth < distanceBetweenObjectsEast && distanceBetweenObjectsSouth < distanceBetweenObjectsWest) choixDirection = 4;
            else if (distanceBetweenObjectsEast < distanceBetweenObjectsSouth && distanceBetweenObjectsEast < distanceBetweenObjectsNorth && distanceBetweenObjectsEast < distanceBetweenObjectsWest) choixDirection = 2;
            else if (distanceBetweenObjectsWest < distanceBetweenObjectsSouth && distanceBetweenObjectsWest < distanceBetweenObjectsEast && distanceBetweenObjectsWest < distanceBetweenObjectsNorth) choixDirection = 1;
        }

        // Monstre TP
        if (MonsterTP && tempsAvantDeplacement < 0)
        {
            choixPlatform = Random.Range(1, 82);
            TeleporterMonster();
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

        if (other.CompareTag("Player") && !boolIgnoreCollisionPlayer)
        {
            if (lifeBarManager != null) lifeBarManager.Reset();
            // CORRIGÉ : désactive au lieu de Destroy pour éviter NullRef
            Player.SetActive(false);
        }

        if (other.gameObject.tag == tagMonster && monster02Script != null)
        {
            _collisionAvecMonster = true;
            monster02Script.enabled = false;
        }

        if (other.CompareTag("orientationChangerDroite") && followMonster) _collisionAvecChangerOrientationDroite = true;
        if (other.CompareTag("orientationChangerGauche") && followMonster) _collisionAvecChangerOrientationGauche = true;
        if (other.CompareTag("orientationChangerAv") && followMonster) _collisionAvecChangerOrientationAv = true;
        if (other.CompareTag("orientationChangerArr") && followMonster) _collisionAvecChangerOrientationArr = true;

        // CORRIGÉ : utilise itemDIS caché
        if (other.CompareTag("itemTimeDiv202")) itemDIS.itemDetruitF();
        if (other.CompareTag("itemTimeStop02")) itemDIS.itemDetruitF();
        if (other.CompareTag("itemInvincibilty02")) itemDIS.itemDetruitF();
        if (other.CompareTag("itemBonbonScoreRouge02")) itemDIS.itemDetruitF();
        if (other.CompareTag("itemBonbonScoreVert02")) itemDIS.itemDetruitF();
        if (other.CompareTag("itemRandom02")) itemDIS.itemDetruitF();

        if (other.CompareTag("itemTimeDiv2")) itemDIS.itemDetruitMonsterF();
        if (other.CompareTag("itemTimeStop")) itemDIS.itemDetruitMonsterF();
        if (other.CompareTag("itemInvincibilty")) itemDIS.itemDetruitMonsterF();
        if (other.CompareTag("itemBonbonScoreRouge")) itemDIS.itemDetruitMonsterF();
        if (other.CompareTag("itemBonbonScoreVert")) itemDIS.itemDetruitMonsterF();
        if (other.CompareTag("itemRandom")) itemDIS.itemDetruitMonsterF();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("changeCubeMaterial")) _collisionOK = false;

        if (other.gameObject.tag == tagMonster && monster02Script != null)
        {
            _collisionAvecMonster = false;
            monster02Script.enabled = true;
        }

        if (other.CompareTag("orientationChangerDroite")) _collisionAvecChangerOrientationDroite = false;
        if (other.CompareTag("orientationChangerGauche")) _collisionAvecChangerOrientationGauche = false;
        if (other.CompareTag("orientationChangerAv")) _collisionAvecChangerOrientationAv = false;
        if (other.CompareTag("orientationChangerArr")) _collisionAvecChangerOrientationArr = false;
    }

    public void tAvDepX2()
    {
        if (zm == null) zm = Object.FindFirstObjectByType<zoomManager>();
        tempsAvantDeplacement = 4.0f;
        tempsAvantDeplacementOrigine = 4.0f;
        zm?.audioItemTimeDiv2();
        durationItem = 10.0f;
    }

    public void powerStop()
    {
        if (zm == null) zm = Object.FindFirstObjectByType<zoomManager>();
        tempsAvantDeplacement = 10.0f;
        zm?.audioItemTStop();
        durationItem = 10.0f;
    }

    public void powerInvincibilty()
    {
        if (zm == null) zm = Object.FindFirstObjectByType<zoomManager>();
        boolIgnoreCollisionPlayer = true;
        zm?.audioItemTInvincibility();
        durationItem = 10.0f;
    }

    public void itemBonbonScoreRouge()
    {
        if (zm == null) zm = Object.FindFirstObjectByType<zoomManager>();
        zm?.audioItemTBonbon();
        zm?.ajoutScoreBonbonRouge();
    }

    public void itemBonbonScoreVert()
    {
        if (zm == null) zm = Object.FindFirstObjectByType<zoomManager>();
        zm?.audioItemTBonbon();
        zm?.ajoutScoreBonbonVert();
    }

    public void ResetDepuisMonster()
    {
        if (lifeBarManager != null) lifeBarManager.Reset();
    }

    private void TeleporterMonster()
    {
        Vector3[] positions = GetPositions();
        if (choixPlatform >= 1 && choixPlatform <= positions.Length)
        {
            bool[] pos = GetPosArray();
            if (pos[choixPlatform - 1])
            {
                Monster01Vector.transform.position = positions[choixPlatform - 1];
                tempsAvantDeplacement = tempsAvantDeplacementOrigine;
            }
        }
    }

    private bool[] GetPosArray()
    {
        return new bool[] {
            pos01,pos02,pos03,pos04,pos05,pos06,pos07,pos08,pos09,
            pos10,pos11,pos12,pos13,pos14,pos15,pos16,pos17,pos18,pos19,
            pos20,pos21,pos22,pos23,pos24,pos25,pos26,pos27,pos28,pos29,
            pos30,pos31,pos32,pos33,pos34,pos35,pos36,pos37,pos38,pos39,
            pos40,pos41,pos42,pos43,pos44,pos45,pos46,pos47,pos48,pos49,
            pos50,pos51,pos52,pos53,pos54,pos55,pos56,pos57,pos58,pos59,
            pos60,pos61,pos62,pos63,pos64,pos65,pos66,pos67,pos68,pos69,
            pos70,pos71,pos72,pos73,pos74,pos75,pos76,pos77,pos78,pos79,
            pos80,pos81
        };
    }

    private Vector3[] GetPositions()
    {
        return new Vector3[] {
            new Vector3(0,20,0), new Vector3(2,20,0), new Vector3(4,20,0), new Vector3(6,20,0), new Vector3(8,20,0), new Vector3(10,20,0), new Vector3(12,20,0), new Vector3(14,20,0), new Vector3(16,20,0),
            new Vector3(0,20,2), new Vector3(2,20,2), new Vector3(4,20,2), new Vector3(6,20,2), new Vector3(8,20,2), new Vector3(10,20,2), new Vector3(12,20,2), new Vector3(14,20,2), new Vector3(16,20,2),
            new Vector3(0,20,4), new Vector3(2,20,4), new Vector3(4,20,4), new Vector3(6,20,4), new Vector3(8,20,4), new Vector3(10,20,4), new Vector3(12,20,4), new Vector3(14,20,4), new Vector3(16,20,4),
            new Vector3(0,20,6), new Vector3(2,20,6), new Vector3(4,20,6), new Vector3(6,20,6), new Vector3(8,20,6), new Vector3(10,20,6), new Vector3(12,20,6), new Vector3(14,20,6), new Vector3(16,20,6),
            new Vector3(0,20,8), new Vector3(2,20,8), new Vector3(4,20,8), new Vector3(6,20,8), new Vector3(8,20,8), new Vector3(10,20,8), new Vector3(12,20,8), new Vector3(14,20,8), new Vector3(16,20,8),
            new Vector3(0,20,10), new Vector3(2,20,10), new Vector3(4,20,10), new Vector3(6,20,10), new Vector3(8,20,10), new Vector3(10,20,10), new Vector3(12,20,10), new Vector3(14,20,10), new Vector3(16,20,10),
            new Vector3(0,20,12), new Vector3(2,20,12), new Vector3(4,20,12), new Vector3(6,20,12), new Vector3(8,20,12), new Vector3(10,20,12), new Vector3(12,20,12), new Vector3(14,20,12), new Vector3(16,20,12),
            new Vector3(0,20,14), new Vector3(2,20,14), new Vector3(4,20,14), new Vector3(6,20,14), new Vector3(8,20,14), new Vector3(10,20,14), new Vector3(12,20,14), new Vector3(14,20,14), new Vector3(16,20,14),
            new Vector3(0,20,16), new Vector3(2,20,16), new Vector3(4,20,16), new Vector3(6,20,16), new Vector3(8,20,16), new Vector3(10,20,16), new Vector3(12,20,16), new Vector3(14,20,16), new Vector3(16,20,16)
        };
    }
}