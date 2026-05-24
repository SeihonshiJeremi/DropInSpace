using UnityEngine;

public class CubeMusicManager : MonoBehaviour
{
    public static CubeMusicManager Instance;

    [Header("Notes de la gamme — Do pentatonique")]
    private float[] notes = new float[]
    {
        261.63f, // Do4
        293.66f, // Ré4
        329.63f, // Mi4
        392.00f, // Sol4
        440.00f, // La4
        523.25f, // Do5
        587.33f, // Ré5
        659.25f, // Mi5
        784.00f, // Sol5
        880.00f, // La5
    };

    [Header("Son")]
    public AudioClip noteSon; // un son court et doux — bell ou xylophone
    public float volumeBase = 0.4f;
    public float volumeMax = 1.0f;

    private int comboActuel = 0;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void JouerNoteCube()
    {
        // Choisit la note selon le combo — monte dans la gamme
        int index = Mathf.Clamp(comboActuel, 0, notes.Length - 1);
        float freq = notes[index];

        // Volume qui monte avec le combo
        float volume = Mathf.Lerp(volumeBase, volumeMax, (float)comboActuel / notes.Length);

        // Crée un AudioSource temporaire avec la bonne fréquence
        JouerFrequence(freq, volume);

        comboActuel++;
    }

    public void ResetCombo()
    {
        comboActuel = 0;
    }

    void JouerFrequence(float freq, float volume)
    {
        // Utilise un oscillateur simple pour jouer la fréquence
        var go = new GameObject("NoteTemp");
        var src = go.AddComponent<AudioSource>();

        if (noteSon != null)
        {
            // Si tu as un AudioClip — joue-le avec pitch ajusté
            src.clip = noteSon;
            src.pitch = freq / 261.63f; // pitch relatif à Do4
            src.volume = volume;
            src.Play();
            Destroy(go, noteSon.length + 0.1f);
        }
        else
        {
            // Pas de clip — joue une sinusoïde pure
            StartCoroutine(JouerSinusoide(src, go, freq, volume));
        }
    }

    System.Collections.IEnumerator JouerSinusoide(AudioSource src, GameObject go, float freq, float volume)
    {
        int sampleRate = AudioSettings.outputSampleRate;
        int samples = sampleRate / 4; // 0.25 secondes
        float[] data = new float[samples];

        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float env = 1f - (float)i / samples; // fondu sortant
            data[i] = Mathf.Sin(2 * Mathf.PI * freq * t) * env * volume;
        }

        AudioClip clip = AudioClip.Create("note", samples, 1, sampleRate, false);
        clip.SetData(data, 0);
        src.clip = clip;
        src.Play();

        yield return new WaitForSeconds(0.3f);
        Destroy(go);
    }
}