using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio")]
    public AudioMixer audioMixer;
    public AudioSource musicSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Coupe la musique AVANT qu'elle joue si volume à zéro
        float musique = PlayerPrefs.GetFloat("Musique", 0.75f);
        if (musicSource != null && musique <= 0.001f)
        {
            musicSource.volume = 0f;
            Debug.Log("[AudioManager] Musique coupée au démarrage");
        }
    }

    void Start()
    {
        ApplyAllSettings();
    }

    public void ApplyAllSettings()
    {
        ApplyVolume(PlayerPrefs.GetFloat("Volume", 0.75f));
        ApplyMusique(PlayerPrefs.GetFloat("Musique", 0.75f));
        ApplyEffets(PlayerPrefs.GetFloat("Effets", 0.75f));
        Debug.Log($"[AudioManager] Settings appliqués");
    }

    public void ApplyVolume(float value)
    {
        if (audioMixer != null)
        {
            float db = value > 0.001f ? Mathf.Log10(value) * 20 : -80f;
            audioMixer.SetFloat("Volume", db);
            Debug.Log($"[AudioManager] Volume db : {db}");
        }
        else Debug.LogWarning("[AudioManager] AudioMixer NULL !");
    }

    public void ApplyMusique(float value)
    {
        if (audioMixer != null)
        {
            float db = value > 0.001f ? Mathf.Log10(value) * 20 : -80f;
            audioMixer.SetFloat("Musique", db);
        }
        if (musicSource != null)
            musicSource.volume = value;
        Debug.Log($"[AudioManager] Musique : {value}");
    }

    public void ApplyEffets(float value)
    {
        if (audioMixer != null)
        {
            float db = value > 0.001f ? Mathf.Log10(value) * 20 : -80f;
            audioMixer.SetFloat("Effets", db);
            Debug.Log($"[AudioManager] Effets db : {db}");
        }
    }
}