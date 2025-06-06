using UnityEngine;
using UnityEngine.AI;
public class Roomba: MonoBehaviour
{
    public Transform[] cleaningPoints; // Assign in Inspector
    public float stuckCheckTime=1f;  // How often to check if stuck
    public float stuckDistance=0.1f; // If moved less than this, considered stuck
    public float unstickRadius=2.0f; // How far to look for a random unstick point
    private NavMeshAgent agent;
    private int currentPoint=0;
    private Vector3 lastPosition;
    private float lastMoveTime;
    void Start()
    {
        agent=GetComponent<NavMeshAgent>();
        if(cleaningPoints.Length>0)
        {
            agent.destination=cleaningPoints[0].position;
            lastPosition=transform.position;
            lastMoveTime=Time.time;
        }
    }
    void Update()
    {
        if(cleaningPoints.Length==0) return;
        if(!agent.pathPending && agent.remainingDistance<0.2f)
        {
            currentPoint=(currentPoint+1)%cleaningPoints.Length;
            agent.destination=cleaningPoints[currentPoint].position;
            lastPosition=transform.position;
            lastMoveTime=Time.time;
        }
        if(Time.time-lastMoveTime>stuckCheckTime)
        {
            float distanceMoved=Vector3.Distance(transform.position, lastPosition);
            if(distanceMoved<stuckDistance)
            {
                // Pick a random nearby point on NavMesh to try to get unstuck
                Vector3 randomDirection=Random.insideUnitSphere*unstickRadius;
                randomDirection=randomDirection+transform.position;
                NavMeshHit hit;
                if(NavMesh.SamplePosition(randomDirection,out hit,1.0f,NavMesh.AllAreas))
                {
                    agent.destination=hit.position;
                }
            }
            lastPosition=transform.position;
            lastMoveTime=Time.time;
        }
    }
}
