using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteGridManager : MonoBehaviour
{
    public static InfiniteGridManager Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject cubeGridPrefab;
    public GameObject cubeDestructPrefab;

    [Header("Grille")]
    public int columnLength = 9;
    public int forwardLength = 9;
    public float x_Space = 2f;
    public float z_Space = 2f;

    [Header("Difficulte")]
    [Range(0, 60)] public int cubesRetiresMin = 15;
    [Range(0, 60)] public int cubesRetiresMax = 45;
    public int cubesRetiresParVague = 3;

    [Header("Destructeurs visuels")]
    public float hauteurSpawnDestructeur = 5f;
    public float delaiAutoDestructeur = 3f;

    [Header("Monstres")]
    public GameObject monster01;
    public GameObject monster02;

    [Header("Player")]
    public GameObject player; // PlayerGroupe

    // Etat interne
    private List<GameObject> cubesActifs = new List<GameObject>();
    private List<GameObject> destructeursActifs = new List<GameObject>();
    private int vagueCourante = 0;
    private int cubesColories = 0;
    private int cubesCreesPourVague = 0;
    private bool _respawnEnCours = false;

    private MonsterRandomizer randomizer01;
    private MonsterRandomizer randomizer02;

    private const int INDEX_CENTRE = 40;

    private static readonly Vector3[] positions = new Vector3[]
    {
        new Vector3(0,0,0),  new Vector3(2,0,0),  new Vector3(4,0,0),  new Vector3(6,0,0),  new Vector3(8,0,0),  new Vector3(10,0,0), new Vector3(12,0,0), new Vector3(14,0,0), new Vector3(16,0,0),
        new Vector3(0,0,2),  new Vector3(2,0,2),  new Vector3(4,0,2),  new Vector3(6,0,2),  new Vector3(8,0,2),  new Vector3(10,0,2), new Vector3(12,0,2), new Vector3(14,0,2), new Vector3(16,0,2),
        new Vector3(0,0,4),  new Vector3(2,0,4),  new Vector3(4,0,4),  new Vector3(6,0,4),  new Vector3(8,0,4),  new Vector3(10,0,4), new Vector3(12,0,4), new Vector3(14,0,4), new Vector3(16,0,4),
        new Vector3(0,0,6),  new Vector3(2,0,6),  new Vector3(4,0,6),  new Vector3(6,0,6),  new Vector3(8,0,6),  new Vector3(10,0,6), new Vector3(12,0,6), new Vector3(14,0,6), new Vector3(16,0,6),
        new Vector3(0,0,8),  new Vector3(2,0,8),  new Vector3(4,0,8),  new Vector3(6,0,8),  new Vector3(8,0,8),  new Vector3(10,0,8), new Vector3(12,0,8), new Vector3(14,0,8), new Vector3(16,0,8),
        new Vector3(0,0,10), new Vector3(2,0,10), new Vector3(4,0,10), new Vector3(6,0,10), new Vector3(8,0,10), new Vector3(10,0,10),new Vector3(12,0,10),new Vector3(14,0,10),new Vector3(16,0,10),
        new Vector3(0,0,12), new Vector3(2,0,12), new Vector3(4,0,12), new Vector3(6,0,12), new Vector3(8,0,12), new Vector3(10,0,12),new Vector3(12,0,12),new Vector3(14,0,12),new Vector3(16,0,12),
        new Vector3(0,0,14), new Vector3(2,0,14), new Vector3(4,0,14), new Vector3(6,0,14), new Vector3(8,0,14), new Vector3(10,0,14),new Vector3(12,0,14),new Vector3(14,0,14),new Vector3(16,0,14),
        new Vector3(0,0,16), new Vector3(2,0,16), new Vector3(4,0,16), new Vector3(6,0,16), new Vector3(8,0,16), new Vector3(10,0,16),new Vector3(12,0,16),new Vector3(14,0,16),new Vector3(16,0,16)
    };

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        Physics.gravity = new Vector3(0, -9.81f, 0);
        if (monster01 != null) randomizer01 = monster01.GetComponentInChildren<MonsterRandomizer>();
        if (monster02 != null) randomizer02 = monster02.GetComponentInChildren<MonsterRandomizer>();
        BloquerDrop();
        GenererVague();
    }

    // ══════════════════════════════════════════════════════════════════════
    // BLOQUER DROP
    // ══════════════════════════════════════════════════════════════════════

    private void BloquerDrop()
    {
        if (player == null) return;
        rollingCube rc = player.GetComponentInChildren<rollingCube>();
        Rigidbody rb = player.GetComponentInChildren<Rigidbody>();
        if (rc != null) { rc.enabled = false; rc.desacTouche = true; }
        if (rb != null) { rb.useGravity = false; rb.isKinematic = true; }
        // Cache Drop hors champ
        player.transform.position = new Vector3(999f, 999f, 999f);
    }

    // ══════════════════════════════════════════════════════════════════════
    // GENERATION
    // ══════════════════════════════════════════════════════════════════════

    public void GenererVague()
    {
        foreach (GameObject c in cubesActifs) if (c != null) Destroy(c);
        foreach (GameObject d in destructeursActifs) if (d != null) Destroy(d);
        cubesActifs.Clear();
        destructeursActifs.Clear();
        cubesColories = 0;
        vagueCourante++;

        BloquerDrop();

        int nbRetires = Mathf.Min(
            cubesRetiresMin + (vagueCourante - 1) * cubesRetiresParVague,
            cubesRetiresMax);

        HashSet<int> cubesGardes = SelectionnerCubesJouables(nbRetires);
        HashSet<int> cubesRetires = new HashSet<int>();
        for (int i = 0; i < positions.Length; i++)
            if (!cubesGardes.Contains(i)) cubesRetires.Add(i);

        foreach (int idx in cubesGardes)
            cubesActifs.Add(Instantiate(cubeGridPrefab, positions[idx], Quaternion.identity));

        cubesCreesPourVague = cubesActifs.Count;
        Debug.Log($"[InfiniteGridManager] Vague {vagueCourante} — {cubesCreesPourVague} cubes — {cubesRetires.Count} retires");

        StartCoroutine(AnimerDestructeurs(cubesRetires));
    }

    public IEnumerator RegenerarGrille()
    {
        yield return new WaitForSeconds(0.5f);
        GenererVague();
    }

    // ══════════════════════════════════════════════════════════════════════
    // SELECTION BFS
    // ══════════════════════════════════════════════════════════════════════

    private HashSet<int> SelectionnerCubesJouables(int nbRetires)
    {
        HashSet<int> gardes = new HashSet<int>();
        for (int i = 0; i < positions.Length; i++) gardes.Add(i);

        List<int> candidats = new List<int>();
        for (int i = 0; i < positions.Length; i++)
            if (i != INDEX_CENTRE) candidats.Add(i);
        Melanger(candidats);

        int retires = 0;
        foreach (int idx in candidats)
        {
            if (retires >= nbRetires) break;
            gardes.Remove(idx);
            if (!TousAccessibles(gardes)) gardes.Add(idx);
            else retires++;
        }
        return gardes;
    }

    private bool TousAccessibles(HashSet<int> gardes)
    {
        if (!gardes.Contains(INDEX_CENTRE)) return false;
        HashSet<int> visites = new HashSet<int>();
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(INDEX_CENTRE);
        visites.Add(INDEX_CENTRE);

        while (queue.Count > 0)
        {
            int idx = queue.Dequeue();
            int col = idx % 9;
            int row = idx / 9;
            for (int dist = 1; dist <= 2; dist++)
            {
                int[] voisins = {
                    (row - dist >= 0) ? idx - 9 * dist : -1,
                    (row + dist <= 8) ? idx + 9 * dist : -1,
                    (col - dist >= 0) ? idx - dist     : -1,
                    (col + dist <= 8) ? idx + dist      : -1
                };
                foreach (int v in voisins)
                {
                    if (v < 0 || !gardes.Contains(v) || visites.Contains(v)) continue;
                    if (dist == 2) { int m = (idx + v) / 2; if (gardes.Contains(m)) continue; }
                    visites.Add(v);
                    queue.Enqueue(v);
                }
            }
        }
        return visites.Count == gardes.Count;
    }

    // ══════════════════════════════════════════════════════════════════════
    // DESTRUCTEURS VISUELS
    // ══════════════════════════════════════════════════════════════════════

    private IEnumerator AnimerDestructeurs(HashSet<int> positionsRetirees)
    {
        yield return new WaitForSeconds(0.2f);

        List<int> listeRetires = new List<int>(positionsRetirees);
        Melanger(listeRetires);

        foreach (int idx in listeRetires)
        {
            Vector3 posSpawn = positions[idx];
            posSpawn.y = hauteurSpawnDestructeur;
            GameObject d = Instantiate(cubeDestructPrefab, posSpawn, Quaternion.identity);
            destructeursActifs.Add(d);
            StartCoroutine(AutoDetruire(d, delaiAutoDestructeur));
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(delaiAutoDestructeur + 0.5f);
        yield return StartCoroutine(SpawnerTout());
    }

    private IEnumerator AutoDetruire(GameObject obj, float delai)
    {
        yield return new WaitForSeconds(delai);
        if (obj != null) { destructeursActifs.Remove(obj); Destroy(obj); }
    }

    // ══════════════════════════════════════════════════════════════════════
    // SPAWN DROP + MONSTRES
    // ══════════════════════════════════════════════════════════════════════

    private IEnumerator SpawnerTout()
    {
        _respawnEnCours = true;
        Debug.Log("[InfiniteGridManager] SpawnerTout appelé !");

        // Position Drop — centre de la grille
        Vector3 posSpawnDrop = positions[INDEX_CENTRE];

        // Positions monstres
        var (posMonstre01, posMonstre02) = ReplacerMonstres();

        // SpawnScaleEffect Drop
        SpawnScaleEffect effectPlayer = player != null ? player.GetComponent<SpawnScaleEffect>() : null;
        Debug.Log($"[InfiniteGridManager] effectPlayer={effectPlayer != null} pos={posSpawnDrop}");

        if (effectPlayer != null)
            StartCoroutine(effectPlayer.LancerSpawn(posSpawnDrop));
        else
            Debug.LogWarning("[InfiniteGridManager] SpawnScaleEffect non trouve sur PlayerGroupe !");

        // SpawnScaleEffect Monster01
        if (monster01 != null && posMonstre01 != Vector3.zero)
        {
            SpawnScaleEffect e = monster01.GetComponent<SpawnScaleEffect>();
            if (e != null) StartCoroutine(e.LancerSpawn(posMonstre01));
        }

        // SpawnScaleEffect Monster02
        if (monster02 != null && monster02.activeSelf && posMonstre02 != Vector3.zero)
        {
            SpawnScaleEffect e = monster02.GetComponent<SpawnScaleEffect>();
            if (e != null) StartCoroutine(e.LancerSpawn(posMonstre02));
        }

        // Attend fin animation (0.6s) + delai (1s)
        yield return new WaitForSeconds(1.6f + 1f);

        _respawnEnCours = false;
        Debug.Log("[InfiniteGridManager] Spawn termine — jeu demarre !");
    }

    public IEnumerator RespawnDrop()
    {
        yield return StartCoroutine(SpawnerTout());
    }

    // ══════════════════════════════════════════════════════════════════════
    // SIGNAL CUBE
    // ══════════════════════════════════════════════════════════════════════

    public void SignalerCubeColorie()
    {
        cubesColories++;
        Debug.Log($"[InfiniteGridManager] {cubesColories} / {cubesCreesPourVague}");
        if (cubesColories >= cubesCreesPourVague)
        {
            Debug.Log("[InfiniteGridManager] Tous colories — nouvelle vague !");
            if (InfiniteModeManager.Instance != null) InfiniteModeManager.Instance.NouvelleVague();
            else GenererVague();
        }
    }

    public void SignalerCubeDecolorie()
    {
        cubesColories = Mathf.Max(0, cubesColories - 1);
    }

    // ══════════════════════════════════════════════════════════════════════
    // REPLACER MONSTRES
    // ══════════════════════════════════════════════════════════════════════

    private (Vector3, Vector3) ReplacerMonstres()
    {
        Vector3 pos1 = Vector3.zero;
        Vector3 pos2 = Vector3.zero;

        List<GameObject> dispos = cubesActifs.FindAll(c => c != null);
        if (dispos.Count < 2) return (pos1, pos2);

        Vector3 posDrop = new Vector3(positions[INDEX_CENTRE].x, 0f, positions[INDEX_CENTRE].z);

        // Melange
        for (int i = dispos.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (dispos[i], dispos[j]) = (dispos[j], dispos[i]);
        }

        // Cubes loin de Drop (distance min 6)
        List<GameObject> loin = dispos.FindAll(c =>
            Vector3.Distance(new Vector3(c.transform.position.x, 0f, c.transform.position.z), posDrop) >= 6f);
        if (loin.Count < 2) loin = dispos;

        if (monster01 != null && loin.Count >= 1)
        {
            pos1 = loin[0].transform.position;
            if (randomizer01 != null) randomizer01.RandomiserMonstre();
        }

        if (monster02 != null && loin.Count >= 2)
        {
            pos2 = loin[1].transform.position;
            if (randomizer02 != null) randomizer02.RandomiserMonstre();
        }

        return (pos1, pos2);
    }

    // ══════════════════════════════════════════════════════════════════════
    // UTILITAIRES
    // ══════════════════════════════════════════════════════════════════════

    public bool RespawnEnCours() => _respawnEnCours;
    public int GetVague() => vagueCourante;
    public int NombreCubesRestants() => cubesCreesPourVague - cubesColories;

    public Vector3 GetPositionAleatoire()
    {
        List<GameObject> actifs = cubesActifs.FindAll(c => c != null);
        if (actifs.Count == 0) return positions[INDEX_CENTRE] + Vector3.up;
        return actifs[Random.Range(0, actifs.Count)].transform.position + Vector3.up;
    }

    private void Melanger(List<int> liste)
    {
        for (int i = liste.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (liste[i], liste[j]) = (liste[j], liste[i]);
        }
    }
}