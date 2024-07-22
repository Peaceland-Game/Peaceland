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

    public void FindPlayerInScene()
    {
        if (!playerObject)
        {
            playerInMemorySelection = false;
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
                Debug.LogWarning("Player is missing");
                return;
            }
        }
    }
    public void InitPlayer(PlayerObjectReference playerRef)
    {
        playerObject = playerRef.gameObject;    
        userInterface = playerRef.userInterface;
        userInterface.RegisterEventListener();
        controller = playerRef.controller;
        tablet = playerRef.tablet;
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
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                ToggleJournal();
            }
        }
    }



    public void ToggleJournal()
    {
        if (!controller.isConvo) 
        {
            TogglePause();
            Debug.Log("toggle pause");
            userInterface.ToggleJournal(paused);
            controller.GetComponent<Selector>().enabled = !controller.GetComponent<Selector>().enabled;
        }
    }

    //public void TogglePauseMenu()
    //{
    //    TogglePause();
    //    userInterface.TogglePauseMenu(paused);
    //}

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

        if (GetMoney == 5)
        {
            AddTheme("Curiosity", 2);
        }

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


    public double GetTotalKarma()
    {
        double totalKarma = 0;
        foreach (var val in karmaPoints.Values)
        {
            totalKarma += val;
        }
        return totalKarma;
    }

    public bool HasPositiveKarma()
    {
        return GetTotalKarma() > nuetralMaxKarma;
    }
    public bool HasNegativeKarma()
    {
        return GetTotalKarma() < -nuetralMaxKarma;
    }
    public bool HasNeutralKarma()
    {
        return !HasNegativeKarma() && !HasPositiveKarma();
    }

    public string GetKarmaClass()
    {
        if (HasPositiveKarma()) return "Positive";
        if (HasNegativeKarma()) return "Negative";
        return "Nuetral";
    }






    public void StopPlayer()
    {
        controller.StopPlayer();
    }

    public void LastDemoConvo() 
    {
        controller.LastDemoConvo();
    }

    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
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
