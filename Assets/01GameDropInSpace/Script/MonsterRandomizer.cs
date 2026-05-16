using System.Collections;
using UnityEngine;

/// <summary>
/// A placer sur Monster01 et Monster02.
/// Active un modele aleatoire et configure monsterZoom en consequence.
/// Bloque le mouvement jusqu'au premier contact avec un cube.
/// </summary>
public class MonsterRandomizer : MonoBehaviour
{
    [Header("Modeles 3D — assigne dans l'Inspector")]
    public GameObject meduseM3D;
    public GameObject moleculeMonster;
    public GameObject changeColorMonster;
    public GameObject deathSearcher;

    [Header("References")]
    public GameObject player;

    [Header("Delai apres atterrissage")]
    public float delaiAvantMouvement = 1f;

    // Cache
    private monsterZoom monsterScript;
    private GameObject[] tousLesModeles;
    private bool enAttenteAterissage = false;

    void Awake()
    {
        Initialiser();
    }

    void OnEnable()
    {
        if (monsterScript == null || tousLesModeles == null)
            Initialiser();
    }

    private void Initialiser()
    {
        // monsterZoom peut etre sur ce GameObject ou sur le parent
        monsterScript = GetComponent<monsterZoom>();
        if (monsterScript == null)
            monsterScript = GetComponentInParent<monsterZoom>();

        tousLesModeles = new GameObject[]
        {
            meduseM3D,
            moleculeMonster,
            changeColorMonster,
            deathSearcher
        };
    }

    /// <summary>Appele par InfiniteGridManager a chaque vague.</summary>
    public void RandomiserMonstre()
    {
        // S'assure que tout est initialise
        if (tousLesModeles == null) Initialiser();

        // Bloque le mouvement du monstre — attend l'atterrissage
        if (monsterScript != null)
        {
            monsterScript.tempsAvantDeplacement = 9999f; // Bloque
            enAttenteAterissage = true;
        }

        // Desactive tous les modeles
        foreach (GameObject modele in tousLesModeles)
            if (modele != null) modele.SetActive(false);

        // Choisit un modele aleatoire
        int choix = Random.Range(0, tousLesModeles.Length);

        // S'assure que le modele existe
        while (tousLesModeles[choix] == null)
            choix = Random.Range(0, tousLesModeles.Length);

        tousLesModeles[choix].SetActive(true);

        // Configure monsterZoom selon le modele choisi
        if (monsterScript == null) return;

        // Reset toutes les directions
        monsterScript.DirectionAleatoire = false;
        monsterScript.DirectionHorizontale = false;
        monsterScript.DirectionHorizontaleSingle = false;
        monsterScript.DirectionVertical = false;
        monsterScript.DirectionVerticalSingle = false;
        monsterScript.followMonster = false;
        monsterScript.MonsterTP = false;

        // Verifie que le modele choisi est bien actif
        if (tousLesModeles[choix] == null)
        {
            Debug.LogWarning("[MonsterRandomizer] Modele null — fallback meduse");
            if (meduseM3D != null) meduseM3D.SetActive(true);
            monsterScript.DirectionAleatoire = true;
            return;
        }

        switch (choix)
        {
            case 0: // Meduse — bouge dans tous les sens aleatoirement
                monsterScript.DirectionAleatoire = true;
                Debug.Log("[MonsterRandomizer] Meduse — Direction aleatoire");
                break;

            case 1: // MoleculeMonster — horizontal ou vertical
                bool horizontal = Random.value > 0.5f;
                monsterScript.DirectionHorizontale = horizontal;
                monsterScript.DirectionVertical = !horizontal;
                Debug.Log($"[MonsterRandomizer] Molecule — {(horizontal ? "Horizontal" : "Vertical")}");
                break;

            case 2: // ChangeColorMonster — aleatoire + efface les cubes
                monsterScript.DirectionAleatoire = true;
                Debug.Log("[MonsterRandomizer] ChangeColor — Direction aleatoire + efface cubes");
                break;

            case 3: // DeathSearcher — suit Drop
                monsterScript.followMonster = true;
                Debug.Log("[MonsterRandomizer] DeathSearcher — Suit Drop");
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!enAttenteAterissage) return;

        // Premier contact avec un cube — attend 1s puis debloque
        if (collision.collider.CompareTag("changeCubeMaterial") ||
            collision.collider.CompareTag("Platform"))
        {
            enAttenteAterissage = false;
            StartCoroutine(DebloquerMouvement());
        }
    }

    private IEnumerator DebloquerMouvement()
    {
        yield return new WaitForSeconds(delaiAvantMouvement);

        if (monsterScript != null)
            monsterScript.tempsAvantDeplacement = monsterScript.tempsAvantDeplacementOrigine;

        Debug.Log($"[MonsterRandomizer] {gameObject.name} debloque !");
    }
}