using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Tutoriel integre au premier stage joue.
/// Affiche des messages sequentiels pendant le jeu.
/// Ne reapparait jamais une fois vu — fonctionne en Arcade ET en Exploration.
/// A placer sur un GameObject dans TOUS les stages.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject tutorialPanel;        // Panel qui contient le texte
    public TMP_Text tutorialText;           // Texte du message

    [Header("Parametres")]
    public float dureAffichage = 3.0f;     // Duree d'affichage de chaque message

    // Cle PlayerPrefs pour savoir si le tuto a deja ete vu
    private const string PREFS_KEY = "TutorialVu";

    void Start()
    {
        // Si le tutoriel a deja ete vu — on ne l'affiche pas
        if (PlayerPrefs.GetInt(PREFS_KEY, 0) == 1)
        {
            if (tutorialPanel != null) tutorialPanel.SetActive(false);
            return;
        }

        // Cache le panel au depart
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        // Lance la sequence de messages
        StartCoroutine(LancerTutoriel());
    }

    IEnumerator LancerTutoriel()
    {
        // Attend 1 seconde avant le premier message
        yield return new WaitForSeconds(1.0f);

        // Message 1 — Deplacer Drop
        yield return AfficherMessage("Deplace-toi !");

        // Message 2 — Colorier les cubes
        yield return AfficherMessage("Colorie les cubes !");

        // Message 3 — Eviter les ennemis (attend qu'un ennemi soit proche)
        yield return new WaitForSeconds(2.0f);
        yield return AfficherMessage("Evite les ennemis !");

        // Message 4 — Timer
        yield return new WaitForSeconds(3.0f);
        yield return AfficherMessage("Fais vite !");

        // Tutoriel termine — on le marque comme vu
        PlayerPrefs.SetInt(PREFS_KEY, 1);
        PlayerPrefs.Save();
    }

    IEnumerator AfficherMessage(string message)
    {
        // Affiche le panel et le texte
        if (tutorialPanel != null) tutorialPanel.SetActive(true);
        if (tutorialText != null)
        {
            tutorialText.text = message;
            // Lance l'animation d'agrandissement
            StartCoroutine(AnimerTexte());
        }

        // Attend la duree d'affichage
        yield return new WaitForSeconds(dureAffichage);

        // Cache le panel
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        // Petite pause entre les messages
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator AnimerTexte()
    {
        // Duree de l'animation en secondes
        float dureeAnim = 0.4f;
        float elapsed = 0f;

        // Part de 0 et grandit jusqu'a 1
        tutorialText.transform.localScale = Vector3.zero;

        while (elapsed < dureeAnim)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dureeAnim;

            // Effet de rebond leger — depasse un peu puis revient
            float scale = Mathf.SmoothStep(0f, 1f, t) * 1.1f;
            if (t > 0.8f) scale = Mathf.Lerp(1.1f, 1.0f, (t - 0.8f) / 0.2f);

            tutorialText.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        // Taille finale exacte
        tutorialText.transform.localScale = Vector3.one;
    }

    // Appele depuis DebugExploration pour reset le tutoriel pendant les tests
    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey(PREFS_KEY);
        Debug.Log("Tutoriel remis a zero !");
    }
}