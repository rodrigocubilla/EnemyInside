using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitLogic : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CloseGame()
    {
        Screen.fullScreen = false;
        Debug.Log("Application quit");
    }
}
