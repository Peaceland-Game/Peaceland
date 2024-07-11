using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    // UI stuff
    public Image fadeImage;
    public TextMeshProUGUI buzz1;
    public TextMeshProUGUI buzz2;
    public Button wakeUpButton;
    public Button goToComputerButton;
    public TextMeshProUGUI pickUpTabletPrompt;

    public GameObject tablet;

    private bool waitForPlayer = false;
    private GameObject selectorUI;
    // Player stuff
    public Camera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        fadeImage.gameObject.SetActive(true);
        buzz1.gameObject.SetActive(false);
        buzz2.gameObject.SetActive(false);
        wakeUpButton.gameObject.SetActive(false);
        goToComputerButton.gameObject.SetActive(false);
        pickUpTabletPrompt.gameObject.SetActive(false);

        StartCoroutine(WaitThen(0.01f, () =>
        {
            selectorUI = GameObject.FindWithTag("StandardUISelector");
            if (selectorUI)
            {
                selectorUI.SetActive(false);
            }
        }));

        StartCoroutine(Wait(2));
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForPlayer)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (selectorUI)
                    selectorUI.SetActive(true);
                SceneManager.LoadScene("HubWorld2");
            }
        }
    }
    IEnumerator WaitThen(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }
    /// <summary>
    /// Waits a few seconds
    /// </summary>
    /// <returns> nothing...? </returns>
    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        StartCoroutine(fadeInOutText(buzz1, 1, seconds, 69, true, false));

        yield return new WaitForSeconds(seconds);

        StartCoroutine(fadeInOutText(buzz2, 1, seconds, 420, true, false));

        yield return new WaitForSeconds(seconds);

        wakeUpButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Fade out the black screen
    /// </summary>
    /// <param name="duration"> duration of the fade </param>
    /// <param name="onFadeComplete"> irrelevant (probably) </param>
    /// <returns> uh </returns>
    private IEnumerator FadeCoroutine(float duration, System.Action onFadeComplete)
    {
        wakeUpButton.gameObject.SetActive(false);

        // Fade back to transparent
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / duration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            buzz1.color = new Color(buzz1.color.r, buzz1.color.g, buzz1.color.b, alpha);
            buzz2.color = new Color(buzz2.color.r, buzz2.color.g, buzz2.color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);
        buzz1.color = new Color(buzz1.color.r, buzz1.color.g, buzz1.color.b, 0f);
        buzz2.color = new Color(buzz2.color.r, buzz2.color.g, buzz2.color.b, 0f);

        fadeImage.gameObject.SetActive(false);
        buzz1.gameObject.SetActive(false);
        buzz2.gameObject.SetActive(false);

        // Execute the teleport action
        onFadeComplete?.Invoke();

        // Wait as the player "wakes up"

        yield return new WaitForSeconds(3);

        goToComputerButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Fades a text box in and/or out
    /// </summary>
    /// <param name="textBox"> the text box to fade </param>
    /// <param name="fadeInTime"> the duration of the fade in </param>
    /// <param name="solidDuration"> the duration that the text stays solid </param>
    /// <param name="fadeOutTime"> the duration of the fade out </param>
    /// <param name="fadeIn"> whether the text should fade in </param>
    /// <param name="fadeOut"> whether the text should fade out </param>
    /// <returns></returns>
    private IEnumerator fadeInOutText(TextMeshProUGUI textBox, float fadeInTime, float solidDuration, float fadeOutTime, bool fadeIn, bool fadeOut)
    {
        if (fadeIn)
        {
            textBox.gameObject.SetActive(true);

            // Fade to opaque
            float elapsedTime = 0f;

            while (elapsedTime < fadeInTime)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeInTime);
                textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, alpha);
                yield return null;
            }
        }

        yield return new WaitForSeconds(solidDuration);

        if (fadeOut)
        {
            // Fade back to transparent
            float elapsedTime = 0f;

            while (elapsedTime < fadeOutTime)
            {
                elapsedTime += Time.deltaTime;
                float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeOutTime);
                textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, alpha);
                yield return null;
            }

            textBox.gameObject.SetActive(false);
        }
    }

    private IEnumerator fadeOut()
    {
        fadeImage.gameObject.SetActive(true);

        // Fade to opaque
        float elapsedTime = 0f;

        while (elapsedTime < 4)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / 4);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }
    }

    /// <summary>
    /// If only it was so easy
    /// </summary>
    public void WakeUp()
    {
        StartCoroutine(FadeCoroutine(1, () => { }));
    }

    public void TakeTabletPrompt()
    {
        StartCoroutine(fadeInOutText(pickUpTabletPrompt, 0.5f, 3, 1337, true, false));
        waitForPlayer = true;

    }

    public void EndScene()
    {
        StartCoroutine(fadeOut());
    }
}
