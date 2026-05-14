using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SliderController : MonoBehaviour
{
    public Slider slider;
    public float step = 0.1f;

    private DIS_Controls controls;
    private float moveInput;
    private float cooldown = 0.2f;
    private float lastMoveTime;

    void Awake()
    {
        controls = new DIS_Controls();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Update()
    {
        GameObject selected = EventSystem.current?.currentSelectedGameObject;
        if (selected != gameObject) return;
        if (slider == null) return;
        if (Time.time - lastMoveTime < cooldown) return;

        Vector2 move = controls.Gameplay.Move.ReadValue<Vector2>();

        if (move.x > 0.5f)
        {
            slider.value = Mathf.Clamp01(slider.value + step);
            lastMoveTime = Time.time;
        }
        else if (move.x < -0.5f)
        {
            slider.value = Mathf.Clamp01(slider.value - step);
            lastMoveTime = Time.time;
        }
    }
}