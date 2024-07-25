using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script used to return to the title screen.
/// </summary>
public class ReturnToTitle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Method that is called when the game needs to return to title screen.
    /// </summary>
    public void Return() 
    {
        SceneManager.LoadScene("StartScreen");
    }
}
