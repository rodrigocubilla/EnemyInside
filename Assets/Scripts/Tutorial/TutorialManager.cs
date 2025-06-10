using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<TutorialStep> steps;
    private int current = 0;

    private void Start()
    {
        if (steps.Count > 0) steps[0].Enter(current);
    }

    private void Update()
    {
        if (current < steps.Count && steps[current].IsComplete)
        {
            steps[current].Exit();
            current++;
            if (current < steps.Count) steps[current].Enter(current);
        }
    }
}
