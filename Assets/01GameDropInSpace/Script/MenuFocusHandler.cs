using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Force le focus de l'EventSystem sur le premier bouton interactif du menu
/// après chaque changement de scène ou activation de panel.
/// </summary>
public class MenuFocusHandler : MonoBehaviour
{
    public static MenuFocusHandler Instance;

    [Header("Références")]
    public GameObject[] panels;
    public GameObject firstButtonInEachPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Invoke(nameof(TryForceMenuFocus), 0.1f);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[MenuFocusHandler] Scène chargée : {scene.name}");
        Invoke(nameof(TryForceMenuFocus), 0.1f);
    }

    /// <summary>
    /// Tente de sélectionner un bouton interactif visible
    /// </summary>
    public void TryForceMenuFocus()
    {
        var eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            Debug.LogWarning("[MenuFocusHandler] EventSystem introuvable !");
            return;
        }

        // Cherche tous les boutons interactifs dans les panels actifs
        foreach (var panel in panels)
        {
            if (panel == null || !panel.activeInHierarchy) continue;

            var buttons = panel.GetComponentsInChildren<Button>(true);
            foreach (var btn in buttons)
            {
                if (btn.gameObject.activeInHierarchy && btn.interactable)
                {
                    Debug.Log($"[MenuFocusHandler] Focus mis sur : {btn.gameObject.name}");
                    eventSystem.SetSelectedGameObject(null);
                    eventSystem.SetSelectedGameObject(btn.gameObject);
                    return;
                }
            }
        }

        // Fallback
        if (firstButtonInEachPanel != null && firstButtonInEachPanel.activeInHierarchy)
        {
            eventSystem.SetSelectedGameObject(firstButtonInEachPanel);
            Debug.Log($"[MenuFocusHandler] Fallback focus sur : {firstButtonInEachPanel.name}");
        }
    }

    /// <summary>
    /// À appeler quand on ouvre un panel
    /// </summary>
    public void RefreshFocusOnPanelOpen()
    {
        TryForceMenuFocus();
    }
}