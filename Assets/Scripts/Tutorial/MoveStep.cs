using UnityEngine;

[CreateAssetMenu(fileName = "MoveStep", menuName = "Scriptable Objects/MoveStep")]
public class MoveStep : TutorialStep
{
    public TutorialUI uiPrefab;
    private TutorialUI instance;
    private GameObject canvas;

    public override void Enter(int current)
    {
        base.Enter(current);
        canvas = GameObject.FindWithTag("TutorialCanvas");
        instance = Instantiate(uiPrefab, canvas.transform);
        instance.ShowMessage("W - A - S - D para moverte", current);
        PlayerController.onAnyMovementKey += OnMoveKey;
    }

    private void OnMoveKey()
    {
        IsComplete = true;
    }

    public override void Exit()
    {
        PlayerController.onAnyMovementKey -= OnMoveKey;
        instance.Hide();
        Destroy(instance.gameObject);
    }
}
