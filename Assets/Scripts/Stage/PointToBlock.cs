using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToBlock : MonoBehaviour
{
    public bool isCovered = false;

    [SerializeField] private LayerMask blockLayers; // Layers de "Solid" y "Pushable"
    //[SerializeField] private KillZone linkedKillZone; // KillZone asociada a este punto

    private void FixedUpdate()
    {
        // Verifica si hay un objeto bloqueante en este punto
        Collider2D blocker = Physics2D.OverlapCircle(transform.position, 0.2f, blockLayers);
        if (blocker != null)
        {
            //linkedKillZone.ForceBlockedStatus(true); // Forza el estado "bloqueado"
            isCovered = true; // Marca como cubierto
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
