using UnityEngine;
using TMPro;

public class DiamondBlink : MonoBehaviour
{
    public float speed = 3f;
    private TextMeshProUGUI tmp;

    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float alpha = Mathf.Abs(Mathf.Sin(Time.time * speed));
        Color c = tmp.color;
        c.a = alpha;
        tmp.color = c;
    }
}