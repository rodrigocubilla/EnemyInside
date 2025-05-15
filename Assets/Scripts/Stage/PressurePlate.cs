using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public UnityEvent OnOver;
    public LayerMask pushables;

    private void FixedUpdate() 
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(0.2f, 0.2f), 0f);

        if (collider?.gameObject.tag == "Player")
        {   
            Debug.Log("dale");
            if (!collider.GetComponent<PlayerController>().isDead)
                OnOver.Invoke();
        }

        if (collider?.gameObject.layer == 7)
        {
            OnOver.Invoke();
        }
    }
}
