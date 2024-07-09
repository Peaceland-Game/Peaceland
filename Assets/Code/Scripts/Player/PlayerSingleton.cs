using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralWorlds;
using PixelCrushers.DialogueSystem;
using UnityEngine.InputSystem;
using Gaia;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance;
    public JournalController journal;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private UserInterface userInterface;
    //private PlayerMovement playerMovement;
    public bool paused = false;
    private FirstPersonController controller;
    public bool isMouseLocked;
    public Selector selector;
    public bool playerInMemorySelection = false;
    [SerializeField]
    private Transform carryPos;
    private Transform heldItem;
  // public bool playerInHub = false;

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
    public void DisableMovement()
    {
        controller.enabled = false;
        selector.enabled = false;


    }
    public void EnableMovement()
    {
        controller.enabled = true;
        selector.enabled = true;
    }
    public void SelectMemoryString()
    {
        DisableMovement();
        
        userInterface.ToggleMemorySelectUI(true);
    }
    public void DeselectMemory()
    {
        EnableMovement();
        playerInMemorySelection = false;
        userInterface.ToggleMemorySelectUI(false);
    }

    void HandleInterfaceInput()
    {
        if (!playerInMemorySelection)
        {
           // Debug.Log("player not in memory select");
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                //   Debug.Log("escape pressed");
                TogglePauseMenu();

            }
            else if(Keyboard.current.eKey.wasPressedThisFrame)
            {
                ToggleJournal();
            }
        }
        //else if (Keyboard.current.jKey.wasPressedThisFrame)
        //{
        //    TakeJournalScreenshot();
        //}
    }
    //void TakeJournalScreenshot()
    //{
    //    journal.AddJournalEntry();
    //}


    public void ToggleJournal()
    {
        TogglePause();
        userInterface.ToggleJournal(paused);
    }

    public void TogglePauseMenu()
    {
        TogglePause();
        userInterface.TogglePauseMenu(paused);
    }

    /// <summary>
    /// Toggles whether in UI or first person mode 
    /// </summary>
    private void TogglePause()
    {
        paused = !paused;
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        controller.enabled = !paused;

    }

    void OnConversationEnd(Transform actor)
    {

    }

    public void PickUpItem(Transform item) 
    {
        item.parent = carryPos;
        item.localPosition = Vector3.zero;
        item.localRotation = Quaternion.identity;
        item.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        heldItem = item;
    }

    public void DropItem(Transform dropPoint) 
    {
        dropPoint.GetComponent<MeshRenderer>().enabled = false;
        heldItem.parent = dropPoint;
        heldItem.localPosition = Vector3.zero;
        heldItem.localRotation = dropPoint.localRotation;
        heldItem.localScale = dropPoint.localScale;
    }

    public void ResetItem(Transform resetPos) 
    {
        heldItem.parent = resetPos;
        heldItem.localPosition = Vector3.zero;
        heldItem.localRotation = resetPos.localRotation;
        heldItem.localScale = resetPos.localScale;
    }
}
