namespace FractalShaderEditor {
    using UnityEditor;
    using UnityEngine;

    public class FractalShaderVersionChanges : EditorWindow {

        //Variables.
        Vector2 scrollPosition = Vector2.zero;
        GUIStyle _headerLabel = null;
        GUIStyle headerLabel {
            get {
                if (_headerLabel == null) {
                    _headerLabel = new GUIStyle(EditorStyles.boldLabel);
                    _headerLabel.alignment = TextAnchor.MiddleCenter;
                    _headerLabel.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
                }
                return _headerLabel;
            }
        }
        GUIStyle _subHeaderLabel = null;
        GUIStyle subHeaderLabel {
            get {
                if (_subHeaderLabel == null) {
                    _subHeaderLabel = new GUIStyle(EditorStyles.boldLabel);
                    _subHeaderLabel.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
                }
                return _subHeaderLabel;
            }
        }
        GUIStyle _boldWrappedLabel = null;
        GUIStyle boldWrappedLabel {
            get {
                if (_boldWrappedLabel == null) {
                    _boldWrappedLabel = new GUIStyle(EditorStyles.boldLabel);
                    _boldWrappedLabel.wordWrap = true;
                }
                return _boldWrappedLabel;
            }
        }
        GUIStyle _wrappedLabel = null;
        GUIStyle wrappedLabel {
            get {
                if (_wrappedLabel == null) {
                    _wrappedLabel = new GUIStyle(EditorStyles.label);
                    _wrappedLabel.padding = new RectOffset(25, 0, 0, 0);
                    _wrappedLabel.wordWrap = true;
                }
                return _wrappedLabel;
            }
        }
        GUIStyle _bulletPointAlignedAtTop = null;
        GUIStyle bulletPointAlignedAtTop {
            get {
                if (_bulletPointAlignedAtTop == null) {
                    _bulletPointAlignedAtTop = new GUIStyle(EditorStyles.label);
                    _bulletPointAlignedAtTop.alignment = TextAnchor.UpperCenter;
                }
                return _bulletPointAlignedAtTop;
            }
        }

        //Draw the GUI.
        void OnGUI() {

            //Display the version change text.
            EditorGUILayout.LabelField("Fractal Shader Version Changes", headerLabel);
            EditorGUILayout.GetControlRect();
            EditorGUILayout.LabelField("If you have any comments or suggestions as to how we could improve Fractal Shader, or if you want to report a bug " +
                    "in the software, feel free to e-mail us on info@battenbergsoftware.com and we'll get back to you. Thanks!", boldWrappedLabel);
            EditorGUILayout.GetControlRect();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            EditorGUILayout.LabelField("Version 1.0.4", subHeaderLabel);
            EditorGUILayout.LabelField("Improved example scene UI layout at a range of screen resolutions.", wrappedLabel);
            addBulletPoint();
            EditorGUILayout.LabelField("Improved headers, indentation and messaging in material inspector.", wrappedLabel);
            addBulletPoint();
            EditorGUILayout.LabelField("Example scene now performs animations at a speed relative to delta time.", wrappedLabel);
            addBulletPoint();
            EditorGUILayout.LabelField("Updated minimum Unity version from 2018.4 to 2019.4.", wrappedLabel);
            addBulletPoint();
            EditorGUILayout.GetControlRect();
            EditorGUILayout.LabelField("Version 1.0.3", subHeaderLabel);
            EditorGUILayout.LabelField("Can now adjust the hue, saturation and value of fractals via new shader parameters.", wrappedLabel);
            addBulletPoint();
            EditorGUILayout.LabelField("Added the editor scripts to the \"FractalShaderEditor\" namespace.", wrappedLabel);
            addBulletPoint();
            EditorGUILayout.GetControlRect();
            EditorGUILayout.LabelField("Version 1.0.2", subHeaderLabel);
            EditorGUILayout.LabelField("Added support for transparency, which can be turned on or off on the material. When turned on, a master transparency " +
                    "level for the fractal can be set along with a minimum and maximum number of iterations to allow the fractal to fade from transparent to " +
                    "opaque between these levels.", wrappedLabel);
            addBulletPoint();
            EditorGUILayout.LabelField("Added this version changes window.", wrappedLabel);
            addBulletPoint();
            EditorGUILayout.GetControlRect();
            EditorGUILayout.LabelField("Version 1.0.1", subHeaderLabel);
            EditorGUILayout.LabelField("Removed \"ZTest Always\" from the shader - this is not needed and was causing some confusion.", wrappedLabel);
            addBulletPoint();
            EditorGUILayout.LabelField("Improved the \"Static Noise\" fractal in the example scene.", wrappedLabel);
            addBulletPoint();
            EditorGUILayout.GetControlRect();
            EditorGUILayout.LabelField("Version 1.0.0", subHeaderLabel);
            EditorGUILayout.LabelField("Initial release.", wrappedLabel);
            EditorGUILayout.EndScrollView();
        }

        //Adds a bullet point before the label that has just been added.
        void addBulletPoint() {
            Rect rect = GUILayoutUtility.GetLastRect();
            EditorGUI.LabelField(new Rect(17, rect.yMin - 1, 12, rect.height), "•", bulletPointAlignedAtTop);
        }
    }
}
