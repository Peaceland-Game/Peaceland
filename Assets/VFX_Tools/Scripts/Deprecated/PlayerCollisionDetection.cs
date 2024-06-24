using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionDetection : MonoBehaviour
{
    //the number for the table layer
    private int tableLayer = 1 << 3;
    //the number for the key layer
    private int keyLayer = 1 << 6;


    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            StartCoroutine(PauseToReset());
            Debug.Log("Collision with table");
        }
        if (collision.gameObject.layer == 6)
        {
            Debug.Log("Collision with key");
        }
    }

    private IEnumerator PauseToReset()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
