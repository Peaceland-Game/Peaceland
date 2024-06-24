using System.Drawing;
using System.Reflection;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GizmosPlus : MonoBehaviour
{
    public static void DrawWirePlane(Vector3 pos, Vector3 n, Vector2 size)
    {
        Vector3[] points = GetPlanePoints(n, size);

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 point = pos + points[i];
            Vector3 next = pos + points[(i + 1) < points.Length ? i + 1 : 0];

            Gizmos.DrawLine(point, next);
            Gizmos.DrawLine(point, pos);
        }
    }

    public static void DrawWirePlaneNoX(Vector3 pos, Vector3 n, Vector2 size)
    {
        Vector3[] points = GetPlanePoints(n, size);

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 point = pos + points[i];
            Vector3 next = pos + points[(i + 1) < points.Length ? i + 1 : 0];

            Gizmos.DrawLine(point, next);
        }

    }

    public static void DrawWireArrow(Vector3 pos, Vector3 n, float stemScale, float headScale)
    {
        Vector3 norm = n.normalized;
        Vector3 endOfStem = pos + norm * stemScale;

        Gizmos.DrawLine(pos, endOfStem);
        DrawArrowHead(endOfStem, norm, headScale);
    }

    private static void DrawArrowHead(Vector3 pos, Vector3 n, float headScale)
    {
        Vector3[] points = GetPlanePoints(n, Vector2.one * headScale * headScale);

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 point = pos + points[i];

            Gizmos.DrawLine(point, pos);
            Gizmos.DrawLine(point, pos + n * headScale);
        }
    }

    private static Vector3[] GetPlanePoints(Vector3 n, Vector3 size)
    {
        Vector2 halfSize = size / 2.0f;

        Vector3 right = Vector3.right;
        Vector3 left = -right;
        Vector3 forward = Vector3.forward;

        // Make sure not the same by some threshold 
        if ((Vector3.up - n).sqrMagnitude <= 0.0001f)
        {

        }
        else
        {
            left = Vector3.Cross(n, Vector3.up).normalized;
            right = Vector3.Cross(Vector3.up, n).normalized;
            forward = Vector3.Cross(right, n).normalized;
        }

        Vector3[] points =
        {
            right * halfSize.x + forward * halfSize.y,
            right * halfSize.x - forward * halfSize.y,
            left * halfSize.x - forward * halfSize.y,
            left * halfSize.x + forward * halfSize.y,
        };

        return points;
    }


    private void OnDrawGizmos()
    {
        DrawWirePlane(this.transform.position, this.transform.up, Vector2.one);
        DrawWireArrow(this.transform.position, this.transform.up, 0.3f, 0.3f);
    }
}
