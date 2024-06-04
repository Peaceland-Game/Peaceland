using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private GameObject historyMenu;
    public ScrollToBottom scrollToBottom;  // Reference to the ScrollToBottom script

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

    public void ToggleHistoryMenu(PlayerMovement.MovementState state)
    {
        var menuActive = !historyMenu.activeInHierarchy;
        historyMenu.SetActive(menuActive);
        scrollToBottom.ScrollToBottomInstant();
        if (state == PlayerMovement.MovementState.Talking) return;
        Time.timeScale = menuActive ? 0 : 1;
        Cursor.lockState = menuActive ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
