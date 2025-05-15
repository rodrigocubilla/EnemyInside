using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum PlayerMode
{
    Monster,
    Human
}

public class HUDEventReciever : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;

    public static Action<PlayerMode> OnChangeMode;
    public static Action<int> OnPlayerMove;

    private void Awake() {
        OnChangeMode += OnChangeModeHandler;

        DontDestroyOnLoad(this);
    }

    private void OnChangeModeHandler(PlayerMode mode)
    {
        switch (mode)
        {
            case PlayerMode.Monster:
                text.text = "MONSTER TURN";
                break;
            case PlayerMode.Human:
                text.text = "HUMAN TURN";
                break;
        }
    }
}
