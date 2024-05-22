using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBehaviour : MonoBehaviour
{
    [SerializeField]
    private
    GameObject outerRing, outerRingActive, middleRing, middleRingActive, centerRing, centerRingActive, slider;
    [SerializeField] private Transform parent;

    GameObject activeRing, hiddenActiveRing;
    [SerializeField] private bool isActiveLock = true;
    [SerializeField] private float roationSpeed = 1000;
    
    
    // Start is called before the first frame update
    void Start()
    {
        SetActiveRing(outerRing);
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
        //Debug.Log($"{name} is active lock");
        
    }

    public void HandlePlayerInteraction()
    {
        // Rotate the active ring
        if (Input.GetKey(KeyCode.A))
        {
            //Debug.Log("rotation left");
            activeRing.transform.Rotate(parent.forward * Time.deltaTime * roationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
           // Debug.Log("rotation right");
            activeRing.transform.Rotate(parent.forward * Time.deltaTime * -roationSpeed);
        }
        //Debug.Log(parent.forward);

        // Change the active ring
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (activeRing == outerRing)
            {
                SetActiveRing(centerRing);
            }
            else if (activeRing == centerRing)
            {
                SetActiveRing(middleRing);
            }
            else if (activeRing == middleRing)
            {
                SetActiveRing(outerRing);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (activeRing == outerRing)
            {
                SetActiveRing(middleRing);
            }
            else if (activeRing == middleRing)
            {
                SetActiveRing(centerRing);
            }
            else if (activeRing == centerRing)
            {
                SetActiveRing(outerRing);
            }
        }
    }

    private void SetActiveRing(GameObject newActiveRing)
    {
        // Deactivate the current active ring and match the rotations
        if (activeRing == outerRing)
        {
            outerRingActive.transform.rotation = outerRing.transform.rotation;
            outerRing.SetActive(true);
            outerRingActive.SetActive(false);
        }
        else if (activeRing == middleRing)
        {
            middleRingActive.transform.rotation = middleRing.transform.rotation;
            middleRing.SetActive(true);
            middleRingActive.SetActive(false);
        }
        else if (activeRing == centerRing)
        {
            centerRingActive.transform.rotation = centerRing.transform.rotation;
            centerRing.SetActive(true);
            centerRingActive.SetActive(false);
        }

        // Activate the new active ring
        if (newActiveRing == outerRing)
        {
            outerRingActive.transform.rotation = outerRing.transform.rotation;
            outerRing.SetActive(false);
            outerRingActive.SetActive(true);
        }
        else if (newActiveRing == middleRing)
        {
            middleRingActive.transform.rotation = middleRing.transform.rotation;
            middleRing.SetActive(false);
            middleRingActive.SetActive(true);
        }
        else if (newActiveRing == centerRing)
        {
            centerRingActive.transform.rotation = centerRing.transform.rotation;
            centerRing.SetActive(false);
            centerRingActive.SetActive(true);
        }

        activeRing = newActiveRing;
    }

    public void SetLockAsActive(bool active)
    {
        isActiveLock = active;
    }
}
