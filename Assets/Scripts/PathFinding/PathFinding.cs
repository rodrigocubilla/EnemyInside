using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private Transform level;
    [SerializeField] private GameObject defaultCube;
    [SerializeField] private Vector3Int offset;
    [SerializeField] private GameObject pathfindingAgent;
    [SerializeField] private List<Vector3> waypoints = new List<Vector3>();
    [SerializeField] private GameObject obstacleCylinder;
    [SerializeField] private Transform refCube;

    private List<Vector2Int> waypoints2d;
    private NavMeshPath path;
    private GameObject player;
    private Vector3Int previousPos;
    private Transform agent;
    private Vector3Int destiny;
    private bool finded;
    private List<GameObject> cubes = new List<GameObject>();

    public Action<Vector2Int[]> OnPathFinded;
    public Action OnCantReachPosition;

    void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerController>()?.gameObject;

        ConvertLevelTo3D(level);
    }

    private void Update()
    {
        if (agent.GetComponent<NavMeshAgent>().remainingDistance == 0f && !finded)
        {
            finded = true;

            waypoints2d = new List<Vector2Int>();
            foreach (var waypoint in waypoints)
                waypoints2d.Add((waypoint - offset).deajust());

            OnPathFinded?.Invoke(waypoints2d.ToArray());
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            PathFindToPoint(new Vector2Int((int)refCube.position.x, (int)refCube.position.y));
        }
    }

    private void ConvertLevelTo3D(Transform level)
    {
        for (int i = 0; i < level.childCount; i++)
        {
            var child = level.GetChild(i);
            var col = child.GetComponent<BoxCollider2D>();
            if (col != null && col.enabled)
                cubes.Add(Instantiate(defaultCube, child.position.adjusted() + offset, Quaternion.identity));
        }

#if UNITY_EDITOR
        // Solo para debug en editor: generar obstáculos visibles
        for (int y = -15; y <= 15; y++)
        {
            for (int x = -15; x <= 15; x++)
            {
                Instantiate(obstacleCylinder, new Vector3(x, y, 0).adjusted() + offset + new Vector3(.5f, 0f, .5f), Quaternion.identity);
            }
        }
#endif

        agent = Instantiate(pathfindingAgent, player.transform.position.adjusted() + offset, Quaternion.identity).transform;
        previousPos = agent.position.ToInt();
    }

    public void PathFindToPoint(Vector2Int point)
    {
        foreach (var cube in cubes)
            Destroy(cube);
        cubes.Clear();

        for (int i = 0; i < level.childCount; i++)
        {
            var child = level.GetChild(i);
            var col = child.GetComponent<BoxCollider2D>();
            if (col != null && col.enabled)
                cubes.Add(Instantiate(defaultCube, child.position.adjusted() + offset, Quaternion.identity));
        }

        var navAgent = agent.GetComponent<NavMeshAgent>();
        navAgent.isStopped = false;
        navAgent.Warp(player.transform.position.adjusted() + offset);
        agent.position = player.transform.position.adjusted() + offset;

        waypoints = new List<Vector3>();
        previousPos = agent.position.ToInt();
        waypoints.Add(previousPos);

        // Eliminado: NavMeshBuilder.BuildNavMesh();

        destiny = ((Vector3Int)point).adjusted() + offset;
        navAgent.SetDestination(destiny);

        StartCoroutine(WaitBeforeFind());
    }

    IEnumerator WaitBeforeFind()
    {
        yield return new WaitForSeconds(1f);
        finded = false;
    }

    private void FixedUpdate()
    {
        var navAgent = agent.GetComponent<NavMeshAgent>();
        if (navAgent.isStopped) return;

        if (navAgent.path.status == NavMeshPathStatus.PathPartial)
        {
            OnCantReachPosition?.Invoke();
            navAgent.isStopped = true;
            return;
        }

        if (agent == null) return;

        if (previousPos != agent.transform.position.ToInt())
        {
            previousPos = agent.transform.position.ToInt();
            waypoints.Add(previousPos);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (player)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(player.transform.position.adjusted() + offset, 0.5f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(destiny, 0.5f);

        if (waypoints2d == null || waypoints2d.Count < 2) return;

        Gizmos.color = Color.blue;
        for (int i = 0; i < waypoints2d.Count - 1; i++)
        {
            Gizmos.DrawLine((Vector2)waypoints2d[i], (Vector2)waypoints2d[i + 1]);
        }
    }
#endif

    public Vector2Int[] GetWaypoints()
    {
        return waypoints2d?.ToArray();
    }
}
