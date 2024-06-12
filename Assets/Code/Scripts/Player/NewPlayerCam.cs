using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class NewPlayerCam : MonoBehaviour
{
    [SerializeField]
    PlayerMovement player;
    [SerializeField]
    GameObject cameraHolder;

    /// <summary>
    /// Game object that holds the entire player
    /// </summary>
    [SerializeField] GameObject playerPrefab;

    // Sensitivity variables for mouse movement
    public float sensX;
    public float sensY;

    // Current rotation values for X and Y axes
    float xRotation;
    float yRotation;

    private PlayerControls playerControls;
    private Vector2 lookInput;

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerControls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //if(player.state == PlayerMovement.MovementState.Talking) 
        //{
        //    return;
        //}

        // Get input from the mouse
        float mouseX = lookInput.x * Time.deltaTime * sensX;
        float mouseY = lookInput.y * Time.deltaTime * sensY;

        // Update the Y rotation based on mouse X movement
        yRotation += mouseX;

        // Update the X rotation based on mouse Y movement and clamp it to avoid flipping
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the rotation to the camera
        cameraHolder.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        // Apply the rotation to the player's orientation (horizontal rotation only)
        //orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        // Apply the rotation to the player as well
        playerPrefab.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void ZeroRotation()
    {
        cameraHolder.transform.rotation = Quaternion.identity;
    }

    public void SetYRotation(float yRotation)
    {
        this.yRotation = yRotation;
    }
}
