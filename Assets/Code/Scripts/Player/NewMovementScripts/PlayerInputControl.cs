using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputControl : MonoBehaviour
{
    [SerializeField] Movement movement;
    [SerializeField] MouseMovement mouseLook;

    Controls controls;
    Controls.PlayerKeyboardActions playerActions;

    // getting inputs for movement
    Vector2 horizontalInput;
    Vector2 mouseInput;

    // Start is called before the first frame update
    private void Awake()
    {
        controls = new Controls();
        playerActions = controls.PlayerKeyboard;

    }

    // Update is called once per frame
    void Update()
    {

        movement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);

    }
    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>();
        //Debug.Log(horizontalInput);

    }
    public void OnMouseX(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mouseInput.x = context.ReadValue<float>();
        }

    }
    public void OnMouseY(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mouseInput.y = context.ReadValue<float>();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movement.OnSprintPressed(true);
        }
        else
        {
            movement.OnSprintPressed(false);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movement.OnJumpPress();
            Debug.Log("jumping");
        }

    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movement.OnCrouchPress(true);
        }
        else
        {
            movement.OnCrouchPress(false);
        }
    }



}
