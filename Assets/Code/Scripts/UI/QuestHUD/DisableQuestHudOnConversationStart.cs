using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;

/// <summary>
/// Script to disable the Quest HUD when a conversation starts and re-enable it when the conversation ends.
/// </summary>
public class DisableQuestHudOnConversationStart : MonoBehaviour
{
    private GameObject Hud;
    public GameObject reticle;
    /// <summary>
    /// Initializes the component and finds the Quest HUD after a short delay.
    /// </summary>
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
    /// <summary>
    /// Coroutine that waits for a specified duration before executing an action.
    /// </summary>
    /// <param name="duration">The duration to wait in seconds.</param>
    /// <param name="action">The action to execute after waiting.</param>
    /// <returns>An IEnumerator for the coroutine system.</returns>
    IEnumerator WaitForThen(float duration, Action action)
    {
        yield return new WaitForSeconds(duration);
        action();
    }

    /// <summary>
    /// Called when a conversation starts. Disables the Quest HUD and reticle.
    /// </summary>
    public void OnConversationStart()
    {
        //Debug.Log("Disabling Quest Hud During conversation");
        Hud.SetActive(false);
        reticle.SetActive(false);
    }
    /// <summary>
    /// Called when a conversation ends. Enables the Quest HUD and reticle.
    /// </summary>
    public void OnConversationEnd()
    {
     //   Debug.Log("Enabling quest hud");
        Hud.SetActive(true);
        reticle.SetActive(true);
    }
}
