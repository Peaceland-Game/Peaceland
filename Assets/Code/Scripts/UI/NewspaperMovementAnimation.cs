using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewspaperMovement : MonoBehaviour
{
    public Transform targetTransform1;
    public Transform targetTransform2;
    public Transform targetTransform3;
    [SerializeField] private GameObject newspaper1;
    [SerializeField] private GameObject newspaper2;
    [SerializeField] private GameObject newspaper3;
    [SerializeField] private GameObject authorizationForm;
    public float duration = 1f;

    private int moveCounter;

    private Vector3 startPosition;

    private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCounter < 3)
        {
            switch (moveCounter)
            {
                case 0:
                    MoveObject(newspaper1, targetTransform1);
                    break;

                case 1:
                    MoveObject(newspaper2, targetTransform2);
                    break;

                case 2:
                    MoveObject(newspaper3 , targetTransform3);
                    break;
            }
        }



        if (Keyboard.current.enterKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            authorizationForm.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    public void MoveObject(GameObject newspaper, Transform transform)
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Smoothly interpolate position
            newspaper.transform.position = Vector3.Lerp(startPosition, transform.position, t);
        }

        // Ensure final position and scale are exact
        newspaper.transform.position = transform.position;

        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        moveCounter++;
    }
}
