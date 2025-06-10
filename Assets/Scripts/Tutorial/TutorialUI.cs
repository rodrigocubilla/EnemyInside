using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    private RectTransform rectTransform;
    private TextMeshProUGUI text;

    [SerializeField] private List<Vector3> positions;
    [SerializeField] private List<Vector2> scales;

    public void ShowMessage(string message, int current) {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        rectTransform.localPosition = positions[current];
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scales[current].x);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scales[current].y);
        text.text = message;

        StartCoroutine(TextPulse());
    }

    public void Hide() {
        StopAllCoroutines();
    }

    private IEnumerator TextPulse()
    {
        while (true)
        {
            float t = Mathf.PingPong(Time.time / (1f / 2f), 1f);

            float smooth = Mathf.SmoothStep(0f, 1f, t);

            text.fontSize = Mathf.Lerp(120, 136, smooth);
            yield return null;
        }
    }
}
