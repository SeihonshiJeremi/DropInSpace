using UnityEngine;
using UnityEngine.UI;

public class LuminositeOverlay : MonoBehaviour
{
    public static LuminositeOverlay Instance;

    public Image overlay;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(transform.root.gameObject);
            return;
        }

        if (overlay == null)
        {
            Debug.LogError("[Luminosite] Image non assignée !");
            return;
        }

        float lum = PlayerPrefs.GetFloat("Luminosite", 0.75f);
        ApplyLuminosite(lum);
        Debug.Log($"[Luminosite] Chargée : {lum}");
    }

    public void ApplyLuminosite(float value)
    {
        if (overlay == null) return;

        if (value < 0.75f)
        {
            float alpha = Mathf.InverseLerp(0.75f, 0f, value) * 0.8f;
            overlay.color = new Color(0, 0, 0, alpha);
        }
        else
        {
            float alpha = Mathf.InverseLerp(0.75f, 1f, value) * 0.4f;
            overlay.color = new Color(1, 1, 1, alpha);
        }
        Debug.Log($"[Luminosite] Applied : {value}");
    }
}