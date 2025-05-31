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
    public bool isGameOver = false;
    public int actualSceneID { get; private set; }

    private AsyncOperation _asyncOperation;

    [SerializeField] private List<string> scenes = new List<string>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // Ya existía otro SceneLoader, así que nos destruimos
            Destroy(gameObject);
            return;
        }
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
        // 1) Si estamos en modo “reset” (R) -> NO debemos emitir LevelStart
        if (isResetting || isGameOver)
        {
            isResetting = false;
            isGameOver = false;
            return;
        }

        // 2) Si llegamos aquí, es que no es un reset; lanzamos el LevelStart
        if (scene.buildIndex > 2)
        {
            actualSceneID = scene.buildIndex - 2;
            StaticVariables.level = actualSceneID;

            LevelStartEvent levelStart = new LevelStartEvent
            {
                level = StaticVariables.level
            };
            AnalyticsService.Instance.RecordEvent(levelStart);
            Debug.Log($"Lanzar Level Start: {scene.name} id: {actualSceneID}");

            StaticVariables.reset = false;
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
    public void CancelPendingAsyncLoad()
    {
        if (_asyncOperation != null && !_asyncOperation.isDone)
        {
            // Para “anularla” basta con perder la referencia.
            // También podrías forzar allowSceneActivation = false, pero en la práctica
            // si nunca le vuelves a dar “true” nunca se activará.
            _asyncOperation = null;
        }
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
        isGameOver = false;
        yield return new WaitUntil( () => {return _asyncOperation.isDone;} );
        _asyncOperation.allowSceneActivation = false;
    }
}
