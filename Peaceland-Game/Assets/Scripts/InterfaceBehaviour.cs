using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject keySprite;
    [SerializeField] private GameObject lockPickRules;
    [SerializeField] private GameObject NoiseBar;
    private Vector3 noiseBarFullScale = new(1.311804f, 1.396252f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateNoiseBar(float noiseAmount, float maxNoise)
    {
        // Ensure that noiseAmount is never less than a small value (e.g., 0.01f)
        noiseAmount = Mathf.Max(noiseAmount, 0.01f);

        // Calculate the noise ratio (between 0 and 1)
        float noiseRatio = noiseAmount / maxNoise;

        // Set the scale of the NoiseBar based on the noise ratio
        Vector3 newScale = noiseBarFullScale;
        newScale.y *= noiseRatio;
        NoiseBar.transform.localScale = newScale;
    }
    public void PositionKeyIcon(Vector3 itemWorldPos)
    {
        //keySprite.transform.localPosition = Camera.main.WorldToScreenPoint(itemWorldPos);
    }
    public void HideKeyIcon()
    {
        keySprite.SetActive(false);
    }
    public void ShowKeyIcon()
    {
        keySprite.SetActive(true);
    }
    public void HideLockPickRules()
    {
        lockPickRules.SetActive(false);
    }
    public void ShowLockPickRules()
    {
        lockPickRules.SetActive(true);
    }
}
