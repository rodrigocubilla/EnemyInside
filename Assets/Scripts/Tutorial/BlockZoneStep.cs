using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "BlockZoneStep", menuName = "Scriptable Objects/BlockZoneStep")]
public class BlockZoneStep : TutorialStep
{
    public TutorialUI uiPrefab;
    public List<KillZone> zones; 
    private TutorialUI instance;
    private GameObject canvas;
    private Light2D globalLight;
    private Light2D playerLight;
    private GameObject[] boxLight;
    private int remaining;

    public override void Enter(int current)
    {
        base.Enter(current);
        zones = FindObjectsByType<KillZone>(FindObjectsSortMode.None).ToList<KillZone>();
        globalLight = GameObject.FindWithTag("GlobalLight").GetComponent<Light2D>();
        playerLight = FindObjectOfType<PlayerController>().GetComponent<Light2D>();
        boxLight = GameObject.FindGameObjectsWithTag("BoxLight");
        canvas = GameObject.FindWithTag("TutorialCanvas");

        remaining = zones.Count;
        instance = Instantiate(uiPrefab, canvas.transform);
        instance.ShowMessage("Bloquea las zonas de ejecución", current);
        //Disminuir luz global
        globalLight.intensity = 0.04f;
        playerLight.intensity = 1f;

        foreach (GameObject box in boxLight)
        {
            LightPulse light = box.GetComponent<LightPulse>();
            light.Highlight();
        }

        foreach (KillZone z in zones)
        {
            z.OnBlocked += OnZoneBlocked;
            z.Highlight();
        }
    }

    private void OnZoneBlocked(KillZone z)
    {
        z.OnBlocked -= OnZoneBlocked;
        z.StopAllCoroutines();
        remaining--;
        if (remaining <= 0) IsComplete = true;
    }

    public override void Exit()
    {
        foreach (GameObject box in boxLight)
        {
            LightPulse light = box.GetComponent<LightPulse>();
            light.OffLight();
        }
        foreach (KillZone z in zones)
        {
            z.OffLight();
        }
        instance.Hide();
        Destroy(instance.gameObject);
    }

}
