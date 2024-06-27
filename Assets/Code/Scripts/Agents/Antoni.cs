using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antoni : CitizenAI
{
    public GameObject player;
    public List<int> turnAroundWaypoints = new(){ 1, 2 };
    public SphereCollider minimumMoveDistance;
    public SphereCollider loseAgentDistance;
    public bool IsAiActive = false;

    [Header("Turning")]
    public float lookAroundDuration = 3f; // Time spent looking around
    public float rotationSpeed = 90f; // Degrees per second
    private bool isLookingAround = false;
    private float lookAroundTimer = 0f;

    protected override void Start()
    {
        base.Start();
        if (turnAroundWaypoints.Count > waypoints.Length)
            throw new System.IndexOutOfRangeException("Turn around locations is greater than number of waypoints!");
        
    }
    protected override void Update()
    {
        
        AntoniPathing();
    }
    public void StartAntoni()
    {
        IsAiActive = true;
    }
    private void StartLookingAround()
    {
        Debug.Log("Antoni is about to turn around");
        isLookingAround = true;
        lookAroundTimer = 0f;
        agent.isStopped = true;
    }

    private void LookAround()
    {
        lookAroundTimer += Time.deltaTime;
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if (lookAroundTimer >= lookAroundDuration)
        {
            isLookingAround = false;
            agent.isStopped = false;
            Wait();
        }
    }
    protected void AntoniPathing()
    {
        if (IsAiActive)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                if (turnAroundWaypoints.Contains(currentWaypoint))
                {
                    if (!isLookingAround)
                    {
                        StartLookingAround();
                    }
                    else
                    {
                        LookAround();
                    }
                }
                else
                {
                    Wait();
                }
            }
        }
    }

    protected void Wait()
    {
        Debug.Log($"Waiting at waypoint {currentWaypoint}");
        if (minimumMoveDistance.bounds.Contains(player.transform.position))
        {
            if (false)
            {
                // Player is caught, implement game over logic here
                Debug.Log("Player caught!");
            }
            else
            {
                GotoNextPoint();
            }
        }
    }
    protected void BeginMoveToNextPoint()
    {
        GotoNextPoint();
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.layer == LayerReference.PLAYER)
        //{
        //    Debug.Log("Player Too far, lost Antoni");
        //}
    }
}
