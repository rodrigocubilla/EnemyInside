using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using static EventManager;

public class WinBlock : MonoBehaviour
{
    private int playerLives;
    private bool _sent = false;

    private void FixedUpdate() {
        if (_sent) return;

        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(0.2f, 0.2f), 0f);

        if (collider?.gameObject.tag == "Player")
        {

            if (SceneManager.GetActiveScene().buildIndex != 9)
            {
                playerLives = FindFirstObjectByType<PlayerController>().Lives;
                LevelCompleteEvent levelComplete = new LevelCompleteEvent
                {
                    level = StaticVariables.level,
                };
                AnalyticsService.Instance.RecordEvent(levelComplete);
                Debug.Log($"Lanzar Level Complete id:{StaticVariables.level}, playerliv: {playerLives} gameOvers: {StaticVariables.gameOver}");
            }
            else
            {
                //Remplazar por gameFinished
                GameFinishEvent gameFinish = new GameFinishEvent
                {
                    level = StaticVariables.level,
                    gameOver = StaticVariables.totalGameOver
                };
                AnalyticsService.Instance.RecordEvent(gameFinish);
                Debug.Log($"Lanzar Evento Game Finish level: {StaticVariables.level} gameOvers: {StaticVariables.totalGameOver}");
            }
            _sent = true;
            SceneLoader.instance.ActivateScene();
        }
    }
}
