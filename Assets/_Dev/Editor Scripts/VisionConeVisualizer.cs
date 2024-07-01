using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionConeVisualizer : MonoBehaviour
{
    public Stealth stealthScript;
    public int resolution = 20;
    public float alpha = 0.3f;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;

    void Start()
    {
        var scale = stealthScript.gameObject.transform.localScale;
        transform.localScale = new Vector3(1 / scale.x, 1 / scale.y, 1 / scale.z);
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[resolution + 2];
        triangles = new int[resolution * 3];
        colors = new Color[resolution + 2];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = new Color(1, 1, 0, alpha); // Yellow with transparency
        }
    }

    void Update()
    {
        float currentDetectionDistance = stealthScript.heardPlayer ? stealthScript.largerDetectionDistance : stealthScript.detectionDistance;
        Debug.Log(currentDetectionDistance);
        float angle = stealthScript.fieldOfViewInDegrees * Mathf.Deg2Rad;

        vertices[0] = Vector3.zero;
        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            float currentAngle = Mathf.Lerp(-angle / 2, angle / 2, t);
            Vector3 direction = Quaternion.Euler(0, currentAngle * Mathf.Rad2Deg, 0) * Vector3.forward;
            vertices[i + 1] = direction * currentDetectionDistance;
        }

        for (int i = 0; i < resolution; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }
}