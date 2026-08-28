using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [Header("Mode")]
    public bool demoMode;  // Coch dans l'Inspector si build démo

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

    // ═══════════════════════════════════════════════════════
    //  INITIALISATION MODE DÉMO  (appelé par StartScreen)
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
}