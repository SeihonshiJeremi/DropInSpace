using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// Tutoriel du SpaceHub.
/// Detecte automatiquement si le joueur utilise une manette ou le clavier
/// et affiche le bon message de controles.
/// </summary>
public class TutorialSpaceHub : MonoBehaviour
{
    [Header("UI")]
    public GameObject tutorialPanel;
    public TMP_Text tutorialText;

    [Header("Parametres")]
    public float delaiAvantAffichage = 2.0f;
    public float dureeAffichage = 5.0f;

    private const string PREFS_KEY = "TutorialSpaceHubVu";

    void Start()
    {
        if (PlayerPrefs.GetInt(PREFS_KEY, 0) == 1)
        {
            if (tutorialPanel != null) tutorialPanel.SetActive(false);
            return;
        }

        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        StartCoroutine(LancerTutoriel());
    }

    IEnumerator LancerTutoriel()
    {
        yield return new WaitForSeconds(delaiAvantAffichage);

        string message = DetecterControle();

        if (tutorialPanel != null) tutorialPanel.SetActive(true);
        if (tutorialText != null)
        {
            tutorialText.text = message;
            StartCoroutine(AnimerTexte());
        }

        yield return new WaitForSeconds(dureeAffichage);

        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        PlayerPrefs.SetInt(PREFS_KEY, 1);
        PlayerPrefs.Save();
    }

    private string DetecterControle()
    {
        var gamepads = Gamepad.all;
        if (gamepads.Count > 0)
        {
            string nomManette = gamepads[0].name.ToLower();

            if (nomManette.Contains("dualsense") || nomManette.Contains("ps5"))
                return "Oriente avec le stick gauche\nAccelere avec Croix\nEngage le Warp Roll avec Rond !";
            else if (nomManette.Contains("dualshock") || nomManette.Contains("ps4"))
                return "Oriente avec le stick gauche\nAccelere avec Croix\nEngage le Warp Roll avec Rond !";
            else if (nomManette.Contains("switch") || nomManette.Contains("nintendo"))
                return "Oriente avec le stick gauche\nAccelere avec B\nEngage le Warp Roll avec A !";
            else
                // Xbox par defaut
                return "Oriente avec le stick gauche\nAccelere avec A\nEngage le Warp Roll avec B !";
        }

        return "Oriente avec les fleches ou ZQSD\nAccelere avec Espace\nEngage le Warp Roll avec E !";
    }

    IEnumerator AnimerTexte()
    {
        float dureeAnim = 0.4f;
        float elapsed = 0f;

        tutorialText.transform.localScale = Vector3.zero;

        while (elapsed < dureeAnim)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dureeAnim;

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
        Debug.Log("Tutoriel SpaceHub remis a zero !");
    }
}