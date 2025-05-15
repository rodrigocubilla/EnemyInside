using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBlock : MonoBehaviour
{
    private void FixedUpdate() {
        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(0.2f, 0.2f), 0f);

        if (collider?.gameObject.tag == "Player")
        {
            Debug.Log("Go");
            SceneLoader.instance.ActivateScene();
        }
    }
}
