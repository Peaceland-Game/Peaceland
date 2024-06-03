using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private GameObject historyMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHistoryMenu();
        }
    }

    public void ToggleHistoryMenu()
    {
        var menuActive = !historyMenu.activeInHierarchy;
        historyMenu.SetActive(menuActive);
        Time.timeScale = menuActive ? 0 : 1;
        Cursor.lockState = menuActive ? CursorLockMode.Locked : CursorLockMode.None;    //not working or timescale makes you not move mouse?
    }
}
