using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadScene : MonoBehaviour
{
    public void UnloadSceneByName(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }
    public void UnloadSceneAfterSeconds(string name, float seconds = 1)
    {
        StartCoroutine(WaitThen(seconds, () =>
        {
            UnloadSceneByName(name);
        }));
    }
    public void UnloadSceneAfterOneSecond(string name)
    {
        UnloadSceneAfterSeconds(name, 1);
    }

    IEnumerator WaitThen(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }

}
