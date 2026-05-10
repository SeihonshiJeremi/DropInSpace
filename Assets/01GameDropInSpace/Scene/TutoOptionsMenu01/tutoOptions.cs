using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class tutoOptions : MonoBehaviour
{
    // Son
    public AudioMixer mainMixer;
    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("volume", volume);
    }

    // Fullscreen
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // Quality
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // Résolutions
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolution;
    private int currentResolutionIndex = 0;

    void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolution = new List<Resolution>();
        resolutionDropdown.ClearOptions();

        // CORRIGÉ : refreshRateRatio au lieu de refreshRate (obsolète)
        double currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                filteredResolution.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolution.Count; i++)
        {
            // CORRIGÉ : refreshRateRatio.value pour affichage
            string resolutionOption = filteredResolution[i].width + "x" + filteredResolution[i].height
                + " " + Mathf.RoundToInt((float)filteredResolution[i].refreshRateRatio.value) + "Hz";
            options.Add(resolutionOption);

            if (filteredResolution[i].width == Screen.width && filteredResolution[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        // CORRIGÉ : Mathf.Max(options.Count) sans argument = 0, probablement voulu = dernière option
        resolutionDropdown.value = options.Count > 0 ? options.Count - 1 : 0;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolution[resolutionIndex];
        // CORRIGÉ : Screen.fullScreen = Screen.fullScreen redondant, simplifié
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}