using System;
using TMPro;
using UnityEngine;

public class HUDMovesView : MonoBehaviour 
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LeanTweenType easeType;

    private bool isHuman = true;

    private void Awake() {
        HUDEventReciever.OnChangeMode += OnChangeMode;

        HUDEventReciever.OnPlayerMove += MoveUpdate;

        DontDestroyOnLoad(this);
    }

    private void OnChangeMode(PlayerMode mode, bool shouldInvokeHuman)
    {
        switch (mode)
        {
            case PlayerMode.Monster:
                LeanTween.moveY(gameObject, -8f, 0.5f).setEase(easeType);
                isHuman = false;
                break;
            case PlayerMode.Human:
                LeanTween.moveY(gameObject, -5f, 0.5f).setEase(easeType);
                isHuman = true;
                break;
        }
    }

    private void MoveUpdate(int movesLeft) 
    {
        text.text = "MOVES: " + movesLeft;
    }
}