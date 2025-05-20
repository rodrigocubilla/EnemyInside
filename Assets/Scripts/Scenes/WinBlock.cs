using UnityEngine;
using UnityEngine.SceneManagement;

public class WinBlock : MonoBehaviour
{
    private int playerLives;

    private void Awake()
    {
        playerLives = FindObjectOfType<PlayerController>().Lives;
    }
    private void FixedUpdate() {
        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(0.2f, 0.2f), 0f);

        if (collider?.gameObject.tag == "Player")
        {
            int sceneId = SceneLoader.instance.actualSceneID;

            if (SceneManager.GetActiveScene().buildIndex != 9)
            {
                int losingLives = 5 - playerLives;
                Debug.Log("Lanzar Level Complete");
            }
            else
            {
                Debug.Log("Lanzar Evento Game Over");
            }

            SceneLoader.instance.ActivateScene();
        }
    }
}
