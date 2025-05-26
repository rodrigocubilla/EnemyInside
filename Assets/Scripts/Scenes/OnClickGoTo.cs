using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class OnClickActionButton : MonoBehaviour
{
    public enum ButtonAction
    {
        LoadScene,
        ResetCurrentLevel,
        StartDataCollection,
        StopDataCollection
    }

    [Header("General")]
    [SerializeField] private ButtonAction action;
    [SerializeField] private string sceneToLoad; // Solo se usa si la acción es LoadScene
    [SerializeField] private TextMeshPro title;
    [SerializeField] private TextMeshPro text;
    private PlayerController playerController;

    [Header("Services script")]
    [SerializeField] private Services services;

    private void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(0))
        {
            switch (action)
            {
                case ButtonAction.LoadScene:
                    text.color = Color.yellow;
                    AnimateAndLoadScene(sceneToLoad);
                    break;

                case ButtonAction.ResetCurrentLevel:
                    text.color = Color.red;
                    if (playerController == null)
                        playerController = FindObjectOfType<PlayerController>();

                    if (playerController != null)
                        playerController.ResetLevel();
                    else
                        Debug.LogWarning("PlayerController not found in the scene.");
                    break;

                case ButtonAction.StartDataCollection:
                    text.color = Color.yellow;
                    services?.StartDataCollection();
                    break;

                case ButtonAction.StopDataCollection:
                    text.color = Color.red;
                    services?.StopDataCollection();
                    break;
            }
        }
    }

    private void OnMouseExit()
    {
        text.color = Color.white;
    }

    private void AnimateAndLoadScene(string targetScene)
    {
        LeanTween.moveX(title.gameObject, -100f, 1.6f);
        LeanTween.moveX(text.gameObject, 100f, 1.6f).setOnComplete(() =>
        {
            SceneManager.LoadScene(targetScene);
        });
    }
}
