using UnityEngine;
using UnityEditor;
using System.IO;

public class MapCamera : MonoBehaviour {
    public int mapResolution = 2048;
    public float mapHeight = 1000f;
    public LayerMask mapLayers;
    public string saveFilePath = "Assets/MapScreenshot.png";

    private Camera mapCamera;

    void OnEnable() {
        SetupCamera();
    }

    private void SetupCamera() {
        if (mapCamera == null) {
            mapCamera = GetComponent<Camera>();
            if (mapCamera == null) {
                mapCamera = gameObject.AddComponent<Camera>();
            }
        }

        //mapCamera.transform.position = new Vector3(0, mapHeight, 0);
        //mapCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
        //mapCamera.orthographic = true;
        mapCamera.aspect = 1; // Force 1:1 aspect ratio
        //mapCamera.cullingMask = mapLayers;
        //mapCamera.clearFlags = CameraClearFlags.SolidColor;
        //mapCamera.backgroundColor = Color.clear;
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Tools/Take Map Screenshot")]
    public static void TakeMapScreenshot() {
        MapCamera mapCam = FindObjectOfType<MapCamera>();
        if (mapCam == null) {
            Debug.LogError("No MapCamera found in the scene!");
            return;
        }
        mapCam.CaptureMapScreenshot();
    }

    public void CaptureMapScreenshot() {
        if (mapCamera == null) {
            SetupCamera();
        }

        RenderTexture rt = new RenderTexture(mapResolution, mapResolution, 24);
        mapCamera.targetTexture = rt;
        mapCamera.Render();

        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D(mapResolution, mapResolution, TextureFormat.RGBA32, false);
        screenShot.ReadPixels(new Rect(0, 0, mapResolution, mapResolution), 0, 0);
        screenShot.Apply();

        mapCamera.targetTexture = null;
        RenderTexture.active = null;

        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(saveFilePath, bytes);

        AssetDatabase.Refresh();
        Debug.Log("Map screenshot saved to: " + saveFilePath);

        DestroyImmediate(screenShot);
        rt.Release();
    }
#endif
}