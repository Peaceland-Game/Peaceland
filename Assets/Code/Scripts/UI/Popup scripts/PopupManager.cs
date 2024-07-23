//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using TMPro;
//using UnityEngine;

//public class PopupManager : MonoBehaviour
//{
//    [SerializeField] public List<TextMeshProUGUI> popups;

//    [SerializeField] public TextMeshProUGUI karmaPopup;

//    void Start()
//    {
//        popups = GetComponentsInChildren<TextMeshProUGUI>().ToList();

//        // Get the karma popup
//        foreach (TextMeshProUGUI p in popups)
//        {
//            if (p.name == "KarmaPopup")
//            {
//                karmaPopup = p;
//                break;
//            }
//        }

//        // Set popups to inactive
//        foreach (TextMeshProUGUI p in popups)
//        {
//            // for testing. Remove this if you remove the test buttons (or don't)
//            if (p.name != "TestButtonGainKarma" && p.name != "TestButtonLoseKarma")
//            {
//                p.gameObject.SetActive(false);
//            }

//            //p.gameObject.SetActive(false);
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }

//    /// <summary>
//    /// Sets up a karma popup
//    /// </summary>
//    /// <param name="value"> the change in karma. Use a positive number to add karma, or a negative number to take it away </param>
//    public void CreateKarmaPopup(int value)
//    {
//        // Check if karma is being gained or lost, and adjusting the text accordingly
//        if(value > 0)
//        {
//            karmaPopup.color = Color.green;
//            karmaPopup.text = $"Gained {value} karma";
//        }
//        else if(value < 0)
//        {
//            karmaPopup.color = Color.red;
//            karmaPopup.text = $"Lost {Mathf.Abs(value)} karma";
//        }
//        else
//        {
//            // wait why did I do this
//        }

//        StartCoroutine(FadeInOutPopup(karmaPopup, 1, 3, 1));
//        StartCoroutine(MoveAndHold(karmaPopup, 0.1f, 0, 3, 2, true));
//    }

//    /// <summary>
//    /// Fades a popup in and out
//    /// </summary>
//    /// <param name="popup"> the popup to fade </param>
//    /// <param name="fadeInTime"> time fading in </param>
//    /// <param name="holdTime"> time staying solid </param>
//    /// <param name="fadeOutTime"> time fading out </param>
//    /// <returns> what? </returns>
//    public IEnumerator FadeInOutPopup(TextMeshProUGUI popup, float fadeInTime, float holdTime, float fadeOutTime)
//    {
//        popup.color = new Color(popup.color.r, popup.color.g, popup.color.b, 0);
//        popup.gameObject.SetActive(true);
        
//        float timer = 0f;

//        // Fade in
//        while(timer < fadeInTime)
//        {
//            timer += Time.deltaTime;

//            float alpha = timer / fadeInTime;
//            popup.color = new Color(popup.color.r, popup.color.g, popup.color.b, alpha);
            
//            yield return null;
//        }

//        // Hold
//        yield return new WaitForSeconds(holdTime);

//        timer = 0f;

//        // Fade out
//        while (timer < fadeOutTime)
//        {
//            timer += Time.deltaTime;

//            float alpha = 1 - (timer / fadeOutTime);
//            popup.color = new Color(popup.color.r, popup.color.g, popup.color.b, alpha);

//            yield return null;
//        }

//        // Return to inactive state
//        popup.gameObject.SetActive(false);
//    }

//    /// <summary>
//    /// Moves a popup
//    /// </summary>
//    /// <param name="popup"> the popup to move </param>
//    /// <param name="verticalMove"> how the popup should move on the y axis </param>
//    /// <param name="horizontalMove"> how the popup should move on the x axis </param>
//    /// <param name="time"> the time to spend moving </param>
//    /// <param name="resetPos"> whether the popup should reset its position at the end of its move </param>
//    /// <returns></returns>
//    public IEnumerator MovePopup(TextMeshProUGUI popup, float verticalMove, float horizontalMove, float time, bool resetPos)
//    {
//        // Get the start position
//        Vector3 startPos = popup.transform.position;
        
//        float timer = 0f;

//        // Move based on elapsed time
//        while (timer < time)
//        {
//            timer += Time.deltaTime;

//            float moveFraction = timer / time;
//            popup.transform.Translate(new Vector3((horizontalMove * moveFraction), (verticalMove * moveFraction), 0));

//            yield return null;
//        }

//        // Reset position if necessary
//        if(resetPos)
//        {
//            popup.transform.position = startPos;
//        }
//    }

//    /// <summary>
//    /// Moves a popup, and then holds position for a duration
//    /// </summary>
//    /// <param name="popup"> the popup to move </param>
//    /// <param name="verticalMove"> how the popup should move on the y axis </param>
//    /// <param name="horizontalMove"> how the popup should move on the x axis </param>
//    /// <param name="moveTime"> the time to spend moving </param>
//    /// <param name="holdTime"> the time to spend holding the final position </param>
//    /// <param name="resetPos"> whether the popup should reset its position at the end of its move </param>
//    /// <returns></returns>
//    public IEnumerator MoveAndHold(TextMeshProUGUI popup, float verticalMove, float horizontalMove, float moveTime, float holdTime, bool resetPos)
//    {
//        // Get the start position
//        Vector3 startPos = popup.transform.position;

//        // Move
//        yield return StartCoroutine(MovePopup(popup, verticalMove, horizontalMove, moveTime, false));

//        // Hold
//        yield return new WaitForSeconds(holdTime);

//        // Reset position if necessary
//        if (resetPos)
//        {
//            popup.transform.position = startPos;
//        }
//    }
//}
