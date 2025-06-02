using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CleaningRobot : MonoBehaviour
{
    [Header(" Elements")]
    [SerializeField] private AudioSource robotSound;
    [SerializeField] private AudioSource completeSound;
    private NavMeshAgent agent;
    private bool[,] cleanedTiles; //status of tiles
    private Vector2 gridSize;
    private float tileSize = 3f; // element size in the grid
    private Vector2 gridStart; // start point
    private bool doneFlag =  false; //completing flag

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        InitializeGrid();
        robotSound.Play();
        SetNextDestination();
    }

    void InitializeGrid()
    {
        // get grid bound from environment
        Bounds navMeshBounds = GetNavMeshBounds();
        Debug.Log("Bound x: "+navMeshBounds.min.x + " , " + navMeshBounds.max.x+
            " z: " + navMeshBounds.min.z + " , " + navMeshBounds.max.z);
        gridStart = new Vector2(navMeshBounds.min.x, navMeshBounds.min.z);
        gridSize = new Vector2(
            Mathf.CeilToInt(navMeshBounds.size.x / tileSize),
            Mathf.CeilToInt(navMeshBounds.size.z / tileSize)
        );
        cleanedTiles = new bool[(int)gridSize.x, (int)gridSize.y];
    }

    void SetNextDestination()
    {
        if (doneFlag) return;
        // Find the nearest uncleaned tile
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (!cleanedTiles[x, y])
                {
                    Vector3 targetWorldPos = new Vector3(
                        gridStart.x + x * tileSize, 0, gridStart.y + y * tileSize);
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(targetWorldPos, out hit, tileSize, NavMesh.AllAreas))
                    {
                        agent.SetDestination(hit.position);
                        cleanedTiles[x, y] = true; //set true after the target tile is cleaned
                        return;
                    }
                }
            }
        }
        // All tiles cleaned
        Debug.Log("Cleaning complete!");
        robotSound.Stop();
        completeSound.Play();
        doneFlag = true;
    }

    void Update()
    {
        //if reaching the target , then go to next
        if (!agent.pathPending && agent.remainingDistance < 2f)
        {
            SetNextDestination();
        }
    }

    Bounds GetNavMeshBounds()
    {
        // Get NavMesh triangle data
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        if (navMeshData.vertices.Length == 0)
        {
            Debug.LogWarning("No NavMesh found!");
            return new Bounds();
        }

        // Compare each vertices to get min and max value
        Vector3 min = navMeshData.vertices[0];
        Vector3 max = navMeshData.vertices[0];
        foreach (Vector3 vertex in navMeshData.vertices)
        {
            min = Vector3.Min(min, vertex);
            max = Vector3.Max(max, vertex);
        }

        // Create bounds 
        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    private void OnDrawGizmos() //for debugging
    {
        if (cleanedTiles == null || cleanedTiles.Length == 0) return;
        //draw the cubes for each tile
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 targetWorldPos = new Vector3(
                        gridStart.x + x * tileSize, 0, gridStart.y + y * tileSize
                    );
                if (cleanedTiles[x, y])
                    Gizmos.color = Color.green; //turn green for next target
                else
                    Gizmos.color = Color.yellow; 
                Gizmos.DrawCube(targetWorldPos, Vector3.one * (tileSize / 2));
            }
        }
        //draw the direction of movement
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
    }
}
