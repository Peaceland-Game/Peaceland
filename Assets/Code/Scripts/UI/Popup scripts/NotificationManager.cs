using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    private Queue<(GameObject prefab, string message)> notificationQueue = new Queue<(GameObject, string)>();
    private bool isDisplayingNotification = false;
    private PopupNotification currentNotification;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
                        Debug.Log($"showing notifcation at {currentNotification.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition} ");
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

}