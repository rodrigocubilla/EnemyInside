using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickGoTo : MonoBehaviour
{
    [SerializeField] private string scene;
    [SerializeField] private TextMeshPro title;
    [SerializeField] private TextMeshPro text;

    private void OnMouseOver() {
        text.color = Color.yellow;

        if (Input.GetMouseButtonDown(0))
        {
            
            LeanTween.moveX(title.gameObject, -100f, 1.6f);
            LeanTween.moveX(text.gameObject, 100f, 1.6f).setOnComplete(() => {
                SceneManager.LoadScene(scene);
            });
        }
    }

    private void OnMouseExit() {
        text.color = Color.white;
    }
}
