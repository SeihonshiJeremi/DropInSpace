using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Maintient la selection du premier bouton quand la manette
/// reprend le focus ou quand aucun bouton n'est selectionne.
/// A placer sur l'EventSystem de chaque scene avec des menus.
/// </summary>
public class SelectionManager : MonoBehaviour
{
    [Header("Bouton selectionne par defaut")]
    public GameObject premierBouton; // Assigner le bouton ARCADE dans l'Inspector

    private GameObject dernierBouton;

    void Update()
    {
        // Si aucun bouton n'est selectionne — reselectonne le dernier ou le premier
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (dernierBouton != null)
                EventSystem.current.SetSelectedGameObject(dernierBouton);
            else if (premierBouton != null)
                EventSystem.current.SetSelectedGameObject(premierBouton);
        }
        else
        {
            // Memorise le dernier bouton selectionne
            dernierBouton = EventSystem.current.currentSelectedGameObject;
        }
    }

    void OnEnable()
    {
        // Selectionne le premier bouton au demarrage
        if (premierBouton != null)
            EventSystem.current.SetSelectedGameObject(premierBouton);
    }
}