using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Effets visuels mode Obscur pour la scene de transition.
/// Tous les effets d'ObscurVisuel — s'active toujours sans verifier ModeObscurActif.
/// </summary>
public class ObscurTransitionVisuel : MonoBehaviour
{
    [Header("Lumieres")]
    public Light directionalLight;
    public float intensiteDirectional = 0.3f;

    // Valeurs identiques a ObscurVisuel
    private const float VIGNETTE_BASE = 0.999f;
    private const float VIGNETTE_SMOOTH = 0.3f;
    private const float VIGNETTE_FIXE = 0.95f;
    private const float VIGNETTE_FIXE_SM = 0.5f;
    private const float PULSATION_AMP = 0.006f;
    private const float PULSATION_VIT = 1.2f;
    private const float VACILLEMENT_AMP = 0.015f;
    private const float VACILLEMENT_VIT = 8f;
    private const float BLOOM_INTENSITY = 2f;
    private const float BLOOM_THRESHOLD = 0.8f;
    private const float BLOOM_SCATTER = 0.5f;
    private const float INTENSITE_MOUV = 0.9995f;
    private const float INTENSITE_ARRET = 0.999f;
    private const float VITESSE_TRANSITION = 3f;

    private Volume volumePostProcess;
    private Volume volumeFixe;
    private Vignette vignette;
    private Vignette vignetteFixe;
    private Bloom bloom;
    private Camera mainCamera;
    private Transform dropTransform;
    private float intensiteActuelle = VIGNETTE_BASE;

    void Start()
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
            dropTransform = playerObj.transform;

        // Volume dynamique
        GameObject vo = new GameObject("TransitionVolume");
        volumePostProcess = vo.AddComponent<Volume>();
        volumePostProcess.isGlobal = true;
        volumePostProcess.priority = 10;

        VolumeProfile profile = ScriptableObject.CreateInstance<VolumeProfile>();
        vignette = profile.Add<Vignette>(true);
        vignette.active = true;
        vignette.intensity.value = VIGNETTE_BASE;
        vignette.smoothness.value = VIGNETTE_SMOOTH;
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
        bloom.intensity.value = BLOOM_INTENSITY;
        bloom.threshold.value = BLOOM_THRESHOLD;
        bloom.scatter.value = BLOOM_SCATTER;
        bloom.intensity.overrideState = true;
        bloom.threshold.overrideState = true;
        bloom.scatter.overrideState = true;
        volumePostProcess.profile = profile;

        // Volume fixe
        GameObject fo = new GameObject("TransitionVolumeFixe");
        volumeFixe = fo.AddComponent<Volume>();
        volumeFixe.isGlobal = true;
        volumeFixe.priority = 9;

        VolumeProfile pf = ScriptableObject.CreateInstance<VolumeProfile>();
        vignetteFixe = pf.Add<Vignette>(true);
        vignetteFixe.active = true;
        vignetteFixe.intensity.value = VIGNETTE_FIXE;
        vignetteFixe.smoothness.value = VIGNETTE_FIXE_SM;
        vignetteFixe.color.value = Color.black;
        vignetteFixe.rounded.value = true;
        vignetteFixe.center.value = new Vector2(0.5f, 0.5f);
        vignetteFixe.intensity.overrideState = true;
        vignetteFixe.smoothness.overrideState = true;
        vignetteFixe.color.overrideState = true;
        vignetteFixe.rounded.overrideState = true;
        vignetteFixe.center.overrideState = true;
        volumeFixe.profile = pf;
    }

    void Update()
    {
        if (vignette == null || mainCamera == null || dropTransform == null) return;

        // Centre suit Drop
        Vector3 screenPos = mainCamera.WorldToViewportPoint(dropTransform.position);
        float vacX = Mathf.PerlinNoise(Time.time * VACILLEMENT_VIT, 0f) - 0.5f;
        float vacY = Mathf.PerlinNoise(0f, Time.time * VACILLEMENT_VIT) - 0.5f;
        vignette.center.value = new Vector2(
            screenPos.x + vacX * VACILLEMENT_AMP,
            screenPos.y + vacY * VACILLEMENT_AMP);

        // Intensite
        intensiteActuelle = Mathf.Lerp(intensiteActuelle, INTENSITE_ARRET,
                                       Time.deltaTime * VITESSE_TRANSITION);

        float pulsation = Mathf.Sin(Time.time * PULSATION_VIT) * PULSATION_AMP;
        vignette.intensity.value = intensiteActuelle + pulsation;
    }

    void OnDestroy()
    {
        if (mainCamera != null)
        {
            var cam = mainCamera.GetComponent<UniversalAdditionalCameraData>();
            if (cam != null) cam.renderPostProcessing = false;
        }
        if (volumePostProcess != null) Destroy(volumePostProcess.gameObject);
        if (volumeFixe != null) Destroy(volumeFixe.gameObject);
    }
}