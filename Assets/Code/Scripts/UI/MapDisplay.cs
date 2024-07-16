using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using PixelCrushers.DialogueSystem;

public class MapDisplay : MonoBehaviour, IScrollHandler {


    public Image mapImage;
    public RectTransform mapRect;
    public RectTransform playerMarker;
    public Transform player;
    public Vector2 worldTopLeft;
    public Vector2 worldBottomRight;
    public float minZoom = 1f;
    public float maxZoom = 2f;
    public float zoomSpeed = 0.01f;

    private ScrollRect scrollRect;
    private Vector2 worldSize;
    public float currentZoom = 1.5f;
    public GameObject objectiveMarkerPrefab;

    private Dictionary<string, RectTransform> mapMarkers = new();

    Vector2 playerPosNormalized;

    public bool showDebugVisuals = false;

    private void Start() {
        scrollRect = GetComponent<ScrollRect>();
        if (mapRect == null)
            mapRect = mapImage.rectTransform;

        worldSize = worldBottomRight - worldTopLeft;

         SetContentSize();
        //Debug.Log($"Initial content size: {mapRect.sizeDelta}");
        //Debug.Log($"Viewport size: {scrollRect.viewport.rect.size}");
        scrollRect.normalizedPosition = new Vector2(0.5f, 0.5f);

        //AddMarkerByName("cylo", "Cylo", "ObjectiveMarker.png");
    }


    //private void OnDrawGizmos() {
    //    if (!showDebugVisuals) return;

    //    // Draw world boundaries
    //    Gizmos.color = Color.red;
    //    Vector3 topLeft = new Vector3(worldTopLeft.x, 0, worldTopLeft.y);
    //    Vector3 topRight = new Vector3(worldBottomRight.x, 0, worldTopLeft.y);
    //    Vector3 bottomLeft = new Vector3(worldTopLeft.x, 0, worldBottomRight.y);
    //    Vector3 bottomRight = new Vector3(worldBottomRight.x, 0, worldBottomRight.y);

    //    Gizmos.DrawLine(topLeft, topRight);
    //    Gizmos.DrawLine(topRight, bottomRight);
    //    Gizmos.DrawLine(bottomRight, bottomLeft);
    //    Gizmos.DrawLine(bottomLeft, topLeft);

    //    // Draw player position
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawSphere(player.position, 1f);
    //}

    private void Update() {
        UpdatePlayerPosition();
    }


    public void OnScroll(PointerEventData eventData) {
        // Convert screen position to local position within the scroll rect
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            scrollRect.viewport, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        OnZoom(eventData.scrollDelta.y, localPoint);

    }
    private void UpdatePlayerPosition() {
        if (!player) return;
        playerMarker.anchoredPosition = WorldToMapLoc(player.position);
        float angle = Mathf.Atan2(player.forward.x, player.forward.z) * Mathf.Rad2Deg;
        playerMarker.rotation = Quaternion.Euler(0, 0, -angle); ;
    }
    public GameObject AddMapMarker(string name, Vector3 worldPosition, string markerType)
    {
        // Instantiate the marker prefab
        GameObject markerObject = Instantiate(objectiveMarkerPrefab, mapImage.transform);
        RectTransform markerRect = markerObject.GetComponent<RectTransform>();

        // Set the marker's position on the map
        markerRect.anchoredPosition = WorldToMapLoc(worldPosition);


        // Load and set the marker's image based on the markerType
        Image markerImage = markerObject.GetComponent<Image>();
        Sprite markerSprite = Resources.Load<Sprite>("MapMarkers/" + markerType);
        if (markerSprite != null)
        {
            markerImage.sprite = markerSprite;
            mapMarkers.Add(name, markerRect);
        }
        else
        {
            Debug.LogWarning($"Marker sprite '{markerType}' not found in Resources/MapMarkers/");
        }

        return markerObject;
    }
    public void AddMarkerByName(string markerName, string gameObjectName, string markerType)
    {
        var go = GameObject.Find(gameObjectName);
        if (!go) throw new Exception($"Cannot find game object {gameObjectName}");

        AddMapMarker(markerName, go.transform.position, markerType);
    }

    public void RemoveMapMarker(string name)
    {
        mapMarkers.Remove(name);
    }
    
    public Vector2 WorldToMapLoc(Vector3 pos)
    {
        var posNormal = new Vector2(Mathf.InverseLerp(worldTopLeft.x, worldBottomRight.x, pos.x),
            Mathf.InverseLerp(worldBottomRight.y, worldTopLeft.y, pos.z));

        return new Vector2(
            mapRect.rect.width * posNormal.x,
            mapRect.rect.height * posNormal.y
        );

    }
    public void OnZoom(float zoomDelta, Vector2 mousePosition) {
        float newZoom = currentZoom * (1f + zoomDelta * zoomSpeed);
        SetZoom(newZoom, mousePosition);
    }

    private void SetZoom(float zoom, Vector2 mousePosition) {
        // Store old position and size
        Vector2 oldSize = mapRect.sizeDelta;
        Vector2 oldPosition = scrollRect.content.anchoredPosition;

        // Calculate zoom
        currentZoom = Mathf.Clamp(zoom, minZoom, maxZoom);

        // Set new size
        Vector2 viewportSize = scrollRect.viewport.rect.size;
        Vector2 newSize = viewportSize * currentZoom;
        mapRect.sizeDelta = newSize;

        // Calculate the position delta
        Vector2 mousePositionOnContent = mousePosition + oldPosition;
        Vector2 newMousePositionOnContent = mousePositionOnContent * (newSize.x / oldSize.x);
        Vector2 positionDelta = newMousePositionOnContent - mousePosition;

        // Set new position
        scrollRect.content.anchoredPosition = positionDelta;

        // Ensure player marker stays in the correct position
        UpdatePlayerPosition();
    }

    private void SetContentSize() {
        Vector2 viewportSize = scrollRect.viewport.rect.size;
        Vector2 oldSize = mapRect.sizeDelta;
        Vector2 newSize = viewportSize * Mathf.Max(currentZoom, 1f);
        mapRect.sizeDelta = newSize;

        // Adjust content position to zoom towards the center
        Vector2 sizeDelta = newSize - oldSize;
        Vector2 adjustedPosition = scrollRect.content.anchoredPosition - (sizeDelta * 0.5f);
        scrollRect.content.anchoredPosition = adjustedPosition;

        // Ensure player marker stays in the correct position
        UpdatePlayerPosition();
    }

    private void OnEnable()
    {
        //Lua.RegisterFunction(nameof(AddMapMarker), this, SymbolExtensions.GetMethodInfo(() => AddMapMarker("", Vector3.zero, "ObjectiveMarker")));
        Lua.RegisterFunction(nameof(RemoveMapMarker), this, SymbolExtensions.GetMethodInfo(() => RemoveMapMarker("")));
        Lua.RegisterFunction(nameof(AddMarkerByName), this, SymbolExtensions.GetMethodInfo(() => AddMarkerByName("", "", "")));

    }

}



