using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingPuzzle : Interactable
{
    [SerializeField] Delay ringPuzzleManager;
    [SerializeField] GameObject ringPuzzleObj;
    [SerializeField] float walkawayRange;
    private Transform player;

    bool cleanup = false;

    private void Start()
    {
        ringPuzzleObj.SetActive(false);
    }

    private void Update()
    {
        if(player != null)
        {
            bool inRange = Vector3.Distance(this.transform.position, player.transform.position) <= walkawayRange;

            if(!inRange)
            {
                // Pause puzzle when out of range 
                ringPuzzleObj.SetActive(false);
                ringPuzzleManager.canRun = false;
            }
        }

        if(ringPuzzleManager.isComplete && !cleanup)
        {
            // Close game open door 

            ringPuzzleObj.SetActive(false);

            CamFocusInteractable focus = this.GetComponent<CamFocusInteractable>(); 
            if(focus != null)
            {
                focus.ResetThisFocus();
            }
        }
    }

    public override void Interact(Transform source)
    {
        if (ringPuzzleManager.isComplete)
            return;

        Inventory inventory = source.GetComponent<Inventory>();
        if (inventory == null)
            return;

        if (!inventory.hasKey)
            return;

        player = source;

        ringPuzzleObj.SetActive(true);
        ringPuzzleManager.canRun = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, walkawayRange);
    }
}
