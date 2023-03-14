using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class monsterZoom : MonoBehaviour
{
    public float durationItem = 10.0f;
    public float durationItemOrigine = 10.0f;
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

   //pour le monstre qui suit le joueur
    public bool followMonster;
    public float distanceBetweenObjectsNorth;
    public float distanceBetweenObjectsSouth;
    public float distanceBetweenObjectsEast;
    public float distanceBetweenObjectsWest;
    public GameObject North;
    public GameObject South;
    public GameObject East;
    public GameObject West;
    public float assemblePtForNorth;
    public float assemblePtForSouth;
    public float assemblePtForEast;
    public float assemblePtForWest;

    //pour item
    public GameObject item;
    public GameObject scriptZoomManager;


    //pour item invincibility
    public bool boolIgnoreCollisionPlayer = false;
    public Transform ignoreCollisionPlayer;

    //pour item random
    public int choixItemRandom;

    //pour le monstre qui se tp
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


    //lifeBar
    public int reset = 3;

    void Start()
    {
        Transform ignore = ignoreCollision as Transform;
        Physics.IgnoreCollision(ignore.GetComponent<Collider>(), GetComponent<Collider>());

        choixItemRandom = Random.Range(1, 6);
        durationItem = 10.0f;

        //lifeBar = GameObject.FindWithTag("lifeBar");
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

        //lifebar
        if(reset == 1)
        {            
           
        }
    }

    void FixedUpdate()
    {

        tempsAvantDeplacement -= Time.deltaTime;
        durationItem -= Time.deltaTime;

        //pour reinitialiser temps origine lors pouvoir
        if (durationItem < 0)
        {
            durationItem = durationItemOrigine;
            tempsAvantDeplacementOrigine = 2.0f;

            //pour remettre collision item invincibility
            Transform ignorePlayer = ignoreCollision as Transform;
            Physics.IgnoreCollision(ignorePlayer.GetComponent<Collider>(), GetComponent<Collider>());
            boolIgnoreCollisionPlayer = false;

            //pour itemRandom
            choixItemRandom = Random.Range(1, 6);

        } 

        //pour item invincibility
        if(durationItem > 0 && boolIgnoreCollisionPlayer == true)
        {
            Transform ignorePlayer = ignoreCollisionPlayer as Transform;
            Physics.IgnoreCollision(ignorePlayer.GetComponent<Collider>(), GetComponent<Collider>());
        }

        


        //direction en fonction du monstre
        if (DirectionAleatoire == true) choixDirection = Random.Range(1, 5);
        if (DirectionHorizontale == true) choixDirection = Random.Range(1, 3);
        if (DirectionVertical == true) choixDirection = Random.Range(3, 5);
        if (DirectionHorizontaleSingle == true) choixDirection = Random.Range(1, 2);
        if (DirectionVerticalSingle == true) choixDirection = Random.Range(3, 4);

        //direction lors de collision avec changer
        if (_collisionAvecChangerOrientationGauche == true) choixDirection = 2;
        if (_collisionAvecChangerOrientationDroite == true) choixDirection = 1;
        if (_collisionAvecChangerOrientationAv == true) choixDirection = 4;
        if (_collisionAvecChangerOrientationArr == true) choixDirection = 3;

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

        //monstre qui suit le joueur        
            distanceBetweenObjectsNorth = Vector3.Distance(North.transform.position, Player.transform.position);
            distanceBetweenObjectsSouth = Vector3.Distance(South.transform.position, Player.transform.position);
            distanceBetweenObjectsEast = Vector3.Distance(East.transform.position, Player.transform.position);
            distanceBetweenObjectsWest = Vector3.Distance(West.transform.position, Player.transform.position);

            Debug.DrawLine(North.transform.position, Player.transform.position, Color.red);
            Debug.DrawLine(South.transform.position, Player.transform.position, Color.blue);
            Debug.DrawLine(East.transform.position, Player.transform.position, Color.green);
            Debug.DrawLine(West.transform.position, Player.transform.position, Color.yellow);

            distanceBetweenObjectsNorth = Mathf.Round(distanceBetweenObjectsNorth);
            distanceBetweenObjectsSouth = Mathf.Round(distanceBetweenObjectsSouth);
            distanceBetweenObjectsEast = Mathf.Round(distanceBetweenObjectsEast);
            distanceBetweenObjectsWest = Mathf.Round(distanceBetweenObjectsWest);

            assemblePtForNorth = distanceBetweenObjectsSouth + distanceBetweenObjectsEast + distanceBetweenObjectsWest;
            assemblePtForSouth = distanceBetweenObjectsNorth + distanceBetweenObjectsEast + distanceBetweenObjectsWest;
            assemblePtForEast = distanceBetweenObjectsNorth + distanceBetweenObjectsWest + distanceBetweenObjectsSouth;
            assemblePtForWest = distanceBetweenObjectsNorth + distanceBetweenObjectsSouth + distanceBetweenObjectsEast;

        if (followMonster == true && distanceBetweenObjectsNorth < distanceBetweenObjectsSouth && distanceBetweenObjectsNorth < distanceBetweenObjectsEast && distanceBetweenObjectsNorth < distanceBetweenObjectsWest) choixDirection = 3;
        if (followMonster == true && distanceBetweenObjectsSouth < distanceBetweenObjectsNorth && distanceBetweenObjectsSouth < distanceBetweenObjectsEast && distanceBetweenObjectsSouth < distanceBetweenObjectsWest) choixDirection = 4;
        if (followMonster == true && distanceBetweenObjectsEast < distanceBetweenObjectsSouth && distanceBetweenObjectsEast < distanceBetweenObjectsNorth && distanceBetweenObjectsEast < distanceBetweenObjectsWest) choixDirection = 2;
        if (followMonster == true && distanceBetweenObjectsWest < distanceBetweenObjectsSouth && distanceBetweenObjectsWest < distanceBetweenObjectsEast && distanceBetweenObjectsWest < distanceBetweenObjectsNorth) choixDirection = 1;
        /*if (followMonster == true && distanceBetweenObjectsSouth < assemblePtForSouth) choixDirection = 4;
        if (followMonster == true && distanceBetweenObjectsEast < assemblePtForEast) choixDirection = 2;
        if (followMonster == true && distanceBetweenObjectsWest < assemblePtForWest) choixDirection = 1;*/

        //monstre qui se teleporte mais bouge aps
        if (MonsterTP == true && tempsAvantDeplacement < 0)
        {
            choixPlatform = Random.Range(1, 82);
            if (pos01 == true && choixPlatform == 1) { Monster01Vector.transform.position = position01; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos02 == true && choixPlatform == 2) { Monster01Vector.transform.position = position02; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos03 == true && choixPlatform == 3) { Monster01Vector.transform.position = position03; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos04 == true && choixPlatform == 4) { Monster01Vector.transform.position = position04; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos05 == true && choixPlatform == 5) { Monster01Vector.transform.position = position05; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos06 == true && choixPlatform == 6) { Monster01Vector.transform.position = position06; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos07 == true && choixPlatform == 7) { Monster01Vector.transform.position = position07; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos08 == true && choixPlatform == 8) { Monster01Vector.transform.position = position08; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos09 == true && choixPlatform == 9) { Monster01Vector.transform.position = position09; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos10 == true && choixPlatform == 10) { Monster01Vector.transform.position = position10; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos11 == true && choixPlatform == 11) { Monster01Vector.transform.position = position11; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos12 == true && choixPlatform == 12) { Monster01Vector.transform.position = position12; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos13 == true && choixPlatform == 13) { Monster01Vector.transform.position = position13; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos14 == true && choixPlatform == 14) { Monster01Vector.transform.position = position14; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos15 == true && choixPlatform == 15) { Monster01Vector.transform.position = position15; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos16 == true && choixPlatform == 16) { Monster01Vector.transform.position = position16; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos17 == true && choixPlatform == 17) { Monster01Vector.transform.position = position17; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos18 == true && choixPlatform == 18) { Monster01Vector.transform.position = position18; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos19 == true && choixPlatform == 19) { Monster01Vector.transform.position = position19; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos20 == true && choixPlatform == 20) { Monster01Vector.transform.position = position20; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos21 == true && choixPlatform == 21) { Monster01Vector.transform.position = position21; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos22 == true && choixPlatform == 22) { Monster01Vector.transform.position = position22; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos23 == true && choixPlatform == 23) { Monster01Vector.transform.position = position23; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos24 == true && choixPlatform == 24) { Monster01Vector.transform.position = position24; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos25 == true && choixPlatform == 25) { Monster01Vector.transform.position = position25; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos26 == true && choixPlatform == 26) { Monster01Vector.transform.position = position26; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos27 == true && choixPlatform == 27) { Monster01Vector.transform.position = position27; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos28 == true && choixPlatform == 28) { Monster01Vector.transform.position = position28; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos29 == true && choixPlatform == 29) { Monster01Vector.transform.position = position29; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos30 == true && choixPlatform == 30) { Monster01Vector.transform.position = position30; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos31 == true && choixPlatform == 31) { Monster01Vector.transform.position = position31; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos32 == true && choixPlatform == 32) { Monster01Vector.transform.position = position32; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos33 == true && choixPlatform == 33) { Monster01Vector.transform.position = position33; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos34 == true && choixPlatform == 34) { Monster01Vector.transform.position = position34; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos35 == true && choixPlatform == 35) { Monster01Vector.transform.position = position35; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos36 == true && choixPlatform == 36) { Monster01Vector.transform.position = position36; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos37 == true && choixPlatform == 37) { Monster01Vector.transform.position = position37; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos38 == true && choixPlatform == 38) { Monster01Vector.transform.position = position38; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos39 == true && choixPlatform == 39) { Monster01Vector.transform.position = position39; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos40 == true && choixPlatform == 40) { Monster01Vector.transform.position = position40; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos41 == true && choixPlatform == 41) { Monster01Vector.transform.position = position41; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos42 == true && choixPlatform == 42) { Monster01Vector.transform.position = position42; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos43 == true && choixPlatform == 43) { Monster01Vector.transform.position = position43; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos44 == true && choixPlatform == 44) { Monster01Vector.transform.position = position44; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos45 == true && choixPlatform == 45) { Monster01Vector.transform.position = position45; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos46 == true && choixPlatform == 46) { Monster01Vector.transform.position = position46; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos47 == true && choixPlatform == 47) { Monster01Vector.transform.position = position47; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos48 == true && choixPlatform == 48) { Monster01Vector.transform.position = position48; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos49 == true && choixPlatform == 49) { Monster01Vector.transform.position = position49; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos50 == true && choixPlatform == 50) { Monster01Vector.transform.position = position50; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos51 == true && choixPlatform == 51) { Monster01Vector.transform.position = position51; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos52 == true && choixPlatform == 52) { Monster01Vector.transform.position = position52; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos53 == true && choixPlatform == 53) { Monster01Vector.transform.position = position53; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos54 == true && choixPlatform == 54) { Monster01Vector.transform.position = position54; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos55 == true && choixPlatform == 55) { Monster01Vector.transform.position = position55; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos56 == true && choixPlatform == 56) { Monster01Vector.transform.position = position56; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos57 == true && choixPlatform == 57) { Monster01Vector.transform.position = position57; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos58 == true && choixPlatform == 58) { Monster01Vector.transform.position = position58; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos59 == true && choixPlatform == 59) { Monster01Vector.transform.position = position59; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos60 == true && choixPlatform == 60) { Monster01Vector.transform.position = position60; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos61 == true && choixPlatform == 61) { Monster01Vector.transform.position = position61; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos62 == true && choixPlatform == 62) { Monster01Vector.transform.position = position62; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos63 == true && choixPlatform == 63) { Monster01Vector.transform.position = position63; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos64 == true && choixPlatform == 64) { Monster01Vector.transform.position = position64; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos65 == true && choixPlatform == 65) { Monster01Vector.transform.position = position65; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos66 == true && choixPlatform == 66) { Monster01Vector.transform.position = position66; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos67 == true && choixPlatform == 67) { Monster01Vector.transform.position = position67; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos68 == true && choixPlatform == 68) { Monster01Vector.transform.position = position68; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos69 == true && choixPlatform == 69) { Monster01Vector.transform.position = position69; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos70 == true && choixPlatform == 70) { Monster01Vector.transform.position = position70; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos71 == true && choixPlatform == 71) { Monster01Vector.transform.position = position71; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos72 == true && choixPlatform == 72) { Monster01Vector.transform.position = position72; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos73 == true && choixPlatform == 73) { Monster01Vector.transform.position = position73; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos74 == true && choixPlatform == 74) { Monster01Vector.transform.position = position74; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos75 == true && choixPlatform == 75) { Monster01Vector.transform.position = position75; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos76 == true && choixPlatform == 76) { Monster01Vector.transform.position = position76; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos77 == true && choixPlatform == 77) { Monster01Vector.transform.position = position77; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos78 == true && choixPlatform == 78) { Monster01Vector.transform.position = position78; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos79 == true && choixPlatform == 79) { Monster01Vector.transform.position = position79; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos80 == true && choixPlatform == 80) { Monster01Vector.transform.position = position80; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
            if (pos81 == true && choixPlatform == 81) { Monster01Vector.transform.position = position81; tempsAvantDeplacement = tempsAvantDeplacementOrigine; }
        }       
    }

    IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        _isMoving = true;

        for (int i = 0; i < (90 / _rollSpeed); i++)
        {
            transform.RotateAround(anchor, axis, _rollSpeed);
           // yield return new WaitForSeconds(0.01f);
            yield return new WaitForFixedUpdate();
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

        if (other.gameObject.tag == "Player" && boolIgnoreCollisionPlayer == false)
        {
            ResetDepuisMonster();
            Destroy(Player);
        }        

        if (other.gameObject.tag == tagMonster)
        {
            _collisionAvecMonster = true;
            Monster02.GetComponent<monsterZoom>().enabled = false;
        }

        if (other.gameObject.tag == "orientationChangerDroite" && followMonster == true) _collisionAvecChangerOrientationDroite = true;
        if (other.gameObject.tag == "orientationChangerGauche" && followMonster == true) _collisionAvecChangerOrientationGauche = true;
        if (other.gameObject.tag == "orientationChangerAv" && followMonster == true) _collisionAvecChangerOrientationAv = true;
        if (other.gameObject.tag == "orientationChangerArr" && followMonster == true) _collisionAvecChangerOrientationArr = true;

        //destruction de l'item si le monstre rentre dedans
        if (other.gameObject.tag == "itemTimeDiv202") item.GetComponent<itemDropInSpace>().itemDetruitF();
        if (other.gameObject.tag == "itemTimeStop02") item.GetComponent<itemDropInSpace>().itemDetruitF();
        if (other.gameObject.tag == "itemInvincibilty02") item.GetComponent<itemDropInSpace>().itemDetruitF();
        if (other.gameObject.tag == "itemBonbonScoreRouge02") item.GetComponent<itemDropInSpace>().itemDetruitF();
        if (other.gameObject.tag == "itemBonbonScoreVert02") item.GetComponent<itemDropInSpace>().itemDetruitF();
        if (other.gameObject.tag == "itemRandom02") item.GetComponent<itemDropInSpace>().itemDetruitF();

        if (other.gameObject.tag == "itemTimeDiv2") item.GetComponent<itemDropInSpace>().itemDetruitMonsterF();
        if (other.gameObject.tag == "itemTimeStop") item.GetComponent<itemDropInSpace>().itemDetruitMonsterF();
        if (other.gameObject.tag == "itemInvincibilty") item.GetComponent<itemDropInSpace>().itemDetruitMonsterF();
        if (other.gameObject.tag == "itemBonbonScoreRouge") item.GetComponent<itemDropInSpace>().itemDetruitMonsterF();
        if (other.gameObject.tag == "itemBonbonScoreVert") item.GetComponent<itemDropInSpace>().itemDetruitMonsterF();
        if (other.gameObject.tag == "itemRandom") item.GetComponent<itemDropInSpace>().itemDetruitMonsterF();

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

    //pourvoir des items
    public void tAvDepX2()
    {
        tempsAvantDeplacement = 4.0f;
        tempsAvantDeplacementOrigine = 4.0f;
        scriptZoomManager.GetComponent<zoomManager>().audioItemTimeDiv2();
        Debug.Log("tAvDepX2");
        durationItem = 10.0f;
    }

    public void powerStop()
    {
        tempsAvantDeplacement = 10.0f;
        scriptZoomManager.GetComponent<zoomManager>().audioItemTStop();
        Debug.Log("stop");
        durationItem = 10.0f;
    }

    public void powerInvincibilty()
    {
        boolIgnoreCollisionPlayer = true;
        durationItem = 10.0f;
        scriptZoomManager.GetComponent<zoomManager>().audioItemTInvincibility();
        Debug.Log("Invincibilty");
        durationItem = 10.0f;
    }

    public void itemBonbonScoreRouge()
    {
        scriptZoomManager.GetComponent<zoomManager>().audioItemTBonbon();
        scriptZoomManager.GetComponent<zoomManager>().ajoutScoreBonbonRouge();
        Debug.Log("rouge");
    }

    public void itemBonbonScoreVert()
    {
        scriptZoomManager.GetComponent<zoomManager>().audioItemTBonbon();
        scriptZoomManager.GetComponent<zoomManager>().ajoutScoreBonbonVert();
        Debug.Log("vert");
    }

    public void ResetDepuisMonster()
    {
        lifeBar.GetComponent<LifeBarManager>().Reset();
    }



    //Ligne01;
    Vector3 position01 = new Vector3(0, 20, 0);
    Vector3 position02 = new Vector3(2, 20, 0);
    Vector3 position03 = new Vector3(4, 20, 0);
    Vector3 position04 = new Vector3(6, 20, 0);
    Vector3 position05 = new Vector3(8, 20, 0);
    Vector3 position06 = new Vector3(10, 20, 0);
    Vector3 position07 = new Vector3(12, 20, 0);
    Vector3 position08 = new Vector3(14, 20, 0);
    Vector3 position09 = new Vector3(16, 20, 0);

    //Ligne02;
    Vector3 position10 = new Vector3(0, 20, 2);
    Vector3 position11 = new Vector3(2, 20, 2);
    Vector3 position12 = new Vector3(4, 20, 2);
    Vector3 position13 = new Vector3(6, 20, 2);
    Vector3 position14 = new Vector3(8, 20, 2);
    Vector3 position15 = new Vector3(10, 20, 2);
    Vector3 position16 = new Vector3(12, 20, 2);
    Vector3 position17 = new Vector3(14, 20, 2);
    Vector3 position18 = new Vector3(16, 20, 2);

    //Ligne03;
    Vector3 position19 = new Vector3(0, 20, 4);
    Vector3 position20 = new Vector3(2, 20, 4);
    Vector3 position21 = new Vector3(4, 20, 4);
    Vector3 position22 = new Vector3(6, 20, 4);
    Vector3 position23 = new Vector3(8, 20, 4);
    Vector3 position24 = new Vector3(10, 20, 4);
    Vector3 position25 = new Vector3(12, 20, 4);
    Vector3 position26 = new Vector3(14, 20, 4);
    Vector3 position27 = new Vector3(16, 20, 4);

    //Ligne04
    Vector3 position28 = new Vector3(0, 20, 6);
    Vector3 position29 = new Vector3(2, 20, 6);
    Vector3 position30 = new Vector3(4, 20, 6);
    Vector3 position31 = new Vector3(6, 20, 6);
    Vector3 position32 = new Vector3(8, 20, 6);
    Vector3 position33 = new Vector3(10, 20, 6);
    Vector3 position34 = new Vector3(12, 20, 6);
    Vector3 position35 = new Vector3(14, 20, 6);
    Vector3 position36 = new Vector3(16, 20, 6);

    //Ligne05
    Vector3 position37 = new Vector3(0, 20, 8);
    Vector3 position38 = new Vector3(2, 20, 8);
    Vector3 position39 = new Vector3(4, 20, 8);
    Vector3 position40 = new Vector3(6, 20, 8);
    Vector3 position41 = new Vector3(8, 20, 8);
    Vector3 position42 = new Vector3(10, 20, 8);
    Vector3 position43 = new Vector3(12, 20, 8);
    Vector3 position44 = new Vector3(14, 20, 8);
    Vector3 position45 = new Vector3(16, 20, 8);

    //Ligne06
    Vector3 position46 = new Vector3(0, 20, 10);
    Vector3 position47 = new Vector3(2, 20, 10);
    Vector3 position48 = new Vector3(4, 20, 10);
    Vector3 position49 = new Vector3(6, 20, 10);
    Vector3 position50 = new Vector3(8, 20, 10);
    Vector3 position51 = new Vector3(10, 20, 10);
    Vector3 position52 = new Vector3(12, 20, 10);
    Vector3 position53 = new Vector3(14, 20, 10);
    Vector3 position54 = new Vector3(16, 20, 10);

    //Ligne07
    Vector3 position55 = new Vector3(0, 20, 12);
    Vector3 position56 = new Vector3(2, 20, 12);
    Vector3 position57 = new Vector3(4, 20, 12);
    Vector3 position58 = new Vector3(6, 20, 12);
    Vector3 position59 = new Vector3(8, 20, 12);
    Vector3 position60 = new Vector3(10, 20, 12);
    Vector3 position61 = new Vector3(12, 20, 12);
    Vector3 position62 = new Vector3(14, 20, 12);
    Vector3 position63 = new Vector3(16, 20, 12);

    //Ligne08
    Vector3 position64 = new Vector3(0, 20, 14);
    Vector3 position65 = new Vector3(2, 20, 14);
    Vector3 position66 = new Vector3(4, 20, 14);
    Vector3 position67 = new Vector3(6, 20, 14);
    Vector3 position68 = new Vector3(8, 20, 14);
    Vector3 position69 = new Vector3(10, 20, 14);
    Vector3 position70 = new Vector3(12, 20, 14);
    Vector3 position71 = new Vector3(14, 20, 14);
    Vector3 position72 = new Vector3(16, 20, 14);

    //Ligne09
    Vector3 position73 = new Vector3(0, 20, 16);
    Vector3 position74 = new Vector3(2, 20, 16);
    Vector3 position75 = new Vector3(4, 20, 16);
    Vector3 position76 = new Vector3(6, 20, 16);
    Vector3 position77 = new Vector3(8, 20, 16);
    Vector3 position78 = new Vector3(10, 20, 16);
    Vector3 position79 = new Vector3(12, 20, 16);
    Vector3 position80 = new Vector3(14, 20, 16);
    Vector3 position81 = new Vector3(16, 20, 16);
}
