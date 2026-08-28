using UnityEngine;
using UnityEngine.UI;

public class CoolToggle : MonoBehaviour
{
    public Image background;
    public RectTransform knob;
    public bool isOn = false;
    public float speed = 8f;

    private Color colorOn = new Color(0.306f, 0.804f, 0.769f, 1f);
    private Color colorOff = new Color(0.1f, 0.1f, 0.1f, 1f);
    private float knobOnX = 18f;
    private float knobOffX = -18f;

    void Awake()
    {
        if (background != null)
            background.color = isOn ? colorOn : colorOff;
        if (knob != null)
            knob.anchoredPosition = new Vector2(isOn ? knobOnX : knobOffX, 0);
    }

    void Update()
    {
        float targetX = isOn ? knobOnX : knobOffX;
        knob.anchoredPosition = Vector2.Lerp(
            knob.anchoredPosition,
            new Vector2(targetX, 0),
            Time.deltaTime * speed
        );
        background.color = Color.Lerp(
            background.color,
            isOn ? colorOn : colorOff,
            Time.deltaTime * speed
        );
    }

    public void Toggle()
    {
        isOn = !isOn;
    }
}