using UnityEngine;

/// <summary>
/// Empeche les monstres de tomber dans le vide en mode Infini.
/// Verifie qu'il y a un cube dans la direction du mouvement avant de bouger.
/// A placer sur Monster01 et Monster02 dans Stage01-01Infini.
/// </summary>
public class MonsterBoundaryCheck : MonoBehaviour
{
    [Header("Detection")]
    public float rayDistance = 2.5f;  // Distance de detection devant le monstre
    public float rayHeightOffset = 1f;   // Hauteur du raycast par rapport au monstre
    public LayerMask cubeLayer;          // Layer des cubes — laisser Everything si pas de layer specifique

    private monsterZoom monsterScript;

    void Start()
    {
        monsterScript = GetComponent<monsterZoom>();
        if (monsterScript == null)
            monsterScript = GetComponentInParent<monsterZoom>();
    }

    void FixedUpdate()
    {
        if (monsterScript == null) return;
        if (!monsterScript.enabled) return;

        // Verifie les 4 directions
        VerifierEtBloquerDirection();
    }

    private void VerifierEtBloquerDirection()
    {
        // Direction actuelle du monstre selon choixDirection
        Vector3 dir = GetDirectionActuelle();
        if (dir == Vector3.zero) return;

        // Origine du raycast — position du monstre + offset hauteur
        Vector3 origine = transform.position + Vector3.up * rayHeightOffset;

        // Raycast vers le bas depuis la position cible
        Vector3 positionCible = transform.position + dir * rayDistance;
        Vector3 origineVersBas = new Vector3(positionCible.x, origine.y + 2f, positionCible.z);

        bool cubeDevant = Physics.Raycast(origineVersBas, Vector3.down, 4f, cubeLayer == 0 ? Physics.AllLayers : cubeLayer);

        if (!cubeDevant)
        {
            // Pas de cube dans cette direction — change de direction
            ChoisirNouvelleDirection(dir);
        }
    }

    private Vector3 GetDirectionActuelle()
    {
        switch (monsterScript.choixDirection)
        {
            case 1: return Vector3.left;
            case 2: return Vector3.right;
            case 3: return Vector3.forward;
            case 4: return Vector3.back;
            default: return Vector3.zero;
        }
    }

    private void ChoisirNouvelleDirection(Vector3 dirBloquee)
    {
        // Essaie les autres directions
        Vector3[] directions = { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        int[] choix = { 1, 2, 3, 4 };

        for (int i = 0; i < directions.Length; i++)
        {
            if (directions[i] == dirBloquee) continue;

            Vector3 posTest = transform.position + directions[i] * rayDistance;
            Vector3 origineTest = new Vector3(posTest.x, transform.position.y + 3f, posTest.z);

            if (Physics.Raycast(origineTest, Vector3.down, 5f, cubeLayer == 0 ? Physics.AllLayers : cubeLayer))
            {
                monsterScript.choixDirection = choix[i];
                return;
            }
        }

        // Aucune direction valide — reste sur place
        monsterScript.tempsAvantDeplacement = 1f;
    }
}