namespace FractalShaderEditor {
    using System;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class FractalMaterialEditor : ShaderGUI {

        //Variables.
        static int previewSize = 3;
        static GUIStyle _versionStyle = null;
        static GUIStyle versionStyle {
            get {
                if (_versionStyle == null) {
                    _versionStyle = new GUIStyle(GUI.skin.label);
                    _versionStyle.fontStyle = FontStyle.Bold;
                    _versionStyle.normal.textColor = new Color(0, 0.4f, 0.8f);
                    _versionStyle.hover.textColor = new Color(0, 0.4f, 0.8f);
                    _versionStyle.active.textColor = new Color(0, 0.4f, 0.8f);
                    _versionStyle.focused.textColor = new Color(0, 0.4f, 0.8f);
                    _versionStyle.alignment = TextAnchor.MiddleRight;
                }
                return _versionStyle;
            }
        }

        //On GUI.
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties) {

            //Only draw anything if the inspector is visible.
            if (materialEditor.isVisible) {

                //Get the current material.
                Material material = materialEditor.target as Material;

                //Display the transparency settings.
                EditorGUILayout.LabelField(new GUIContent("Transparency Settings"), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                bool isTransparent = EditorGUILayout.Toggle("Use Transparency", material.GetInt("isTransparent") == 1);
                if (isTransparent) {
                    material.SetInt("isTransparent", 1);
                    material.SetFloat("_zWrite", 0);
                    material.SetFloat("_sourceBlend", (float) BlendMode.SrcAlpha);
                    material.SetFloat("_destinationBlend", (float) BlendMode.OneMinusSrcAlpha);
                    material.SetFloat("masterAlpha", EditorGUILayout.Slider("Master Alpha", material.GetFloat("masterAlpha"), 0, 1));
                    float minimumFractalAlphaIterations = material.GetFloat("minimumFractalAlphaIterations");
                    float maximumFractalAlphaIterations = material.GetFloat("maximumFractalAlphaIterations");
                    EditorGUILayout.MinMaxSlider(new GUIContent("Fractal Alpha Range"), ref minimumFractalAlphaIterations, ref maximumFractalAlphaIterations, 0,
                            1);
                    material.SetFloat("minimumFractalAlphaIterations", minimumFractalAlphaIterations);
                    material.SetFloat("maximumFractalAlphaIterations", maximumFractalAlphaIterations);
                }
                else {
                    material.SetInt("isTransparent", 0);
                    material.SetFloat("_zWrite", 1);
                    material.SetFloat("_sourceBlend", (float) BlendMode.One);
                    material.SetFloat("_destinationBlend", (float) BlendMode.Zero);
                }
                EditorGUI.indentLevel--;

                //Display the fractal settings.
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField(new GUIContent("Fractal Settings"), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                int fractalType = EditorGUILayout.IntPopup(new GUIContent("Fractal Type", "The type of fractal to display."), material.GetInt("fractalType"),
                        new GUIContent[] { new GUIContent("Mandelbrot"), new GUIContent("Julia") }, new int[] { 0, 1 });
                material.SetInt("fractalType", fractalType);
                material.SetInt("iterations", Math.Min(Math.Max(EditorGUILayout.IntField(new GUIContent("Iterations", "The number of iterations for each " +
                        "pixel in the fractal image. Larger values will allow for more distinct colours in the fractal but will take longer to render."),
                        material.GetInt("iterations")), 2), 9999));
                material.SetFloat("convergenceThreshold", Math.Min(Math.Max(EditorGUILayout.FloatField(new GUIContent("Convergence Threshold",
                        "The value at which each pixel in the fractal algorithm is deemed to have \"converged\". Keep this as low as possible to ensure " +
                        "rendering is quick, but the image quality is not compromised."), material.GetFloat("convergenceThreshold")), 1),
                        9999));
                material.SetInt("smoothing", EditorGUILayout.Toggle(new GUIContent("Smoothing", "Smooths the fractal colours. This does not work for all " +
                        "colouring methods, so ensure it is turned off if it doesn't make any difference. If this option is selected and the fractal still " +
                        "does not appear to be as smooth as it could be, try increasing the convergence threshold, above."),
                        material.GetInt("smoothing") == 1) ? 1 : 0);
                material.SetInt("multibrot", Math.Min(Math.Max(EditorGUILayout.IntField(new GUIContent("Multibrot", "A \"Multibrot\" is an extension of the " +
                        "Mandelbrot/Julia set which is rendered by raising each point in the complex plane to a value other than the default 2. Experiment " +
                        "to see what Multibrots from 2 to 5 look like!"), material.GetInt("multibrot")), 2), 5));
                Vector2 centre = EditorGUILayout.Vector2Field(new GUIContent("Centre", "The centre point of the fractal to render. This can be set manually, " +
                        "or using the preview image, below."), new Vector2(material.GetFloat("centreX"), material.GetFloat("centreY")));
                float scale = EditorGUILayout.FloatField(new GUIContent("Scale", "The scale of the fractal to render. This can be set manually, or using the " +
                        "preview image, below."), material.GetFloat("scale"));
                EditorGUI.indentLevel--;
                if (fractalType == 1) {
                    EditorGUILayout.Separator();
                    EditorGUILayout.LabelField(new GUIContent("Julia Fractal Type Settings"), EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    Rect juliaFractalConstantRectangle = EditorGUILayout.GetControlRect(true);
                    EditorGUI.LabelField(new Rect(juliaFractalConstantRectangle.xMin, juliaFractalConstantRectangle.yMin, EditorGUIUtility.labelWidth,
                            juliaFractalConstantRectangle.height), new GUIContent("Constant", "The complex constant to use when generating a Julia fractal."));
                    material.SetFloat("juliaConstantReal", EditorGUI.FloatField(new Rect(juliaFractalConstantRectangle.xMin + EditorGUIUtility.labelWidth,
                            juliaFractalConstantRectangle.yMin, (juliaFractalConstantRectangle.width - EditorGUIUtility.labelWidth) * 0.4f,
                            juliaFractalConstantRectangle.height), material.GetFloat("juliaConstantReal"), EditorStyles.numberField));
                    EditorGUI.LabelField(new Rect(juliaFractalConstantRectangle.xMin + (juliaFractalConstantRectangle.width * 0.4f) +
                            (EditorGUIUtility.labelWidth * 0.6f), juliaFractalConstantRectangle.yMin, (juliaFractalConstantRectangle.width -
                            EditorGUIUtility.labelWidth) * 0.2f, juliaFractalConstantRectangle.height), "+ i", EditorStyles.label);
                    material.SetFloat("juliaConstantImaginary", EditorGUI.FloatField(new Rect(juliaFractalConstantRectangle.xMin +
                            (juliaFractalConstantRectangle.width * 0.6f) + (EditorGUIUtility.labelWidth * 0.4f), juliaFractalConstantRectangle.yMin,
                            (juliaFractalConstantRectangle.width - EditorGUIUtility.labelWidth) * 0.4f, juliaFractalConstantRectangle.height),
                            material.GetFloat("juliaConstantImaginary"), EditorStyles.numberField));
                    EditorGUI.indentLevel--;
                }

                //Display the colour settings.
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField(new GUIContent("Colour Settings"), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                material.SetColor("backgroundColour", EditorGUILayout.ColorField(new GUIContent("Background Colour",
                        "The background colour to merge with the fractal."), material.GetColor("backgroundColour")));
                material.SetFloat("backgroundColourAmount", EditorGUILayout.Slider(new GUIContent("Background Amount", "The amount that the background " +
                        "colour is to be merged with the fractal. A value of 0 ignores the background colour completely, while a value of 1 shows only the " +
                        "background colour."), material.GetFloat("backgroundColourAmount"), 0, 1));
                for (int i = 0; i < 3; i++) {
                    string colourName = i == 0 ? "Red" : (i == 1 ? "Green" : "Blue");
                    EditorGUILayout.Separator();
                    EditorGUILayout.LabelField(new GUIContent(colourName), EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    int formula = EditorGUILayout.IntPopup(new GUIContent("Formula", "The formula for calculating the " + colourName.ToLower() +
                            " component of the fractal colour."), material.GetInt(colourName.ToLower() + "Formula"),
                            new GUIContent[] { new GUIContent("Fixed Value"), new GUIContent("Iterations"), new GUIContent("Sine (Iterations * Multiplier)"),
                        new GUIContent("Cosine (Iterations * Multiplier)"), new GUIContent("Tangent (Iterations * Multiplier)"),
                        new GUIContent("Real (Zn) * Multiplier"), new GUIContent("Imaginary (Zn) * Multiplier"),
                        new GUIContent("Sine (Real (Zn) * Multiplier)"), new GUIContent("Cosine (Real (Zn) * Multiplier)"),
                        new GUIContent("Tangent (Real (Zn) * Multiplier)"), new GUIContent("Sine (Imaginary (Zn) * Multiplier)"),
                        new GUIContent("Cosine (Imaginary (Zn) * Multiplier)"), new GUIContent("Tangent (Imaginary (Zn) * Multiplier)") },
                            new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
                    material.SetInt(colourName.ToLower() + "Formula", formula);
                    if (formula == 0)
                        material.SetFloat(colourName.ToLower() + "FixedValue", Math.Min(Math.Max(EditorGUILayout.FloatField(new GUIContent("Value",
                                "The fixed " + colourName.ToLower() + " value to set."), material.GetFloat(colourName.ToLower() + "FixedValue")), 0), 1));
                    else if (formula >= 2)
                        material.SetFloat(colourName.ToLower() + "Multiplier", EditorGUILayout.FloatField(new GUIContent("Multiplier",
                                "The multiplier to apply to the selected formula for calculating the " + colourName.ToLower() +
                                " component of the fractal colour."), material.GetFloat(colourName.ToLower() + "Multiplier")));
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField(new GUIContent("HSV"), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                float h = EditorGUILayout.Slider(new GUIContent("Hue Change",
                        "The amount the hue of each pixel should be changed. Changes the colours of the fractal while maintaining its other properties."),
                        material.GetFloat("hueChange"), -1, 1);
                material.SetFloat("hueChange", h);
                float s = EditorGUILayout.Slider(new GUIContent("Saturation Multiplier",
                        "The amount the saturation of each pixel should be changed. Can be used to make the colours brighter or closer to grey."),
                        material.GetFloat("saturationMultiplier"), 0, 2);
                material.SetFloat("saturationMultiplier", s);
                float v = EditorGUILayout.Slider(new GUIContent("Value Multiplier",
                        "The amount the value of each pixel should be changed. Can be used to darken/brighten the fractal's colours."),
                        material.GetFloat("valueMultiplier"), 0, 2);
                material.SetFloat("valueMultiplier", v);
                if (EditorGUI.EndChangeCheck()) {
                    float vsu = v * s * Mathf.Cos(h * (Mathf.PI / 2));
                    float vsw = v * s * Mathf.Sin(h * (Mathf.PI / 2));
                    material.SetVector("HSVMatrix1", new Vector4(
                        .299f * v + .701f * vsu + .168f * vsw,
                        .587f * v - .587f * vsu + .330f * vsw,
                        .114f * v - .114f * vsu - .497f * vsw,
                        0));
                    material.SetVector("HSVMatrix2", new Vector4(
                        .299f * v - .299f * vsu - .328f * vsw,
                        .587f * v + .413f * vsu + .035f * vsw,
                        .114f * v - .114f * vsu + .292f * vsw,
                        0));
                    material.SetVector("HSVMatrix3", new Vector4(
                        .299f * v - .3f * vsu + 1.25f * vsw,
                        .587f * v - .588f * vsu - 1.05f * vsw,
                        .114f * v + .886f * vsu - .203f * vsw,
                        0));
                }
                EditorGUI.indentLevel -= 2;
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField(new GUIContent("Preview"), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                previewSize = EditorGUILayout.IntPopup(new GUIContent("Preview Size",
                        "Whether to draw a preview image, below, and if so what size to draw it at."), previewSize,
                        new GUIContent[] { new GUIContent("Off"), new GUIContent("Small"), new GUIContent("Medium"), new GUIContent("Large") },
                        new int[] { 0, 1, 2, 3 });
                if (previewSize > 0)
                    EditorGUILayout.HelpBox("Use the left mouse button to drag the preview fractal below, and the mouse wheel to zoom in or out. When using " +
                            "the mouse wheel, hold shift to zoom at a faster rate or control to zoom at a slower rate. These operations will affect the " +
                            "\"centre\" and \"scale\" properties, above.", MessageType.Info);
                EditorGUI.indentLevel--;

                //Draw the preview image, if required.
                if (previewSize > 0) {
                    int targetWidth = Mathf.RoundToInt((EditorGUIUtility.currentViewWidth * 9) / (previewSize == 3 ? 10 : previewSize == 2 ? 20 : 40));
                    Rect previewRectangle = EditorGUILayout.GetControlRect(false, targetWidth, GUIStyle.none);
                    previewRectangle.width = Math.Min(previewRectangle.width, targetWidth);
                    previewRectangle.height = previewRectangle.width;
                    float centreHorizontally = ((float) ((EditorGUIUtility.currentViewWidth * 9) / 10) - previewRectangle.width) / 2;
                    previewRectangle.xMin += centreHorizontally;
                    previewRectangle.xMax += centreHorizontally;
                    for (int i = 0; i < 3; i++) {
                        EditorGUI.DrawRect(previewRectangle, i % 2 == 0 ? Color.black : Color.white);
                        previewRectangle.xMin += previewRectangle.width * 0.01f;
                        previewRectangle.xMax -= previewRectangle.width * 0.01f;
                        previewRectangle.yMin += previewRectangle.width * 0.01f;
                        previewRectangle.yMax -= previewRectangle.width * 0.01f;
                    }

                    //Draw the preview texture, creating it if necessary.
                    Texture2D previewTexture = new Texture2D(32, 32);
                    GUI.SetNextControlName("Preview Texture");
                    EditorGUI.DrawPreviewTexture(previewRectangle, previewTexture, material);
                    GameObject.DestroyImmediate(previewTexture);

                    //The mouse wheel zooms in and out of the fractal.
                    if (Event.current.type == EventType.ScrollWheel) {
                        Vector2 mousePosition = Event.current.mousePosition - new Vector2(previewRectangle.xMin, previewRectangle.yMin);
                        if (Math.Abs(Event.current.delta.y) > 0 && mousePosition.x > 0 && mousePosition.x < previewRectangle.xMax && mousePosition.y > 0 &&
                                mousePosition.y < previewRectangle.yMax) {
                            GUI.FocusControl("Preview Texture");
                            scale *= (Event.current.delta.y / (Event.current.shift ? 10 : (Event.current.control ? 1000 : 100))) + 1;
                            Event.current.Use();
                        }
                    }

                    //The left mouse button drags the preview image.
                    else if (Event.current.type == EventType.MouseDrag && Event.current.button == 0) {
                        Vector2 mousePosition = Event.current.mousePosition - new Vector2(previewRectangle.xMin, previewRectangle.yMin);
                        if (mousePosition.x > 0 && mousePosition.x < previewRectangle.xMax && mousePosition.y > 0 && mousePosition.y < previewRectangle.yMax) {
                            GUI.FocusControl("Preview Texture");
                            Vector2 mouseDelta = new Vector2(-Event.current.delta.x, Event.current.delta.y);
                            centre += (mouseDelta * scale) / previewRectangle.width;
                            Event.current.Use();
                        }
                    }
                }

                //Set the centre and scale properties, now that they have potentially been modified by the dragging/scaling of the preview image.
                material.SetFloat("centreX", centre.x);
                material.SetFloat("centreY", centre.y);
                material.SetFloat("scale", scale);

                //Version details.
                EditorGUILayout.GetControlRect();
                Rect rect = EditorGUILayout.GetControlRect();
                if (GUI.Button(new Rect(rect.xMax - 100, rect.yMin, 100, rect.height), "Version 1.0.4", versionStyle)) {
                    FractalShaderVersionChanges versionChangesEditorWindow = EditorWindow.GetWindow<FractalShaderVersionChanges>();
                    versionChangesEditorWindow.minSize = new Vector2(800, 600);
                    versionChangesEditorWindow.titleContent = new GUIContent("Fractal Shader - Version Changes");
                }
            }
        }
    }
}