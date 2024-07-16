using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Holds lighting information in a scriptable object that can be used to change the lighting during runtime
/// </summary>
[CreateAssetMenu(fileName = "New Lighting Profile", menuName = "Lighting/Gaia Lighting Profile")]
public class LightingProfile : ScriptableObject
{
    [Header("Profile Settings")]
    public bool updateChangesInRealtime = true;
    public enum SkyTypeEnum { HDRI, Procedural }
    public SkyTypeEnum profileSkyType = SkyTypeEnum.HDRI;

    [Header("Post Processing Settings")]
    public VolumeProfile postProcessingProfile;

    [Header("Skybox Settings")]
    public Cubemap hdriSkybox;
    public Color skyboxTint = Color.white;
    [Range(0f, 8f)]
    public float skyboxExposure = 1f;
    [Range(-180f, 180f)]
    public float skyboxRotationOffset = 0f;

    [Header("Sun Settings")]
    [Range(-180f, 180f)]
    public float sunRotation = 0f;
    [Range(-90f, 90f)]
    public float sunPitch = 50f;
    public bool useKelvin = true;
    public Gradient kelvinGradient;
    [Range(1000f, 20000f)]
    public float kelvin = 5750f;
    [Range(0f, 8f)]
    public float sunIntensity = 5.5f;

    [Header("Shadow Settings")]
    [Range(0f, 1f)]
    public float sunShadowStrength = 1f;
    public enum ShadowCastingMode { Soft, Hard }
    public ShadowCastingMode sunShadowCastingMode = ShadowCastingMode.Soft;
    public enum ShadowResolution { FromQualitySettings, Low, Medium, High, VeryHigh }
    public ShadowResolution sunShadowResolution = ShadowResolution.FromQualitySettings;
    [Range(0f, 1000f)]
    public float shadowDistance = 512f;

    

    public enum AmbientModeEnum { Skybox, Gradient, Color, Trilight }
    [Header("Ambient Settings")]
    public AmbientModeEnum ambientMode = AmbientModeEnum.Trilight;
    public Color skyAmbient = Color.white;
    public Color equatorAmbient = Color.gray;
    public Color groundAmbient = Color.black;

    
    public enum FogModeEnum { Linear, Exponential, ExponentialSquared }
    [Header("Fog Settings")]
    public FogModeEnum fogMode = FogModeEnum.Exponential;
    public Color fogColor = Color.white;
    [Range(0f, 1f)]
    public float fogDensity = 0.001f;
    public float fogHeight = 0f;
    [Range(0f, 1f)]
    public float fogGradient = 0.75f;

    public Color GetSunColor()
    {
        if (useKelvin)  
        {
            // Map Kelvin value to gradient
            float t = Mathf.InverseLerp(1000f, 20000f, kelvin);
            return kelvinGradient.Evaluate(t);
        }
        else
        {
            // If not using Kelvin, return the middle color of the gradient
            return kelvinGradient.Evaluate(0.5f);
        }
    }
}