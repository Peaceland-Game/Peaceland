using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisualController : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] Renderer renderer;
    [SerializeField] float speed;
    [SerializeField] AnimationCurve curve;

    [Header("Overall Visuals")]
    [SerializeField] EmotionalState emotionalState;

    [Header("Eyes")]
    [SerializeField] List<Renderer> eyes;
    [SerializeField] List<EyeSettings> eyeSettings;

    private Material[] materials;

    private int activeCoroutines = 0;
    private EmotionalState holdState;

    private void Start()
    {
        materials = renderer.materials;
        holdState = emotionalState;
    }

    // Update is called once per frame
    void Update()
    {
        DebugLogic();
        EyeLogic();
    }

    public void EyeLogic()
    {
        if (emotionalState == holdState)
        {
            return;
        }

        holdState = emotionalState;

        UpdateEyes();

    }

    public void UpdateEyes()
    {
        /*
        // Load new settings into each eye renderer's materials 
        foreach (Renderer eyeRenderer in eyes)
        {
            EyeSettings settings = FindEyeSettings(emotionalState);
            settings?.LoadAttributesIntoEye(eyeRenderer, eyeRenderer.material.shader);
        }
        */

        for (int i = 0; i < eyes.Count; i++)
        {
            // Determining a stupid, arbitrary convention:
            // - even numbered eyes have their cutout center X value multiplied by -1,
            // - odd numbered eyes have their X value multiplied by 1
            Vector3 offset;
            if (i % 2 == 0)
            {
                offset = new Vector3(-1, 1, 1);
            }
            else
            {
                offset = new Vector3(1, 1, 1);
            }

            EyeSettings settings = FindEyeSettings(emotionalState);
            settings?.LoadAttributesIntoEye(eyes[i], eyes[i].material.shader, offset);
        }
    }

    /// <summary>
    /// Searches for the first 
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    private EyeSettings FindEyeSettings(EmotionalState state)
    {
        foreach (EyeSettings eye in eyeSettings)
        {
            if(eye.state == state)
            {
                return eye;
            }
        }

        // Eye not found 
        Debug.LogWarning("Eye state " + state + " not found");
        return null;
    }

    [Serializable]
    private class EyeSettings
    {
        [SerializeField] public EmotionalState state;
        [SerializeField] ShaderPropertyEdit.ShaderProperties eyeAttributes;

        public void LoadAttributesIntoEye(Renderer eyeRenderer, Shader shader, Vector3 cutoutOffset)
        {
            // Call the generic methods for loading shader properties for the material
            ShaderPropertyEdit.GeneratePropertyHelpers(eyeAttributes, shader);
            ShaderPropertyEdit.LoadIntoMaterial(Application.isEditor ? eyeRenderer.sharedMaterial : eyeRenderer.material, eyeAttributes);

            // Update a specific property that the eyes have that allow the
            // cutout portion to be properly mirrored for left / right eyes.
            // Narai, I'm deeply sorry for defiling your beautiful tool.
            eyeRenderer.material.SetVector("_CutoutCenterOffset", cutoutOffset);
        }
    }


    private enum EmotionalState
    {
        CONTENT,
        SLEEPY,
        STUNNED,
        SAD,
        PLEASED,
        JOYFUL,
        ANGRY
    }

    #region Debug

    private void DebugLogic()
    {
        if (activeCoroutines > 0)
            return;

        bool shouldTransition = Input.GetKeyDown(KeyCode.T);

        if (shouldTransition)
        {
            foreach (Material mat in materials)
            {
                StartCoroutine(Transition(mat));
                activeCoroutines++;
            }
        }
    }

    private IEnumerator Transition(Material mat)
    {
        float lerp = mat.GetFloat("_NoiseType");
        print(lerp);
        if(lerp <= .001f)
        {
            while (lerp <= 1.0f)
            {
                mat.SetFloat("_NoiseType", curve.Evaluate(Mathf.PingPong(lerp, 1.0f)));

                lerp += speed * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (lerp >= 0.0f)
            {
                mat.SetFloat("_NoiseType", curve.Evaluate(Mathf.PingPong(lerp, 1.0f)));

                lerp -= speed * Time.deltaTime;
                yield return null;
            }
        }

        activeCoroutines--;
    }

    #endregion
}
