using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ImagePopup : MonoBehaviour
{
    [SerializeField] float minimumDuration = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MinimumDuration());
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.eKey.wasPressedThisFrame)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator MinimumDuration()
    {
        yield return new WaitForSeconds(minimumDuration);
    }
}
