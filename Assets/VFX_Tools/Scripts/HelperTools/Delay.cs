using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delay : MonoBehaviour
{  
    //Declare all of the game objects
    public GameObject outerLock;
    public GameObject middleLock;
    public GameObject innerLock;
    public bool isComplete = false;
   
    public bool canRun = false;
   
    // Start is called before the first frame update
    void Start()
    {
      
        middleLock.SetActive(false);
        innerLock.SetActive(false);
    }
    
    void Update()
    {
        // Don't run untill started by interaction 
        if (!canRun)
            return;

        //Setting the Y rotationangle for each part of the lock
        Transform outerTransform = outerLock.transform;
        Transform midTransform = middleLock.transform;
        Transform innerTransform = innerLock.transform;

        Quaternion oRotation = outerTransform.rotation;
        Quaternion mRotation = midTransform.rotation;
        Quaternion iRotation = innerTransform.rotation;

        float y = oRotation.eulerAngles.y;
        float z = mRotation.eulerAngles.y;
        float x = iRotation.eulerAngles.y;

       

        //if the lock is active, check and see if it is in the range
        if (outerLock.activeSelf)
        {
            //When clicked,check the angles
            if (Input.GetMouseButtonDown(0))
            {
                if (oRotation.eulerAngles.y >= 90 && oRotation.eulerAngles.y <= 120)
                {
                  
                    //if it is show the nect part 
                    middleLock.SetActive(true);

                }
            
               

            }
            
        }

        //Repeat steps above with the middle lock
        if (middleLock.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (mRotation.eulerAngles.y >= 315 && mRotation.eulerAngles.y <= 340)
                {
                    innerLock.SetActive(true);
                   
                }

            }

        }


        //Repeat the same process for the inner lock
        if (innerLock.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (iRotation.eulerAngles.y >= 320 && iRotation.eulerAngles.y <= 360)
                {
                    //However, if it is completed, set isComplete to true to show that it is completed.
                    isComplete = true;
                    //UnityEditor.EditorApplication.isPlaying = false;
                    //Application.Quit();
                }

            }

        }


    }

    
   
  

}
