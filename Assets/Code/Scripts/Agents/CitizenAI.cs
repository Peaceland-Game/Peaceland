using UnityEngine;
using UnityEngine.AI;

public class CitizenAI : MonoBehaviour {
    public GameObject WaypointParent;
    protected Transform[] waypoints;
    protected int currentWaypoint = 0;
    protected NavMeshAgent agent;

    void Start() {
        //waypoints = WaypointParent.GetComponentsInChildren<Transform>();
        agent = GetComponent<NavMeshAgent>();
        if (WaypointParent)
        {
            SetWaypoints(WaypointParent);
        }
        GotoNextPoint();
    }

    protected void GotoNextPoint() {
        if (waypoints.Length == 0)
            return;

        agent.destination = waypoints[currentWaypoint].position;
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }

    protected virtual void Update() {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
    public void SetWaypoints(GameObject wayPointsParent, bool randomize = false)
    {
        waypoints = wayPointsParent.GetComponentsInChildren<Transform>();
        if (randomize) waypoints = ShuffleWaypoints(waypoints);
    }
    Transform[] ShuffleWaypoints(Transform[] originalWaypoints)
    {
        Transform[] shuffledWaypoints = (Transform[])originalWaypoints.Clone();
        for (int i = 0; i < shuffledWaypoints.Length; i++)
        {
            Transform temp = shuffledWaypoints[i];
            int randomIndex = Random.Range(i, shuffledWaypoints.Length);
            shuffledWaypoints[i] = shuffledWaypoints[randomIndex];
            shuffledWaypoints[randomIndex] = temp;
        }
        return shuffledWaypoints;
    }
    
    
}
