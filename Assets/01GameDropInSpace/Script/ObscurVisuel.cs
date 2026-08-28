using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ObscurVisuel : MonoBehaviour
{
    [Header("Lumieres — assigne dans l'Inspector")]
    public Light directionalLight;

    [Header("Lumieres")]
    public float intensiteDirectional = 1.8f;
    public float intensiteOriginale = 1.9f;

    [Header("Vignette dynamique — suit Drop")]
    public float vignetteIntensiteBase = 0.999f;
    public float vignetteSmoothness = 0.3f;
    public float intensiteEnMouvement = 0.9995f;
    public float intensiteArret = 0.999f;
    public float vitesseTransition = 3f;

    [Header("Pulsation")]
    public float pulsationAmplitude = 0.006f;
    public float pulsationVitesse = 1.2f;

    [Header("Vacillement")]
    public float vacillementAmplitude = 0.015f;
    public float vacillementVitesse = 8f;

    [Header("Bloom")]
    public float bloomIntensity = 2f;
    public float bloomThreshold = 0.8f;
    public float bloomScatter = 0.5f;

    [Header("Vignette fixe")]
    public float vignetteFixeIntensity = 0.95f;
    public float vignetteFixeSmoothness = 0.5f;

    [Header("Ecran noir")]
    public float intervalleEcranNoir = 10f;
    public float dureeEcranNoir = 2f;
    public float dureeFondu = 0.5f;

    // Privees
    private Volume volumePostProcess;
    private Volume volumeFixe;
    private Vignette vignette;
    private Vignette vignetteFixe;
    private Bloom bloom;
    private Camera mainCamera;
    private Transform dropTransform;
    private rollingCube dropScript;
    private float intensiteActuelle;

    // Ecran noir
    private float timerEcranNoir = 10f;
    private bool ecranNoirActif = false;
    private float timerEcranCourant = 0f;
    private UnityEngine.UI.Image panneauImage;

    [ContextMenu("Reset valeurs par defaut")]
    public void ResetValeurs()
    {
        intensiteDirectional = 1.8f;
        intensiteOriginale = 1.9f;
        vignetteIntensiteBase = 0.999f;
        vignetteSmoothness = 0.3f;
        intensiteEnMouvement = 0.9995f;
        intensiteArret = 0.999f;
        vitesseTransition = 3f;
        pulsationAmplitude = 0.006f;
        pulsationVitesse = 1.2f;
        vacillementAmplitude = 0.015f;
        vacillementVitesse = 8f;
        bloomIntensity = 2f;
        bloomThreshold = 0.8f;
        bloomScatter = 0.5f;
        vignetteFixeIntensity = 0.95f;
        vignetteFixeSmoothness = 0.5f;
        intervalleEcranNoir = 10f;
        dureeEcranNoir = 2f;
        dureeFondu = 0.5f;
    }

    void Awake() { ResetValeurs(); }

    void Start()
    {
        if (PlayerPrefs.GetInt("ModeObscurActif", 0) == 0) { enabled = false; return; }
        AppliquerEffets();
        timerEcranNoir = intervalleEcranNoir;
    }

    void Update()
    {
        if (vignette == null || mainCamera == null || dropTransform == null) return;

        // ── Centre vignette suit Drop ──────────────────────────────────
        float paniqueMultiplier = 1f;
        if (zoomManager.Instance != null)
        {
            float timer = zoomManager.Instance.timerNiveau;
            if (timer < 20f) paniqueMultiplier = Mathf.Lerp(5f, 1f, timer / 20f);
        }

        Vector3 screenPos = mainCamera.WorldToViewportPoint(dropTransform.position);
        float vacX = Mathf.PerlinNoise(Time.time * vacillementVitesse * paniqueMultiplier, 0f) - 0.5f;
        float vacY = Mathf.PerlinNoise(0f, Time.time * vacillementVitesse * paniqueMultiplier) - 0.5f;
        vignette.center.value = new Vector2(
            screenPos.x + vacX * vacillementAmplitude * paniqueMultiplier,
            screenPos.y + vacY * vacillementAmplitude * paniqueMultiplier);

        // ── Ecran noir ─────────────────────────────────────────────────
        timerEcranNoir -= Time.deltaTime;
        if (timerEcranNoir <= 0f && !ecranNoirActif)
        {
            ecranNoirActif = true;
            timerEcranCourant = dureeEcranNoir;
        }

        if (ecranNoirActif)
        {
            timerEcranCourant -= Time.deltaTime;

            float alpha = 0f;
            if (timerEcranCourant > dureeEcranNoir - dureeFondu)
                alpha = 1f - ((timerEcranCourant - (dureeEcranNoir - dureeFondu)) / dureeFondu);
            else if (timerEcranCourant < dureeFondu)
                alpha = timerEcranCourant / dureeFondu;
            else
                alpha = 1f;

            // Panneau noir progressif
            if (panneauImage != null)
                panneauImage.color = new Color(0f, 0f, 0f, alpha);

            if (timerEcranCourant <= 0f)
            {
                ecranNoirActif = false;
                timerEcranNoir = intervalleEcranNoir;
                if (panneauImage != null)
                    panneauImage.color = new Color(0f, 0f, 0f, 0f);
            }
            // Pas de return — vignette continue de suivre Drop
        }

        // ── Intensite vignette ─────────────────────────────────────────
        bool enMouvement = dropScript != null && dropScript._isMoving;
        float cible = enMouvement ? intensiteEnMouvement : intensiteArret;
        intensiteActuelle = Mathf.Lerp(intensiteActuelle, cible, Time.deltaTime * vitesseTransition);

        float pulsation = Mathf.Sin(Time.time * pulsationVitesse * paniqueMultiplier)
                                   * pulsationAmplitude * paniqueMultiplier;
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

        // Volume dynamique
        GameObject vo = new GameObject("ObscurVolume");
        volumePostProcess = vo.AddComponent<Volume>();
        volumePostProcess.isGlobal = true;
        volumePostProcess.priority = 10;

        VolumeProfile profile = ScriptableObject.CreateInstance<VolumeProfile>();
        vignette = profile.Add<Vignette>(true);
        vignette.active = true;
        vignette.intensity.value = vignetteIntensiteBase;
        vignette.smoothness.value = vignetteSmoothness;
        vignette.color.value = Color.black;
        vignette.rounded.value = true;
        vignette.center.value = new Vector2(0.5f, 0.5f);
        vignette.intensity.overrideState = true;
        vignette.smoothness.overrideState = true;
        vignette.color.overrideState = true;
        vignette.rounded.overrideState = true;
        vignette.center.overrideState = true;

        bloom = profile.Add<Bloom>(true);
        bloom.active = true;
        bloom.intensity.value = bloomIntensity;
        bloom.threshold.value = bloomThreshold;
        bloom.scatter.value = bloomScatter;
        bloom.intensity.overrideState = true;
        bloom.threshold.overrideState = true;
        bloom.scatter.overrideState = true;
        volumePostProcess.profile = profile;

        // Volume fixe
        GameObject fo = new GameObject("ObscurVolumeFixe");
        volumeFixe = fo.AddComponent<Volume>();
        volumeFixe.isGlobal = true;
        volumeFixe.priority = 9;

        VolumeProfile pf = ScriptableObject.CreateInstance<VolumeProfile>();
        vignetteFixe = pf.Add<Vignette>(true);
        vignetteFixe.active = true;
        vignetteFixe.intensity.value = vignetteFixeIntensity;
        vignetteFixe.smoothness.value = vignetteFixeSmoothness;
        vignetteFixe.color.value = Color.black;
        vignetteFixe.rounded.value = true;
        vignetteFixe.center.value = new Vector2(0.5f, 0.5f);
        vignetteFixe.intensity.overrideState = true;
        vignetteFixe.smoothness.overrideState = true;
        vignetteFixe.color.overrideState = true;
        vignetteFixe.rounded.overrideState = true;
        vignetteFixe.center.overrideState = true;
        volumeFixe.profile = pf;

        // Panneau noir UI
        GameObject canvas = new GameObject("ObscurCanvas");
        Canvas c = canvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        c.sortingOrder = 100;
        canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        GameObject panneau = new GameObject("PanneauNoir");
        panneau.transform.SetParent(canvas.transform, false);
        panneauImage = panneau.AddComponent<UnityEngine.UI.Image>();
        panneauImage.color = new Color(0f, 0f, 0f, 0f); // Transparent au depart
        RectTransform rt = panneau.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        Debug.Log("[ObscurVisuel] Mode Obscur applique !");
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