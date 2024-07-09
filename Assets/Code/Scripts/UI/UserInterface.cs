using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject journal;
    public ScrollToBottom scrollToBottom;  // Reference to the ScrollToBottom script
    public GameObject memorySelectUI;
    [SerializeField] GameObject loadScreen;
    [SerializeField] private UnityEngine.UI.Slider loadingSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    ToggleHistoryMenu();
        //}
    }
    public void TogglePauseMenu(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
    }

    public void ToggleJournal(bool isPaused)
    {
        journal.SetActive(isPaused);
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
