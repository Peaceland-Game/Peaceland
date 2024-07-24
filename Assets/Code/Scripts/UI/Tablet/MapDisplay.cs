using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Script that handles the display of the map on the tablet
/// </summary>
public class MapDisplay : MonoBehaviour, IScrollHandler {


    public Image mapImage;              // Reference to the map image inside the viewport
    public RectTransform mapRect;       // Reference to the map rect transform inside the viewport
    public RectTransform playerMarker;  // Reference to the player marker rect transform, child of the map image
    public Transform player;            // Reference to the player transform
    public Vector2 worldTopLeft;        // World position of the top left corner of the map
    public Vector2 worldBottomRight;    // World position of the bottom right corner of the map
    public float minZoom = 1f;          
    public float maxZoom = 2f;
    public float zoomSpeed = 0.01f;

    private ScrollRect scrollRect;      // Reference to the scroll rect component
    private Vector2 worldSize;          // Size of the world in world units
    public float currentZoom = 1.5f;    
    public GameObject objectiveMarkerPrefab;    // Prefab for the objective markers

    private Dictionary<string, RectTransform> mapMarkers = new();   // Dictionary of map markers by name

    Vector2 playerPosNormalized;

    public bool showDebugVisuals = false;   // Show debug visuals in the editor

    /// <summary>
    /// Initializes the map display, setting up the scroll rect and initial zoom.
    /// </summary>
    private void Start() {
        scrollRect = GetComponent<ScrollRect>();
        if (mapRect == null)
            mapRect = mapImage.rectTransform;

        worldSize = worldBottomRight - worldTopLeft;

        SetContentSize();
        scrollRect.normalizedPosition = new Vector2(0.5f, 0.5f);
    }

    /// <summary>
    /// Updates the player's position on the map.
    /// </summary>
    private void Update() {
        UpdatePlayerPosition();
    }

    /// <summary>
    /// Handles scroll events for zooming the map.
    /// </summary>
    /// <param name="eventData">Pointer event data containing scroll information.</param>
    public void OnScroll(PointerEventData eventData) {
        // Convert screen position to local position within the scroll rect
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            scrollRect.viewport, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        OnZoom(eventData.scrollDelta.y, localPoint);

    }
    /// <summary>
    /// Updates the player marker's position and rotation on the map.
    /// </summary>
    private void UpdatePlayerPosition() {
        if (!player) return;
        playerMarker.anchoredPosition = WorldToMapLoc(player.position);
        float angle = Mathf.Atan2(player.forward.x, player.forward.z) * Mathf.Rad2Deg;
        playerMarker.rotation = Quaternion.Euler(0, 0, -angle); ;
    }
    /// <summary>
    /// Adds a marker to the map at the specified world position.
    /// </summary>
    /// <param name="name">The name of the marker.</param>
    /// <param name="worldPosition">The world position of the marker.</param>
    /// <param name="markerType">The type of marker to add.</param>
    /// <returns>The instantiated marker GameObject.</returns>
    public GameObject AddMapMarker(string name, Vector3 worldPosition, string markerType) {
        // Instantiate the marker prefab
        GameObject markerObject = Instantiate(objectiveMarkerPrefab, mapImage.transform);
        RectTransform markerRect = markerObject.GetComponent<RectTransform>();

        // Set the marker's position on the map
        markerRect.anchoredPosition = WorldToMapLoc(worldPosition);


        // Load and set the marker's image based on the markerType
        Image markerImage = markerObject.GetComponent<Image>();
        Sprite markerSprite = Resources.Load<Sprite>("MapMarkers/" + markerType);
        if (markerSprite != null) {
            markerImage.sprite = markerSprite;
            mapMarkers.Add(name, markerRect);
        }
        else {
            Debug.LogWarning($"Marker sprite '{markerType}' not found in Resources/MapMarkers/");
        }

        return markerObject;
    }
    /// <summary>
    /// Adds a marker to the map based on a GameObject's name.
    /// </summary>
    /// <param name="markerName">The name of the marker.</param>
    /// <param name="gameObjectName">The name of the GameObject to place the marker on.</param>
    /// <param name="markerType">The type of marker to add.</param>
    public void AddMarkerByName(string markerName, string gameObjectName, string markerType) {
        var go = GameObject.Find(gameObjectName);
        if (!go) throw new Exception($"Cannot find game object {gameObjectName}");

        AddMapMarker(markerName, go.transform.position, markerType);
    }
    /// <summary>
    /// Removes a marker from the map.
    /// </summary>
    /// <param name="name">The name of the marker to remove.</param>
    public void RemoveMapMarker(string name) {
        mapMarkers.Remove(name);
    }
    /// <summary>
    /// Converts a world position to a map position.
    /// </summary>
    /// <param name="pos">The world position to convert.</param>
    /// <returns>The corresponding position on the map.</returns>
    public Vector2 WorldToMapLoc(Vector3 pos) {
        var posNormal = new Vector2(Mathf.InverseLerp(worldTopLeft.x, worldBottomRight.x, pos.x),
            Mathf.InverseLerp(worldBottomRight.y, worldTopLeft.y, pos.z));

        return new Vector2(
            mapRect.rect.width * posNormal.x,
            mapRect.rect.height * posNormal.y
        );

    }
    /// <summary>
    /// Handles zooming of the map.
    /// </summary>
    /// <param name="zoomDelta">The amount of zoom to apply.</param>
    /// <param name="mousePosition">The position of the mouse during zooming.</param>
    public void OnZoom(float zoomDelta, Vector2 mousePosition) {
        float newZoom = currentZoom * (1f + zoomDelta * zoomSpeed);
        SetZoom(newZoom, mousePosition);
    }
    /// <summary>
    /// Sets the zoom level of the map.
    /// </summary>
    /// <param name="zoom">The new zoom level.</param>
    /// <param name="mousePosition">The position of the mouse during zooming.</param>
    private void SetZoom(float zoom, Vector2 mousePosition)
    {
        // Store old size
        Vector2 oldSize = mapRect.sizeDelta;

        // Calculate zoom
        currentZoom = Mathf.Clamp(zoom, minZoom, maxZoom);

        // Set new size
        Vector2 viewportSize = scrollRect.viewport.rect.size;
        Vector2 newSize = viewportSize * currentZoom;
        mapRect.sizeDelta = newSize;

        // Calculate the new position
        Vector2 normalizedMousePos = new Vector2(
            mousePosition.x / scrollRect.viewport.rect.width,
            mousePosition.y / scrollRect.viewport.rect.height
        );
        Vector2 newContentPos = new Vector2(
            (newSize.x - viewportSize.x) * normalizedMousePos.x,
            (newSize.y - viewportSize.y) * normalizedMousePos.y
        );
        scrollRect.content.anchoredPosition = -newContentPos;

        // Ensure player marker stays in the correct position
        UpdatePlayerPosition();
    }
    /// <summary>
    /// Sets the size of the map content based on the current zoom level.
    /// </summary>
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
    /// <summary>
    /// Registers Lua functions when the script is enabled.
    /// </summary>
    private void OnEnable() {
        //Lua.RegisterFunction(nameof(AddMapMarker), this, SymbolExtensions.GetMethodInfo(() => AddMapMarker("", Vector3.zero, "ObjectiveMarker")));
        Lua.RegisterFunction(nameof(RemoveMapMarker), this, SymbolExtensions.GetMethodInfo(() => RemoveMapMarker("")));
        Lua.RegisterFunction(nameof(AddMarkerByName), this, SymbolExtensions.GetMethodInfo(() => AddMarkerByName("", "", "")));

    }

}



