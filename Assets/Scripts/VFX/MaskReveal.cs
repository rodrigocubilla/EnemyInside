using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskReveal : MonoBehaviour
{
    [SerializeField] private float revealSpeed;
    [SerializeField] private Vector3 revealScale;
    [SerializeField] private LeanTweenType easeType;

    private bool onRevealMask => Input.GetKeyDown(KeyCode.M);
    private bool revealed = false;

    public static Action OnRevealMask;
    public static Action OnHideMask;

    private void Awake() 
    {
        OnRevealMask += RevealMask;
        OnHideMask += HideMask;
    }

    private void HideMask()
    {
        var player = FindObjectOfType<PlayerController>();

        if (player != null)
            gameObject.transform.position = player.gameObject.transform.position;

        LeanTween.scale(gameObject, new Vector3(0,0,0), revealSpeed).setEase(easeType);
        revealed = false;
    }

    private void Update() {
        if (onRevealMask)
            RevealMask();
    }

    private void RevealMask()
    {
        var player = FindObjectOfType<PlayerController>();

        if (player != null)
            gameObject.transform.position = player.gameObject.transform.position;

        if (revealed)
        {
            LeanTween.scale(gameObject, new Vector3(0,0,0), revealSpeed).setEase(easeType);
            revealed = false;
        }
        else
        {
            LeanTween.scale(gameObject, revealScale, revealSpeed).setEase(easeType);
            revealed = true;
        }
    }
}
