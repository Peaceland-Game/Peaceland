using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antoni : CitizenAI
{
    public GameObject player;
    public SphereCollider minimumMoveDistance;
    public SphereCollider loseAgentDistance;
    protected override void Update()
    {
        AntoniPathing();
    }
    protected void AntoniPathing()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            Wait();
    }

    protected void Wait()
    {
        if (minimumMoveDistance.bounds.Contains(player.transform.position))
        {
            GotoNextPoint();
        }
    }
    protected void BeginMoveToNextPoint()
    {
        GotoNextPoint();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerReference.PLAYER)
        {
            Debug.Log("Player Too far, lost Antoni");
        }
    }
}
