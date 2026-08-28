using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [Header("Bouton selectionne par defaut")]
    public GameObject premierBouton;
    public bool navigationBloquee = false;

    private GameObject dernierBouton;
    private bool modeGameOver = false;
    private GameObject premierBoutonGameOver = null;

    void Update()
    {
        if (navigationBloquee)
        {
            if (EventSystem.current.currentSelectedGameObject != premierBouton)
                EventSystem.current.SetSelectedGameObject(premierBouton);
            return;
        }

        // En mode Game Over, on force la sélection sur les boutons du Game Over uniquement
        if (modeGameOver)
        {
            if (premierBoutonGameOver != null &&
                EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(premierBoutonGameOver);
            return;
        }

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (dernierBouton != null && dernierBouton.activeInHierarchy)
                SetSelection(dernierBouton);
            else if (premierBouton != null && premierBouton.activeInHierarchy)
                SetSelection(premierBouton);
        }
        else if (EventSystem.current.currentSelectedGameObject != dernierBouton)
        {
            if (dernierBouton != null)
                SetDiamonds(dernierBouton, false);
            SetDiamonds(EventSystem.current.currentSelectedGameObject, true);
            dernierBouton = EventSystem.current.currentSelectedGameObject;
        }
    }

    void OnEnable()
    {
        // Ne sélectionne que si le bouton est actif dans la hiérarchie
        if (premierBouton != null && premierBouton.activeInHierarchy)
            SetSelection(premierBouton);
    }

    // Appelé par InfiniteGameOverUI.AfficherGameOver()
    public void ActiverModeGameOver(GameObject premierBoutonGO)
    {
        modeGameOver = true;
        premierBoutonGameOver = premierBoutonGO;

        // Désélectionner les boutons du jeu
        if (dernierBouton != null)
            SetDiamonds(dernierBouton, false);
        dernierBouton = null;

        // Sélectionner le premier bouton du Game Over
        if (premierBoutonGO != null)
            EventSystem.current.SetSelectedGameObject(premierBoutonGO);
    }

    void SetSelection(GameObject bouton)
    {
        if (dernierBouton != null)
            SetDiamonds(dernierBouton, false);
        EventSystem.current.SetSelectedGameObject(bouton);
        SetDiamonds(bouton, true);
        dernierBouton = bouton;
    }

    public void ResetSelection()
    {
        modeGameOver = false;
        premierBoutonGameOver = null;
        foreach (var btn in FindObjectsByType<Button>(FindObjectsSortMode.None))
            SetDiamonds(btn.gameObject, false);
        dernierBouton = null;
        if (premierBouton != null && premierBouton.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(premierBouton);
            SetDiamonds(premierBouton, true);
        }
    }

    public void ForceDiamonds(GameObject bouton, bool actif)
    {
        SetDiamonds(bouton, actif);
    }

    void SetDiamonds(GameObject bouton, bool actif)
    {
        Transform left = bouton.transform.Find("DiamondLeft");
        Transform right = bouton.transform.Find("DiamondRight");
        bool aDiamants = left != null || right != null;
        if (left != null) left.gameObject.SetActive(actif);
        if (right != null) right.gameObject.SetActive(actif);
        if (aDiamants)
        {
            Image img = bouton.GetComponent<Image>();
            if (img != null)
            {
                img.color = actif
                    ? new Color(0.306f, 0.804f, 0.769f, 0.3f)
                    : new Color(0.04f, 0.03f, 0.08f, 0.5f);
            }
        }
    }
}