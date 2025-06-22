using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ObjectPulse : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(PulseCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator PulseCoroutine()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 maxScale = originalScale * 1.2f;

        while (true)
        {
            float t = Mathf.PingPong(Time.time * 2f /*velocidad*/, 1f);
            float smooth = Mathf.SmoothStep(0f, 1f, t);
            transform.localScale = Vector3.Lerp(originalScale, maxScale, smooth);

            yield return null;
        }
    }
}
