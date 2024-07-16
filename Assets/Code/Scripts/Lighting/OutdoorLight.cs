using UnityEngine;

/// <summary>
/// Script attached to outdoor lights in the game world so that they will automatically turn on when the lighting changes
/// </summary>
public class OutdoorLight : MonoBehaviour
{
    public Light lightComponent;
    public float dayIntensity = 0f;
    public float nightIntensity = 133f;

    /// <summary>
    /// Finds the lighting controller in the scene
    /// registers the event listener
    /// </summary>
    private void Start()
    {
        
        // Find the DynamicLightingController in the scene and subscribe to its event
        DynamicLightingController lightingController = FindObjectOfType<DynamicLightingController>();
        if (lightingController != null)
        {
            lightingController.OnLightingProfileChanged += HandleLightingProfileChanged;
        }
        else
        {
            Debug.LogWarning("DynamicLightingController not found in the scene!");
        }

        // Initialize the light based on the current profile
        UpdateLightBasedOnProfile(lightingController.lightingProfiles[lightingController.currentProfileIndex]);
    }

    /// <summary>
    /// Called when the lighting changes from the event listener
    /// </summary>
    /// <param name="sender">the object that sent the event (the lighting conntroller)</param>
    /// <param name="e">The event that was raised, contains info about the current and old lighting profiles</param>
    private void HandleLightingProfileChanged(object sender, LightingProfileChangedEventArgs e)
    {
        UpdateLightBasedOnProfile(e.NewProfile);    
    }

    /// <summary>
    /// Updates the light intensity based on the lighting profile
    /// </summary>
    /// <param name="profile"></param>
    private void UpdateLightBasedOnProfile(LightingProfile profile)
    {
        if (profile.name.Contains("Night"))
        {
            lightComponent.intensity = nightIntensity;
        }
        else
        {
            lightComponent.intensity = dayIntensity;
        }
    }

    /// <summary>
    /// Unregister the event listener if the object is destroyed
    /// </summary>
    private void OnDestroy()
    {
        // Unsubscribe from the event when the object is destroyed
        DynamicLightingController lightingController = FindObjectOfType<DynamicLightingController>();
        if (lightingController != null)
        {
            lightingController.OnLightingProfileChanged -= HandleLightingProfileChanged;
        }
    }
}