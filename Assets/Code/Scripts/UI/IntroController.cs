using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroController : MonoBehaviour
{
    [Header("UI Elements")]
    public Image fadeImage;
    public TextMeshProUGUI buzz1;
    public TextMeshProUGUI buzz2;
    public Button wakeUpButton;
    public Button goToComputerButton;
    public TextMeshProUGUI pickUpTabletPrompt;
    

    [Header("Other References")]
    public GameObject tablet;
    public Camera playerCam;
    public Transform CameraConversationLoc;

    [Header("Wake up sequence")]
    public Image topEyelid;
    public Image bottomEyelid;
    public float eyeOpenSpeed = 1f;
    public float wakeUpDuration = 2f;

    [Header("Tablet tutorial")]
    public Transform tabletPickupLoc;
    public GameObject tutorialPrefab;
    private GameObject tabletTutorialInstance;
    public Tablet playerTablet;

    private bool waitForPlayer = false;
    private GameObject selectorUI;

    private void Start()
    {
        InitializeUI();
        
        StartCoroutine(IntroSequence());
    }

    private void Update()
    {
        if (waitForPlayer && Keyboard.current.eKey.wasPressedThisFrame)
        {
            waitForPlayer = false;
            StartTabletTutorial();
        }
        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            LoadHubWorld();
        }
        if (Keyboard.current.f2Key.wasPressedThisFrame) {
            
            StartTabletTutorial();
        }
    }

    private void StartTabletTutorial() {
        waitForPlayer = false;
        pickUpTabletPrompt.gameObject.SetActive(false);
        StartCoroutine(MoveToTarget(tablet.transform, 1f));
    }

    private IEnumerator MoveToTarget(Transform transform, float duration) {
        transform.GetPositionAndRotation(
            out Vector3 startPosition, 
            out Quaternion startRotation);
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Smoothly interpolate position
            // Smoothly interpolate rotation
            transform.SetPositionAndRotation(
                Vector3.Lerp(startPosition, tabletPickupLoc.position, t), 
                Quaternion.Slerp(startRotation, tabletPickupLoc.rotation, t));

            yield return null;
        }

        // Ensure final position and rotation exactly match the target
        transform.SetPositionAndRotation(tabletPickupLoc.position, tabletPickupLoc.rotation);
        Debug.Log("show tablet");
        playerTablet.gameObject.SetActive(true);
        tabletTutorialInstance = Instantiate(tutorialPrefab, playerTablet.transform);
    }


    private void InitializeUI()
    {
        topEyelid.gameObject.SetActive(false);
        bottomEyelid.gameObject.SetActive(false);
        fadeImage.gameObject.SetActive(true);
        buzz1.gameObject.SetActive(false);
        buzz2.gameObject.SetActive(false);
        wakeUpButton.gameObject.SetActive(false);
        goToComputerButton.gameObject.SetActive(false);
        pickUpTabletPrompt.gameObject.SetActive(false);

        StartCoroutine(WaitThen(0.01f, () =>
        {
            selectorUI = GameObject.FindWithTag("StandardUISelector");
            if (selectorUI) selectorUI.SetActive(false);
        }));
    }

    private IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(2);

        yield return StartCoroutine(FadeText(buzz1, 1, 2, false));
        yield return StartCoroutine(FadeText(buzz2, 1, 2, false));


        wakeUpButton.gameObject.SetActive(true);
    }

    public void WakeUp()
    {
      //  Debug.Log("start wakeup");
        StartCoroutine(WakeUpSequence());
    }

    private IEnumerator WakeUpSequence()
    {
        wakeUpButton.gameObject.SetActive(false);

        // Ensure eyelids are fully open at the start
        topEyelid.rectTransform.anchoredPosition = new Vector2(0, -Screen.height * 0.5f);
        bottomEyelid.rectTransform.anchoredPosition = new Vector2(0, Screen.height * 0.5f);
        topEyelid.gameObject.SetActive(true);
        bottomEyelid.gameObject.SetActive(true);

        // Fade from black if needed
        yield return StartCoroutine(FadeImage(fadeImage, 1f, true));
        fadeImage.gameObject.SetActive(false);

        // Slight delay before "blinking"
        yield return new WaitForSeconds(0.5f);

        Vector3 initialPosition = playerCam.transform.position;
        Quaternion initialRotation = playerCam.transform.rotation;

        // First eye closing (partial)
        yield return StartCoroutine(PartialEyeClose(0.5f, 0.6f));

        // Brief pause
        yield return new WaitForSeconds(0.4f);

        // Open eyes again
        yield return StartCoroutine(OpenEyes(0.4f));

        // Another brief pause
        yield return new WaitForSeconds(0.7f);

        // Final eye closing and camera movement
        float elapsedTime = 0f;
        while (elapsedTime < wakeUpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / wakeUpDuration;

            // Move and rotate camera
            playerCam.transform.SetPositionAndRotation(
                Vector3.Lerp(initialPosition, CameraConversationLoc.position, t), 
                Quaternion.Slerp(initialRotation, CameraConversationLoc.rotation, t));

            // Close eyelids
            float eyeCloseT = Mathf.Sin(t * Mathf.PI * 0.5f); // Ease out
            topEyelid.rectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(-Screen.height * 0.5f, 0, eyeCloseT));
            bottomEyelid.rectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(Screen.height * 0.5f, 0, eyeCloseT));

            yield return null;
        }

        // Ensure eyelids are fully closed
        topEyelid.rectTransform.anchoredPosition = Vector2.zero;
        bottomEyelid.rectTransform.anchoredPosition = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        // Hide eyelids
        topEyelid.gameObject.SetActive(false);
        bottomEyelid.gameObject.SetActive(false);

        // Start the dialogue
        // StartDialogue();

        goToComputerButton.gameObject.SetActive(true);
    }

    private IEnumerator PartialEyeClose(float duration, float closeAmount)
    {
        float elapsedTime = 0f;
        float startTopPos = -Screen.height * 0.5f;
        float startBottomPos = Screen.height * 0.5f;
        float targetTopPos = -Screen.height * 0.5f * (1 - closeAmount);
        float targetBottomPos = Screen.height * 0.5f * (1 - closeAmount);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float eyeCloseT = Mathf.Sin(t * Mathf.PI * 0.5f); // Ease out

            topEyelid.rectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(startTopPos, targetTopPos, eyeCloseT));
            bottomEyelid.rectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(startBottomPos, targetBottomPos, eyeCloseT));

            yield return null;
        }
    }

    private IEnumerator OpenEyes(float duration)
    {
        float elapsedTime = 0f;
        Vector2 topStart = topEyelid.rectTransform.anchoredPosition;
        Vector2 bottomStart = bottomEyelid.rectTransform.anchoredPosition;
        Vector2 topEnd = new Vector2(0, -Screen.height * 0.5f);
        Vector2 bottomEnd = new Vector2(0, Screen.height * 0.5f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float eyeOpenT = Mathf.Sin(t * Mathf.PI * 0.5f); // Ease out

            topEyelid.rectTransform.anchoredPosition = Vector2.Lerp(topStart, topEnd, eyeOpenT);
            bottomEyelid.rectTransform.anchoredPosition = Vector2.Lerp(bottomStart, bottomEnd, eyeOpenT);

            yield return null;
        }

        topEyelid.rectTransform.anchoredPosition = topEnd;
        bottomEyelid.rectTransform.anchoredPosition = bottomEnd;
    }

    public void TakeTabletPrompt()
    {
        StartCoroutine(FadeText(pickUpTabletPrompt, 0.5f, 3, false));
        waitForPlayer = true;
    }

    public void EndScene()
    {
        StartCoroutine(FadeImage(fadeImage, 4, false));
    }

    private IEnumerator FadeImage(Image image, float duration, bool fadeOut)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = fadeOut ? 1 - (elapsedTime / duration) : elapsedTime / duration;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
        image.gameObject.SetActive(!fadeOut);
    }

    private IEnumerator FadeText(TextMeshProUGUI text, float fadeDuration, float displayDuration, bool fadeOut)
    {
        text.gameObject.SetActive(true);
        yield return StartCoroutine(FadeTextAlpha(text, fadeDuration, !fadeOut));
        yield return new WaitForSeconds(displayDuration);
        if (fadeOut)
        {
            yield return StartCoroutine(FadeTextAlpha(text, fadeDuration, false));
            text.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeTextAlpha(TextMeshProUGUI text, float duration, bool fadeIn)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = fadeIn ? elapsedTime / duration : 1 - (elapsedTime / duration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }
    }

    private IEnumerator WaitThen(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }

    private void LoadHubWorld()
    {
        if (selectorUI) selectorUI.SetActive(true);
        {
            StartCoroutine(FadeToBlack());
        }
        
    }


    public IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        Debug.Log("fade to black");
        fadeImage.gameObject.SetActive(true);
        buzz1.gameObject.SetActive(false);
        buzz2.gameObject.SetActive(false);
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            if (fadeImage.color.a < 1)
            {
                fadeImage.color = new Color(0, 0, 0, fadeImage.color.a + Time.deltaTime);
            }

            yield return null;
        }
        if (elapsedTime >= 1)
        {
            SceneManager.LoadScene("HubWorld2");
        }

    }

}