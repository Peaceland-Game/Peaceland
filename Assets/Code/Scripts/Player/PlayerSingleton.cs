using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralWorlds;
using PixelCrushers.DialogueSystem;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Gaia;
using UnityEngine.SceneManagement;


public class MoneyCollectedEvent : UnityEvent { }

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance;
    [SerializeField] private Tablet tablet;
    // private Camera playerCamera;
    [SerializeField] private UserInterface userInterface;
    //private PlayerMovement playerMovement;
    public bool paused = false;
    [SerializeField] private FirstPersonController controller;
    public bool isMouseLocked;
    [SerializeField] private Selector selector;
    public bool playerInMemorySelection = false;
    
    public MoneyCollectedEvent onMoneyCollected = new MoneyCollectedEvent();

    public GameObject playerObject;

    // public bool playerInHub = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    //void Start()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //        SceneManager.sceneLoaded += OnSceneLoaded;

    //        //playerMovement = GetComponent<PlayerMovement>();
    //       // Gaia.GaiaAPI.SetRuntimePlayerAndCamera(gameObject, playerCamera, true);
    //    }
    //    else
    //    {
    //        Destroy(this);

    //    }

    //    //FloraAutomationAPI.SetRenderCamera(newCamera);

    //}

    public void FindPlayerInScene()
    {
        if (!playerObject)
        {
          //  Debug.Log("Found new player object, redoing references");
            playerObject = GameObject.FindWithTag("Player");
            if (playerObject)
            {
                var playerRef = playerObject.GetComponent<PlayerObjectReference>();
                userInterface = playerRef.userInterface;
                userInterface.RegisterEventListener();
                controller = playerRef.controller;
                tablet = playerRef.tablet;
            }
            else
            {
                throw new System.Exception("Player is missing!");
            }
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayerInScene();

    }



    // Update is called once per frame
    void Update()
    {
        if (playerObject == null)
        {

        }
        HandleInterfaceInput();
        isMouseLocked = Cursor.lockState == CursorLockMode.Locked;


    }
    public void DisableMovement()
    {
        if (playerObject)
        {
            if (!selector)
            {
                selector = playerObject.GetComponent<Selector>();
            }
            if (!controller)
            {
                controller = playerObject.GetComponent<FirstPersonController>();
            }
        }
        controller.enabled = false;
        selector.enabled = false;


    }
    public void EnableMovement()
    {
        if (playerObject)
        {
            if (!selector)
            {
                selector = playerObject.GetComponent<Selector>();
            }
            if (!controller)
            {
                controller = playerObject.GetComponent<FirstPersonController>();
            }
        }
        Cursor.lockState = CursorLockMode.Locked;
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
                //TogglePauseMenu();
                ToggleJournal();

            }
            //else if(Keyboard.current.qKey.wasPressedThisFrame)
            //{
            //    ToggleJournal();
            //}
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
    public void AddMoney(double amt)
    {
        var money = DialogueLua.GetVariable("PlayerMoney").asInt;
        money += (int)amt;
        DialogueLua.SetVariable("PlayerMoney", money);

        onMoneyCollected.Invoke();
    }
    public void ForceSubtractMoney(double amt)
    {
        var money = DialogueLua.GetVariable("PlayerMoney").asInt;
        DialogueLua.SetVariable("PlayerMoney", money - (int)amt);
        onMoneyCollected.Invoke();
    }
    public bool SubtractMoney(double amt)
    {
        var money = DialogueLua.GetVariable("PlayerMoney").asInt;
        if (amt > money) return false;
        DialogueLua.SetVariable("PlayerMoney", money - (int)amt);
        onMoneyCollected.Invoke();
        return true;
    }

    void OnConversationEnd(Transform actor)
    {

    }

    

    

    

    public void StopPlayer()
    {
        controller.StopPlayer();
    }

    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction(nameof(StopPlayer), this, SymbolExtensions.GetMethodInfo(() => StopPlayer()));
        Lua.RegisterFunction(nameof(AddMoney), this, SymbolExtensions.GetMethodInfo(() => AddMoney(0)));
        Lua.RegisterFunction(nameof(ForceSubtractMoney), this, SymbolExtensions.GetMethodInfo(() => ForceSubtractMoney(0)));
        Lua.RegisterFunction(nameof(SubtractMoney), this, SymbolExtensions.GetMethodInfo(() => SubtractMoney(0)));
        // Lua.RegisterFunction(nameof(AddOne), this, SymbolExtensions.GetMethodInfo(() => AddOne((double)0)));
    }
    /*
    void OnDisable()
    {
        if (true)
        {
            // Remove the functions from Lua: (Replace these lines with your own.)
            Lua.UnregisterFunction(nameof(StopPlayer));
            //   Lua.UnregisterFunction(nameof(AddOne));
        }
    }
    */
}
