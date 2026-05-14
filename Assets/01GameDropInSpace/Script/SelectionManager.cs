using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [Header("Bouton selectionne par defaut")]
    public GameObject premierBouton;
    public bool navigationBloquee = false;

    private GameObject dernierBouton;

    void Update()
    {
        if (navigationBloquee)
        {
            if (EventSystem.current.currentSelectedGameObject != premierBouton)
                EventSystem.current.SetSelectedGameObject(premierBouton);
            return;
        }

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (dernierBouton != null)
                SetSelection(dernierBouton);
            else if (premierBouton != null)
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
        if (premierBouton != null)
            SetSelection(premierBouton);
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
        foreach (var btn in FindObjectsOfType<Button>())
            SetDiamonds(btn.gameObject, false);
        dernierBouton = null;
        EventSystem.current.SetSelectedGameObject(premierBouton);
        SetDiamonds(premierBouton, true);
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

        // Change couleur Image uniquement si le bouton a des diamants
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