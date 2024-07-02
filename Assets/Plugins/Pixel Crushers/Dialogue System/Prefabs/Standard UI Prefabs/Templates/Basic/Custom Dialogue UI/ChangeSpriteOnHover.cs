using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSpriteOnHover : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Sprite optionImageDuringHover;
    [SerializeField] Sprite optionImageNormal;
    [SerializeField] Image currentImage;

    public void OnHover()
    {
        currentImage.sprite = optionImageDuringHover;
    }

    public void OnExitHover()
    {
        currentImage.sprite = optionImageNormal;
    }
}
