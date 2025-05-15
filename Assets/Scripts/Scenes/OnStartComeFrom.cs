using UnityEngine;

public class OnStartComeFrom : MonoBehaviour 
{
    [SerializeField] private Vector3 gototo;
    [SerializeField] private LeanTweenType typeEase;

    private void Start() 
    {
        LeanTween.move(gameObject, gototo, 0.5f).setEase(typeEase);
    }
}