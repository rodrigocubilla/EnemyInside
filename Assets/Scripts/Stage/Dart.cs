using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Start() 
    {
        Destroy(gameObject, 10f);
    }

    private void FixedUpdate() 
    {
        transform.Translate(Vector3.up * Time.fixedDeltaTime * speed);
    }

    private void LateUpdate() 
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(0.2f, 0.2f), 0f);

        if (collider?.gameObject.tag == "Player")
        {   
            if (!collider.GetComponent<PlayerController>().isDead)
            {
                collider.GetComponent<PlayerController>().Kill("Dart");
                Destroy(gameObject);
            }
        }

        if (collider?.gameObject.layer == 7 || collider?.gameObject.layer == 6)
        {   
            Destroy(gameObject);
        }
    }
}
