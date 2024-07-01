using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntoniStealthAI : MonoBehaviour
{
    enum State
    {
        Patrol,
        Alert,
        Seek
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

    private int currentWaypoint = 0;
    private UnityEngine.AI.NavMeshAgent agent;

    private float seekTime;
    private float currentSeekTime;

    private Stealth stealthScript;

    void Start()
    {
        currentState = State.Patrol;
        stealthScript = GetComponent<Stealth>();
        currentSeekTime = stealthScript.secondsAgentSeeks;
        seekTime = currentSeekTime;
        currentDirection = 1;
        directionTimer = 0;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        GoToNextPoint();
    }

    void GoToNextPoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.destination = waypoints[currentWaypoint].position;
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }
    void GoToPlayerPoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.destination = playerPoint.position;
    }
    void Patrol()
    {
        agent.isStopped = false;
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextPoint();

        //if they spot the player, seek them
        if (stealthScript.detectedPlayer)
        {
            currentSeekTime = 0;
            currentState = State.Seek;
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

        //after a bit of no sound, go back on patrol
        if (!stealthScript.heardPlayer)
        {
            currentState = State.Patrol;
            currentWaypoint--;
            GoToNextPoint();
        }

        //if they see the player, seek them
        if (stealthScript.detectedPlayer)
        {
            currentSeekTime = 0;
            currentState = State.Seek;
        }
    }
    void Seek()
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
            currentState = State.Patrol;
            GoToNextPoint();
        }

        //if the agent loses the player after a bit, go on alert
        if (!stealthScript.detectedPlayer && currentSeekTime >= seekTime)
        {
            currentState = State.Alert;
        }
    }
    void Update()
    {
       // Debug.Log(currentState);
        switch (currentState)
        {
            //move from one waypoint to another
            case State.Patrol:
                Patrol();
                break;

            //spin around for an amount of time
            case State.Alert:
                Alert();
                break;

            //seek the player
            case State.Seek:
                
                break;
        }
        
    }
}
