using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental;
using UnityEngine;

/// <summary>
/// Controller of all the themes in the notes page to reveal them when unlocked.
/// This should be on the parent object of each of the themes
/// </summary>
public class ThemeNotesController : MonoBehaviour
{
    public List<Theme> themes = new();
    // Start is called before the first frame update
    void Start()
    {
        themes = GetComponentsInChildren<Theme>().ToList();
    }

    /// <summary>
    /// Attempts to reveal the theme by calling the child's RevealTheme function
    /// </summary>
    /// <param name="name">The name of the theme to reveal. This should match the game object's name in unity</param>
    public void RevealTheme(string name)
    {
        if (themes.Count == 0) themes = GetComponentsInChildren<Theme>().ToList();
        var theme = themes.FirstOrDefault(theme =>
        {
            var n = theme.themeName.ToLower();
            return n == name.ToLower();

        });

        if (!theme)
        {
            //throw new System.Exception($"Tried to reveal missing artifact {name}");
            Debug.LogError($"Tried to reveal missing theme {name}");
            return;
        }
        theme.RevealTheme();
    }

    /// <summary>
    /// Attempts to reveal the theme by calling the child's RevealTheme function
    /// </summary>
    /// <param name="target">The theme to reveal</param>
    public void RevealTheme(Theme target)
    {
        var theme = themes.FirstOrDefault(theme => theme == target);

        if(!theme)
        {
            Debug.LogError($"Tried to reveal missing theme {name}");
            return;
        }

        theme.RevealTheme();
    }

    /// <summary>
    /// Attempts to hide the named theme... in case you want to do that for some reason
    /// </summary>
    /// <param name="name">the name of the theme to hide. This should match the game object's name in Unity</param>
    public void HideTheme(string name)
    {
        var theme = themes.FirstOrDefault(theme => theme.gameObject.name == name);

        if (!theme)
        {
            Debug.LogError($"Tried to hide missing theme {name}");
            return;
        }
        theme.HideTheme();
    }

    /// <summary>
    /// Attempts to hide the named theme... in case you want to do that for some reason
    /// </summary>
    /// <param name="name">the theme to hide.</param>
    public void HideArtifact(Theme target)
    {
        var theme = themes.FirstOrDefault(theme => theme == target);

        if (!theme)
        {
            Debug.LogError($"Tried to hide missing theme {name}");
            return;
        }
        theme.HideTheme();
    }
}
