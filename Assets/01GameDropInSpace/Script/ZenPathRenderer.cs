using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class ZenPathRenderer : MonoBehaviour
{
    [Header("Points du chemin optimal")]
    public Transform[] cheminPoints;

    [Header("Style comète")]
    public Color couleurTete = new Color(1f, 0.95f, 0.7f, 1f);
    public Color couleurQueue = new Color(1f, 0.6f, 0.2f, 0f);
    public float largeurTete = 0.15f;
    public float largeurQueue = 0.02f;

    [Header("Particules spirituelles")]
    public int nombreParticules = 12;
    public float vitesseParticules = 0.08f;
    public float tailleParticule = 0.08f;
    public Color couleurParticule = new Color(1f, 0.98f, 0.85f, 0.9f);

    [Header("Fondu")]
    public float delaiAvantFondu = 3f;   // secondes avant que le fondu commence
    public float dureeFondu = 4f;   // secondes pour disparaître complètement
    public float dureeReapparition = 1f;  // secondes pour réapparaître au bouton A

    private LineRenderer lineRenderer;
    private List<GameObject> particules = new List<GameObject>();
    private List<float> progressParticules = new List<float>();

    private float alphaActuel = 1f;
    private float largeurActuelle;
    private bool estVisible = true;
    private bool enFondu = false;
    private bool enReapparition = false;
    private float timerFondu = 0f;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("ModeZenActif", 0) != 1)
        {
            gameObject.SetActive(false);
            return;
        }

        if (cheminPoints != null && cheminPoints.Length >= 2)
            DemarrerChemin();
    }

    public void DemarrerChemin()
    {
        if (cheminPoints == null || cheminPoints.Length < 2)
        {
            Debug.LogWarning("ZenPathRenderer : pas assez de points !");
            return;
        }

        largeurActuelle = largeurTete;
        alphaActuel = 1f;

        ConfigurerLineRenderer();
        CreerParticules();
        StartCoroutine(AnimerParticules());
        StartCoroutine(GererFondu());
    }

    void Update()
    {
        // Bouton A gamepad — ou touche espace clavier
        bool boutonA = Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame;
        bool espace = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;

        if ((boutonA || espace) && !estVisible)
            StartCoroutine(Reapparaitre());
    }

    // ── Gestion du fondu progressif ───────────────────────────────────
    IEnumerator GererFondu()
    {
        // Attend 3 secondes avant de commencer le fondu
        yield return new WaitForSeconds(delaiAvantFondu);

        enFondu = true;
        timerFondu = 0f;

        while (timerFondu < dureeFondu)
        {
            timerFondu += Time.deltaTime;
            alphaActuel = Mathf.Lerp(1f, 0f, timerFondu / dureeFondu);
            AppliquerAlpha(alphaActuel);
            yield return null;
        }

        alphaActuel = 0f;
        AppliquerAlpha(0f);
        estVisible = false;
        enFondu = false;
    }

    IEnumerator Reapparaitre()
    {
        if (enReapparition) yield break;
        enReapparition = true;
        estVisible = true;

        float timer = 0f;
        while (timer < dureeReapparition)
        {
            timer += Time.deltaTime;
            alphaActuel = Mathf.Lerp(0f, 1f, timer / dureeReapparition);
            AppliquerAlpha(alphaActuel);
            yield return null;
        }

        alphaActuel = 1f;
        AppliquerAlpha(1f);
        enReapparition = false;

        // Relance le fondu
        StartCoroutine(GererFondu());
    }

    // ── Applique l'alpha à la ligne et aux particules ─────────────────
    void AppliquerAlpha(float alpha)
    {
        // Ligne — gradient avec alpha
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(couleurTete, 0f),
                new GradientColorKey(new Color(1f, 0.75f, 0.35f), 0.4f),
                new GradientColorKey(new Color(couleurQueue.r, couleurQueue.g, couleurQueue.b), 1f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(alpha,        0f),
                new GradientAlphaKey(0.6f * alpha, 0.5f),
                new GradientAlphaKey(0f,           1f)
            }
        );
        lineRenderer.colorGradient = gradient;

        // Largeur de la ligne proportionnelle à l'alpha
        AnimationCurve courbe = new AnimationCurve();
        courbe.AddKey(0f, largeurTete * alpha);
        courbe.AddKey(0.3f, largeurTete * 0.6f * alpha);
        courbe.AddKey(1f, largeurQueue * alpha);
        lineRenderer.widthCurve = courbe;
    }

    void ConfigurerLineRenderer()
    {
        Shader shaderCible = Shader.Find("Universal Render Pipeline/Particles/Unlit");
        if (shaderCible == null)
            shaderCible = Shader.Find("Universal Render Pipeline/Unlit");
        if (shaderCible == null)
            shaderCible = Shader.Find("Unlit/Color");

        lineRenderer.material = new Material(shaderCible);
        lineRenderer.material.color = couleurTete;

        lineRenderer.positionCount = cheminPoints.Length;
        lineRenderer.useWorldSpace = true;
        lineRenderer.numCapVertices = 5;
        lineRenderer.numCornerVertices = 5;
        lineRenderer.widthMultiplier = 1f;

        AnimationCurve courbe = new AnimationCurve();
        courbe.AddKey(0f, largeurTete);
        courbe.AddKey(0.3f, largeurTete * 0.6f);
        courbe.AddKey(1f, largeurQueue);
        lineRenderer.widthCurve = courbe;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(couleurTete, 0f),
                new GradientColorKey(new Color(1f, 0.75f, 0.35f), 0.4f),
                new GradientColorKey(new Color(couleurQueue.r, couleurQueue.g, couleurQueue.b), 1f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f,   0f),
                new GradientAlphaKey(0.6f, 0.5f),
                new GradientAlphaKey(0f,   1f)
            }
        );
        lineRenderer.colorGradient = gradient;

        for (int i = 0; i < cheminPoints.Length; i++)
            if (cheminPoints[i] != null)
                lineRenderer.SetPosition(i, cheminPoints[i].position);
    }

    void CreerParticules()
    {
        foreach (var p in particules)
            if (p != null) Destroy(p);
        particules.Clear();
        progressParticules.Clear();

        Shader shaderCible = Shader.Find("Universal Render Pipeline/Particles/Unlit");
        if (shaderCible == null)
            shaderCible = Shader.Find("Universal Render Pipeline/Unlit");
        if (shaderCible == null)
            shaderCible = Shader.Find("Unlit/Color");

        for (int i = 0; i < nombreParticules; i++)
        {
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            p.name = "ZenParticule_" + i;
            p.transform.SetParent(transform);
            p.transform.localScale = Vector3.one * tailleParticule;
            Destroy(p.GetComponent<Collider>());

            Material mat = new Material(shaderCible);
            mat.color = couleurParticule;
            p.GetComponent<Renderer>().material = mat;

            float progress = (float)i / nombreParticules;
            progressParticules.Add(progress);
            particules.Add(p);
            p.transform.position = ObtenirPositionSurChemin(progress);
        }
    }

    IEnumerator AnimerParticules()
    {
        while (true)
        {
            for (int i = 0; i < particules.Count; i++)
            {
                if (particules[i] == null) continue;

                progressParticules[i] += Time.deltaTime * vitesseParticules * (0.8f + i * 0.04f);
                if (progressParticules[i] > 1f)
                    progressParticules[i] -= 1f;

                Vector3 pos = ObtenirPositionSurChemin(progressParticules[i]);
                pos.y += Mathf.Sin(Time.time * 1.5f + i * 0.8f) * 0.08f;
                particules[i].transform.position = pos;

                // Alpha particule — suit l'alpha global + pulsation
                float pulsation = 0.5f + Mathf.Sin(Time.time * 2f + i * 1.2f) * 0.4f;
                float alphaFinal = alphaActuel * pulsation;
                Color c = couleurParticule;
                c.a = alphaFinal;
                particules[i].GetComponent<Renderer>().material.color = c;

                float taille = tailleParticule * alphaActuel * (0.8f + Mathf.Sin(Time.time * 1.8f + i * 0.6f) * 0.2f);
                particules[i].transform.localScale = Vector3.one * taille;
            }

            yield return null;
        }
    }

    Vector3 ObtenirPositionSurChemin(float t)
    {
        if (cheminPoints == null || cheminPoints.Length < 2)
            return Vector3.zero;

        t = Mathf.Clamp01(t);
        float totalSegments = cheminPoints.Length - 1;
        float indexFloat = t * totalSegments;
        int indexA = Mathf.FloorToInt(indexFloat);
        int indexB = Mathf.Min(indexA + 1, cheminPoints.Length - 1);
        float segmentT = indexFloat - indexA;

        if (cheminPoints[indexA] == null || cheminPoints[indexB] == null)
            return Vector3.zero;

        return Vector3.Lerp(cheminPoints[indexA].position, cheminPoints[indexB].position, segmentT);
    }

    void OnDrawGizmos()
    {
        if (cheminPoints == null || cheminPoints.Length < 2) return;

        Gizmos.color = new Color(1f, 0.8f, 0.3f, 0.8f);
        for (int i = 0; i < cheminPoints.Length - 1; i++)
        {
            if (cheminPoints[i] != null && cheminPoints[i + 1] != null)
            {
                Gizmos.DrawLine(cheminPoints[i].position, cheminPoints[i + 1].position);
                Gizmos.DrawSphere(cheminPoints[i].position, 0.12f);
            }
        }
        if (cheminPoints[cheminPoints.Length - 1] != null)
            Gizmos.DrawSphere(cheminPoints[cheminPoints.Length - 1].position, 0.12f);
    }

    void OnDestroy()
    {
        foreach (var p in particules)
            if (p != null) Destroy(p);
    }
}