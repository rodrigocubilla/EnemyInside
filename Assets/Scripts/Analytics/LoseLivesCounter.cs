using System;
using UnityEngine;

public class LoseLivesCounter : MonoBehaviour
{
    public static LoseLivesCounter Instance {  get; private set; }
    [NonSerialized] public int losingLives = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }
}
