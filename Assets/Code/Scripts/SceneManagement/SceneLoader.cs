using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // For UI elements (loading screen)

/// <summary>
/// This script is used to load multiple scenes additively one by one.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public List<string> sceneNames = new List<string>(); // List of scenes to load
    public GameObject loadingScreen; // Reference to the loading screen GameObject
    public Slider progressBar; // Reference to the loading progress bar (optional)
    public GameObject player; 
    public GameObject loaderCamera;
    public string mainSceneName = "TerrainCreation";
    public string sceneLoaderSceneName = "SceneLoader";

    /// <summary>
    /// Start loading the scenes additively one by one.
    /// </summary>
    void Start()
    {
        StartCoroutine(LoadScenesOneByOne());
    }
    /// <summary>
    /// Coroutine to load the scenes additively one by one.
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadScenesOneByOne()
    {
        //// Show loading screen
        //if (loadingScreen != null)
        //{
        //    loadingScreen.SetActive(true);
        //}

        // Load the scene loader scene additively
        foreach (string sceneName in sceneNames)
        {
            yield return StartCoroutine(LoadAdditiveScene(sceneName));
        }

        // Hide loading screen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
            loaderCamera.SetActive(false);
            // player.SetActive(true);

            // Unload the scene loader scene
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneLoaderSceneName));
            // Set the main scene as the active scene (this scene will control the lighting)
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainSceneName));
        }
    }

    /// <summary>
    /// Loads a single scene additively.
    /// </summary>
    /// <param name="sceneName">the scene name to load</param>
    /// <returns></returns>
    IEnumerator LoadAdditiveScene(string sceneName)
    {
        // Check if the scene is already loaded
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            // Start loading the scene asynchronously
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // While the scene is loading, update the progress bar (if available)
            while (!asyncLoad.isDone)
            {
                if (progressBar != null)
                {
                    progressBar.value = asyncLoad.progress;
                }
                yield return null;
            }

            Debug.Log($"Scene {sceneName} loaded additively.");
        }
        else
        {
            Debug.Log($"Scene {sceneName} is already loaded.");
        }
        
    }
}
