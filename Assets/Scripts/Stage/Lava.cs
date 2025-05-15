using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private LayerMask blockLayers;
    [SerializeField] private bool blockedRight;
    [SerializeField] private bool blockedLeft;
    [SerializeField] private bool blockedUp;
    [SerializeField] private bool blockedDown;

    public bool blocked;

    private void FixedUpdate() 
    {
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + Vector3.right * transform.localScale.x/2, Vector2.right, .5f, blockLayers); 
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + Vector3.left * transform.localScale.x/2, Vector2.left, .5f, blockLayers); 
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position + Vector3.up * transform.localScale.x/2, Vector2.up, .5f, blockLayers); 
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position + Vector3.down * transform.localScale.x/2, Vector2.down, .5f, blockLayers); 

        blockedRight = hitRight ? true : false;
        blockedLeft = hitLeft ? true : false;
        blockedUp = hitUp ? true : false;
        blockedDown = hitDown ? true : false;

        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(0.2f, 0.2f), 0f);

        if (collider?.gameObject.tag == "Player")
        {   
            if (!collider.GetComponent<PlayerController>().isDead)
                collider.GetComponent<PlayerController>().Kill();
        }

        blocked = blockedRight && blockedLeft && blockedUp && blockedDown;
    }
}
