using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the selection of memories on the memory tree in the hub world
/// </summary>
public class MemorySelector : MonoBehaviour
{   
    
    public List<MemoryString> memoryStrings = new();    //list holding all of the memory strings on the tree
    private Camera mainCamera;          //reference to the main camera (player camera)
    private Transform cameraTransform;  //used to hold the previous camera's position and rotation when moving it to a memory string
    private GameObject player;      //reference to the player game object
    public GameObject cameraParent; 
    public float cameraMoveTime = 1.0f; // Duration of camera movement in seconds
    public MemoryString activeMemoryString; //reference to the activated memory string
    public MemoryTreePhoto currentActivePhoto;  //reference to the active photo
    /// <summary>
    /// Gets the player object and camera reference
    /// </summary>
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        mainCamera = Camera.main;
    }

    /// <summary>
    /// Call the player input handler if the player is in a memory string
    /// </summary>
    void Update()
    {
        if (PlayerSingleton.Instance.playerInMemorySelection)
        {
            HandlePlayerInput();
        }
    }
    /// <summary>
    /// Handles player input while in the memory string
    /// </summary>
    void HandlePlayerInput()
    {
        //check if escape was pressed to cancel out of the string
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            
            activeMemoryString.DeselectPhoto(currentActivePhoto);
            StartCoroutine(MoveCameraSmooth(cameraParent.transform, activeMemoryString.transform, false));

        }
        //load the memory if e was pressed on an active photo
        else if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            LoadMemory();
        }

    }
    /// <summary>
    /// Currently hardcoded to load the terrain creation scene
    /// </summary>
    void LoadMemory()
    {
        SceneManager.LoadScene(activeMemoryString.sceneNameToLoad);
    }

    /// <summary>
    /// Set a memory string activated, called by the onUse component
    /// </summary>
    /// <param name="memoryString">The memory string component that was selected to use</param>
    public void ActivateMemoryString(MemoryString memoryString)
    {
        //get the active photo from the memory string, will be the first uncompleted memory
        var activeMemory = memoryString.GetActivePhoto();
        if (activeMemory != null)
        {
            PlayerSingleton.Instance.SelectMemoryString();  //Tell player singleton to select memory string to disable movement
            memoryString.SelectPhoto(activeMemory);         //set active photo, and string, then move the camera with coroutine
            currentActivePhoto = activeMemory;
            activeMemoryString = memoryString;
            StartCoroutine(MoveCameraSmooth(activeMemory.cameraAnchor, activeMemory.transform, true));
        }

    }
    /// <summary>
    /// Moves the camera from its current position to the target position and looks at the target over a short time
    /// </summary>
    /// <param name="targetAnchor">The target position to move the camera to</param>
    /// <param name="lookAtTarget">The target position to look at when finished moving</param>
    /// <param name="enableMemorySelect">true if memory is selected, false if memory selection is exited</param>
    /// <returns></returns>
    private IEnumerator MoveCameraSmooth(Transform targetAnchor, Transform lookAtTarget, bool enableMemorySelect)
    {
        //store current transform of camera to move it back to if needed
        Transform cameraTransform = mainCamera.transform;

        //set initial position and rotation for movement
        Vector3 startPosition = cameraTransform.position;
        Quaternion startRotation = cameraTransform.rotation;

        float elapsedTime = 0;

        while (elapsedTime < cameraMoveTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / cameraMoveTime;

            // Smoothly interpolate position
            cameraTransform.position = Vector3.Lerp(startPosition, targetAnchor.position, t);

            // Smoothly interpolate rotation
            Vector3 targetDirection = lookAtTarget.position - cameraTransform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            cameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        // Ensure final position and rotation are exact
        cameraTransform.position = targetAnchor.position;
        cameraTransform.LookAt(lookAtTarget);

        // Set the parent after reaching the final position
        cameraTransform.SetParent(targetAnchor);
        cameraTransform.localPosition = Vector3.zero;

        PlayerSingleton.Instance.playerInMemorySelection = enableMemorySelect;
        //if exiting out of the memory string, enable player movement at end
        if (!enableMemorySelect)
        {
            PlayerSingleton.Instance.DeselectMemory();
        }
        
    }



}
