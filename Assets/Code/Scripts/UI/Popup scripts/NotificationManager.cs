using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum NotificationType
{
    ArtifactPopup,
    KarmaPopup
}

/// <summary>
/// Controls the display of notifications in the game.
/// </summary>
public class NotificationManager : MonoBehaviour
{
    
    public static NotificationManager Instance { get; private set; }

    private Queue<(GameObject prefab, string message)> notificationQueue = new Queue<(GameObject, string)>();
    private bool isDisplayingNotification = false;
    private PopupNotification currentNotification;

    [SerializeField]
    private List<string> notificationPrefabNames = new List<string>();

    // Dictionary to map NotificationType to prefab index
    private Dictionary<NotificationType, int> notificationTypeToIndex = new Dictionary<NotificationType, int>();


    
    private List<GameObject> notificationPrefabs = new();

    /// <summary>
    /// Initializes the NotificationManager singleton and loads notification prefabs.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            notificationPrefabs.Clear();
            foreach (string prefabName in notificationPrefabNames) {
                GameObject prefab = Resources.Load(prefabName) as GameObject;
                if (prefab != null) {
                    notificationPrefabs.Add(prefab);
                }
                else {
                    Debug.LogError($"Failed to load notification prefab: {prefabName}");
                }
            }
            InitializeNotificationTypeDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Initializes the dictionary mapping NotificationType to prefab indices.
    /// </summary>
    private void InitializeNotificationTypeDictionary()
    {
        notificationTypeToIndex.Add(NotificationType.ArtifactPopup, 0);
        notificationTypeToIndex.Add(NotificationType.KarmaPopup, 1);
       // Debug.Log("initialized dictionary enum types");
        
    }
    /// <summary>
    /// Manages the display of notifications from the queue.
    /// </summary>
    private void Update()
    {
        if (notificationQueue.Any())
        {
            if (!isDisplayingNotification) {
                var (prefab, message) = notificationQueue.Dequeue();
                if (message != null)
                {
                    if (Instantiate(prefab).TryGetComponent(out PopupNotification currentNotification))
                    {
                       // Debug.Log($"showing notifcation at {currentNotification.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition} ");
                        isDisplayingNotification = true;
                        currentNotification.UpdateArtifactName(message);
                        StartCoroutine(WaitForEndOfNotifcation(currentNotification.GetTotalNotifcationTime()));
                    }
                }
            } 
        }

        
    }
    /// <summary>
    /// Coroutine that waits for the end of a notification's display time.
    /// </summary>
    /// <param name="seconds">The duration to wait in seconds.</param>
    /// <returns>An IEnumerator for the coroutine system.</returns>
    private IEnumerator WaitForEndOfNotifcation(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isDisplayingNotification = false;
    }
    /// <summary>
    /// Adds a notification to the queue using a prefab and message.
    /// </summary>
    /// <param name="notificationPrefab">The GameObject prefab for the notification.</param>
    /// <param name="message">The message to display in the notification.</param>
    public void QueueNotification(GameObject notificationPrefab, string message = "")
    {
        
        notificationQueue.Enqueue((notificationPrefab, message));
    }
    /// <summary>
    /// Adds a notification to the queue using a prefab index and message.
    /// </summary>
    /// <param name="index">The index of the notification prefab in the list.</param>
    /// <param name="message">The message to display in the notification.</param>
    public void QueueNotification(int index, string message = "")
    {
        if (index < notificationPrefabs.Count) {
            QueueNotification(notificationPrefabs[index], message);
        }
    }
    /// <summary>
    /// Adds a notification to the queue using a NotificationType and message.
    /// </summary>
    /// <param name="notificationType">The type of notification to queue.</param>
    /// <param name="message">The message to display in the notification.</param>
    public void QueueNotification(NotificationType notificationType, string message = "")
    {
        if (notificationTypeToIndex.TryGetValue(notificationType, out int index))
        {
            Debug.Log($"Looking for notification at index {index}");
            Debug.Log($"Notification prefabs list: {notificationPrefabs.Count}");
            if (index < notificationPrefabs.Count)
                QueueNotification(notificationPrefabs[index], message);
        }
        else
        {
            Debug.LogWarning($"Notification type {notificationType} not found or index out of range.");
        }
    }

}