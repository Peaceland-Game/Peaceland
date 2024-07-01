using UnityEngine;

public class OutdoorLight : MonoBehaviour
{
    public Light lightComponent;
    public float dayIntensity = 0f;
    public float nightIntensity = 133f;

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

    private void HandleLightingProfileChanged(object sender, LightingProfileChangedEventArgs e)
    {
        UpdateLightBasedOnProfile(e.NewProfile);
    }

    private void UpdateLightBasedOnProfile(LightingProfile profile)
    {
        // Assuming the profile name contains "Night" for night profiles
        if (profile.name.Contains("Night"))
        {
            Debug.Log($"Turning on {name}");
            lightComponent.intensity = nightIntensity;
        }
        else
        {
            Debug.Log($"Turning off {name}");
            lightComponent.intensity = dayIntensity;
        }
    }

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