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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            notificationPrefabs.Clear();
            foreach (string prefabName in notificationPrefabNames)
            {
                GameObject prefab = Resources.Load(prefabName) as GameObject;
                if (prefab != null)
                {
                    notificationPrefabs.Add(prefab);
                }
                else
                {
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
    private void InitializeNotificationTypeDictionary()
    {
        notificationTypeToIndex.Add(NotificationType.ArtifactPopup, 0);
        notificationTypeToIndex.Add(NotificationType.KarmaPopup, 1);
       // Debug.Log("initialized dictionary enum types");
        
    }
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
    private IEnumerator WaitForEndOfNotifcation(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isDisplayingNotification = false;
    }

    public void QueueNotification(GameObject notificationPrefab, string message = "")
    {
        
        notificationQueue.Enqueue((notificationPrefab, message));
    }

    public void QueueNotification(int index, string message = "")
    {
        if (index < notificationPrefabs.Count) {
            QueueNotification(notificationPrefabs[index], message);
        }
    }
    public void QueueNotification(NotificationType notificationType, string message = "")
    {
        if (notificationTypeToIndex.TryGetValue(notificationType, out int index))
        {
            //Debug.Log($"Looking for notification at index {index}");
           // Debug.Log($"Notification prefabs list: {notificationPrefabs.Count}");
            if (index < notificationPrefabs.Count)
                QueueNotification(notificationPrefabs[index], message);
        }
        else
        {
            Debug.LogWarning($"Notification type {notificationType} not found or index out of range.");
        }
    }

}