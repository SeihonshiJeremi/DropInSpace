using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ZenVisuel : MonoBehaviour
{
    [Header("Lumieres — assigne dans l'Inspector")]
    public Light directionalLight;

    [Header("Lumieres")]
    public float intensiteDirectional = 1.4f;   // plus douce qu'Obscur
    public float intensiteOriginale = 1.9f;

    [Header("Vignette dynamique — suit Drop")]
    public float vignetteIntensiteBase = 0.45f;      // beaucoup plus légère qu'Obscur
    public float vignetteSmoothness = 0.6f;           // plus douce
    public float intensiteEnMouvement = 0.4f;
    public float intensiteArret = 0.5f;
    public float vitesseTransition = 1.5f;            // transition lente et apaisante

    [Header("Pulsation douce")]
    public float pulsationAmplitude = 0.03f;          // respiration subtile
    public float pulsationVitesse = 0.4f;             // très lente

    [Header("Bloom chaud")]
    public float bloomIntensity = 1.2f;
    public float bloomThreshold = 0.9f;
    public float bloomScatter = 0.7f;

    [Header("Vignette fixe")]
    public float vignetteFixeIntensity = 0.35f;
    public float vignetteFixeSmoothness = 0.7f;

    [Header("Couleurs chaudes")]
    public Color couleurVignette = new Color(0.3f, 0.15f, 0.05f);   // brun chaud
    public Color couleurFilter = new Color(1f, 0.92f, 0.75f);      // doré chaud

    // Privées
    private Volume volumePostProcess;
    private Volume volumeFixe;
    private Vignette vignette;
    private Vignette vignetteFixe;
    private Bloom bloom;
    private ColorAdjustments colorAdjustments;
    private Camera mainCamera;
    private Transform dropTransform;
    private rollingCube dropScript;
    private float intensiteActuelle;

    [ContextMenu("Reset valeurs par defaut")]
    public void ResetValeurs()
    {
        intensiteDirectional = 1.4f;
        intensiteOriginale = 1.9f;
        vignetteIntensiteBase = 0.45f;
        vignetteSmoothness = 0.6f;
        intensiteEnMouvement = 0.4f;
        intensiteArret = 0.5f;
        vitesseTransition = 1.5f;
        pulsationAmplitude = 0.03f;
        pulsationVitesse = 0.4f;
        bloomIntensity = 1.2f;
        bloomThreshold = 0.9f;
        bloomScatter = 0.7f;
        vignetteFixeIntensity = 0.35f;
        vignetteFixeSmoothness = 0.7f;
        couleurVignette = new Color(0.3f, 0.15f, 0.05f);
        couleurFilter = new Color(1f, 0.92f, 0.75f);
    }

    void Awake() { ResetValeurs(); }

    void Start()
    {
        if (PlayerPrefs.GetInt("ModeZenActif", 0) == 0) { enabled = false; return; }
        AppliquerEffets();
    }

    void Update()
    {
        if (vignette == null || mainCamera == null || dropTransform == null) return;

        // ── Centre vignette suit Drop doucement ───────────────────────
        Vector3 screenPos = mainCamera.WorldToViewportPoint(dropTransform.position);

        // Pas de vacillement brusque — juste un léger flottement
        float flotX = Mathf.Sin(Time.time * 0.3f) * 0.008f;
        float flotY = Mathf.Cos(Time.time * 0.25f) * 0.008f;
        vignette.center.value = new Vector2(
            Mathf.Lerp(vignette.center.value.x, screenPos.x + flotX, Time.deltaTime * 2f),
            Mathf.Lerp(vignette.center.value.y, screenPos.y + flotY, Time.deltaTime * 2f));

        // ── Intensité vignette — respiration lente ────────────────────
        bool enMouvement = dropScript != null && dropScript._isMoving;
        float cible = enMouvement ? intensiteEnMouvement : intensiteArret;
        intensiteActuelle = Mathf.Lerp(intensiteActuelle, cible, Time.deltaTime * vitesseTransition);

        float pulsation = Mathf.Sin(Time.time * pulsationVitesse) * pulsationAmplitude;
        vignette.intensity.value = intensiteActuelle + pulsation;
    }

    private void AppliquerEffets()
    {
        if (directionalLight != null)
            directionalLight.intensity = intensiteDirectional;

        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            var cam = mainCamera.GetComponent<UniversalAdditionalCameraData>();
            if (cam != null) cam.renderPostProcessing = true;
        }

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            dropTransform = playerObj.transform;
            dropScript = playerObj.GetComponent<rollingCube>();
        }

        intensiteActuelle = vignetteIntensiteBase;

        // ── Volume dynamique ──────────────────────────────────────────
        GameObject vo = new GameObject("ZenVolume");
        volumePostProcess = vo.AddComponent<Volume>();
        volumePostProcess.isGlobal = true;
        volumePostProcess.priority = 10;

        VolumeProfile profile = ScriptableObject.CreateInstance<VolumeProfile>();

        // Vignette chaude
        vignette = profile.Add<Vignette>(true);
        vignette.active = true;
        vignette.intensity.value = vignetteIntensiteBase;
        vignette.smoothness.value = vignetteSmoothness;
        vignette.color.value = couleurVignette;
        vignette.rounded.value = true;
        vignette.center.value = new Vector2(0.5f, 0.5f);
        vignette.intensity.overrideState = true;
        vignette.smoothness.overrideState = true;
        vignette.color.overrideState = true;
        vignette.rounded.overrideState = true;
        vignette.center.overrideState = true;

        // Bloom doux
        bloom = profile.Add<Bloom>(true);
        bloom.active = true;
        bloom.intensity.value = bloomIntensity;
        bloom.threshold.value = bloomThreshold;
        bloom.scatter.value = bloomScatter;
        bloom.intensity.overrideState = true;
        bloom.threshold.overrideState = true;
        bloom.scatter.overrideState = true;

        // Filtre de couleur chaud/doré — absent dans Obscur, spécifique au Zen
        colorAdjustments = profile.Add<ColorAdjustments>(true);
        colorAdjustments.active = true;
        colorAdjustments.colorFilter.value = couleurFilter;
        colorAdjustments.colorFilter.overrideState = true;

        volumePostProcess.profile = profile;

        // ── Volume fixe ───────────────────────────────────────────────
        GameObject fo = new GameObject("ZenVolumeFixe");
        volumeFixe = fo.AddComponent<Volume>();
        volumeFixe.isGlobal = true;
        volumeFixe.priority = 9;

        VolumeProfile pf = ScriptableObject.CreateInstance<VolumeProfile>();
        vignetteFixe = pf.Add<Vignette>(true);
        vignetteFixe.active = true;
        vignetteFixe.intensity.value = vignetteFixeIntensity;
        vignetteFixe.smoothness.value = vignetteFixeSmoothness;
        vignetteFixe.color.value = couleurVignette;
        vignetteFixe.rounded.value = true;
        vignetteFixe.center.value = new Vector2(0.5f, 0.5f);
        vignetteFixe.intensity.overrideState = true;
        vignetteFixe.smoothness.overrideState = true;
        vignetteFixe.color.overrideState = true;
        vignetteFixe.rounded.overrideState = true;
        vignetteFixe.center.overrideState = true;
        volumeFixe.profile = pf;

        Debug.Log("[ZenVisuel] Mode Zen applique !");
    }

    void OnDestroy()
    {
        if (directionalLight != null)
            directionalLight.intensity = intensiteOriginale;
        if (mainCamera != null)
        {
            var cam = mainCamera.GetComponent<UniversalAdditionalCameraData>();
            if (cam != null) cam.renderPostProcessing = false;
        }
        if (volumePostProcess != null) Destroy(volumePostProcess.gameObject);
        if (volumeFixe != null) Destroy(volumeFixe.gameObject);
    }
}