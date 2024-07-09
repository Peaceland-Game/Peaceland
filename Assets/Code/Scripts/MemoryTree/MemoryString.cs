using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controls a single memory string of memory photos
/// </summary>
public class MemoryString : MonoBehaviour
{
    public List<MemoryTreePhoto> photos = new();    //list of all of the memory photos
    
    /// <summary>
    /// Set the photo as active by showing the description
    /// </summary>
    /// <param name="activePhoto">The photo to activate</param>
    public void SelectPhoto(MemoryTreePhoto activePhoto)
    {
        activePhoto.ToggleDescription(true);
    }
    /// <summary>
    /// Deselect photo by hiding description 
    /// </summary>
    /// <param name="activePhoto">photo description to hide</param>
    public void DeselectPhoto(MemoryTreePhoto activePhoto)
    {
        activePhoto.ToggleDescription(false);
    }
    
    /// <summary>
    /// Gets the active photo by finding the first uncompleted memory
    /// </summary>
    /// <returns>A reference to the first photo that is not marked as completed</returns>
    public MemoryTreePhoto GetActivePhoto()
    {

        for (int x = 0; x < photos.Count; x++)
        {
            if (!photos[x].IsComplete)
                return photos[x];
        }

        return null;
    }


}
