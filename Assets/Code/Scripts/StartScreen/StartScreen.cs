using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    [Tooltip("This is a subsitute before we implement a save system")]
    [SerializeField] string sceneName; 

    /// <summary>
    /// Continue to the most recent save file 
    /// </summary>
    public void Continue()
    {
        // Load using FileIO system 

        // Load correct scene 
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Reload the current save file to default values 
    /// </summary>
    public void NewGame()
    {
        // Override current file 
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Access the settings to the game 
    /// </summary>
    public void Settings()
    {

    }

    /// <summary>
    /// Closes the game 
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
