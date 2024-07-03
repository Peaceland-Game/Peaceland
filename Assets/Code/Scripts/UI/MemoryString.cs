using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MemoryString : MonoBehaviour
{
    public List<MemoryTreePhoto> photos = new();
   // [HideInInspector] public List<Transform> cameraAnchors = new();
   // private List<Canvas> photoDescriptions = new();

  //  public List<bool> memoriesCompleted = new();

   
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(photos.Count);
        //photos.ForEach(photo =>
        //{
        //    cameraAnchors.Add(photo.GetComponentInChildren<CameraAnchor>().transform);
        //    photoDescriptions.Add(photo.GetComponentInChildren<Canvas>());
        //    memoriesCompleted.Add(false);
        //});


    }
    public void SelectPhoto(MemoryTreePhoto activePhoto)
    {
        activePhoto.ToggleDescription(true);
    }
    public void DeselectPhoto(MemoryTreePhoto activePhoto)
    {
        activePhoto.ToggleDescription(false);
    }
    
    public MemoryTreePhoto GetActivePhoto()
    {

        for (int x = 0; x < photos.Count; x++)
        {
            if (!photos[x].IsComplete)
                return photos[x];
        }

        return null;
    }

    //public bool StringCompleted()
    //{
    //    for (int i = 0; i < photos.Count; i++)
    //    {
    //        if (!memoriesCompleted[i]) return false;
    //    }
    //    return true;

    //}
    


}
