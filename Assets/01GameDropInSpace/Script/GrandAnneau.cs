using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A placer sur le grand anneau pres du soleil.
/// Quand Drop traverse l'anneau, une chaine de petits anneaux apparait
/// et guide Drop vers la planete non visitee la plus proche.
/// </summary>
public class GrandAnneau : MonoBehaviour
{
    [Header("Planetes")]
    public PlaneteExploration[] planetes; // Assigner les 8 planetes dans l'Inspector

    [Header("Petits anneaux")]
    public GameObject petitAnneauPrefab;     // Prefab du petit anneau
    public int nombreAnneaux = 12;           // Nombre de petits anneaux
    public float distanceEntreAnneaux = 8f;  // Distance entre chaque anneau

    [Header("Effets")]
    public ParticleSystem particlesTraversee;
    public float delaiApparitionAnneau = 0.15f; // Delai entre chaque anneau

    private bool aEteTraverse = false;
    private List<GameObject> petitsAnneaux = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (aEteTraverse) return;

        aEteTraverse = true;
        StartCoroutine(DeclencherChaine(other.transform));
    }

    IEnumerator DeclencherChaine(Transform drop)
    {
        // Lance les particules
        if (particlesTraversee != null)
            particlesTraversee.Play();

        // Trouve la planete non visitee la plus proche
        PlaneteExploration cible = TrouverPlanetePlusProche(drop.position);

        if (cible == null)
        {
            Debug.Log("Toutes les planetes ont ete visitees !");
            aEteTraverse = false;
            yield break;
        }

        Debug.Log("Planete cible : " + cible.gameObject.name);

        // Cree les petits anneaux un par un
        // La direction est recalculee a chaque anneau pour suivre la planete en mouvement
        for (int i = 1; i <= nombreAnneaux; i++)
        {
            // Recalcule la direction vers la planete a chaque instant
            Vector3 directionActuelle = (cible.transform.position - drop.position).normalized;
            float distanceTotale = Vector3.Distance(drop.position, cible.transform.position);
            float distanceAnneau = distanceTotale / (nombreAnneaux - i + 1);

            Vector3 position = drop.position + directionActuelle * distanceAnneau * i;

            if (petitAnneauPrefab != null)
            {
                // L'anneau pointe vers la planete en mouvement
                Quaternion rotation = Quaternion.LookRotation(cible.transform.position - position);
                GameObject anneau = Instantiate(petitAnneauPrefab, position, rotation);

                // Script sur le petit anneau pour qu'il continue a pointer vers la planete
                PetitAnneauSuivi suivi = anneau.AddComponent<PetitAnneauSuivi>();
                suivi.cible = cible.transform;

                petitsAnneaux.Add(anneau);
            }

            yield return new WaitForSeconds(delaiApparitionAnneau);
        }

        // Attend que Drop suive les anneaux puis nettoie
        yield return new WaitForSeconds(5.0f);
        NettoyerAnneaux();
        aEteTraverse = false;
    }

    PlaneteExploration TrouverPlanetePlusProche(Vector3 positionDrop)
    {
        PlaneteExploration plusProche = null;
        float distanceMin = float.MaxValue;

        foreach (var planete in planetes)
        {
            // Ignore les planetes deja visitees
            if (planete == null) continue;
            int numero = planete.numeroPlanete;
            string data = PlayerPrefs.GetString("ExplorationProgress", "00000000");
            if (data.Length > numero - 1 && data[numero - 1] == '1') continue;

            float distance = Vector3.Distance(positionDrop, planete.transform.position);
            if (distance < distanceMin)
            {
                distanceMin = distance;
                plusProche = planete;
            }
        }

        return plusProche;
    }

    void NettoyerAnneaux()
    {
        foreach (var anneau in petitsAnneaux)
            if (anneau != null) Destroy(anneau);
        petitsAnneaux.Clear();
    }
}