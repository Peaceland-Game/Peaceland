using UnityEngine;

public class FakeReflection : MonoBehaviour {
    public Camera mainCamera;
    public Camera reflectionCamera;
    public Transform reflectiveSurface;

    void Update() {
        Vector3 reflectionCameraPosition = mainCamera.transform.position;
        reflectionCameraPosition.y = -reflectionCameraPosition.y + 2 * reflectiveSurface.position.y;

        reflectionCamera.transform.position = reflectionCameraPosition;

        Vector3 reflectionCameraEulerAngles = mainCamera.transform.eulerAngles;
        reflectionCameraEulerAngles.x = -reflectionCameraEulerAngles.x;

        reflectionCamera.transform.eulerAngles = reflectionCameraEulerAngles;
    }
}
