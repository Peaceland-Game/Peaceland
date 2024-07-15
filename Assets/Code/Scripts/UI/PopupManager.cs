using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] public List<TextMeshProUGUI> popups;

    [SerializeField] public TextMeshProUGUI karmaPopup;

    void Start()
    {
        popups = GetComponents<TextMeshProUGUI>().ToList();

        foreach (TextMeshProUGUI p in popups)
        {
            p.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sets up a karma popup
    /// </summary>
    /// <param name="value"> the change in karma. Use a positive number to add karma, or a negative number to take it away </param>
    public void CreateKarmaPopup(int value)
    {
        if(value > 0)
        {
            karmaPopup.color = Color.green;
            karmaPopup.text = $"Gained {value} karma";
        }
        else if(value < 0)
        {
            karmaPopup.color = Color.red;
            karmaPopup.text = $"Lost {Mathf.Abs(value)} karma";
        }
        else
        {
            // wait why did I do this
        }

        karmaPopup.color = new Color(karmaPopup.color.r, karmaPopup.color.g, karmaPopup.color.b, 0);
        karmaPopup.gameObject.SetActive(true);

        StartCoroutine(FadeInOutKarmaPopup());
    }

    public IEnumerator FadeInOutKarmaPopup()
    {
        float timer = 0f;

        while(timer < 1f)
        {
            timer += Time.deltaTime;

            float alpha = timer / 1f;
            karmaPopup.color = new Color(karmaPopup.color.r, karmaPopup.color.g, karmaPopup.color.b, alpha);
            
            yield return null;
        }

        yield return new WaitForSeconds(3);

        timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime;

            float alpha = 1 - (timer / 1f);
            karmaPopup.color = new Color(karmaPopup.color.r, karmaPopup.color.g, karmaPopup.color.b, alpha);

            yield return null;
        }

        karmaPopup.gameObject.SetActive(false);
    }
}
