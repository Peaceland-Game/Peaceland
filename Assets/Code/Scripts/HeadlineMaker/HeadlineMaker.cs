using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
