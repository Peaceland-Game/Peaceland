using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCam : MonoBehaviour
{
    [SerializeField]
    PlayerMovement player;

    /// <summary>
    /// Game object that holds the entire player
    /// </summary>
    [SerializeField] GameObject playerPrefab;

    // Sensitivity variables for mouse movement
    public float sensX;
    public float sensY;

    // Reference to the player's orientation
    public Transform orientation;

    // Current rotation values for X and Y axes
    float xRotation;
    float yRotation;

    private void Start()
    {
        // Lock the cursor to the center of the screen and hide it
       // Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(player.state == PlayerMovement.MovementState.Talking) 
        {
            return;
        }

        // Get input from the mouse
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        // Update the Y rotation based on mouse X movement
        yRotation += mouseX;

        // Update the X rotation based on mouse Y movement and clamp it to avoid flipping
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the rotation to the camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        // Apply the rotation to the player's orientation (horizontal rotation only)
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        //Turn the player model as well
        playerPrefab.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    public void ZeroRotation()
    {
        transform.rotation = Quaternion.identity;
    }

    public void SetYRotation(float yRotation) 
    {
        this.yRotation = yRotation;
    }
}
