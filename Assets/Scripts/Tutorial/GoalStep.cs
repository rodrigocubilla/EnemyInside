using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "GoalStep", menuName = "Scriptable Objects/GoalStep")]
public class GoalStep : TutorialStep
{
    public TutorialUI uiPrefab;
    private TutorialUI instance;
    private GameObject canvas;

    private LightPulse goalLight;

    public override void Enter(int current)
    {
        base.Enter(current);
        canvas = GameObject.FindWithTag("TutorialCanvas");

        goalLight = GameObject.FindWithTag("GoalLight").GetComponent<LightPulse>();
        goalLight.Highlight();

        instance = Instantiate(uiPrefab, canvas.transform);
        instance.ShowMessage("Dirigite hacia la meta", current);
    }

    public override void Exit()
    {
        base.Exit();
        Destroy(instance);
    }
}
