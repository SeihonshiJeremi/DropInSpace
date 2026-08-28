using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerTutoOptions02 : MonoBehaviour
{
    public AudioMixer theMixer;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("MasterVol"))
        {
            theMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            theMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        }

        if (PlayerPrefs.HasKey("SFXvolume"))
        {
            theMixer.SetFloat("SFXvolume", PlayerPrefs.GetFloat("SFXvolume"));
        }
    }


}
