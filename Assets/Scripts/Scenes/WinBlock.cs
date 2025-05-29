using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using static EventManager;

public class WinBlock : MonoBehaviour
{
    private int playerLives;
    private bool _sent = false;


    private void Awake()
    {
        playerLives = FindFirstObjectByType<PlayerController>().Lives;
    }

    private void FixedUpdate() {
        if (_sent) return;

        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(0.2f, 0.2f), 0f);

        if (collider?.gameObject.tag == "Player")
        {
            int sceneId = SceneLoader.instance.actualSceneID;

            if (SceneManager.GetActiveScene().buildIndex != 9)
            {
                int losingLives = 5 - playerLives;
                LevelCompleteEvent levelComplete = new LevelCompleteEvent
                {
                    level = SceneLoader.instance.actualSceneID,
                    live = losingLives,
                };
                AnalyticsService.Instance.RecordEvent(levelComplete);
                Debug.Log("Lanzar Level Complete");
            }
            else
            {
                GameOverEvent gameOver = new GameOverEvent
                {
                    live = playerLives,
                };
                AnalyticsService.Instance.RecordEvent(gameOver);
                Debug.Log("Lanzar Evento Game Over");
            }
            _sent = true;
            SceneLoader.instance.ActivateScene();
        }
    }
}
