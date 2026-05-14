using UnityEngine;
using UnityEngine.UI;

public class CRTOverlay : MonoBehaviour
{
    public static CRTOverlay Instance;

    private RawImage overlay;
    private Texture2D scanlineTexture;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject); // garde le CRTCanvas entier
        }
        else
        {
            Destroy(transform.root.gameObject);
            return;
        }

        overlay = GetComponent<RawImage>();
        CreateScanlineTexture();

        bool filtreActif = PlayerPrefs.GetInt("Filtre", 0) == 1;
        SetFiltre(filtreActif);
        Debug.Log($"[CRT] Instance créée — filtre : {filtreActif}");
    }

    void CreateScanlineTexture()
    {
        int height = 3;
        scanlineTexture = new Texture2D(1, height);
        scanlineTexture.filterMode = FilterMode.Point;
        scanlineTexture.wrapMode = TextureWrapMode.Repeat;

        for (int y = 0; y < height; y++)
        {
            Color c = (y % 2 == 0)
                ? new Color(0, 0, 0, 0.25f)
                : new Color(0, 0, 0, 0f);
            scanlineTexture.SetPixel(0, y, c);
        }
        scanlineTexture.Apply();

        overlay.texture = scanlineTexture;
        overlay.uvRect = new Rect(0, 0, 1, Screen.height / (float)height);
        Debug.Log($"[CRT] Texture créée");
    }

    public void SetFiltre(bool actif)
    {
        overlay.color = actif ? Color.white : new Color(0, 0, 0, 0);
        Debug.Log($"[CRT] Overlay : {actif}");
    }
}