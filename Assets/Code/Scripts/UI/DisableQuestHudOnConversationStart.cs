using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;

public class DisableQuestHudOnConversationStart : MonoBehaviour
{
    private GameObject Hud;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForThen(.1f, () =>
        {
            Hud = GameObject.FindWithTag("QuestHUD");
            if (!Hud)
            {
                Debug.LogError("Missing Quest HUD");
            }
        }));
    }

    IEnumerator WaitForThen(float duration, Action action)
    {
        yield return new WaitForSeconds(duration);
        action();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnConversationStart()
    {
        //Debug.Log("Disabling Quest Hud During conversation");
        Hud.SetActive(false);
    }
    public void OnConversationEnd()
    {
     //   Debug.Log("Enabling quest hud");
        Hud.SetActive(true);
    }
}
