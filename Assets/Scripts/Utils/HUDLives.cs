using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDLives : MonoBehaviour
{
    public static Action<int> OnLifeChange;

    [SerializeField] private List<GameObject> hearts = new List<GameObject>();

    private void Awake() {
        DontDestroyOnLoad(this);

        OnLifeChange += LifeChange;
    }

    private void LifeChange(int life)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(life > i);
        }
    }
}
