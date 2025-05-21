using System;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class Services : MonoBehaviour
{
    //Escena anterior a menu de inicio para preguntar por colecci�n de datos
    //[SerializeField] private SceneController sceneController;
    async void Awake()
    {
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
        //sceneController.StartGame();
    }

    //Iniciar juego sin recolecci�n de datos
    public void StopDataCollection()
    {
        //sceneController.StartGame();
    }
}
