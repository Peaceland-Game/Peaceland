using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralWorlds;
using PixelCrushers.DialogueSystem;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Gaia;
using UnityEngine.SceneManagement;

/// <summary>
/// Custom event to raise when the player collects money, listened for in the ui script
/// </summary>
public class MoneyCollectedEvent : UnityEvent { }

/// <summary>
/// Handles various player functions such as toggling pause menu, picking up money, and storing karma information
/// </summary>
public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance;                     //singleton
    private Tablet tablet;                     //reference to the tablet on the user interface
    private UserInterface userInterface;       //reference to the user interface object

    [SerializeField] private FirstPersonController controller;  //refrence to player movement controller

    private Selector selector;                 //reference to the player's selector component

    private UniversalSoundManager soundManager;

    public bool playerInMemorySelection = false;                //flag is the player is the memory selection, this is only used in the hub world
    public bool paused = false;                                 //flag if game is paused
    public bool isMouseLocked;                                  //flag if the mouse is locked to the screen or not

    public MoneyCollectedEvent onMoneyCollected = new MoneyCollectedEvent();    //create event
    public GameObject playerObject;                                             //reference to the player parent object in the scene

    [Header("Notifications")]
    public GameObject karmaNotificationPrefab;

    public int GetMoney
    {
        get { return DialogueLua.GetVariable("PlayerMoney").asInt; }
    }

    [Header("Karma Points")]
    [SerializeField] private double nuetralMaxKarma = 8;
    public Dictionary<string, double> karmaPoints = new();                      //holds karma information


    /// <summary>
    /// register singleton and onSceneLoaded event listener
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Add a theme to the dictionary
    /// </summary>
    /// <param name="theme">The key, or theme that should be added</param>
    /// <param name="amt">The amount of karma pertaining to that theme/key</param>
    public void AddTheme(string theme, double amt)
    {
        soundManager.ThemeGet();

        if (karmaPoints.ContainsKey(theme))
        {
            karmaPoints[theme] += amt;
        }
        else
        {
            tablet.AddTheme(theme);
            karmaPoints.Add(theme, amt);
            //Instantiate(karmaNotificationPrefab);
            if (NotificationManager.Instance != null)
            {
                // NotificationManager.Instance.QueueNotification(karmaNotificationPrefab);
                NotificationManager.Instance.QueueNotification(NotificationType.KarmaPopup);

            }
            else
            {
                Debug.LogWarning("NotificationManager is not initialized!");
            }
        }
    }
    /// <summary>
    /// Simple coroutine to wait for a certain amount of time before executing an action
    /// </summary>
    /// <param name="seconds">time to wait in seconds</param>
    /// <param name="action">runnable to perform after waiting</param>
    /// <returns></returns>
    public IEnumerator WaitThen(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }

    /// <summary>
    /// Get the amount of karma for a specific theme
    /// </summary>
    /// <param name="theme">The theme/key to check</param>
    /// <returns></returns>
    public double GetThemeKarma(string theme)
    {
        if (karmaPoints.TryGetValue(theme, out double val))
        {
            return val;
        }
        return 0;
    }
    /// <summary>
    /// Used to initialize the player singleton with the player object
    /// </summary>
    /// <param name="playerRef">player reference object instance that stores the player data</param>
    public void InitPlayer(PlayerObjectReference playerRef)
    {
        playerObject = playerRef.gameObject;    
        userInterface = playerRef.userInterface;
        userInterface.RegisterEventListener();
        controller = playerRef.controller;
        tablet = playerRef.tablet;
    }
    /// <summary>
    /// Getter for sound manager component
    /// </summary>
    /// <param name="mgr">The sound manager component</param>
    public void GetSoundManager(UniversalSoundManager mgr)
    {
        soundManager = mgr;
        controller.GetSoundManager(soundManager);
    }

    /// <summary>
    /// Handle UI input every frame
    /// </summary>
    void Update()
    {
        HandleInterfaceInput();
        isMouseLocked = Cursor.lockState == CursorLockMode.Locked;
    }
    /// <summary>
    /// turns off movement
    /// </summary>
    public void DisableMovement()
    {
        ToggleMovement(false);
    }
    /// <summary>
    /// Turns on or off movement and selector components
    /// </summary>
    /// <param name="canMove">boolean if movement should be enabled or not</param>
    private void ToggleMovement(bool canMove) {
        if (playerObject) {
            if (!selector) {
                selector = playerObject.GetComponent<Selector>();
            }
            if (!controller) {
                controller = playerObject.GetComponent<FirstPersonController>();
            }
        }
        controller.enabled = canMove;
        selector.enabled = canMove;
    }
    /// <summary>
    /// Turns on movement
    /// </summary>
    public void EnableMovement()
    {
        ToggleMovement(true);
        Cursor.lockState = CursorLockMode.Locked;
        
    }
    /// <summary>
    /// Called in the hub world to select a memory string
    /// </summary>
    public void SelectMemoryString()
    {
        DisableMovement();
        userInterface.ToggleMemorySelectUI(true);
    }
    /// <summary>
    /// Called in the hub world to deselect a memory string
    /// </summary>
    public void DeselectMemory()
    {
        EnableMovement();
        playerInMemorySelection = false;
        userInterface.ToggleMemorySelectUI(false);
    }
    /// <summary>
    /// Called every frame to handle pressing the escape key to toggle the tablet
    /// </summary>
    void HandleInterfaceInput()
    {
        if (!playerInMemorySelection)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                ToggleTablet();
            }
        }
    }


    /// <summary>
    /// handles turning on or off the pause menu (tablet)
    /// </summary>
    public void ToggleTablet()
    {
        if (!controller.isConvo) 
        {
            TogglePause();
            userInterface.ToggleTablet(paused);
            controller.GetComponent<Selector>().enabled = !controller.GetComponent<Selector>().enabled;
        }
    }

    /// <summary>
    /// Toggles whether in UI or first person mode, unlocks the cursor and disables the movement 
    /// </summary>
    private void TogglePause()
    {
        paused = !paused;
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        controller.enabled = !paused;

    }

    /// <summary>
    /// Adds money to the player by increasing the lua variable in the Dialogue System
    /// </summary>
    /// <param name="amt">The amount of money to give the player</param>
    public void AddMoney(double amt)
    {

        var money = DialogueLua.GetVariable("PlayerMoney").asInt;
        money += (int)amt;
        DialogueLua.SetVariable("PlayerMoney", money);

        //TODO: change this to collecting all coins in the apartment not just getting 5 total money
        if (GetMoney == 5)
        {
            AddTheme("Curiosity", 2);
        }

        //play sound
        soundManager.CoinGet();

        //raise event to update UI
        onMoneyCollected.Invoke();

    }
    /// <summary>
    /// Forces the player to lose money by subtracting from the lua variable in the Dialogue System
    /// doesn't check if the player has enough money
    /// </summary>
    /// <param name="amt">the amount of money to reduce the player's money by</param>
    public void ForceSubtractMoney(double amt)
    {
        var money = DialogueLua.GetVariable("PlayerMoney").asInt;
        DialogueLua.SetVariable("PlayerMoney", money - (int)amt);
        onMoneyCollected.Invoke();
    }
    /// <summary>
    /// Checks if the player has enough money before subtracting from the lua variable in the Dialogue System
    /// </summary>
    /// <param name="amt">the amount of money to reduce the player's money by</param>
    /// <returns>true if the player had enough money, false otherwise</returns>
    public bool SubtractMoney(double amt)
    {
        var money = DialogueLua.GetVariable("PlayerMoney").asInt;
        if (amt > money) return false;
        DialogueLua.SetVariable("PlayerMoney", money - (int)amt);
        soundManager.MakePurchase(); // Might need to be changed if the player can lose money in other ways
        onMoneyCollected.Invoke();
        return true;
    }
    /// <summary>
    /// Get the total value of all theme/karma points
    /// </summary>
    /// <returns>The total value (sum) of all karma points inthe karmaPoints dictionary</returns>
    public double GetTotalKarma()
    {
        double totalKarma = 0;
        foreach (var val in karmaPoints.Values)
        {
            totalKarma += val;
        }
        return totalKarma;
    }
    /// <summary>
    /// Get if the player has positive karma
    /// </summary>
    /// <returns>true if the player has above the max neutral positive karma</returns>
    public bool HasPositiveKarma()
    {
        return GetTotalKarma() > nuetralMaxKarma;
    }
    /// <summary>
    /// Get if the player has negative karma
    /// </summary>
    /// <returns>true if the player has below the min neutral negative karma</returns>
    public bool HasNegativeKarma()
    {
        return GetTotalKarma() < -nuetralMaxKarma;
    }
    /// <summary>
    /// Get if the player has nuetral karma
    /// </summary>
    /// <returns>true if the player has neutral karma, not too high or too low</returns>
    public bool HasNeutralKarma()
    {
        return !HasNegativeKarma() && !HasPositiveKarma();
    }
    /// <summary>
    /// Get a string value of the player's karma class
    /// </summary>
    /// <returns>"Positive" if the player has positive karma, "Negative" if the player has negative karma, "Neutral" otherwise</returns>
    public string GetKarmaClass()
    {
        if (HasPositiveKarma()) return "Positive";
        if (HasNegativeKarma()) return "Negative";
        return "Neutral";
    }
    /// <summary>
    /// Stops the player movement completely and instantly
    /// </summary>
    public void StopPlayer()
    {
        controller.StopPlayer();
    }
    /// <summary>
    /// Used byh lua to tell the player controller that the game will end after the current conversation
    /// </summary>
    public void LastDemoConvo() 
    {
        controller.LastDemoConvo();
    }

    /// <summary>
    /// Called when the object is enabled
    /// This is where the functions are registered with Lua
    /// This is in combination with the custom LUA asset in Assets/ScritableObjects/Lua
    /// </summary>
    void OnEnable()
    {
        
        Lua.RegisterFunction(nameof(StopPlayer), this, SymbolExtensions.GetMethodInfo(() => StopPlayer()));
        Lua.RegisterFunction(nameof(LastDemoConvo), this, SymbolExtensions.GetMethodInfo(() => LastDemoConvo()));
        Lua.RegisterFunction(nameof(AddMoney), this, SymbolExtensions.GetMethodInfo(() => AddMoney(0)));
        Lua.RegisterFunction(nameof(ForceSubtractMoney), this, SymbolExtensions.GetMethodInfo(() => ForceSubtractMoney(0)));
        Lua.RegisterFunction(nameof(SubtractMoney), this, SymbolExtensions.GetMethodInfo(() => SubtractMoney(0)));
        Lua.RegisterFunction(nameof(AddTheme), this, SymbolExtensions.GetMethodInfo(() => AddTheme("", 0)));
        Lua.RegisterFunction(nameof(GetThemeKarma), this, SymbolExtensions.GetMethodInfo(() => GetThemeKarma("")));
        Lua.RegisterFunction(nameof(GetTotalKarma), this, SymbolExtensions.GetMethodInfo(() => GetTotalKarma()));
        Lua.RegisterFunction(nameof(GetKarmaClass), this, SymbolExtensions.GetMethodInfo(() => GetKarmaClass()));
        Lua.RegisterFunction(nameof(HasNeutralKarma), this, SymbolExtensions.GetMethodInfo(() => HasNeutralKarma()));
        Lua.RegisterFunction(nameof(HasNegativeKarma), this, SymbolExtensions.GetMethodInfo(() => HasNegativeKarma()));
        Lua.RegisterFunction(nameof(HasPositiveKarma), this, SymbolExtensions.GetMethodInfo(() => HasPositiveKarma()));
    }
    /*
     * This is necessary if you want to unregister the functions from Lua
     * This game object is never disabled so it is not necessary
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
