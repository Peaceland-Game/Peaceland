using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoorScript;

public class LockBehaviour : MonoBehaviour
{
    [SerializeField]
    private
    GameObject[] rings_inactive = new GameObject[3];
    [SerializeField]
    private
    GameObject[] rings_active = new GameObject[3];
    [SerializeField]
    private GameObject slider;
    [SerializeField] private Transform parent;

    int activeIndex = 0;
    [SerializeField] private bool isActiveLock = false;
    [SerializeField] private float roationSpeed = 1000;

    private List<float> angles = new(){ 0, 22.5f, 45, 67.5f, 90, 112.5f, 135, 157.5f, 180, 202.5f, 225, 247.5f, 270, 292.5f, 315, 337.5f };
    private int angleIndex = 0;

    [SerializeField]
    private int[] ringRotationIndicies = new int[3];

    [SerializeField]
    private float sliderMoveDuration = 1;

    private Vector3 sliderEndPos = new(0, -0.001296194f, 0);

    [SerializeField] private Door door;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveLock)
        {
            HandlePlayerInteraction();
        }
    }
    public void StartLockPicking()
    {
        isActiveLock = true;
        Scramble();
        Debug.Log($"{name} is active lock");
        
    }
    public void Scramble()
    {
        var i = new int[3];
        for (int j = 0; j < 3; j++)
        {
            i[j] = Random.Range(0, angles.Count);
        }
        for (var j = 0; j < 3; j++)
        {
            rings_active[j].transform.localRotation = Quaternion.Euler(0, angles[i[j]], 0);
            rings_inactive[j].transform.localRotation = Quaternion.Euler(0, angles[i[j]], 0);
            ringRotationIndicies[j] = i[j];
        }
    }
    public void TryMoveSlider()
    {
        var b = true;
        for (var i = 0; i < 3; i++)
        {
            if (ringRotationIndicies[i] != 0) 
                b = false;
        }
        if (b == false)
        {
            Debug.Log("locks not aligned");

        }
        else
        {
            StartCoroutine(MoveSlider());
            Debug.Log("opening");
        }

    }
    private IEnumerator MoveSlider()
    {
        Vector3 startPos = slider.transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < sliderMoveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / sliderMoveDuration;
            slider.transform.localPosition = Vector3.Lerp(startPos, sliderEndPos, t);
            yield return null;
        }

        slider.transform.position = sliderEndPos;
        PlayerSingleton.Instance.EndLockPiking();
        door.OpenDoor();
        gameObject.SetActive(false);

    }

    public void HandlePlayerInteraction()
    {
        // Rotate the active ring
        if (Input.GetKeyDown(KeyCode.A))
        {
            angleIndex = ringRotationIndicies[activeIndex];
            //Debug.Log("rotation left");
            ++angleIndex;

            if (angleIndex >= angles.Count) angleIndex = 0;
            ringRotationIndicies[activeIndex] = angleIndex;

            rings_active[activeIndex].transform.localRotation = Quaternion.Euler(0, angles[angleIndex], 0);
            rings_inactive[activeIndex].transform.localRotation = Quaternion.Euler(0, angles[angleIndex], 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // Debug.Log("rotation right");
            angleIndex = ringRotationIndicies[activeIndex];
            --angleIndex;
            ringRotationIndicies[activeIndex] = angleIndex;
            if (angleIndex < 0) angleIndex = angles.Count - 1;
            rings_active[activeIndex].transform.localRotation = Quaternion.Euler(0, angles[angleIndex], 0);
            rings_inactive[activeIndex].transform.localRotation = Quaternion.Euler(0, angles[angleIndex], 0);
        }

        // Change the active ring
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            activeIndex--;
            if (activeIndex < 0) activeIndex = rings_active.Length - 1;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            activeIndex++;
            if (activeIndex >= rings_active.Length) activeIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            TryMoveSlider();
        }
        
        UpdateRingDisplay();
    }
    private void UpdateRingDisplay()
    {
        for (var i = 0; i < rings_active.Length; i++)
        {
            var b = i == activeIndex;

            rings_inactive[i].SetActive(!b);
            rings_active[i].SetActive(b);
        }
    }
    public void SetLockAsActive(bool active)
    {
        isActiveLock = active;
    }
}
