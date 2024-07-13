using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapBoundsHelper : MonoBehaviour {
    public Camera mapCamera;
    public float groundLevel = 0f;
    public int imageWidth = 2048;
    public int imageHeight = 2048;

    private void OnDrawGizmos() {
        if (mapCamera == null) return;

        float aspect = (float)imageWidth / imageHeight;
        float camHeight = mapCamera.orthographicSize * 2;
        float camWidth = camHeight * aspect;

        Vector3 camPos = mapCamera.transform.position;
        camPos.y = groundLevel;

        Vector3 topLeft = camPos + new Vector3(-camWidth / 2, 0, camHeight / 2);
        Vector3 topRight = camPos + new Vector3(camWidth / 2, 0, camHeight / 2);
        Vector3 bottomLeft = camPos + new Vector3(-camWidth / 2, 0, -camHeight / 2);
        Vector3 bottomRight = camPos + new Vector3(camWidth / 2, 0, -camHeight / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);

#if UNITY_EDITOR
        Handles.Label(topLeft, "Top Left: " + topLeft.ToString("F1"));
        Handles.Label(topRight, "Top Right: " + topRight.ToString("F1"));
        Handles.Label(bottomLeft, "Bottom Left: " + bottomLeft.ToString("F1"));
        Handles.Label(bottomRight, "Bottom Right: " + bottomRight.ToString("F1"));
#endif
    }

    [ContextMenu("Log Corner Coordinates")]
    public void LogCornerCoordinates() {
        if (mapCamera == null) {
            Debug.LogError("Map Camera is not assigned!");
            return;
        }

        float aspect = (float)imageWidth / imageHeight;
        float camHeight = mapCamera.orthographicSize * 2;
        float camWidth = camHeight * aspect;

        Vector3 camPos = mapCamera.transform.position;
        camPos.y = groundLevel;

        Vector3 topLeft = camPos + new Vector3(-camWidth / 2, 0, camHeight / 2);
        Vector3 bottomRight = camPos + new Vector3(camWidth / 2, 0, -camHeight / 2);

        Debug.Log($"Top Left: {topLeft.ToString("F2")}");
        Debug.Log($"Bottom Right: {bottomRight.ToString("F2")}");
    }
}