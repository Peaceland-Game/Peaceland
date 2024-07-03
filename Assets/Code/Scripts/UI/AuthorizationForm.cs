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

    public Transform targetTransform;
    public Vector3 finalScale = Vector3.zero;
    public float duration = 1f;

    private bool isShrunk = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Keyboard.current.enterKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame) && !isShrunk)
        {
            isShrunk = true;
            StartCoroutine(ShrinkItem());
        }
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
            objectTransform.position = Vector3.Lerp(startPosition, targetTransform.position, t);

            // Smoothly interpolate scale
            objectTransform.localScale = Vector3.Lerp(startScale, finalScale, t);

            yield return null;
        }

        // Ensure final position and scale are exact
        objectTransform.position = targetTransform.position;
        objectTransform.localScale = finalScale;

        //Enable the controls
        controls.SetActive(true);

        // Disable the GameObject
        gameObject.SetActive(false);
    }
}
