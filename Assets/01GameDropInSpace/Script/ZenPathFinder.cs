using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenPathFinder : MonoBehaviour
{
    [Header("Parametres")]
    public float porteeSaut = 2.5f;
    public float delaiDemarrage = 0.5f;

    private ZenPathRenderer zenPathRenderer;

    void Start()
    {
        if (PlayerPrefs.GetInt("ModeZenActif", 0) != 1) return;

        zenPathRenderer = GetComponent<ZenPathRenderer>();
        if (zenPathRenderer == null)
        {
            Debug.LogWarning("ZenPathFinder : ZenPathRenderer introuvable !");
            return;
        }

        // Si des points manuels sont définis — ZenPathRenderer les gère tout seul
        if (zenPathRenderer.cheminPoints != null && zenPathRenderer.cheminPoints.Length >= 2)
        {
            Debug.Log("[ZenPathFinder] Points manuels détectés — pas de calcul automatique.");
            return;
        }

        StartCoroutine(CalculerCheminAvecDelai());
    }

    IEnumerator CalculerCheminAvecDelai()
    {
        yield return new WaitForSeconds(delaiDemarrage);

        List<changeColorCube> cubesValides = ObtenirCubesValides();

        if (cubesValides.Count < 2)
        {
            Debug.LogWarning("ZenPathFinder : pas assez de cubes valides !");
            yield break;
        }

        List<Vector3> chemin = CalculerChemin(cubesValides);

        if (chemin == null || chemin.Count < 2)
        {
            Debug.LogWarning("ZenPathFinder : impossible de trouver un chemin !");
            yield break;
        }

        AppliquerChemin(chemin);
        Debug.Log($"[ZenPathFinder] Chemin calculé — {chemin.Count} points !");
    }

    // ── Récupère tous les cubes non coloriés ─────────────────────────
    List<changeColorCube> ObtenirCubesValides()
    {
        changeColorCube[] tousLesCubes = FindObjectsByType<changeColorCube>(FindObjectsSortMode.None);
        List<changeColorCube> valides = new List<changeColorCube>();

        foreach (var cube in tousLesCubes)
            if (!cube.changeColorOK)
                valides.Add(cube);

        Debug.Log($"[ZenPathFinder] {valides.Count} cubes valides sur {tousLesCubes.Length} total");
        return valides;
    }

    // ── Algorithme Greedy — rapide et léger ──────────────────────────
    List<Vector3> CalculerChemin(List<changeColorCube> cubes)
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 posDepart = player != null ? player.transform.position : Vector3.zero;

        changeColorCube cubeDepart = TrouverCubeLePlusProche(cubes, posDepart);
        if (cubeDepart == null) return null;

        List<changeColorCube> chemin = new List<changeColorCube>();
        List<changeColorCube> restants = new List<changeColorCube>(cubes);

        changeColorCube courant = cubeDepart;
        chemin.Add(courant);
        restants.Remove(courant);

        while (restants.Count > 0)
        {
            changeColorCube suivant = null;
            float distMin = float.MaxValue;

            foreach (var cube in restants)
            {
                float dist = Vector3.Distance(courant.transform.position, cube.transform.position);
                if (dist <= porteeSaut && dist < distMin)
                {
                    distMin = dist;
                    suivant = cube;
                }
            }

            if (suivant == null) break; // plus de cube accessible
            chemin.Add(suivant);
            restants.Remove(suivant);
            courant = suivant;
        }

        List<Vector3> positions = new List<Vector3>();
        foreach (var cube in chemin)
            positions.Add(cube.transform.position + Vector3.up * 0.6f);

        return positions;
    }

    changeColorCube TrouverCubeLePlusProche(List<changeColorCube> cubes, Vector3 position)
    {
        changeColorCube plusProche = null;
        float distanceMin = float.MaxValue;

        foreach (var cube in cubes)
        {
            float dist = Vector3.Distance(cube.transform.position, position);
            if (dist < distanceMin)
            {
                distanceMin = dist;
                plusProche = cube;
            }
        }

        return plusProche;
    }

    // ── Envoie le chemin calculé à ZenPathRenderer ───────────────────
    void AppliquerChemin(List<Vector3> positions)
    {
        Transform[] points = new Transform[positions.Count];

        for (int i = 0; i < positions.Count; i++)
        {
            GameObject p = new GameObject($"ZenAutoPoint_{i}");
            p.transform.SetParent(transform);
            p.transform.position = positions[i];
            points[i] = p.transform;
        }

        zenPathRenderer.cheminPoints = points;
        zenPathRenderer.DemarrerChemin();
    }
}