using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryString : MonoBehaviour
{
    public List<GameObject> photos;
    [HideInInspector] public List<Transform> cameraAnchors;
    // Start is called before the first frame update
    void Start()
    {

        photos.ForEach(photo => cameraAnchors.Add(photo.transform.GetChild(0).transform));

    }

    
}
