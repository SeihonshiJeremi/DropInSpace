using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class OptionsScreenTutoOptions02 : MonoBehaviour
{
    public Toggle fullscreenTog, vsyncTog;
    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;
    public TMP_Text resolutionLabel;
    public AudioMixer theMixer;
    public TMP_Text mastLabel, musicLabel, sfxLabel;
    public Slider mastSlider, musicSlider, sfxSlider;
    public int activVsync;

    void Start()
    {
        UpdateResLabel();
        fullscreenTog.isOn = Screen.fullScreen;

        if (PlayerPrefs.HasKey("vsync"))
        {
            activVsync = PlayerPrefs.GetInt("vsync", activVsync);
            if (activVsync == 1)
            {
                QualitySettings.vSyncCount = 1;
                vsyncTog.isOn = true;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
                vsyncTog.isOn = false;
            }
        }

        // CORRIGÉ : rechargement des volumes sauvegardés
        if (PlayerPrefs.HasKey("MasterVol"))
            mastSlider.value = PlayerPrefs.GetFloat("MasterVol");
        if (PlayerPrefs.HasKey("MusicVolume"))
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        if (PlayerPrefs.HasKey("SFXvolume"))
            sfxSlider.value = PlayerPrefs.GetFloat("SFXvolume");

        float vol = 0f;
        theMixer.GetFloat("MasterVol", out vol);
        mastSlider.value = vol;
        theMixer.GetFloat("MusicVolume", out vol);
        musicSlider.value = vol;
        theMixer.GetFloat("SFXvolume", out vol);
        sfxSlider.value = vol;

        mastLabel.text = Mathf.RoundToInt(mastSlider.value + 80).ToString();
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
    }

    // CORRIGÉ : Update supprimé (contenait uniquement des Debug.Log)

    public void ResLeft()
    {
        selectedResolution = Mathf.Max(0, selectedResolution - 1);
        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution = Mathf.Min(resolutions.Count - 1, selectedResolution + 1);
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal + " x " + resolutions[selectedResolution].vertical;
    }

    public void ApplyGraphics()
    {
        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
            activVsync = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            activVsync = 0;
        }
        PlayerPrefs.SetInt("vsync", activVsync);
        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
    }

    public void SetMasterVol()
    {
        mastLabel.text = Mathf.RoundToInt(mastSlider.value + 80).ToString();
        theMixer.SetFloat("MasterVol", mastSlider.value);
        PlayerPrefs.SetFloat("MasterVol", mastSlider.value);
    }

    public void SetMusicVol()
    {
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        theMixer.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void SetSFXVol()
    {
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
        theMixer.SetFloat("SFXvolume", sfxSlider.value);
        PlayerPrefs.SetFloat("SFXvolume", sfxSlider.value);
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}