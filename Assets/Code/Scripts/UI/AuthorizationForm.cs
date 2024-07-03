using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AuthorizationForm : MonoBehaviour
{
    /// <summary>
    /// Controls gameObject to turn on when the form goes away
    /// </summary>
    [SerializeField] private GameObject controls;
    /// <summary>
    /// GameObject to turn off the newspapers once the authorization form shrinks
    /// </summary>
    [SerializeField] private GameObject newspapers;
    /// <summary>
    /// Background gameObject to turn off when the controls appear
    /// </summary>
    [SerializeField] private GameObject background;

    public Transform targetTransformUp;
    public Transform targetTransformRight;
    public Vector3 finalScale = Vector3.zero;
    public float duration = 1f;

    private bool isShrunk = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveItem());
    }

    // Update is called once per frame
    void Update()
    {
        if ((Keyboard.current.enterKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame) && !isShrunk)
        {
            isShrunk = true;
            newspapers.SetActive(false);
            StartCoroutine(ShrinkItem());
        }
    }

    private IEnumerator MoveItem()
    {
        Transform objectTransform = this.transform;
        Vector3 startPosition = objectTransform.position;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Smoothly interpolate position
            objectTransform.position = Vector3.Lerp(startPosition, targetTransformUp.position, t);
            yield return null;
        }

        // Ensure final position and scale are exact
        objectTransform.position = targetTransformUp.position;
    }

    private IEnumerator ShrinkItem()
    {
        Transform objectTransform = this.transform;
        Vector3 startPosition = objectTransform.position;
        Vector3 startScale = objectTransform.localScale;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Smoothly interpolate position
            objectTransform.position = Vector3.Lerp(startPosition, targetTransformRight.position, t);

            // Smoothly interpolate scale
            objectTransform.localScale = Vector3.Lerp(startScale, finalScale, t);

            yield return null;
        }

        // Ensure final position and scale are exact
        objectTransform.position = targetTransformRight.position;
        objectTransform.localScale = finalScale;

        //Enable the controls
        controls.SetActive(true);

        //Disable the background and newspapers
        background.SetActive(false);

        // Disable the GameObject
        gameObject.SetActive(false);
    }
}
