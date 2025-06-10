using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class LightPulse : MonoBehaviour
{
    public void Highlight()
    {
        StartCoroutine(PulseCoroutine());
    }

    public void OffLight()
    {
        StopAllCoroutines();
        GetComponentInChildren<Light2D>().intensity = 0f;
    }

    private IEnumerator PulseCoroutine()
    {
        while (true)
        {
            // Calcula un valor t que oscila entre 0 y 1 en medio ciclo
            float t = Mathf.PingPong(Time.time / (1f / 2f), 1f);

            // Aplica SmoothStep para suavizar la curva
            float smooth = Mathf.SmoothStep(0f, 1f, t);

            // Interpola la intensidad de la luz

            GetComponentInChildren<Light2D>().intensity = Mathf.Lerp(0, 2f, smooth);

            yield return null;
        }
    }
}
