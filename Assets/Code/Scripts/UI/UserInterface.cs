using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


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
    // Start is called before the first frame update
    void Start()
    {
        // Get the GraphicRaycaster component attached to this canvas
        raycaster = GetComponent<GraphicRaycaster>();

        // Get the current EventSystem
        eventSystem = EventSystem.current;
    }
    public void RegisterEventListener()
    {
        PlayerSingleton.Instance.onMoneyCollected.AddListener(UpdateMoneyUI);
    }


    // Update is called once per frame
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
    public void TogglePauseMenu(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
    }

    public void ToggleJournal(bool isPaused)
    {
        journal.SetActive(isPaused);
    }
    void UpdateMoneyUI()
    {
        moneyText.text = "Money: " + DialogueLua.GetVariable("PlayerMoney").asInt;
    }

    public void ToggleHistoryMenu(PlayerMovement.MovementState state)
    {
        //var menuActive = !historyMenu.activeInHierarchy;
        //historyMenu.SetActive(menuActive);
        //scrollToBottom.ScrollToBottomInstant();
        //if (state == PlayerMovement.MovementState.Talking) return;
        //Time.timeScale = menuActive ? 0 : 1;
        //Cursor.lockState = menuActive ? CursorLockMode.None : CursorLockMode.Locked;
    }
    public void EnableLoadScreen()
    {
        if (!loadScreen.activeInHierarchy)
        {
            loadScreen.SetActive(true);
        }
    }
    public void DisableLoadScreen()
    {
        loadScreen.SetActive(false);
    }
    public void UpdateLoadingProgress(float progress)
    {
        if (loadingSlider != null)
        {
            loadingSlider.value = progress;
        }
    }

    public void Exit() {
        Application.Quit();
    }
    public void ToggleMemorySelectUI(bool active)
    {
        memorySelectUI.SetActive(active);
    }
}
