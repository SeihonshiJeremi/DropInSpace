using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class VibrationsManager : MonoBehaviour
{
    public static VibrationsManager Instance;

    private bool vibrationsActives = true;

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

        vibrationsActives = PlayerPrefs.GetInt("Vibrations", 1) == 1;
        Debug.Log($"[Vibrations] Chargé : {vibrationsActives}");
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
        StopVibration();
    }

    public void SetVibrations(bool actif)
    {
        vibrationsActives = actif;
        if (!actif) StopVibration();
        Debug.Log($"[Vibrations] : {actif}");
    }

    public void Vibrate(float left, float right, float duration)
    {
        if (!vibrationsActives) return;
        if (Gamepad.current == null) return;

        Gamepad.current.SetMotorSpeeds(left, right);
        StartCoroutine(StopVibrationDelayed(duration));
        Debug.Log($"[Vibrations] Buzz — L:{left} R:{right} durée:{duration}");
    }

    IEnumerator StopVibrationDelayed(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopVibration();
    }

    public void StopVibration()
    {
        if (Gamepad.current == null) return;
        Gamepad.current.SetMotorSpeeds(0, 0);
    }

    void OnApplicationQuit()
    {
        StopVibration();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause) StopVibration();
    }
}