using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EventManager;
using Unity.Services.Analytics;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance {private set; get;}
    public bool isResetting = false;
    public int actualSceneID { get; private set; }

    private AsyncOperation _asyncOperation;

    [SerializeField] private List<string> scenes = new List<string>();
    

    private void Awake() {
        DontDestroyOnLoad(this);

        instance = this;
    }

#if UNITY_EDITOR
    private void OnValidate() {
        scenes = new List<string>();

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            scenes.Add(EditorBuildSettings.scenes[i].path.Split('/')[2].Split('.')[0]);
        }
    }
    #endif

    private void OnEnable()
    => SceneManager.sceneLoaded += OnSceneLoaded;

    private void OnDisable()
        => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex > 2)
        {
            if (!isResetting)
            {
                actualSceneID = scene.buildIndex - 2;
                LevelStartEvent levelStart = new LevelStartEvent
                {
                    level = actualSceneID
                };  
                AnalyticsService.Instance.RecordEvent(levelStart);
                Debug.Log($"Lanzar Level Start: {scene.name} id: {actualSceneID}");
            }
        }
    }

    public void StartLoadingScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncProcess(sceneName));
    }

    public void StartLoadingNextScene()
    {
        string nextScene = scenes[scenes.IndexOf(SceneManager.GetActiveScene().name) + 1];

        StartCoroutine(LoadSceneAsyncProcess(nextScene));
    }

    public void ActivateScene()
    {
        StartCoroutine(ActivateSceneAndDeactivate());
    }

    private IEnumerator LoadSceneAsyncProcess(string sceneName)
    {
        this._asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        this._asyncOperation.allowSceneActivation = false;

        while (!this._asyncOperation.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator ActivateSceneAndDeactivate()
    {
        yield return new WaitUntil( () => {return _asyncOperation.progress > 0.8f;} );
        _asyncOperation.allowSceneActivation = true;
        isResetting = false;
        yield return new WaitUntil( () => {return _asyncOperation.isDone;} );
        _asyncOperation.allowSceneActivation = false;
    }
}
