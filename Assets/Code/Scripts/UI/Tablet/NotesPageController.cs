using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesPageController : MonoBehaviour
{
    //memoryButton[0] would correspond to chatLog[0]
    public List<Button> memoryButtons;
    public List<GameObject> chatLogs;
    public GameObject overviewPage;
    public GameObject memoryPage;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChatLogDisplay(int memoryNumber)
    {
        Debug.Log(memoryNumber);
        overviewPage.SetActive(false);
        memoryPage.SetActive(false);    
        chatLogs[memoryNumber].SetActive(true);
        for(int i = 0; i < chatLogs.Count; i++)
        {
            if (i != memoryNumber)
                chatLogs[i].SetActive(false);
        }
    }
}
