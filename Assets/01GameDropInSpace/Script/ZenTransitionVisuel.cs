using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ZenTransitionVisuel : MonoBehaviour
{
    public Volume volumeTransition;

    void Start()
    {
        if (PlayerPrefs.GetInt("ModeZenActif", 0) != 1)
        {
            gameObject.SetActive(false);
            return;
        }

        if (volumeTransition != null && volumeTransition.profile != null)
        {
            if (volumeTransition.profile.TryGet(out ColorAdjustments ca))
            {
                ca.colorFilter.overrideState = true;
                ca.colorFilter.value = new Color(1f, 0.92f, 0.75f);
            }
            if (volumeTransition.profile.TryGet(out Vignette v))
            {
                v.active = true;
                v.color.overrideState = true;
                v.color.value = new Color(0.6f, 0.3f, 0.1f);
                v.intensity.overrideState = true;
                v.intensity.value = 0.28f;
                v.smoothness.overrideState = true;
                v.smoothness.value = 0.4f;
            }
        }
    }
}