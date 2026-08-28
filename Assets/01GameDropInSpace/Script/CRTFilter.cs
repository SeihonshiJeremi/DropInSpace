using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CRTFilter : MonoBehaviour
{
    public static CRTFilter Instance;

    private PostProcessVolume volume;
    private Vignette vignette;
    private Grain grain;

    void Awake()
    {
        Instance = this;
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out vignette);
        volume.profile.TryGetSettings(out grain);

        // Charge l'état sauvegardé
        bool filtreActif = PlayerPrefs.GetInt("Filtre", 0) == 1;
        SetFiltre(filtreActif);
    }

    public void SetFiltre(bool actif)
    {
        if (vignette != null) vignette.active = actif;
        if (grain != null) grain.active = actif;
        Debug.Log($"[CRT] Filtre : {actif}");
    }
}