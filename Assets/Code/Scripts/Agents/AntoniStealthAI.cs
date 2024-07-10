using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AntoniStealthAI : MonoBehaviour
{
    enum State
    {
        WalkHome,
        Alert,
        AlmostSpotted,
        Spotted,
        TurnAround
    }

    private State currentState;

    //the waypoints the agent tries to move to
    public Transform[] waypoints;

    //the waypoints that the agent will turn around at
    public int[] turnAroundWaypoints;

    public Transform playerPoint;

    //the speed of rotation when alerted
    public int rotationPerSecondWhenAlerted;
    private int currentAlertedRotationDirection;
    //checks this every 1/10 of a second
    public float percentChanceToChangeRotation;
    private float directionTimer;

    //the waypoint the agent is currently traveling towards
    private int currentWaypoint = 0;
    private UnityEngine.AI.NavMeshAgent agent;

    private Stealth stealthScript;
    public VisionConeVisualizer visionConeVisualizer;
    public bool DisplayConeVisualizer = true;
    
    private float currentTurnAroundTimer;

    //how long the agent will look at the player before triggering an event
    public float timeBeforeAgentNoticesPlayer;
    private float currentTimeBeforeNoticed;
    private float timer;

    //TEMPORARY MATERIAL dinicating state
    public GameObject stateIndicator;
    public Material stateMaterial;


    void Start()
    {
        if (!DisplayConeVisualizer)
        {
            visionConeVisualizer.gameObject.SetActive(false);
        }
        currentState = State.WalkHome;
        stealthScript = GetComponent<Stealth>();
        currentAlertedRotationDirection = 1;
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
    void SwitchToWalkHome()
    {
        currentState = State.WalkHome;
        currentWaypoint--;
        GoToNextPoint();
    }
    void SwitchToAlmostSpotted()
    {
        currentTimeBeforeNoticed = 0.1f;
        timer = 0;
        currentState = State.AlmostSpotted;
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
            SwitchToAlmostSpotted();

        //if they hear the player, go on alert
        if (stealthScript.heardPlayer)
            currentState = State.Alert;
    }
    void Alert()
    {
        //stop pathfinding
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        //rotate around y axis
        transform.Rotate(0, rotationPerSecondWhenAlerted * currentAlertedRotationDirection * Time.deltaTime, 0, 0);

        //every 1/10th of a second, check if the agent should change direction
        directionTimer += Time.deltaTime;
        if (directionTimer >= 0.1f && (int)(Random.value * 100) <= percentChanceToChangeRotation)
            currentAlertedRotationDirection *= -1;

        if (directionTimer >= 0.1f)
            directionTimer = 0;

        //after a bit of no sound, go back on WalkHome
        if (!stealthScript.heardPlayer)
            SwitchToWalkHome();

        //if they see the player, seek them
        if (stealthScript.detectedPlayer)
            SwitchToAlmostSpotted();
    }
    void AlmostSpotted()
    {
        agent.isStopped = false;
        agent.velocity = Vector3.zero;
        GoToPlayerPoint();
        
        //if they continue to detect the player, get closer to spotting them
        if (stealthScript.detectedPlayer)
        {
            currentTimeBeforeNoticed += Time.deltaTime;
            timer = 0;
        }
            
        //if they are not detecting them, get closer to being alerted
        if (!stealthScript.detectedPlayer && timer < 0.5)
            timer += Time.deltaTime;
            

        else if (!stealthScript.detectedPlayer && timer >= 0.5)
            currentTimeBeforeNoticed -= Time.deltaTime/4;   

        //spot them if they detected them for long enough
        if (currentTimeBeforeNoticed > timeBeforeAgentNoticesPlayer + 0.1f)
            currentState = State.Spotted;

        //lose them if they did not see them for long enough
        if (currentTimeBeforeNoticed <= 0)
            SwitchToWalkHome();
    }
    void Spotted()
    {
        agent.isStopped = false;
        agent.velocity = Vector3.zero;
        GoToPlayerPoint();

        //if the agent loses the player , go on alert
        if (!stealthScript.detectedPlayer)
        {
            //currentState = State.Alert;
        }
            
    }
    void TurnAround()
    {
        //stop pathfinding
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        //rotate around y axis until they have turned 180 degrees
        currentTurnAroundTimer += Time.deltaTime;
        if (currentTurnAroundTimer <= 1.5)
            transform.Rotate(0, 120 * Time.deltaTime, 0, 0);
        //once they have rotated enough, wait an amount of time
        else
        {
            currentWaypoint++;
            SwitchToWalkHome();
        }
            

        //if they hear the player, go on alert
        if (stealthScript.heardPlayer)
            currentState = State.Alert;

        //if they see the player, seek them
        if (stealthScript.detectedPlayer)
            SwitchToAlmostSpotted();
    }
    void Update()
    {
        Debug.Log(currentState);
        stateIndicator.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        switch (currentState)
        {
            //move from one waypoint to another
            case State.WalkHome:
                stateMaterial.color = Color.green;
                WalkHome();
                break;

            //spin around for an amount of time
            case State.Alert:
                stateMaterial.color = Color.yellow;
                Alert();
                break;

            //look at the player, but do not act (player can run)
            case State.AlmostSpotted:
                stateMaterial.color = new Color(1, 0.6f, 0);//orange
                AlmostSpotted();
                break;

            //player was spotted, the "caught" event can run
            case State.Spotted:
                stateMaterial.color = Color.red;
                Spotted();
                break;

            //turn 180 degrees
            case State.TurnAround:
                stateMaterial.color = Color.blue;
                TurnAround();
                break;
        }

    }
}
