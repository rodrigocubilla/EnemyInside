using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStep", menuName = "Scriptable Objects/TutorialStep")]
public abstract class TutorialStep : ScriptableObject
{
    public bool IsComplete { get; protected set; }

    public virtual void Enter(int current) { IsComplete = false; }
    public virtual void Exit() { }
}
