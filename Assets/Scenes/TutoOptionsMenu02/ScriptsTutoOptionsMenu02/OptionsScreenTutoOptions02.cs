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

    // Start is called before the first frame update
    void Start()
    {
        UpdateResLabel();//rejouter apres supp reso auto
        fullscreenTog.isOn = Screen.fullScreen;

        /*if(QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        } else
        {
            vsyncTog.isOn = true;
        }*/

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



        //UpdateResLabel();

        /* bool foundRes = false;
         for (int i = 0; i < resolutions.Count; i++)
         {
             if(Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
             {
                 foundRes = true;

                 selectedResolution = i;

                 UpdateResLabel();
             }
         }

         if(!foundRes)
         {
             ResItem newRes = new ResItem();
             newRes.horizontal = Screen.width;
             newRes.vertical = Screen.height;

             resolutions.Add(newRes);
             selectedResolution = resolutions.Count - 1;

             UpdateResLabel();
         }*/

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

    // Update is called once per frame
    void Update()
    {
        if (QualitySettings.vSyncCount == 0) Debug.Log("vsyns false");
        if (QualitySettings.vSyncCount == 1) Debug.Log("vsyns true");
    }

    public void ResLeft()
    {
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = 0;
        }

        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }

        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGrpahics()
    {
        //Screen.fullScreen = fullscreenTog.isOn;

        if(vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
            activVsync = 1;
            PlayerPrefs.SetInt("vsync", activVsync);
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            activVsync = 0;
            PlayerPrefs.SetInt("vsync", activVsync);
        }

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
