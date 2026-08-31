using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Ecran de fin de la démo — Drop in Space
/// Boutons SNS : ouvre les liens externes (Discord, X, YouTube, Steam)
/// Quit : reset la session et retourne au menu principal
/// </summary>
public class EndScreenManagerDemo : MonoBehaviour
{
    [Header("Liens externes")]
    public string urlDiscord = "https://discord.gg/TON_INVITATION";
    public string urlX = "https://x.com/TON_COMPTE";
    public string urlYoutube = "https://www.youtube.com/@TA_CHAINE";
    public string urlSteam = "https://store.steampowered.com/app/TON_APPID"; // Page wishlist

    [Header("Scenes")]
    public string sceneMenuPrincipal = "StartDropInSpace";

    // ─── BOUTONS SNS ─────────────────────────────────────────────

    public void OnDiscordClick()
    {
        OpenLink(urlDiscord);
    }

    public void OnXClick()
    {
        OpenLink(urlX);
    }

    public void OnYouTubeClick()
    {
        OpenLink(urlYoutube);
    }

    public void OnSteamClick()
    {
        OpenLink(urlSteam);
    }

    // ─── QUIT : RESET COMPLET ET RETOUR MENU ─────────────────────

    public void OnQuitClick()
    {
        // Reset de la session (score, vies, mode...) comme au démarrage
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.ResetSession();
            GameStateManager.Instance.ClearAllInstances();
        }

        // Nettoyage optionnel des flags de mode (même logique que ArcadeMenuManager)
        PlayerPrefs.SetInt("ModeExploration", 0);
        PlayerPrefs.SetInt("ModeInfiniActif", 0);
        PlayerPrefs.SetInt("ModeObscurActif", 0);
        PlayerPrefs.SetInt("ModeZenActif", 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneMenuPrincipal);
        Debug.Log("[EndScreen] Quit — session reset, retour au menu principal");
    }

    // ─── HELPER ──────────────────────────────────────────────────

    private void OpenLink(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogWarning("[EndScreen] URL vide, lien ignoré");
            return;
        }

        // OpenURL fonctionne en standalone et WebGL (ouvre un nouvel onglet navigateur)
        // Note : pas de WebGL build depuis l'éditeur — ceci marche au runtime
        Application.OpenURL(url);
    }
}