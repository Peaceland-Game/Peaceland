using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AntoniStealthAI : MonoBehaviour
{
    enum State
    {
        WalkHome,
        Alert,
        Spotted,
        TurnAround
    }

    private State currentState;

    //the waypoints the agent tries to move to
    public Transform[] waypoints;

    public Transform playerPoint;

    //the speed of rotation when alerted
    public int rotationPerSecond;
    private int currentDirection;
    //checks this every 1/10 of a second
    public float percentChanceToChangeRotation;
    private float directionTimer;

    //the waypoint the agent is currently traveling towards
    private int currentWaypoint = 0;
    private UnityEngine.AI.NavMeshAgent agent;

    //controls how long the agent seeks the player after they are spotted
    private float seekTime;
    private float currentSeekTime;

    private Stealth stealthScript;
    public VisionConeVisualizer visionConeVisualizer;
    public bool DisplayConeVisualizer = true;

    //the waypoints that the agent will turn around at
    public int[] turnAroundWaypoints;
    private float currentTurnAroundTimer;

    void Start()
    {
        if (!DisplayConeVisualizer)
        {
            visionConeVisualizer.gameObject.SetActive(false);
        }
        currentState = State.WalkHome;
        stealthScript = GetComponent<Stealth>();
        currentSeekTime = stealthScript.secondsAgentSeeks;
        seekTime = currentSeekTime;
        currentDirection = 1;
        directionTimer = 0;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    //the agent walks to a new waypoint
    void GoToNextPoint()
    {
        if (waypoints.Length == 0)
            return;
        
        agent.destination = waypoints[currentWaypoint].position;
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }

    //the agent goes to the player
    void GoToPlayerPoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.destination = playerPoint.position;
    }

    void WalkHome()
    {
        agent.isStopped = false;
        //if the agent reaches a waypoint
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            //check if it is a turnaround waypoint
            foreach (int i in turnAroundWaypoints)
            {
                if (currentWaypoint == i)
                {
                    //set y degrees
                    currentTurnAroundTimer = 0;
                    currentState = State.TurnAround;
                    return;
                }
            }
                
            //if not, pathfind to next point
            GoToNextPoint();
        }
            

        //if they spot the player, seek them
        if (stealthScript.detectedPlayer)
        {
            currentSeekTime = 0;
            currentState = State.Spotted;
            Debug.Log("Player detected");
        }

        //if they hear the player, go on alert
        if (stealthScript.heardPlayer)
        {
            currentState = State.Alert;
        }
    }
    void Alert()
    {
        //stop pathfinding
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        //rotate around y axis
        transform.Rotate(0, rotationPerSecond * currentDirection * Time.deltaTime, 0, 0);

        //every 1/10th of a second, check if the agent should change direction
        directionTimer += Time.deltaTime;
        if (directionTimer >= 0.1f && (int)(Random.value * 100) <= percentChanceToChangeRotation)
        {
            currentDirection *= -1;
        }

        if (directionTimer >= 0.1f)
        {
            directionTimer = 0;
        }

        //after a bit of no sound, go back on WalkHome
        if (!stealthScript.heardPlayer)
        {
            currentState = State.WalkHome;
            currentWaypoint--;
            GoToNextPoint();
        }

        //if they see the player, seek them
        if (stealthScript.detectedPlayer)
        {
            currentSeekTime = 0;
            currentState = State.Spotted;
            Debug.Log("Player detected");
        }
    }
    void Spotted()
    {
        agent.isStopped = false;
        agent.velocity = Vector3.zero;
        GoToPlayerPoint();

        //seek time increases
        currentSeekTime += Time.deltaTime;

        //if they see the player again or are within the seek radius, reset the time
        if (stealthScript.detectedPlayer ||
            stealthScript.distanceToPlayer <= stealthScript.alwaysPersueRadius)
        {
            currentSeekTime = 0;
        }

        //if the agent catches the player, FOR NOW, go back to patrol
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.WalkHome;
            GoToNextPoint();
        }

        //if the agent loses the player after a bit, go on alert
        if (!stealthScript.detectedPlayer && currentSeekTime >= seekTime)
        {
            currentState = State.Alert;
        }
    }
    void TurnAround()
    {
        //stop pathfinding
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        //rotate around y axis until they have turned 180 degrees
        currentTurnAroundTimer += Time.deltaTime;
        if (currentTurnAroundTimer <= 1)
            transform.Rotate(0, 180 * Time.deltaTime, 0, 0);
        //once they have rotated enough, wait an amount of time
        else
        {
            Debug.Log("done rotating");
            currentState = State.WalkHome;
            GoToNextPoint();
        }
            

        //if they hear the player, go on alert
        if (stealthScript.heardPlayer)
        {
            currentState = State.Alert;
        }

        //if they see the player, seek them
        if (stealthScript.detectedPlayer)
        {
            currentSeekTime = 0;
            currentState = State.Spotted;
            Debug.Log("Player detected");
        }
    }
    void Update()
    {
        Debug.Log(currentState);
        switch (currentState)
        {
            //move from one waypoint to another
            case State.WalkHome:
                WalkHome();
                break;

            //spin around for an amount of time
            case State.Alert:
                Alert();
                break;

            //seek the player
            case State.Spotted:
                Spotted();
                break;

            //turn 180 degrees
            case State.TurnAround:
                TurnAround();
                break;
        }

    }
}
