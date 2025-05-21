using System;
using static EventManager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;

public enum PlayerMode
{
    Monster,
    Human
}

public class HUDEventReciever : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;

    public static Action<PlayerMode, bool> OnChangeMode;
    public static Action<int> OnPlayerMove;

    private int count = 0;

    private void Awake() {
        OnChangeMode += OnChangeModeHandler;

        DontDestroyOnLoad(this);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!SceneLoader.instance.isResetting)
            count = 0;
    }

    private void OnChangeModeHandler(PlayerMode mode, bool shouldInvokeHuman)
    {
        switch (mode)
        {
            case PlayerMode.Monster:
                text.text = "MONSTER TURN";
                break;
            case PlayerMode.Human:
                if (shouldInvokeHuman)
                {
                    count++;
                    int sceneId = SceneLoader.instance.actualSceneID;
                    HumanEvent human = new HumanEvent
                    {
                        level = sceneId,
                        count = count,
                    };
                    AnalyticsService.Instance.RecordEvent(human);
                    Debug.Log($"Lanzar Evento Human count: {count} levelid: {sceneId}");
                }
                text.text = "HUMAN TURN";
                break;
        }
    }

    public static void InvokeChangeMode(PlayerMode mode)
    {
        OnChangeMode?.Invoke(mode, true);
    }

    public static void InvokeChangeMode(PlayerMode mode, bool shouldInvokeHuman)
    {
        OnChangeMode?.Invoke(mode, shouldInvokeHuman);
    }
}
