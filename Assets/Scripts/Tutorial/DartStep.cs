using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "DartStep", menuName = "Scriptable Objects/DartStep")]
public class DartStep : TutorialStep
{
    public TutorialUI uiPrefab;
    private TutorialUI instance;
    private GameObject canvas;

    public override void Enter(int current)
    {
        base.Enter(current);
        canvas = GameObject.FindWithTag("TutorialCanvas");

        instance = Instantiate(uiPrefab, canvas.transform);
        instance.ShowMessage("Cuidado con los DARDOS!", current);
    }

    public override void Exit()
    {
        instance.Hide();
        Destroy(instance.gameObject);
    }

}
