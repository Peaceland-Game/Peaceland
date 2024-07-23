using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Manages the user interface elements and interactions in the game,
/// including pause menu, journal, loading screen, and various UI toggles.
/// </summary>
public class UserInterface : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject journal;
    public ScrollToBottom scrollToBottom;  // Reference to the ScrollToBottom script
    public GameObject memorySelectUI;
    [SerializeField] GameObject loadScreen;
    [SerializeField] private UnityEngine.UI.Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI moneyText;

    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    public bool CheckMouseClicks = false;
    public GameObject reticle;
    /// <summary>
    /// Initializes the UserInterface component.
    /// </summary>
    void Start()
    {
        // Get the GraphicRaycaster component attached to this canvas
        raycaster = GetComponent<GraphicRaycaster>();

        // Get the current EventSystem
        eventSystem = EventSystem.current;
    }
    /// <summary>
    /// Registers event listeners for player events.
    /// </summary>
    public void RegisterEventListener()
    {
        PlayerSingleton.Instance.onMoneyCollected.AddListener(UpdateMoneyUI);
    }


    /// <summary>
    /// Updates the UserInterface and checks for mouse clicks if enabled.
    /// </summary>
    void Update()
    {
        if (CheckMouseClicks) {
            // Check for mouse click
            if (Input.GetMouseButtonDown(0)) // 0 is left click, 1 is right click, 2 is middle click
            {
                DetectUIClick();
            }
        }
    }
    /// <summary>
    /// Detects and logs information about UI elements clicked by the user.
    /// </summary>
    void DetectUIClick()
    {
        // Create a PointerEventData with the current mouse position
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = Input.mousePosition;

        // Create a list to receive all results
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast using the Graphics Raycaster and mouse click position
        raycaster.Raycast(pointerData, results);

        // If we hit something, print the info
        if (results.Count > 0)
        {
            StringBuilder clickInfo = new StringBuilder();
            clickInfo.AppendLine("Click detected on UI Element(s):");

            foreach (RaycastResult result in results)
            {
                clickInfo.AppendLine($"- Name: {result.gameObject.name}");
                clickInfo.AppendLine($"  Tag: {result.gameObject.tag}");
                clickInfo.AppendLine($"  Component Types:");

                Component[] components = result.gameObject.GetComponents<Component>();
                foreach (Component component in components)
                {
                    clickInfo.AppendLine($"    {component.GetType().Name}");
                }

                clickInfo.AppendLine(); // Add a blank line between elements
            }

            Debug.Log(clickInfo.ToString());
        }
        else
        {
            Debug.Log("Click detected, but no UI element was hit.");
        }
    }
    /// <summary>
    /// Toggles the pause menu and reticle visibility.
    /// </summary>
    /// <param name="isPaused">Whether the game is paused.</param>
    public void TogglePauseMenu(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
        reticle.SetActive(!isPaused);
    }
    /// <summary>
    /// Toggles the tablet interface and reticle visibility.
    /// </summary>
    /// <param name="isPaused">Whether the tablet is open.</param>
    public void ToggleTablet(bool isPaused)
    {
        journal.SetActive(isPaused);
        reticle.SetActive(!isPaused);
    }
    /// <summary>
    /// Updates the displayed money amount in the UI.
    /// </summary>
    void UpdateMoneyUI()
    {
        moneyText.text = "Money: " + DialogueLua.GetVariable("PlayerMoney").asInt;
    }
    /// <summary>
    /// Enables the loading screen.
    /// </summary>
    public void EnableLoadScreen()
    {
        if (!loadScreen.activeInHierarchy)
        {
           // Debug.Log("Enable loadscreen");
            loadScreen.SetActive(true);
        }
    }
    /// <summary>
    /// Disables the loading screen.
    /// </summary>
    public void DisableLoadScreen()
    {
       // Debug.Log("Disable loadscreen");
        loadScreen.SetActive(false);
    }

    /// <summary>
    /// Updates the loading progress bar.
    /// </summary>
    /// <param name="progress">The loading progress value between 0 and 1.</param>
    public void UpdateLoadingProgress(float progress)
    {
        if (loadingSlider != null)
        {
            loadingSlider.value = progress;
        }
    }
    /// <summary>
    /// Exits the application.
    /// </summary>
    public void Exit() {
        Application.Quit();
    }

    /// <summary>
    /// Toggles the visibility of the memory select UI.
    /// </summary>
    /// <param name="active">Whether the memory select UI should be active.</param>
    public void ToggleMemorySelectUI(bool active)
    {
        memorySelectUI.SetActive(active);
    }
}
