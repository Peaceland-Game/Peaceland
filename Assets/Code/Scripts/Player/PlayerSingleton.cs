using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralWorlds;
using PixelCrushers.DialogueSystem;
using UnityEngine.InputSystem;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private UserInterface userInterface;
    //private PlayerMovement playerMovement;
    public bool paused = false;
    private FirstPersonController controller;
    public bool isMouseLocked;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            controller = GetComponent<FirstPersonController>();
            //playerMovement = GetComponent<PlayerMovement>();
           // Gaia.GaiaAPI.SetRuntimePlayerAndCamera(gameObject, playerCamera, true);
        }
        else
        {
            Destroy(this);

        }
        
        //FloraAutomationAPI.SetRenderCamera(newCamera);

    }

    // Update is called once per frame
    void Update()
    {
        HandleInterfaceInput();
        isMouseLocked = Cursor.lockState == CursorLockMode.Locked;
    }

    void HandleInterfaceInput()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("escape pressed");
            
            paused = !paused;
            Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
            userInterface.TogglePauseMenu(paused);
            controller.enabled = !paused;

        }
    }

    void OnConversationEnd(Transform actor)
    {

    }
}
