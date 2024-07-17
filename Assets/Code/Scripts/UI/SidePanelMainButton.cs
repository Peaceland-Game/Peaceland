using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SidePanelMainButton : MonoBehaviour
{
    public GameObject pageToEnable;
    public List<GameObject> pagesToDisable;
    public List<Button> memoryButtons;
    //i dont think i need to hold a variable for this button itself
    public void OnClickingThisButton()
    {
        pageToEnable.SetActive(true);
        for(int i = 0; i < pagesToDisable.Count; i++)
        {
            pagesToDisable[i].SetActive(false);
        }
    }
}
