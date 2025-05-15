using UnityEngine;

public class AsyncSceneLoader : MonoBehaviour
{
    private void Start() 
    {
        SceneLoader.instance.StartLoadingNextScene();
    }
}
