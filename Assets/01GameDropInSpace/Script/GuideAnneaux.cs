using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// A placer sur le PlayerController dans le SpaceHub.
/// Appuyer sur X affiche une chaine de petits anneaux vers
/// la planete non visitee la plus proche.
/// </summary>
public class GuideAnneaux : MonoBehaviour
{
    [Header("Planetes")]
    public PlaneteExploration[] planetes;

    [Header("Anneaux")]
    public GameObject petitAnneauPrefab;
    public int nombreAnneaux = 12;
    public float delaiApparitionAnneau = 0.15f;
    public float dureeAffichage = 6.0f;

    [Header("Flash blanc")]
    public Image flashImage;
    public float dureeFadeIn = 0.2f;
    public float dureeFadeOut = 0.3f;

    // Input System
    private DIS_Controls controls;
    private List<GameObject> anneaux = new List<GameObject>();
    private bool enCours = false;

    void Awake()
    {
        controls = new DIS_Controls();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Tonneau.performed += ctx => DemanderGuide();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void DemanderGuide()
    {
        if (enCours) return;
        StartCoroutine(AfficherAnneaux());
    }

    IEnumerator AfficherAnneaux()
    {
        enCours = true;

        // Trouve la planete non visitee la plus proche
        PlaneteExploration cible = TrouverPlanetePlusProche(transform.position);

        if (cible == null)
        {
            Debug.Log("Toutes les planetes ont ete visitees !");
            enCours = false;
            yield break;
        }

        // Flash blanc + teleportation
        yield return StartCoroutine(Flash());

        // Teleporte Drop devant le premier anneau
        Vector3 departDevantDrop = transform.position + transform.forward * 5f;
        Vector3 directionVersPlanete = (cible.transform.position - departDevantDrop).normalized;
        transform.position = departDevantDrop + directionVersPlanete * 5f;
        transform.forward = directionVersPlanete;

        // Cree les anneaux un par un
        for (int i = 1; i <= nombreAnneaux; i++)
        {
            Vector3 depart = transform.position;
            Vector3 direction = (cible.transform.position - depart).normalized;
            float distanceTotale = Vector3.Distance(depart, cible.transform.position);
            float pas = distanceTotale / nombreAnneaux;

            Vector3 position = depart + direction * pas * i;
            Quaternion rotation = Quaternion.LookRotation(cible.transform.position - position);

            if (petitAnneauPrefab != null)
            {
                GameObject anneau = Instantiate(petitAnneauPrefab, position, rotation);
                PetitAnneauSuivi suivi = anneau.AddComponent<PetitAnneauSuivi>();
                suivi.cible = cible.transform;
                anneaux.Add(anneau);
            }

            yield return new WaitForSeconds(delaiApparitionAnneau);
        }

        // Attend puis nettoie
        yield return new WaitForSeconds(dureeAffichage);
        NettoyerAnneaux();
        enCours = false;
    }

    IEnumerator Flash()
    {
        if (flashImage == null) yield break;

        float elapsed = 0f;
        flashImage.gameObject.SetActive(true);

        // Fade in
        while (elapsed < dureeFadeIn)
        {
            elapsed += Time.deltaTime;
            flashImage.color = new Color(1f, 1f, 1f, elapsed / dureeFadeIn);
            yield return null;
        }
        flashImage.color = Color.white;

        // Fade out
        elapsed = 0f;
        while (elapsed < dureeFadeOut)
        {
            elapsed += Time.deltaTime;
            flashImage.color = new Color(1f, 1f, 1f, 1f - elapsed / dureeFadeOut);
            yield return null;
        }
        flashImage.gameObject.SetActive(false);
    }

    PlaneteExploration TrouverPlanetePlusProche(Vector3 position)
    {
        PlaneteExploration plusProche = null;
        float distanceMin = float.MaxValue;

        foreach (var planete in planetes)
        {
            if (planete == null) continue;

            string data = PlayerPrefs.GetString("ExplorationProgress", "00000000");
            int index = planete.numeroPlanete - 1;
            if (index >= 0 && index < data.Length && data[index] == '1') continue;

            float distance = Vector3.Distance(position, planete.transform.position);
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
        foreach (var anneau in anneaux)
            if (anneau != null) Destroy(anneau);
        anneaux.Clear();
    }
}