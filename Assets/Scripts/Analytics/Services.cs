using System;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class Services : MonoBehaviour
{
    private SceneController sceneController;
    async void Awake()
    {
        sceneController = GetComponent<SceneController>();
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    //Iniciar juego con recoleccion de datos
    public void StartDataCollection()
    {
        AnalyticsService.Instance.StartDataCollection();
        sceneController.StartGame();
        Debug.Log("start data collection");
    }

    //Iniciar juego sin recolecci�n de datos
    public void StopDataCollection()
    {
        AnalyticsService.Instance.StartDataCollection();
        sceneController.StartGame();
        Debug.Log("stop data collection");
    }
}
