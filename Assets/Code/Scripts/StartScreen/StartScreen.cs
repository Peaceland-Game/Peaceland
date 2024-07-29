using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    [Tooltip("This is a subsitute before we implement a save system")]
    [SerializeField] string sceneName;
    [SerializeField] string continueSceneName;
    [SerializeField] GameObject settings;

    /// <summary>
    /// Continue to the most recent save file 
    /// </summary>
    public void Continue()
    {
        // Load using FileIO system 

        // Load correct scene 
        SceneManager.LoadScene(continueSceneName);
    }

    /// <summary>
    /// Reload the current save file to default values 
    /// </summary>
    public void NewGame()
    {
        // Override current file 
        SceneManager.LoadScene(sceneName);
        PixelCrushers.DialogueSystem.DialogueManager.ResetDatabase();

        string basePath = Directory.GetCurrentDirectory() + "//";
        string finalPath = basePath + "Mem 1" + ".json"; // Hardcoded, should be automated to a folder or pattern 
        if (File.Exists(finalPath))
        {
            File.Delete(finalPath);
        }
    }

    /// <summary>
    /// Access the settings to the game 
    /// </summary>
    public void Settings()
    {
        //Turn on the settings screen
        settings.SetActive(true);
    }

    /// <summary>
    /// Exits the settings back to the title screen
    /// </summary>
    public void ExitSettings()
    {
        //Turn off the settings screen
        settings.SetActive(false);
    }

    /// <summary>
    /// Closes the game 
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
