using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

enum State
{
    Patrol,
    Alert,
    Seek
}
public class PeacekeeperAI : MonoBehaviour
{
    private State currentState;

    //the waypoints the agent tries to move to
    public Transform[] waypoints;

    public Transform playerPoint;

    //the speed of rotation when alerted
    public int rotationPerSecond;

    private int currentWaypoint = 0;
    private UnityEngine.AI.NavMeshAgent agent;

    private float seekTime;
    private float currentSeekTime;

    void Start()
    {
        currentState = State.Patrol;
        currentSeekTime = gameObject.GetComponent<Stealth>().secondsAgentSeeks;
        seekTime = currentSeekTime;
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
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }

    void Update()
    {
        switch (currentState)
        {
            //move from one waypoint to another
            case State.Patrol:
                agent.isStopped = false;
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    GoToNextPoint();

                //if they spot the player, seek them
                if (gameObject.GetComponent<Stealth>().detectedPlayer)
                {
                    currentSeekTime = 0;
                    currentState = State.Seek;
                }

                //if they hear the player, go on alert
                if (gameObject.GetComponent<Stealth>().heardPlayer)
                {
                    currentState = State.Alert;
                }
                break;

            //spin around for an amount of time
            case State.Alert:
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                transform.Rotate(0, rotationPerSecond * Time.deltaTime, 0, 0);

                //after a bit of no sound, go back on patrol
                if (!gameObject.GetComponent<Stealth>().heardPlayer)
                {
                    currentState = State.Patrol;
                }

                //if they see the player, seek them
                if (gameObject.GetComponent<Stealth>().detectedPlayer)
                {
                    currentSeekTime = 0;
                    currentState = State.Seek;
                }
                break;

            //seek the player
            case State.Seek:
                agent.isStopped = false;
                    GoToPlayerPoint();

                //seek time increases
                currentSeekTime += Time.deltaTime;

                //if they see the player again, reset the time
                if (gameObject.GetComponent<Stealth>().detectedPlayer)
                {
                    currentSeekTime = 0;
                }

                //if the agent catches the player, FOR NOW, go back to patrol
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    currentState = State.Patrol;
                }  

                //if the agent loses the player after a bit, go on alert
                if (!gameObject.GetComponent<Stealth>().detectedPlayer && currentSeekTime >= seekTime)
                {
                    currentState = State.Alert;
                }
                break;
        }
        Debug.Log(currentState);
    }
}
