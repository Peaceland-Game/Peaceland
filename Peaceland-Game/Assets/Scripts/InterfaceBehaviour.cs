using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject keySprite;
    [SerializeField] private GameObject lockPickRules;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PositionKeyIcon(Vector3 itemWorldPos)
    {
        //keySprite.transform.localPosition = Camera.main.WorldToScreenPoint(itemWorldPos);
    }
    public void HideKeyIcon()
    {
        keySprite.SetActive(false);
    }
    public void ShowKeyIcon()
    {
        keySprite.SetActive(true);
    }
    public void HideLockPickRules()
    {
        lockPickRules.SetActive(false);
    }
    public void ShowLockPickRules()
    {
        lockPickRules.SetActive(true);
    }
}
