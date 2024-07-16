using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickExample : MonoBehaviour
{
    protected Button yourButton;

    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }
    public virtual void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
    }
}