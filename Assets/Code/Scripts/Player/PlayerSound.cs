using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    //used to get the current state the player is in
    public FirstPersonController firstPersonController;

    //the player's sound radius when sprinting
    public float sprintSoundRadius;
    //the player's sound radius when walking
    public float walkSoundRadius;
    //the player's sound radius when crouching
    public float crouchSoundRadius;

    //used to draw gizmos
    private float currentRadius;

    public float getCurrentSoundFootprint()
    {
        
        //player makes a lot of sound when running or jumping
        if ((firstPersonController.isSprinting && firstPersonController.isWalking) || !firstPersonController.isGrounded)
        {
            currentRadius = sprintSoundRadius;
            return sprintSoundRadius;
        }
        //player makes normal sound when walking
        else if (firstPersonController.isWalking && !firstPersonController.isCrouched)
        {
            currentRadius = walkSoundRadius;
            return walkSoundRadius;
        }
        //player makes little sound when crouching and walking
        else if (firstPersonController.isCrouched && firstPersonController.isWalking)
        {
            currentRadius = crouchSoundRadius;
            return crouchSoundRadius;
        }
        else //player makes no sound when standing still
        {
            currentRadius = 0;
            return 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, currentRadius);
    }
}
