using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Represents a popup that displays an image and can be closed by the user.
/// </summary>
public class ImagePopup : MonoBehaviour
{
    [SerializeField] float minimumDuration = 0.5f;
    /// <summary>
    /// Initializes the component and starts the minimum duration coroutine.
    /// </summary>
    void Start()
    {
        StartCoroutine(MinimumDuration());
    }

    /// <summary>
    /// Checks for user input to destroy the game object.
    /// </summary>
    void Update()
    {
        if(Keyboard.current.eKey.wasPressedThisFrame)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Coroutine that waits for a minimum duration before allowing further actions.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine system.</returns>
    public IEnumerator MinimumDuration()
    {
        yield return new WaitForSeconds(minimumDuration);
    }
}
