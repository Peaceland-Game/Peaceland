using UnityEngine;
using UnityEngine.AI;

public class CitizenAI : MonoBehaviour {
    // public GameObject WaypointParent;
    public Transform[] waypoints;
    private int currentWaypoint = 0;
    private NavMeshAgent agent;

    void Start() {
        //waypoints = WaypointParent.GetComponentsInChildren<Transform>();
        agent = GetComponent<NavMeshAgent>();
        GotoNextPoint();
    }

    void GotoNextPoint() {
        if (waypoints.Length == 0)
            return;

        agent.destination = waypoints[currentWaypoint].position;
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }

    void Update() {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
}
