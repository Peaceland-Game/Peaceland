using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Handles fading and teleporting the player via the fog controller object
/// </summary>
public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;         //singleton

    private Image fadeImage;
    private Coroutine fadeCoroutine;

    
    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //Debug.Log("screen fader awake");
        fadeImage = GetComponent<Image>();
    }

    /// <summary>
    /// Fades the screen using the fadeImage and calls the onFadeComplete action after
    /// </summary>
    /// <param name="fadeDuration">how long the fade animation should take</param>
    /// <param name="onFadeComplete">the action to perform when the fade completes</param>
    public void FadeAndTeleport(float fadeDuration, System.Action onFadeComplete)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);//make sure not to call more than one coroutine
        }
        fadeCoroutine = StartCoroutine(FadeCoroutine(fadeDuration, onFadeComplete));//start fade
    }

    /// <summary>
    /// Handles fading the screen, perform the action, then un fade the screen
    /// </summary>
    /// <param name="duration">duration of the fade animation</param>
    /// <param name="onFadeComplete">the action to perform during the fade animation</param>
    /// <returns></returns>
    private IEnumerator FadeCoroutine(float duration, System.Action onFadeComplete)
    {
        fadeImage.enabled = true;
        // Fade to opaque
        float elapsedTime = 0f;
        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / (duration / 2));
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }

        // Execute the teleport action
        onFadeComplete?.Invoke();

        // Fade back to transparent
        elapsedTime = 0f;
        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / (duration / 2));
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);
        fadeImage.enabled = false;
    }
}