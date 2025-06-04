using UnityEngine;
using Unity.AI.Navigation;
public class NavMeshManager:MonoBehaviour
{
    [Header("NavMesh Settings")]
    public bool autoBakeOnStart=true;
    public LayerMask walkableAreas=-1; // Which surfaces can be walked on
    public LayerMask obstacleAreas=-1; // Which objects are obstacles
    
    private NavMeshSurface navMeshSurface;
    void Start()
    {
        SetupNavMesh();
        if(autoBakeOnStart)
        {
            BakeNavMesh();
        }
    }
    void SetupNavMesh()
    {
        // Get or add NavMeshSurface component
        navMeshSurface=GetComponent<NavMeshSurface>();
        if(navMeshSurface==null)
        {
            navMeshSurface=gameObject.AddComponent<NavMeshSurface>();
        }
        // Configure NavMesh settings for your room
        navMeshSurface.collectObjects=CollectObjects.Children;
        navMeshSurface.useGeometry=NavMeshCollectGeometry.PhysicsColliders;
        navMeshSurface.layerMask=walkableAreas;
        
        // Fine settings for small robot like Roomba
        navMeshSurface.overrideVoxelSize=true;
        navMeshSurface.voxelSize=0.1f;// Smaller for better detail
        navMeshSurface.overrideTileSize=true;
        navMeshSurface.tileSize=50;
        navMeshSurface.buildHeightMesh=true;
    }
    public void BakeNavMesh()
    {
        if(navMeshSurface!=null)
        {
            navMeshSurface.BuildNavMesh();
            Debug.Log("NavMesh build somplete");
        }
    }
    public void ClearNavMesh()
    {
        if(navMeshSurface!=null)
        {
            navMeshSurface.RemoveData();
            Debug.Log("NavMesh cleared!");
        }
    }
    // Call this if you move furniture or change the room layout
    public void UpdateNavMesh()
    {
        BakeNavMesh();
    }
}
