using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsManager : MonoBehaviour
{
    [Header("Toggles")]
    public CoolToggle togglePleinEcran;
    public CoolToggle toggleVibrations;
    public CoolToggle toggleControles;
    public CoolToggle toggleFiltre;

    [Header("Sliders")]
    public Slider sliderVolume;
    public Slider sliderMusique;
    public Slider sliderEffets;
    public Slider sliderLuminosite;

    [Header("Controles Overlay")]
    public GameObject controlesOverlay;
    public GameObject btnFermerControles;

    [Header("Scroll")]
    public ScrollRect scrollRect;

    [Header("Boutons par defaut")]
    public GameObject btnLangue;
    public GameObject btnBackOptions;

    private SelectionManager selectionManager;

    void OnEnable()
    {
        selectionManager = Object.FindFirstObjectByType<SelectionManager>();

        // Reset couleur de tous les boutons Options au démarrage
        foreach (var btn in GetComponentsInChildren<Button>())
        {
            Image img = btn.GetComponent<Image>();
            if (img != null)
                img.color = new Color(1f, 1f, 1f, 0.5f);
        }

        controlesOverlay.SetActive(false);
        toggleControles.isOn = false;
        scrollRect.enabled = true;
        if (selectionManager != null)
            selectionManager.navigationBloquee = false;

        togglePleinEcran.isOn = PlayerPrefs.GetInt("PleinEcran", 1) == 1;
        toggleVibrations.isOn = PlayerPrefs.GetInt("Vibrations", 1) == 1;
        toggleFiltre.isOn = PlayerPrefs.GetInt("Filtre", 0) == 1;

        sliderVolume.value = PlayerPrefs.GetFloat("Volume", 0.75f);
        sliderMusique.value = PlayerPrefs.GetFloat("Musique", 0.75f);
        sliderEffets.value = PlayerPrefs.GetFloat("Effets", 0.75f);
        sliderLuminosite.value = PlayerPrefs.GetFloat("Luminosite", 0.75f);

        sliderVolume.onValueChanged.RemoveAllListeners();
        sliderMusique.onValueChanged.RemoveAllListeners();
        sliderEffets.onValueChanged.RemoveAllListeners();
        sliderLuminosite.onValueChanged.RemoveAllListeners();

        sliderVolume.onValueChanged.AddListener(val => {
            AudioManager.Instance?.ApplyVolume(val);
            PlayerPrefs.SetFloat("Volume", val);
            PlayerPrefs.Save();
        });
        sliderMusique.onValueChanged.AddListener(val => {
            AudioManager.Instance?.ApplyMusique(val);
            PlayerPrefs.SetFloat("Musique", val);
            PlayerPrefs.Save();
        });
        sliderEffets.onValueChanged.AddListener(val => {
            AudioManager.Instance?.ApplyEffets(val);
            PlayerPrefs.SetFloat("Effets", val);
            PlayerPrefs.Save();
        });
        sliderLuminosite.onValueChanged.AddListener(val => {
            LuminositeOverlay.Instance?.ApplyLuminosite(val);
            PlayerPrefs.SetFloat("Luminosite", val);
            PlayerPrefs.Save();
        });

        Debug.Log("[OptionsManager] Panel ouvert — valeurs chargées");
    }

    void SetSelection(GameObject bouton)
    {
        if (selectionManager != null)
        {
            selectionManager.premierBouton = bouton;
            EventSystem.current.SetSelectedGameObject(bouton);
        }
    }

    public void OnPleinEcranClick()
    {
        togglePleinEcran.Toggle();
        Screen.fullScreen = togglePleinEcran.isOn;
        PlayerPrefs.SetInt("PleinEcran", togglePleinEcran.isOn ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log($"[Options] Plein écran : {togglePleinEcran.isOn}");
    }

    public void OnVibrationsClick()
    {
        toggleVibrations.Toggle();
        PlayerPrefs.SetInt("Vibrations", toggleVibrations.isOn ? 1 : 0);
        PlayerPrefs.Save();
        VibrationsManager.Instance?.SetVibrations(toggleVibrations.isOn);
        if (toggleVibrations.isOn)
            VibrationsManager.Instance?.Vibrate(0.5f, 0.5f, 0.3f);
        Debug.Log($"[Options] Vibrations : {toggleVibrations.isOn}");
    }

    public void OnControlesClick()
    {
        toggleControles.Toggle();
        controlesOverlay.SetActive(false);
        controlesOverlay.SetActive(toggleControles.isOn);
        if (toggleControles.isOn)
        {
            scrollRect.enabled = false;
            if (selectionManager != null)
                selectionManager.navigationBloquee = true;
            SetSelection(btnFermerControles);
        }
        else
        {
            scrollRect.enabled = true;
            if (selectionManager != null)
                selectionManager.navigationBloquee = false;
            SetSelection(btnLangue);
        }
        Debug.Log($"[Options] Controles : {toggleControles.isOn}");
    }

    public void OnFermerControlesClick()
    {
        toggleControles.isOn = false;
        controlesOverlay.SetActive(false);
        scrollRect.enabled = true;
        if (selectionManager != null)
        {
            selectionManager.navigationBloquee = false;
            selectionManager.ForceDiamonds(btnFermerControles, false);
            selectionManager.ForceDiamonds(btnBackOptions, false);
        }

        // Reset couleur Image BtnBackOptions
        Image imgBack = btnBackOptions.GetComponent<Image>();
        if (imgBack != null)
            imgBack.color = new Color(1f, 1f, 1f, 0.5f);

        Canvas.ForceUpdateCanvases();
        SetSelection(btnLangue);
        Debug.Log("[Options] Controles fermé");
    }

    public void OnFiltreClick()
    {
        toggleFiltre.Toggle();
        PlayerPrefs.SetInt("Filtre", toggleFiltre.isOn ? 1 : 0);
        PlayerPrefs.Save();
        CRTOverlay.Instance?.SetFiltre(toggleFiltre.isOn);
        Debug.Log($"[Options] Filtre : {toggleFiltre.isOn}");
    }

    public void OnResetScoreClick()
    {
        PlayerPrefs.DeleteKey("Score");
        PlayerPrefs.Save();
        Debug.Log("[Options] Score reset !");
    }

    public void OnResetProgressionClick()
    {
        PlayerPrefs.DeleteKey("Progression");
        PlayerPrefs.Save();
        Debug.Log("[Options] Progression reset !");
    }
}