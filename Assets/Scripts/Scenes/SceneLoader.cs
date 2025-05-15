using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance {private set; get;}

    private AsyncOperation _asyncOperation;

    [SerializeField] private List<string> scenes = new List<string>();
    [SerializeField] private string actualScene;

    private void Awake() {
        DontDestroyOnLoad(this);

        instance = this;

        actualScene = SceneManager.GetActiveScene().name;
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

    public void StartLoadingScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncProcess(sceneName));
    }

    public void StartLoadingNextScene()
    {
        actualScene = SceneManager.GetActiveScene().name;

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
        Debug.Log("Dale");
        yield return new WaitUntil( () => {return _asyncOperation.progress > 0.8f;} );
        _asyncOperation.allowSceneActivation = true;
        yield return new WaitUntil( () => {return _asyncOperation.isDone;} );
        _asyncOperation.allowSceneActivation = false;
    }
}
