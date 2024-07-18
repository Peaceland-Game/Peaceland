using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class CellController : MonoBehaviour
{
    [System.Serializable]
    public class Cell
    {
        public string id;
        public string parentName;
        public string cellName;
        public BoxCollider boxCollider;
        public CellTrigger trigger;
    }

    public List<Cell> cells = new List<Cell>();
    public LayerMask cellLayer;

    void Start()
    {
        ScanCells();
    }

    void ScanCells()
    {
        cells.Clear();
        ScanChildren(transform, "");
       
    }

    void ScanChildren(Transform parent, string parentName)
    {
        foreach (Transform child in parent)
        {
            BoxCollider boxCollider = child.GetComponent<BoxCollider>();
            if (boxCollider != null && child.gameObject.layer == LayerMask.NameToLayer("Cell"))
            {
                CellTrigger trigger = child.gameObject.AddComponent<CellTrigger>();
                trigger.controller = this;

                Cell cell = new Cell
                {
                    id = cells.Count.ToString(),
                    parentName = parentName,
                    cellName = child.name,
                    boxCollider = boxCollider,
                    trigger = trigger
                };
                cells.Add(cell);
            }
            else
            {
                ScanChildren(child, child.name);
            }
        }

        
    }

    public void OnCellEntered(CellTrigger trigger)
    {
        Cell enteredCell = cells.Find(cell => cell.trigger == trigger);
        if (enteredCell != null)
        {
            DisplayCellMessage(enteredCell);
        }
    }

    void DisplayCellMessage(Cell cell)
    {
        string message = $"{cell.parentName}, {cell.cellName}";
        DialogueManager.ShowAlert(message);

        if (cell.cellName == "Your Apartment") DialogueManager.ShowAlert("Kalin, Age 10");
        Debug.Log(message); // Replace this with your preferred method of displaying messages to the player
    }


    public (bool, Vector3) GetCellCenter(string id)
    {
        Cell cell = cells.Find(c => c.id == id);
        if (cell != null)
        {
            Vector3 center = cell.boxCollider.bounds.center;
            float terrainHeight = GetTerrainHeight(center.x, center.z);
            return (true, new Vector3(center.x, terrainHeight, center.z));
        }
        else
        {
            return (false, Vector3.zero);
        }
    }
    private float GetTerrainHeight(float x, float z)
    {
        Terrain terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            var height = terrain.SampleHeight(new Vector3(x, 0, z)); //returns a position way above terrain for some reason
            return height;
        }
        else
        {
            Debug.LogWarning("No active terrain found. Returning 0 as height.");
            return 0f;
        }
    }
}
public class CellTrigger : MonoBehaviour
{
    public CellController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.OnCellEntered(this);
        }
    }
}