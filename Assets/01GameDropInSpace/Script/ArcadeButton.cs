using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Feedback sonore arcade — fonctionne avec Souris, Clavier, Manette ET Tactile (Android).
/// </summary>
public class ArcadeButton : MonoBehaviour,
    ISelectHandler,
    ISubmitHandler,
    IPointerClickHandler
{
    [Header("Sons")]
    public bool jouerHover = true;
    public bool jouerClick = true;

    // ─── SÉLECTION (manette/clavier) ───
    public void OnSelect(BaseEventData eventData)
    {
        if (jouerHover && ButtonSoundManager.Instance != null
            && !(eventData is PointerEventData))
        {
            ButtonSoundManager.Instance.PlayHover();
        }
    }

    // ─── ACTIVATION (Entrée/A bouton) ───
    public void OnSubmit(BaseEventData eventData)
    {
        if (jouerClick && ButtonSoundManager.Instance != null)
            ButtonSoundManager.Instance.PlayClick();
    }

    // ─── TAP TACTILE OU CLIC SOURIS ───
    public void OnPointerClick(PointerEventData eventData)
    {
        if (jouerClick && ButtonSoundManager.Instance != null)
            ButtonSoundManager.Instance.PlayClick();
    }
}