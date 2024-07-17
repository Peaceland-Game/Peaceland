using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class HeadlineMaker : MonoBehaviour
{
    // Todo: Either give ability to read from file or
    //       have ability to write to this data. I 
    //       recommend using the file reading method
    //       since we it won't require us to have this
    //       object active in the main scene. 

    [SerializeField] GameObject btnTopicElement;
    [SerializeField] GameObject btnNoteElement;
    //[SerializeField] GameObject headlineElement; 
    [SerializeField] Transform themesParent;
    [SerializeField] Transform notesParent;
    [SerializeField] TextMeshProUGUI headerTextMesh;
    [SerializeField] TextMeshProUGUI subheaderTextMesh; // :3
    [SerializeField] TextMeshProUGUI noteTextMesh;

    //[SerializeField] Transform headlineParent;
    [Space]
    [SerializeField] List<TopicData> topics;

    // Todo: Remove exposure. Only in editor for debugging 
    [SerializeField] List<GameObject> topicObjs;
    [SerializeField] List<GameObject> noteObjs;
    int selectedTopic = -1; 
    int selectedNote = -1;
    
    public int SelectedTopic { get { return selectedTopic; } }
    public int SelectedNote { get { return selectedNote; } }
    
    
    /*[Header("Animation Pages")]
    [SerializeField] List<RectTransform> pages;
    [SerializeField] float heightOffset;
    [SerializeField] float appearTime;
    [Tooltip("What is the time before letting the next page appear")]
    [SerializeField] float appearOffset;
    [SerializeField] AnimationCurve appearCurve;
    [SerializeField] public bool isRunning;
    [Space]
    [SerializeField] float btnAppearTime;
    [SerializeField] float btnAppearOffset;
    [Header("Animation Header")]
    [SerializeField] Vector3 headerPos;
    [SerializeField] float headerAppearTime; */

    // Current header page
    //private GameObject headerHold; 

    private void Start()
    {
        GenerateTopics();
        //AnimateAppear();
    }

    public void SelectTopic()
    {
        print(this.gameObject.name);
    }

    /// <summary>
    /// Generates the buttons for choosing a topic 
    /// </summary>
    public void GenerateTopics()
    {
        // Replace old objects 
        if (topicObjs.Count > 0)
            ClearTopics();

        ResetSelection();

        for (int i = 0; i < topics.Count; i++)
        {
            // Create object 
            topicObjs.Add(Instantiate(btnTopicElement, themesParent));
            topicObjs[i].AddComponent<TopicClick>();

            // Write text 
            topicObjs[i].GetComponentInChildren<TextMeshProUGUI>().text = topics[i].topic.ToString();

            // Setup button event 
            //topicObjs[i].GetComponent<TopicClick>().HeadlineMaker = this;
        }
    }

    /// <summary>
    /// Generates the buttons for choosing a specific note 
    /// </summary>
    public void GenerateNotes()
    {
        // Check if topic is within range 
        if (topics.Count == 0)
            return;
        if (!(selectedTopic >= 0 && selectedTopic < topics.Count))
            return;

        // Replace old objects 
        if (noteObjs.Count > 0)
            ClearNotes();


        // Generate buttons and fill out note textmesh 
        TopicData topic = topics[selectedTopic];
        for (int i = 0; i < topic.notes.Count; i++)
        {
            // Create object 
            noteObjs.Add(Instantiate(btnNoteElement, notesParent));
            noteObjs[i].AddComponent<NoteClick>();

            // Write text 
            noteObjs[i].GetComponentInChildren<TextMeshProUGUI>().text = topic.notes[i].headline;

        }
    }

    public void GenerateNotes(int index)
    {
        selectedTopic = index;

        GenerateNotes();
    }

    /// <summary>
    /// Generates the final paper sample 
    /// </summary>
    public void GenerateHeadline()
    {
        TopicData topic = topics[selectedTopic];

        // Check if topic is within range 
        if (topic.notes.Count == 0)
            return;
        if (!(selectedNote >= 0 && selectedNote < topic.notes.Count))
            return;

        Note note = topic.notes[selectedNote];

        // Clean up textmeshes 
        noteTextMesh.text = "";
        for (int i = 0; i < topic.notes.Count; i++)
        {
            // Generate notes in text 
            noteTextMesh.text += topic.notes[i].description + "\n\n";
        }

        /*if (headerHold != null)
        {
            Destroy(headerHold);
        }*/

        /*GameObject temp = Instantiate(headlineElement, Vector3.zero, Quaternion.identity, headlineParent);
        temp.GetComponent<RectTransform>().localPosition = headerPos;
        temp.GetComponentInChildren<TextMeshProUGUI>().text = note.headline;*/
        headerTextMesh.text = topic.topic.ToString();
        subheaderTextMesh.text = note.headline;
        
        //headerHold = temp;
    } 

    /// <summary>
    /// Generates a headline based on a note's idnex
    /// </summary>
    /// <param name="index"></param>
    public void GenerateHeadline(int index)
    {
        selectedNote = index;
        GenerateHeadline();
    }

    /// <summary>
    /// Destroys all topic game objects 
    /// </summary>
    public void ClearTopics()
    {
        for(int i = topicObjs.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(topicObjs[i]);
        }

        topicObjs.Clear();
    }

    /// <summary>
    /// Destroys all note game objects 
    /// </summary>
    public void ClearNotes()
    {
        for (int i = noteObjs.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(noteObjs[i]);
        }

        selectedNote = -1;
        noteObjs.Clear();
    }

    /// <summary>
    /// Resets the selection to negative values 
    /// </summary>
    private void ResetSelection()
    {
        selectedTopic = -1;
        selectedNote = -1;
    }

    /// <summary>
    /// Animates the maker to appear 
    /// </summary>
    /*public void AnimateAppear()
    {
        if (isRunning)
            return;

        isRunning = true;
        StartCoroutine(AppearManage());
        StartCoroutine(AnimateButtons(topicObjs));

    }*/

    /// <summary>
    /// Animates the given buttons list to appear
    /// </summary>
    /// <param name="buttons"></param>
    /// <returns></returns>
    /*private IEnumerator AnimateButtons(List<GameObject> buttons)
    {
        // Set to invisible 
        for (int i = 0; i < buttons.Count; i++)
        {
            // Get rendering components 
            Image image = buttons[i].GetComponent<Image>();
            TextMeshProUGUI textMesh = buttons[i].GetComponentInChildren<TextMeshProUGUI>();

            // Remove alpha 
            Vector3 satBtn = (Vector3)(Vector4)image.color;
            Vector3 satTex = (Vector3)(Vector4)textMesh.color;
            image.color = (Vector4)satBtn;
            textMesh.color = (Vector4)satTex;
        }

        // Wait until on board 
        yield return new WaitForSeconds(appearTime);

        for (int i = 0; i < buttons.Count; i++)
        {
            StartCoroutine(AppearButton(buttons[i]));
            yield return new WaitForSeconds(btnAppearOffset);
        }
    }*/

    /// <summary>
    /// Animates the given button to appear 
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    /*private IEnumerator AppearButton(GameObject button)
    {
        // Get rendering components 
        Image image = button.GetComponent<Image>();
        TextMeshProUGUI textMesh = button.GetComponentInChildren<TextMeshProUGUI>();

        // Remove alpha 
        Vector3 satBtn = (Vector3)(Vector4)image.color;
        Vector3 satTex = (Vector3)(Vector4)textMesh.color;
        image.color = (Vector4)satBtn;
        textMesh.color = (Vector4)satTex;

        float timer = 0.0f;

        while (timer <= btnAppearTime)
        {
            float lerp = timer / btnAppearTime;
            // Cast down remove alpha 
            satBtn = (Vector3)(Vector4)image.color;

            image.color = (Vector4)satBtn + new Vector4(0, 0, 0, lerp); ;
            textMesh.color = (Vector4)satTex + new Vector4(0, 0, 0, lerp); ;

            timer += Time.deltaTime;
            yield return null;
        }
    }*/

    /// <summary>
    /// Manages the appearing of the different pages 
    /// </summary>
    /// <returns></returns>
    /*private IEnumerator AppearManage()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            StartCoroutine(AppearPage(pages[i]));
            yield return new WaitForSeconds(appearOffset);
        }
        
        yield return new WaitForSeconds(appearTime);
        isRunning = false;
    }*/

    /// <summary>
    /// Animates the appearance of a single page 
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
   /* private IEnumerator AppearPage(Transform page)
    {
        float timer = 0.0f;
        
        float startPos = page.localPosition.y + heightOffset;
        float targetPos = page.localPosition.y;

        while (timer <= appearTime)
        {
            float lerp = appearCurve.Evaluate( timer / appearTime);
            page.localPosition = new Vector3(page.localPosition.x, Mathf.Lerp(startPos, targetPos, lerp), page.localPosition.z);

            timer += Time.deltaTime;
            yield return null;
        }
    }*/

    /// <summary>
    /// Aniamtes the headline page as if tossed 
    /// onto the screen 
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
/*    private IEnumerator AppearHeadline(RectTransform page)
    {
        float timer = 0.0f;

        while(timer < headerAppearTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

    }*/

    /// <summary>
    /// Represents a single emotino or topic and holds the different notes related 
    /// </summary>
    [System.Serializable]
    private class TopicData
    {
        [SerializeField] public TopicType topic;
        [SerializeField] public List<Note> notes;
    }

    /// <summary>
    /// Represents a note achieved in the memory. Also has the headline that could
    /// be generated by using this note 
    /// </summary>
    [System.Serializable]
    private class Note
    {
        [SerializeField] public string description;
        [SerializeField] public string headline;
    }

    /// <summary>
    /// The different kinds of possible topics to choose from 
    /// </summary>
    public enum TopicType
    {
        OBEDIENCE, 
        CURIOSITY,
        FEAR,
        LOYALTY,
        SUSPICION,
        TRUST,
        SENTIMENTAL,
        ASSERTIVE,
        DETERMINATION
    }
}
