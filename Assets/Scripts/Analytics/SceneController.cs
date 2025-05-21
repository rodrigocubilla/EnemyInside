using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string _sceneName;


    public void StartGame()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
