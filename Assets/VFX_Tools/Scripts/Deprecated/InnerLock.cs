using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerLock : MonoBehaviour
{
    float rotationSpeed = 0.4f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);

        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Get the current rotation angle around the Y axis
            float currentRotationY = transform.eulerAngles.y;

            // Check if the rotation is within the desired range
            if (currentRotationY >= 320 && currentRotationY <= 360)
            {
                // Stop rotation by setting rotation speed to 0
                rotationSpeed = 0f;
            }
        }

    }
}
