using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.InputSystem;

public class DynamicLightingController : MonoBehaviour
{
    public Light directionalLight;
    public LightingProfile[] lightingProfiles;

    public int currentProfileIndex = 0;
    private Coroutine transitionCoroutine;

    [Header("Inspector Testing")]
    [SerializeField] private int testProfileIndex = 0;
    [SerializeField] private bool applyTestProfile = false;
    public event System.EventHandler<LightingProfileChangedEventArgs> OnLightingProfileChanged;

    // Method to trigger the event
    protected virtual void RaiseLightingProfileChangedEvent(LightingProfile oldProfile, LightingProfile newProfile, int newProfileIndex)
    {
        OnLightingProfileChanged?.Invoke(this, new LightingProfileChangedEventArgs(oldProfile, newProfile, newProfileIndex));
    }
    private void Update()
    {
        HandleDayChangeInput();
        //// Check if the test profile should be applied
        //if (applyTestProfile)
        //{
        //    applyTestProfile = false; // Reset the flag
        //    ApplyProfile(lightingProfiles[testProfileIndex]);
        //    currentProfileIndex = testProfileIndex;
        //    Debug.Log($"Applied profile: {lightingProfiles[testProfileIndex].name}");
        //}
    }
    private void HandleDayChangeInput()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Debug.Log("Transitioning to Day");
            TransitionToProfile(0, 5);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Debug.Log("Transitioning to Evening");
            TransitionToProfile(1, 5);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            Debug.Log("Transitioning to Night");
            TransitionToProfile(2, 5);
        }
    }

    public void TransitionToNextProfile()
    {
        int nextProfileIndex = (currentProfileIndex + 1) % lightingProfiles.Length;
        TransitionToProfile(nextProfileIndex, 5f); // 5 second transition
    }
    public void TransitionToProfile(TimeOfDay timeOfDay, float duration)
    {
        int profileIndex = (int)timeOfDay;
        TransitionToProfile(profileIndex, duration);
    }
    private void TransitionToProfile(int profileIndex, float duration)
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(TransitionCoroutine(profileIndex, duration));
    }

    private IEnumerator TransitionCoroutine(int targetProfileIndex, float duration)
    {
        Debug.Log($"Starting transition from profile {currentProfileIndex} to {targetProfileIndex}");
        LightingProfile startProfile = lightingProfiles[currentProfileIndex];
        LightingProfile targetProfile = lightingProfiles[targetProfileIndex];
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            InterpolateLightingSettings(startProfile, targetProfile, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ApplyProfile(targetProfile);
        int oldProfileIndex = currentProfileIndex;
        currentProfileIndex = targetProfileIndex;
        Debug.Log($"Transition complete. Current profile is now {currentProfileIndex}");

        // Raise the event
        RaiseLightingProfileChangedEvent(startProfile, targetProfile, currentProfileIndex);
    }

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
        // Note: Unity doesn't have a built-in way to set fog gradient, you might need to use a custom shader for this
    }

    private void ApplyProfile(LightingProfile profile)
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

        // Post-processing
        // You'll need to implement a way to switch between volume profiles if you're using URP or HDRP
    }
}