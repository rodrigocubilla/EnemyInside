using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private LayerMask blockLayers;

    [Header("Lados a Bloquear")]
    [Tooltip("Marcar si este lado DEBE ser bloqueado")]
    [SerializeField] private bool requireBlockRight = false;
    [SerializeField] private bool requireBlockLeft = false;
    [SerializeField] private bool requireBlockUp = false;
    [SerializeField] private bool requireBlockDown = false;

    [Header("Estado Actual (Solo lectura)")]
    [SerializeField] private bool blockedRight;
    [SerializeField] private bool blockedLeft;
    [SerializeField] private bool blockedUp;
    [SerializeField] private bool blockedDown;

    public bool blocked;

    public bool hasPointToBlock = false;

    [SerializeField] private PointToBlock[] pointsToBlock; // Array de puntos a bloquear

    private void FixedUpdate() 
    {
        blockedRight = Physics2D.Raycast(transform.position + Vector3.right * transform.localScale.x/2, Vector2.right, 0.5f, blockLayers);
        blockedLeft = Physics2D.Raycast(transform.position + Vector3.left * transform.localScale.x/2, Vector2.left, 0.5f, blockLayers);
        blockedUp = Physics2D.Raycast(transform.position + Vector3.up * transform.localScale.x/2, Vector2.up, 0.5f, blockLayers);
        blockedDown = Physics2D.Raycast(transform.position + Vector3.down * transform.localScale.x/2, Vector2.down, 0.5f, blockLayers);

        // Verificación de lados requeridos
        bool rightValid = !requireBlockRight || blockedRight;
        bool leftValid = !requireBlockLeft || blockedLeft;
        bool upValid = !requireBlockUp || blockedUp;
        bool downValid = !requireBlockDown || blockedDown;


        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(0.2f, 0.2f), 0f);

        if (collider?.gameObject.tag == "Player")
        {   
            if (!collider.GetComponent<PlayerController>().isDead)
                collider.GetComponent<PlayerController>().Kill("Spike");
        }

        if(!hasPointToBlock)
        {
            blocked = rightValid && leftValid && upValid && downValid;
        }
        else if(hasPointToBlock && pointsToBlock.Length > 0)
        {
            blocked = false; // Asumimos que está bloqueado por defecto
            foreach (PointToBlock point in pointsToBlock)
            {
                if (point.isCovered) // Si algún punto no está cubierto, no está bloqueado
                {
                    blocked = true;
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float size = 0.3f;

        if (requireBlockRight) Gizmos.DrawRay(transform.position + Vector3.right * transform.localScale.x/2, Vector2.right * size);
        if (requireBlockLeft) Gizmos.DrawRay(transform.position + Vector3.left * transform.localScale.x/2, Vector2.left * size);
        if (requireBlockUp) Gizmos.DrawRay(transform.position + Vector3.up * transform.localScale.x/2, Vector2.up * size);
        if (requireBlockDown) Gizmos.DrawRay(transform.position + Vector3.down * transform.localScale.x/2, Vector2.down * size);
    }
}
