using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[ExecuteInEditMode]
public class MushroomTool : MonoBehaviour
{
    [SerializeField] GameObject[] mushroomPool;
    [SerializeField] float spawnLength;
    [SerializeField] Vector2 leanDirA;
    [SerializeField] Vector2 leanDirB;
    [SerializeField] bool reverseLeanDir;
    [Range(0.0f, 1.0f)]
    [SerializeField] float tiltStrengthMin;
    [Range(0.0f, 1.0f)]
    [SerializeField] float tiltStrengthMax;

    [SerializeField] int spawnCount;

    [SerializeField] List<Mushroom> mushrooms;

    /// <summary>
    /// Randomizes the positions of the mushrooms
    /// </summary>
    public void RedistributeMushrooms()
    {
        if (mushrooms == null || mushrooms.Count == 0)
            ResetMushrooms();

        foreach (var mushroom in mushrooms)
        {
            mushroom.Pos = Vector3.up * UnityEngine.Random.Range(-spawnLength * 0.5f, spawnLength * 0.5f);
        }

        // TODO: Sort by height 
    }

    /// <summary>
    /// Randomize the tilt of the mushrooms
    /// </summary>
    public void RandomizeAttributes()
    {
        if (mushrooms == null || mushrooms.Count == 0)
            ResetMushrooms();

        foreach (var mushroom in mushrooms)
        {
            // Lean direction 
            float leanLerp = UnityEngine.Random.Range(0.0f, 1.0f);
            Vector2 dir = Vector2.Lerp(leanDirA, leanDirB, leanLerp);
            Vector3 leanDir = (reverseLeanDir ? -1.0f : 1.0f) * new Vector3(dir.x, 0.0f, dir.y);

            // Tilt Strength 
            float tiltLerp = UnityEngine.Random.Range(tiltStrengthMin, tiltStrengthMax);

            Vector3 normal = Vector3.Slerp(Vector3.up, leanDir, tiltLerp);

            mushroom.Up = normal;
        }
    }

    /// <summary>
    /// Randomize the mesh of each mushroom. If no mesh(es) then 
    /// spawns in object(s)
    /// </summary>
    public void SpawnRandomMushroomType()
    {
        if (mushrooms == null || mushrooms.Count == 0)
            ResetMushrooms();

        foreach (var mushroom in mushrooms)
        {
            int index = UnityEngine.Random.Range(0, mushroomPool.Length);

            GameObject hold = mushroom.mushroom;
            mushroom.mushroom = Instantiate(mushroomPool[index], Vector3.zero, mushroomPool[index].transform.rotation, this.transform);
            mushroom.mushroom.transform.localPosition = hold.transform.localPosition;
            mushroom.mushroom.transform.up = mushroom.Up;

            DestroyImmediate(hold);
        }
    }

    /// <summary>
    /// Cleanup the mushrooms connected to this tool 
    /// </summary>
    public void Clear()
    {
        for (int i = 0; i < mushrooms.Count; i++)
        {
            DestroyImmediate(mushrooms[i].mushroom);
        }

        mushrooms.Clear();
    }

    private void ResetMushrooms()
    {
        mushrooms = new List<Mushroom>();

        for (int i = 0; i < spawnCount; i++)
        {
            // Lean direction 
            float leanLerp = UnityEngine.Random.Range(0.0f, 1.0f);
            Vector2 dir = Vector2.Lerp(leanDirA, leanDirB, leanLerp);
            Vector3 leanDir = (reverseLeanDir ? -1.0f : 1.0f) * new Vector3(dir.x, 0.0f, dir.y);

            // Tilt Strength 
            float tiltLerp = UnityEngine.Random.Range(tiltStrengthMin, tiltStrengthMax);

            Vector3 normal = Vector3.Slerp(Vector3.up, leanDir, tiltLerp);
            Vector3 pos = Vector3.up * UnityEngine.Random.Range(-spawnLength * 0.5f, spawnLength * 0.5f);
            int index = UnityEngine.Random.Range(0, mushroomPool.Length);

            mushrooms.Add(
                new Mushroom(
                    pos,
                    normal, 
                    Instantiate(mushroomPool[index], pos, mushroomPool[index].transform.rotation, this.transform))
                );
            mushrooms[i].Up = normal;
        }
    }


    [Serializable]
    private class Mushroom
    {
        [SerializeField] private Vector3 pos;
        [SerializeField] private Vector3 up;
        public GameObject mushroom;

        public Vector3 Pos { get { return pos; }  set { this.pos = value; mushroom.transform.localPosition = value; } }
        public Vector3 Up { get { return up; }  set { this.up = value; mushroom.transform.up = value; } }

        public Mushroom(Vector3 pos, Vector3 up, GameObject mushroom)
        {
            this.pos = pos;
            this.up = up;
            this.mushroom = mushroom;

            this.mushroom.transform.localPosition = pos;
            this.mushroom.transform.up = up;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(this.transform.position - this.transform.up * spawnLength * 0.5f, this.transform.position + this.transform.up * spawnLength * 0.5f);

        if (mushrooms == null || mushrooms.Count == 0)
        {
            DrawGuide(Vector3.zero, Vector3.up);
            return;
        }


        foreach (Mushroom m in mushrooms)
        {
            DrawGuide(m.Pos, m.Up);
        }
    }

    private void DrawGuide(Vector3 localPos, Vector3 localUp)
    {
        float lineLength = 0.3f;

        Vector3 pos = this.transform.TransformPoint(localPos);

        Vector3 dirA = this.transform.TransformDirection(new Vector3(leanDirA.x, 0.0f, leanDirA.y));
        Vector3 dirB = this.transform.TransformDirection(new Vector3(leanDirB.x, 0.0f, leanDirB.y));
        Vector3 up = this.transform.TransformDirection(localUp);

        Gizmos.color = Color.white;
        GizmosPlus.DrawWirePlaneNoX(pos, up, Vector3.one * 0.5f);

        Gizmos.color = Color.red;
        GizmosPlus.DrawWireArrow(pos, dirA, lineLength, 0.05f);
        Gizmos.color = Color.blue;
        GizmosPlus.DrawWireArrow(pos, dirB, lineLength, 0.05f);
        Gizmos.color = Color.yellow;
        GizmosPlus.DrawWireArrow(pos, up, lineLength * 0.5f, 0.05f);

        // Draw lean direction 
        Vector2 mid = leanDirA.normalized + leanDirB.normalized;
        Gizmos.color = Color.green;
        GizmosPlus.DrawWireArrow(pos, this.transform.TransformDirection((reverseLeanDir ? -1.0f : 1.0f) * new Vector3(mid.x, 0.0f, mid.y)), lineLength * 0.5f, 0.05f);
    }
}
