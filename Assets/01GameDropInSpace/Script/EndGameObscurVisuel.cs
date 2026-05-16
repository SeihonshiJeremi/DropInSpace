using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Vignette pour SceneTransitionEndGame quand on vient du mode Obscur.
/// Se desactive automatiquement si VictoireObscur != 1.
/// </summary>
public class EndGameObscurVisuel : MonoBehaviour
{
    [Header("Lumieres")]
    public Light directionalLight;
    public float intensiteDirectional = 0.3f;

    private Volume volumePostProcess;
    private Volume volumeFixe;
    private Vignette vignette;
    private Vignette vignetteFixe;
    private Camera mainCamera;
    private Transform dropTransform;
    private float intensiteActuelle = 0.999f;

    void Start()
    {
        // Seulement si on vient du mode Obscur
        if (PlayerPrefs.GetInt("VictoireObscur", 0) == 0)
        {
            enabled = false;
            return;
        }

        // Remet le flag a 0
        PlayerPrefs.SetInt("VictoireObscur", 0);
        PlayerPrefs.SetInt("ModeObscurActif", 0);
        PlayerPrefs.Save();

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
        GameObject vo = new GameObject("EndGameVolume");
        volumePostProcess = vo.AddComponent<Volume>();
        volumePostProcess.isGlobal = true;
        volumePostProcess.priority = 10;

        VolumeProfile profile = ScriptableObject.CreateInstance<VolumeProfile>();
        vignette = profile.Add<Vignette>(true);
        vignette.active = true;
        vignette.intensity.value = 0.999f;
        vignette.smoothness.value = 0.3f;
        vignette.color.value = Color.black;
        vignette.rounded.value = true;
        vignette.center.value = new Vector2(0.5f, 0.5f);
        vignette.intensity.overrideState = true;
        vignette.smoothness.overrideState = true;
        vignette.color.overrideState = true;
        vignette.rounded.overrideState = true;
        vignette.center.overrideState = true;
        volumePostProcess.profile = profile;

        // Volume fixe
        GameObject fo = new GameObject("EndGameVolumeFixe");
        volumeFixe = fo.AddComponent<Volume>();
        volumeFixe.isGlobal = true;
        volumeFixe.priority = 9;

        VolumeProfile pf = ScriptableObject.CreateInstance<VolumeProfile>();
        vignetteFixe = pf.Add<Vignette>(true);
        vignetteFixe.active = true;
        vignetteFixe.intensity.value = 0.95f;
        vignetteFixe.smoothness.value = 0.5f;
        vignetteFixe.color.value = Color.black;
        vignetteFixe.rounded.value = true;
        vignetteFixe.center.value = new Vector2(0.5f, 0.5f);
        vignetteFixe.intensity.overrideState = true;
        vignetteFixe.smoothness.overrideState = true;
        vignetteFixe.color.overrideState = true;
        vignetteFixe.rounded.overrideState = true;
        vignetteFixe.center.overrideState = true;
        volumeFixe.profile = pf;

        Debug.Log("[EndGameObscurVisuel] Vignette EndGame Obscur appliquee !");
    }

    void Update()
    {
        if (vignette == null || mainCamera == null || dropTransform == null) return;

        Vector3 screenPos = mainCamera.WorldToViewportPoint(dropTransform.position);
        float vacX = Mathf.PerlinNoise(Time.time * 8f, 0f) - 0.5f;
        float vacY = Mathf.PerlinNoise(0f, Time.time * 8f) - 0.5f;
        vignette.center.value = new Vector2(
            screenPos.x + vacX * 0.015f,
            screenPos.y + vacY * 0.015f);

        intensiteActuelle = Mathf.Lerp(intensiteActuelle, 0.999f, Time.deltaTime * 3f);
        float pulsation = Mathf.Sin(Time.time * 1.2f) * 0.006f;
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