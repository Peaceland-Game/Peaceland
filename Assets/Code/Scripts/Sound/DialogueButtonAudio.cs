using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueButtonAudio : MonoBehaviour
{
    private UniversalSoundManager soundManager;

    // Start is called before the first frame update
    void Awake()
    {
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<UniversalSoundManager>();

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(DialogueSelectSound);
    }

    public void DialogueSelectSound()
    {
        soundManager.SelectDialogueOptionSound();
        //Debug.Log("Attempted to play dialogue select sound");
    }
}
