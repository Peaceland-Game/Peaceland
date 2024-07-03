using System.Collections;
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
    public float delayBetweenMoves = 1f;

    private int moveCounter = 0;
    private bool isMoving = false;

    void Start()
    {
        StartCoroutine(MoveNewspapersSequentially());
    }

    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            authorizationForm.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator MoveNewspapersSequentially()
    {
        yield return StartCoroutine(MoveObject(newspaper1, targetTransform1));
        yield return new WaitForSeconds(delayBetweenMoves);

        yield return StartCoroutine(MoveObject(newspaper2, targetTransform2));
        yield return new WaitForSeconds(delayBetweenMoves);

        yield return StartCoroutine(MoveObject(newspaper3, targetTransform3));
    }

    private IEnumerator MoveObject(GameObject newspaper, Transform targetTransform)
    {
        Vector3 startPosition = newspaper.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            newspaper.transform.position = Vector3.Lerp(startPosition, targetTransform.position, t);
            yield return null;
        }

        newspaper.transform.position = targetTransform.position;
    }
}