using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// SpaceHub tutorial.
/// Automatically detects:
///   - Gamepad or keyboard
///   - AZERTY or QWERTY layout
/// And displays the correct controls message in English.
/// </summary>
public class TutorialSpaceHub : MonoBehaviour
{
    [Header("UI")]
    public GameObject tutorialPanel;
    public TMP_Text tutorialText;

    [Header("Settings")]
    public float delayBeforeDisplay = 2.0f;
    public float displayDuration = 5.0f;

    private const string PREFS_KEY = "TutorialSpaceHubVu";

    void Start()
    {
        if (PlayerPrefs.GetInt(PREFS_KEY, 0) == 1)
        {
            if (tutorialPanel != null) tutorialPanel.SetActive(false);
            return;
        }

        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        StartCoroutine(LaunchTutorial());
    }

    IEnumerator LaunchTutorial()
    {
        yield return new WaitForSeconds(delayBeforeDisplay);

        string message = DetectControls();

        if (tutorialPanel != null) tutorialPanel.SetActive(true);
        if (tutorialText != null)
        {
            tutorialText.text = message;
            StartCoroutine(AnimateText());
        }

        yield return new WaitForSeconds(displayDuration);

        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        PlayerPrefs.SetInt(PREFS_KEY, 1);
        PlayerPrefs.Save();
    }

    // ═══════════════════════════════════════════════════════
    // KEYBOARD LAYOUT DETECTION (AZERTY / QWERTY)
    // ═══════════════════════════════════════════════════════

    private bool IsAzertyKeyboard()
    {
        // Method 1: ask Unity directly for the active layout
        var kb = Keyboard.current;
        if (kb != null && !string.IsNullOrEmpty(kb.keyboardLayout))
        {
            string layout = kb.keyboardLayout.ToLower();
            if (layout.Contains("azerty") || layout.Contains("french") ||
                layout.Contains("belgian") || layout.Contains("swiss"))
                return true;
        }

        // Method 2 (fallback): guess via the system language
        return Application.systemLanguage == SystemLanguage.French;
    }

    // ═══════════════════════════════════════════════════════
    // CONTROLS DETECTION (GAMEPAD / KEYBOARD)
    // ═══════════════════════════════════════════════════════

    private string DetectControls()
    {
        var gamepads = Gamepad.all;
        if (gamepads.Count > 0)
        {
            string gamepadName = gamepads[0].name.ToLower();

            if (gamepadName.Contains("dualsense") || gamepadName.Contains("ps5"))
                return "Steer with the left stick\nAccelerate with Cross\nEngage Warp Roll with Circle!";
            else if (gamepadName.Contains("dualshock") || gamepadName.Contains("ps4"))
                return "Steer with the left stick\nAccelerate with Cross\nEngage Warp Roll with Circle!";
            else if (gamepadName.Contains("switch") || gamepadName.Contains("nintendo"))
                return "Steer with the left stick\nAccelerate with B\nEngage Warp Roll with A!";
            else
                // Xbox by default
                return "Steer with the left stick\nAccelerate with A\nEngage Warp Roll with B!";
        }

        // ─── KEYBOARD: adapt to the player's layout! ───
        if (IsAzertyKeyboard())
        {
            // French player on AZERTY — show the keys engraved on HIS keyboard
            return "Steer with arrow keys or ZQSD\nAccelerate with Space\nEngage Warp Roll with E!";
        }
        else
        {
            // QWERTY player
            return "Steer with arrow keys or WASD\nAccelerate with Space\nEngage Warp Roll with E!";
        }
    }

    IEnumerator AnimateText()
    {
        float animDuration = 0.4f;
        float elapsed = 0f;

        tutorialText.transform.localScale = Vector3.zero;

        while (elapsed < animDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animDuration;

            // Light bounce effect — overshoot slightly then settle back
            float scale = Mathf.SmoothStep(0f, 1f, t) * 1.1f;
            if (t > 0.8f) scale = Mathf.Lerp(1.1f, 1.0f, (t - 0.8f) / 0.2f);

            tutorialText.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        tutorialText.transform.localScale = Vector3.one;
    }

    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey(PREFS_KEY);
        Debug.Log("SpaceHub tutorial reset!");
    }
}