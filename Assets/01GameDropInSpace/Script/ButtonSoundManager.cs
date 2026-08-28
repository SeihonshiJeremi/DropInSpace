using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Gère les effets sonores des boutons — ROBUSTE aux changements de scène.
/// À placer sur le MÊME GameObject que AudioManager (qui a déjà DontDestroyOnLoad).
/// </summary>
public class ButtonSoundManager : MonoBehaviour
{
    public static ButtonSoundManager Instance;

    [Header("Audio Clips")]
    public AudioClip soundHover;
    public AudioClip soundClick;

    [Header("Volumes")]
    [Range(0f, 1f)] public float hoverVolume = 0.5f;
    [Range(0f, 1f)] public float clickVolume = 0.8f;

    private AudioSource _hoverSource;
    private AudioSource _clickSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Ajoute les AudioSources DIRECTEMENT sur ce GameObject
            // Pas d'objets enfants = pas de problème de destruction
            _hoverSource = gameObject.GetComponent<AudioSource>();
            if (_hoverSource == null)
                _hoverSource = gameObject.AddComponent<AudioSource>();

            _clickSource = gameObject.AddComponent<AudioSource>();

            _hoverSource.playOnAwake = false;
            _clickSource.playOnAwake = false;
            _hoverSource.spatialBlend = 0f;
            _clickSource.spatialBlend = 0f;

            // DontDestroyOnLoad est déjà géré par AudioManager
            // mais on l'ajoute quand même au cas où
            DontDestroyOnLoad(gameObject);

            Debug.Log("[ButtonSoundManager] Initialisé avec AudioSources locales");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Vérifie les références après chaque chargement de scène
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-vérifie que les AudioSources sont toujours là
        if (_hoverSource == null)
        {
            _hoverSource = gameObject.AddComponent<AudioSource>();
            _hoverSource.playOnAwake = false;
            _hoverSource.spatialBlend = 0f;
            Debug.Log("[ButtonSoundManager] Hover AudioSource recréée");
        }

        if (_clickSource == null)
        {
            _clickSource = gameObject.AddComponent<AudioSource>();
            _clickSource.playOnAwake = false;
            _clickSource.spatialBlend = 0f;
            Debug.Log("[ButtonSoundManager] Click AudioSource recréée");
        }
    }

    public void PlayHover()
    {
        if (_hoverSource != null && soundHover != null)
        {
            _hoverSource.PlayOneShot(soundHover, hoverVolume);
        }
        else
        {
            Debug.LogWarning($"[ButtonSoundManager] Hover KO — source={_hoverSource != null}, clip={soundHover != null}");
        }
    }

    public void PlayClick()
    {
        if (_clickSource != null && soundClick != null)
        {
            _clickSource.PlayOneShot(soundClick, clickVolume);
        }
        else
        {
            Debug.LogWarning($"[ButtonSoundManager] Click KO — source={_clickSource != null}, clip={soundClick != null}");
        }
    }
}