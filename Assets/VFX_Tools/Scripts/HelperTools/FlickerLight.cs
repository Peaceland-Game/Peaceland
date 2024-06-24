using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FlickerLight : MonoBehaviour
{
    [SerializeField] Light pointLight; 
    [SerializeField] float flickerSpeed;
    [SerializeField] Vector2 intensityRange;
    [SerializeField] Vector2 lightRangeRange;

    // Update is called once per frame
    void Update()
    {
        float lerp = Mathf.PerlinNoise1D(Time.time * flickerSpeed);
        pointLight.intensity = Mathf.Lerp(intensityRange.x, intensityRange.y, lerp);
        pointLight.range = Mathf.Lerp(lightRangeRange.y, lightRangeRange.x, lerp);
    }
}
