using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Mode")]
    public bool demoMode;  // Coché dans l'Inspector si build démo

    [Header("Layouts")]
    public GameObject menuLayout;
    public GameObject demoLayout;

    [Header("Panels Menu Principal")]
    public GameObject panelArcade;
    public GameObject panelScore;
    public GameObject panelOptions;
    public GameObject panelAchievements;

    [Header("Panels Démo")]
    public GameObject panelScoreDemo;
    public GameObject panelOptionsDemo;

    [Header("Boutons par défaut — Menu Principal")]
    public GameObject btnArcade;
    public GameObject btnArcadeClassic;
    public GameObject btnLangue;

    [Header("Boutons par défaut — Démo")]
    public GameObject btnDemoArcade;
    public GameObject btnDemoScore;
    public GameObject btnDemoOptions;

    // ═══════════════════════════════════════════════════════
    // ⚠️ NOUVEAU : BOUTONS RÉSEAUX SOCIAUX
    // ═══════════════════════════════════════════════════════

    [Header("Réseaux Sociaux — Menu Principal")]
    public Button btnDiscord;
    public Button btnSteam;
    public Button btnX;
    public Button btnYouTube;

    [Header("Réseaux Sociaux — Démo")]
    public Button btnDiscordDemo;
    public Button btnSteamDemo;
    public Button btnXDemo;
    public Button btnYouTubeDemo;

    // ═══════════════════════════════════════════════════════
    // LIENS EXTERNES (URLS)
    // ═══════════════════════════════════════════════════════

    [Header("LIENS EXTERNES — À MODIFIER SELON TON PROJET")]
    public string urlDiscord = "https://discord.gg/jwzve576x";
    public string urlSteam = "https://store.steampowered.com/app/YOUR_STEAM_APP_ID_HERE"; // ← REMPLACE
    public string urlX = "https://x.com/HelieAndDrop";
    public string urlYouTube = "https://www.youtube.com/@HelieAndDrop";

    private SelectionManager selectionManager;
    private MenuFocusHandler focusHandler;

    void Start()
    {
        selectionManager = Object.FindFirstObjectByType<SelectionManager>();
        focusHandler = Object.FindFirstObjectByType<MenuFocusHandler>();

        // Synchronise demoMode avec StartScreen si présent
        StartScreen startScreen = Object.FindFirstObjectByType<StartScreen>();
        if (startScreen != null)
            demoMode = startScreen.demoMode;

        // ═══════════════════════════════════════════════════
        // INITIALISATION DES ÉCOUTEURS SNS
        // ═══════════════════════════════════════════════════

        InitializeSocialButtons();
    }

    void Update()
    {
        bool anyPanelActive;

        if (demoMode)
        {
            anyPanelActive = (panelScoreDemo != null && panelScoreDemo.activeSelf)
                          || (panelOptionsDemo != null && panelOptionsDemo.activeSelf);
        }
        else
        {
            anyPanelActive = (panelArcade != null && panelArcade.activeSelf)
                          || (panelOptions != null && panelOptions.activeSelf)
                          || (panelScore != null && panelScore.activeSelf)
                          || (panelAchievements != null && panelAchievements.activeSelf);
        }

        if (anyPanelActive
            && EventSystem.current != null
            && EventSystem.current.currentSelectedGameObject == null)
        {
            if (selectionManager != null && selectionManager.premierBouton != null)
            {
                EventSystem.current.SetSelectedGameObject(selectionManager.premierBouton);
            }
        }
    }

    // ═══════════════════════════════════════════════════════
    // INITIALISATION DES ÉCOUTEURS POUR LES BOUTONS SNS
    // ═══════════════════════════════════════════════════════

    void InitializeSocialButtons()
    {
        // Menu Principal
        if (btnDiscord != null)
            btnDiscord.onClick.AddListener(OnDiscordClick);

        if (btnSteam != null)
            btnSteam.onClick.AddListener(OnSteamClick);

        if (btnX != null)
            btnX.onClick.AddListener(OnXClick);

        if (btnYouTube != null)
            btnYouTube.onClick.AddListener(OnYouTubeClick);

        // Menu Démo
        if (btnDiscordDemo != null)
            btnDiscordDemo.onClick.AddListener(OnDiscordClick);

        if (btnSteamDemo != null)
            btnSteamDemo.onClick.AddListener(OnSteamClick);

        if (btnXDemo != null)
            btnXDemo.onClick.AddListener(OnXClick);

        if (btnYouTubeDemo != null)
            btnYouTubeDemo.onClick.AddListener(OnYouTubeClick);
    }

    // ═══════════════════════════════════════════════════════
    // MÉTHODES ONCLICK POUR LES RÉSEAUX SOCIAUX
    // ═══════════════════════════════════════════════════════

    public void OnDiscordClick()
    {
        Debug.Log("[MainMenuManager] Overture Discord: " + urlDiscord);
        OpenURL(urlDiscord);
    }

    public void OnSteamClick()
    {
        Debug.Log("[MainMenuManager] Ouverture Steam: " + urlSteam);
        OpenURL(urlSteam);
    }

    public void OnXClick()
    {
        Debug.Log("[MainMenuManager] Ouverture X: " + urlX);
        OpenURL(urlX);
    }

    public void OnYouTubeClick()
    {
        Debug.Log("[MainMenuManager] Ouverture YouTube: " + urlYouTube);
        OpenURL(urlYouTube);
    }

    // Helper pour ouvrir les URLs
    void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    // ═══════════════════════════════════════════════════════
    // INITIALISATION MODE DÉMO  (appelé par StartScreen)
    // ═══════════════════════════════════════════════════════

    public void InitDemoMode()
    {
        demoMode = true;

        // Éteint tout le menu normal
        if (menuLayout != null) menuLayout.SetActive(false);
        if (panelArcade != null) panelArcade.SetActive(false);
        if (panelScore != null) panelScore.SetActive(false);
        if (panelOptions != null) panelOptions.SetActive(false);
        if (panelAchievements != null) panelAchievements.SetActive(false);

        // Allume le demoLayout
        if (demoLayout != null) demoLayout.SetActive(true);
        if (panelScoreDemo != null) panelScoreDemo.SetActive(false);
        if (panelOptionsDemo != null) panelOptionsDemo.SetActive(false);

        // Sélection initiale
        if (selectionManager != null && btnDemoArcade != null)
        {
            selectionManager.premierBouton = btnDemoArcade;
            selectionManager.ResetSelection();
        }

        Debug.Log("[MainMenuManager] InitDemoMode() terminé");
    }

    // ═══════════════════════════════════════════════════════
    //  MENU PRINCIPAL
    // ═══════════════════════════════════════════════════════

    void CloseAllPanels()
    {
        menuLayout.SetActive(true);
        panelArcade.SetActive(false);
        panelScore.SetActive(false);
        panelOptions.SetActive(false);
        panelAchievements.SetActive(false);

        if (selectionManager != null)
        {
            selectionManager.premierBouton = btnArcade;
            selectionManager.ResetSelection();
        }
    }

    public void OnArcadeClick()
    {
        menuLayout.SetActive(false);
        panelArcade.SetActive(true);
        panelScore.SetActive(false);
        panelOptions.SetActive(false);
        panelAchievements.SetActive(false);
        SetSelection(btnArcadeClassic);
        RefreshFocus();
    }

    public void OnExplorationClick()
    {
        SceneManager.LoadScene("SceneTransitionExploration");
    }

    public void OnScoreClick()
    {
        menuLayout.SetActive(false);
        panelArcade.SetActive(false);
        panelScore.SetActive(true);
        panelOptions.SetActive(false);
        panelAchievements.SetActive(false);
        RefreshFocus();
    }

    public void OnOptionsClick()
    {
        menuLayout.SetActive(false);
        panelArcade.SetActive(false);
        panelScore.SetActive(false);
        panelOptions.SetActive(true);
        panelAchievements.SetActive(false);
        SetSelection(btnLangue);
        RefreshFocus();
    }

    public void OnAchievementsClick()
    {
        menuLayout.SetActive(false);
        panelArcade.SetActive(false);
        panelScore.SetActive(false);
        panelOptions.SetActive(false);
        panelAchievements.SetActive(true);
        RefreshFocus();
    }

    // ═══════════════════════════════════════════════════════
    //  MENU DÉMO
    // ═══════════════════════════════════════════════════════

    void CloseAllDemoPanels()
    {
        if (demoLayout != null) demoLayout.SetActive(true);
        if (panelScoreDemo != null) panelScoreDemo.SetActive(false);
        if (panelOptionsDemo != null) panelOptionsDemo.SetActive(false);

        if (selectionManager != null)
        {
            selectionManager.premierBouton = btnDemoArcade;
            selectionManager.ResetSelection();
        }
    }

    public void OnDemoArcadeClick()
    {
        SceneManager.LoadScene("SceneArcadeDemo");
    }

    public void OnDemoExplorationClick()
    {
        SceneManager.LoadScene("SceneTransitionExplorationDemo");
    }

    public void OnDemoScoreClick()
    {
        if (demoLayout != null) demoLayout.SetActive(false);
        if (panelScoreDemo != null) panelScoreDemo.SetActive(true);
        if (panelOptionsDemo != null) panelOptionsDemo.SetActive(false);
        if (btnDemoScore != null) SetSelection(btnDemoScore);
        RefreshFocus();
    }

    public void OnDemoOptionsClick()
    {
        if (demoLayout != null) demoLayout.SetActive(false);
        if (panelScoreDemo != null) panelScoreDemo.SetActive(false);
        if (panelOptionsDemo != null) panelOptionsDemo.SetActive(true);
        if (btnDemoOptions != null) SetSelection(btnDemoOptions);
        RefreshFocus();
    }

    // ═══════════════════════════════════════════════════════
    //  COMMUN
    // ═══════════════════════════════════════════════════════

    public void OnBackClick()
    {
        if (demoMode)
            CloseAllDemoPanels();
        else
            CloseAllPanels();
    }

    public void OnQuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void SetSelection(GameObject bouton)
    {
        if (selectionManager != null)
        {
            selectionManager.premierBouton = bouton;
            EventSystem.current.SetSelectedGameObject(bouton);
        }
    }

    void RefreshFocus()
    {
        if (focusHandler != null)
            focusHandler.RefreshFocusOnPanelOpen();
    }
}