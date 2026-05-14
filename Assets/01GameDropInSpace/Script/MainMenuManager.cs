using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public GameObject menuLayout;
    public GameObject panelArcade;
    public GameObject panelScore;
    public GameObject panelOptions;
    public GameObject panelAchievements;

    [Header("Boutons par defaut par panel")]
    public GameObject btnArcade;
    public GameObject btnArcadeClassic;
    public GameObject btnLangue;

    private SelectionManager selectionManager;

    void Start()
    {
        selectionManager = FindObjectOfType<SelectionManager>();
    }

    void SetSelection(GameObject bouton)
    {
        if (selectionManager != null)
        {
            selectionManager.premierBouton = bouton;
            EventSystem.current.SetSelectedGameObject(bouton);
        }
    }

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
    }

    public void OnOptionsClick()
    {
        menuLayout.SetActive(false);
        panelArcade.SetActive(false);
        panelScore.SetActive(false);
        panelOptions.SetActive(true);
        panelAchievements.SetActive(false);
        SetSelection(btnLangue);
    }

    public void OnAchievementsClick()
    {
        menuLayout.SetActive(false);
        panelArcade.SetActive(false);
        panelScore.SetActive(false);
        panelOptions.SetActive(false);
        panelAchievements.SetActive(true);
    }

    public void OnBackClick()
    {
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