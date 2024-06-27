using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // For UI elements (loading screen)

public class SceneLoader : MonoBehaviour
{
    public List<string> sceneNames = new List<string>(); // List of scenes to load
    public GameObject loadingScreen; // Reference to the loading screen GameObject
    public Slider progressBar; // Reference to the loading progress bar (optional)
    public GameObject player;
    public GameObject loaderCamera;
    void Start()
    {
        StartCoroutine(LoadScenesOneByOne());
    }

    IEnumerator LoadScenesOneByOne()
    {
        // Show loading screen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        foreach (string sceneName in sceneNames)
        {
            yield return StartCoroutine(LoadAdditiveScene(sceneName));
        }

        // Hide loading screen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
            loaderCamera.SetActive(false);
            player.SetActive(true);

            Destroy(gameObject);
        }
    }

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
