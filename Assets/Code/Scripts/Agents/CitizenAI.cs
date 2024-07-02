using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A Basic Citizen AI Agent that will just walk between its waypoints using navmesh
/// </summary>
public class CitizenAI : MonoBehaviour {
    
    public GameObject WaypointParent; //parent game object for the waypoints, to faciliate adding multiple waypoints in the inspector
    protected Transform[] waypoints; //array of waypoints to path between
    protected int currentWaypoint = 0; 
    protected NavMeshAgent agent; //reference to the Citizen's navmesh agent component


    /// <summary>
    /// Initializes the navmesh agent, waypoint array, and tells the Citizen to move to the first point
    /// </summary>
    protected virtual void Start() {
        //waypoints = WaypointParent.GetComponentsInChildren<Transform>();
        agent = GetComponent<NavMeshAgent>();
        if (WaypointParent)
        {
            SetWaypoints(WaypointParent);
        }
        GotoNextPoint();
    }

    /// <summary>
    /// Advances the citizen by incrementing current waypoint and passing the destination to the navmesh agent
    /// </summary>
    protected void GotoNextPoint()
    {
        if (waypoints.Length == 0)
            return;

        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        agent.destination = waypoints[currentWaypoint].position;
    }
    /// <summary>
    /// Checks each frame to see how close to the next waypoint the agent is in order to move it to the next one
    /// </summary>
    protected virtual void Update() {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
    /// <summary>
    /// Used to set the waypoint parent when instantiating the citizens from prefabs in code
    /// </summary>
    /// <param name="wayPointsParent">The parent game object holding the waypoint transforms</param>
    /// <param name="randomize">whether or not to randomize the order of the waypoints</param>
    public void SetWaypoints(GameObject wayPointsParent, bool randomize = false)
    {
        waypoints = wayPointsParent.GetComponentsInChildren<Transform>();
        if (randomize) waypoints = ShuffleWaypoints(waypoints);
    }
    /// <summary>
    /// Randomizes the order of the transform waypoints
    /// </summary>
    /// <param name="originalWaypoints">Array of waypoint transforms</param>
    /// <returns>An array with the same elements of the parameter array in a random order</returns>
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
