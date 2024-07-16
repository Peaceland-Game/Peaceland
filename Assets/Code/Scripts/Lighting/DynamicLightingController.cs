using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Handles changing outdoor lighting between different profiles
/// used to change the time of day during a scene
/// </summary>
[ExecuteAlways]
public class DynamicLightingController : MonoBehaviour
{

    
    public Light directionalLight;                      //reference to the directional light in the scene
    public LightingProfile[] lightingProfiles;          //Array of all lighting profiles, these are scriptable 
    public int currentProfileIndex = 0;                 //hold the value for the current profile
    private Coroutine transitionCoroutine;              //refrence to the transition coroutine

    [Header("Inspector Testing")]
    [SerializeField] private int testProfileIndex = 0;      //used to apply a test profile in the Editor not during runtime
    [SerializeField] private bool applyTestProfile = false; //flag to apply the test profile or not

    public event System.EventHandler<LightingProfileChangedEventArgs> OnLightingProfileChanged; //Event to raise when changing the lighting
    [SerializeField] private DialogueSkipper dialogueSkipper;                                   //reference to the dialogue skipper component used for editor testing

    /// <summary>
    /// Triggers a lighting profile change event for other scripts to hook in to
    /// </summary>
    /// <param name="oldProfile">The lighting profile object we are changing from</param>
    /// <param name="newProfile">The lighting profile object we are changing to</param>
    /// <param name="newProfileIndex">The integer index of the new profile</param>
    protected virtual void RaiseLightingProfileChangedEvent(LightingProfile oldProfile, LightingProfile newProfile, int newProfileIndex)
    {
        OnLightingProfileChanged?.Invoke(this, new LightingProfileChangedEventArgs(oldProfile, newProfile, newProfileIndex));
    }
    /// <summary>
    /// Handles listening for and applying a test profile in the editor
    /// </summary>
    private void Update()
    {
        //// Check if the test profile should be applied
        if (applyTestProfile)
        {
            applyTestProfile = false; // Reset the flag
            ApplyProfile(lightingProfiles[testProfileIndex], GameObject.FindWithTag("UI").GetComponent<UserInterface>());
            currentProfileIndex = testProfileIndex;
            Debug.Log($"Applied test profile: {lightingProfiles[testProfileIndex].name}");
            
        }
    }
    /// <summary>
    /// Handles Transitioning to the next profile (up 1 from the current profile index)
    /// </summary>
    /// <param name="userInterface">A reference to the userInterface script used to display a load screen</param>
    /// <param name="action">A runnable used to perform an action after the transition is over</param>
    public void TransitionToNextProfile(UserInterface userInterface, Action action)
    {
        int nextProfileIndex = (currentProfileIndex + 1) % lightingProfiles.Length;
        TransitionToProfile(nextProfileIndex, 5f, userInterface, action); // 5 second transition
    }
    /// <summary>
    /// Handles transitioning to a specific profile via the Time of day enum type
    /// </summary>
    /// <param name="timeOfDay">The TimeOfDay enum type for time of day usually like Day/Evening/Night</param>
    /// <param name="duration">how long the transition should take in seconds</param>
    /// <param name="userInterface">A reference to the userInterface script used to display a load screen</param>
    /// <param name="action">A runnable used to perform an action after the transition is over</param>
    public void TransitionToProfile(TimeOfDay timeOfDay, float duration, UserInterface userInterface, Action action)
    {
        int profileIndex = (int)timeOfDay;
        TransitionToProfile(profileIndex, duration, userInterface, action);
    }
    /// <summary>
    /// Handles transitioning to a specific profile via the index number
    /// </summary>
    /// <param name="profileIndex">integer index to transition the lighting to</param>
    /// <param name="duration">how long the transition should take in seconds</param>
    /// <param name="userInterface">A reference to the userInterface script used to display a load screen</param>
    /// <param name="action">A runnable used to perform an action after the transition is over</param>
    private void TransitionToProfile(int profileIndex, float duration, UserInterface userInterface, Action action)
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(TransitionCoroutine(profileIndex, duration, userInterface, action));
    }

    /// <summary>
    /// Handles transitioning to a new lighting profile over a set duration
    /// </summary>
    /// <param name="profileIndex">integer index to transition the lighting to</param>
    /// <param name="duration">how long the transition should take in seconds</param>
    /// <param name="userInterface">A reference to the userInterface script used to display a load screen</param>
    /// <param name="action">A runnable used to perform an action after the transition is over</param>
    /// <returns></returns>
    private IEnumerator TransitionCoroutine(int targetProfileIndex, float duration, UserInterface userInterface, Action action)
    {
        Debug.Log($"Starting transition from profile {currentProfileIndex} to {targetProfileIndex}");
        //get start and target profiles
        LightingProfile startProfile = lightingProfiles[currentProfileIndex];
        LightingProfile targetProfile = lightingProfiles[targetProfileIndex];
        float elapsedTime = 0f;

        //interoplate the lighting settings over the duration and update the load bar
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            InterpolateLightingSettings(startProfile, targetProfile, t);

            // Update the loading progress
            userInterface.UpdateLoadingProgress(t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //apply the pofile when finished
        ApplyProfile(targetProfile, userInterface);
        int oldProfileIndex = currentProfileIndex;
        currentProfileIndex = targetProfileIndex;
        Debug.Log($"Transition complete. Current profile is now {currentProfileIndex}");

        // Ensure the loading progress is set to 100% at the end
        userInterface.UpdateLoadingProgress(1f);


#if (UNITY_EDITOR)
        if (dialogueSkipper)
        {
            dialogueSkipper.Skip(); //used to skip early dialogue choices to make testing easier
        }
#endif
        
        action();   //run the passed action at the end of the coroutine, this is currently used to reenable player movement - passed in from MemorySwapper.cs
        RaiseLightingProfileChangedEvent(startProfile, targetProfile, currentProfileIndex);// Raise the event saying the lighting changed
    }

    /// <summary>
    /// Handles the transition of the skybox and directional light
    /// </summary>
    /// <param name="start">The start lighting profile</param>
    /// <param name="target">The target lighting profile</param>
    /// <param name="t">How far a long in the transition we are, passed in as a parameter from the coroutine</param>
    private void InterpolateLightingSettings(LightingProfile start, LightingProfile target, float t)
    {
        // Skybox settings
        RenderSettings.skybox.SetColor("_Tint", Color.Lerp(start.skyboxTint, target.skyboxTint, t));
        RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(start.skyboxExposure, target.skyboxExposure, t));
        RenderSettings.skybox.SetFloat("_Rotation", Mathf.Lerp(start.skyboxRotationOffset, target.skyboxRotationOffset, t));

        // Sun settings
        directionalLight.transform.rotation = Quaternion.Slerp(
            Quaternion.Euler(start.sunPitch, start.sunRotation, 0),
            Quaternion.Euler(target.sunPitch, target.sunRotation, 0),
            t
        );
        directionalLight.intensity = Mathf.Lerp(start.sunIntensity, target.sunIntensity, t);

        // Interpolate Kelvin and use it to get the sun color
        float interpolatedKelvin = Mathf.Lerp(start.kelvin, target.kelvin, t);
        Color startColor = start.GetSunColor();
        Color targetColor = target.GetSunColor();
        directionalLight.color = Color.Lerp(startColor, targetColor, t);

        // Shadow settings
        directionalLight.shadowStrength = Mathf.Lerp(start.sunShadowStrength, target.sunShadowStrength, t);
        QualitySettings.shadowDistance = Mathf.Lerp(start.shadowDistance, target.shadowDistance, t);

        // Ambient settings
        RenderSettings.ambientMode = target.ambientMode == LightingProfile.AmbientModeEnum.Trilight ?
            UnityEngine.Rendering.AmbientMode.Trilight : UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientSkyColor = Color.Lerp(start.skyAmbient, target.skyAmbient, t);
        RenderSettings.ambientEquatorColor = Color.Lerp(start.equatorAmbient, target.equatorAmbient, t);
        RenderSettings.ambientGroundColor = Color.Lerp(start.groundAmbient, target.groundAmbient, t);

        // Fog settings
        RenderSettings.fogMode = (FogMode)target.fogMode;
        RenderSettings.fogColor = Color.Lerp(start.fogColor, target.fogColor, t);
        RenderSettings.fogDensity = Mathf.Lerp(start.fogDensity, target.fogDensity, t);
        RenderSettings.fogStartDistance = Mathf.Lerp(start.fogHeight, target.fogHeight, t);
    }

    
    private void ApplyProfile(LightingProfile profile, UserInterface userInterface)
    {
        // Apply all settings directly without interpolation
        // Skybox
        RenderSettings.skybox.SetColor("_Tint", profile.skyboxTint);
        RenderSettings.skybox.SetFloat("_Exposure", profile.skyboxExposure);
        RenderSettings.skybox.SetFloat("_Rotation", profile.skyboxRotationOffset);
        if (profile.hdriSkybox != null)
        {
            RenderSettings.skybox.SetTexture("_Tex", profile.hdriSkybox);
        }

        // Sun
        directionalLight.transform.rotation = Quaternion.Euler(profile.sunPitch, profile.sunRotation, 0);
        directionalLight.intensity = profile.sunIntensity;
        directionalLight.color = profile.GetSunColor();

        // Shadows
        directionalLight.shadowStrength = profile.sunShadowStrength;
        directionalLight.shadows = profile.sunShadowCastingMode == LightingProfile.ShadowCastingMode.Soft ?
            LightShadows.Soft : LightShadows.Hard;
        QualitySettings.shadowDistance = profile.shadowDistance;

        // Ambient
        RenderSettings.ambientMode = profile.ambientMode == LightingProfile.AmbientModeEnum.Trilight ?
            UnityEngine.Rendering.AmbientMode.Trilight : UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientSkyColor = profile.skyAmbient;
        RenderSettings.ambientEquatorColor = profile.equatorAmbient;
        RenderSettings.ambientGroundColor = profile.groundAmbient;

        // Fog
        RenderSettings.fogMode = (FogMode)profile.fogMode;
        RenderSettings.fogColor = profile.fogColor;
        RenderSettings.fogDensity = profile.fogDensity;
        RenderSettings.fogStartDistance = profile.fogHeight;

        //Debug.Log("Finished transition re enable loadscreen");
        userInterface.DisableLoadScreen();
        // Post-processing
        // You'll need to implement a way to switch between volume profiles if you're using URP or HDRP
    }
}