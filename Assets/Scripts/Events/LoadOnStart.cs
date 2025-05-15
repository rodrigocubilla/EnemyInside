using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnStart : MonoBehaviour 
{
    [SerializeField] private string scene;

    private void Start() 
    {
        SceneManager.LoadScene(scene);
    }
}