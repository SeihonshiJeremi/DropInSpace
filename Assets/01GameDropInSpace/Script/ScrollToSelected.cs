using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollToSelected : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;

    private GameObject lastSelected;

    void Update()
    {
        GameObject selected = EventSystem.current?.currentSelectedGameObject;
        if (selected == null || selected == lastSelected) return;
        lastSelected = selected;

        RectTransform selectedRect = selected.GetComponent<RectTransform>();
        if (selectedRect == null) return;

        // Vérifie que le bouton sélectionné est bien dans ce scroll view
        if (!selectedRect.IsChildOf(content)) return;

        Canvas.ForceUpdateCanvases();

        Vector2 viewportCenter = scrollRect.viewport.rect.center;
        Vector2 selectedPos = scrollRect.viewport.InverseTransformPoint(selectedRect.position);

        float offset = selectedPos.y - viewportCenter.y;
        float contentHeight = content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;

        float normalizedY = (content.anchoredPosition.y - offset) / (contentHeight - viewportHeight);
        normalizedY = Mathf.Clamp01(normalizedY);

        scrollRect.verticalNormalizedPosition = 1f - normalizedY;
    }
}