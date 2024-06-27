using UnityEngine;
using System.IO;

public class ScreenshotCapture : MonoBehaviour
{
    public int resWidth = 1920;
    public int resHeight = 1080;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public Texture2D CaptureScreenshot()
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        mainCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        mainCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        return screenShot;
    }

    public void SaveScreenshot(Texture2D screenShot, string filename)
    {
        byte[] bytes = screenShot.EncodeToPNG();
        string directory = Application.persistentDataPath + "/Screenshots/";
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        File.WriteAllBytes(directory + filename + ".png", bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", directory + filename + ".png"));
    }
}