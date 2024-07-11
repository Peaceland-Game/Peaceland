using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    [Header("Free Cam")]
    [SerializeField] GameObject freeCam;
    [Header("Console")]
    [SerializeField] GameObject debugCanvas;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI logHistoryTextMesh;
    [Space]
    [SerializeField] List<TeleportPoint> teleportPoints;

    private GameObject player;
    private GameObject freeCamObj;
    private bool inDebugCamMode = false;
    private bool consoleIsActive = false;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<FirstPersonController>()?.gameObject;
    }


    void Update()
    {
        // Toggles 
        ToggleDebugCamMode();
        ToggleDebugConsoleMode();

        // Runtime logic 
        DebugConsole();
    }

    /// <summary>
    /// Toggle whether the debug console is active or not 
    /// </summary>
    private void ToggleDebugConsoleMode()
    {
        if (inDebugCamMode)
            return;

        if(Input.GetKeyUp(KeyCode.Tab))
        {
            consoleIsActive = true;
            inputField.ActivateInputField();
            inputField.Select();
        }

        debugCanvas.SetActive(consoleIsActive);
    }

    /// <summary>
    /// Whether or not to set as free cam 
    /// </summary>
    private void ToggleDebugCamMode()
    {
        // Freeze cam toggleable 
        if (consoleIsActive)
            return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            inDebugCamMode = !inDebugCamMode;
            player.SetActive(!inDebugCamMode);

            if (inDebugCamMode)
            {
                // Spawn free cam 
                freeCamObj = Instantiate(freeCam, player.transform.position, player.transform.rotation);
            }
            else
            {
                // Teleport player character to free cam position 
                player.transform.position = freeCamObj.transform.position;

                // Delete free cam 
                Destroy(freeCamObj);
            }
        }
    }

    /// <summary>
    /// Logic that allows the user to ultilize several basic 
    /// commands to help in debugging 
    /// </summary>
    private void DebugConsole()
    {
        if (!consoleIsActive)
            return;

        if(Input.GetKeyUp(KeyCode.Return))
        {

            string[] commands = inputField.text.Split();
            string outputText = "   ";

            if (commands.Length <= 0)
                return;

            switch(commands[0].ToLower()) // Primary command 
            {
                case "clear":
                    logHistoryTextMesh.text = "";
                    outputText += "Console cleared";
                    break;
                case "tp":
                    outputText += Teleport(commands);
                    break;
                case "close":
                    consoleIsActive = false;
                    outputText += "Closing console";
                    break;
                case "help":
                    outputText += "clear, tp, close, swap";
                    break;
                case "swap":
                    outputText += Swap(commands);
                    break;
                default:
                    outputText += "Invalid Command";
                    break;
            }
            
            // Printing 
            logHistoryTextMesh.text += "\n" + inputField.text.ToLower();
            logHistoryTextMesh.text += "\n" + outputText;


            // Cleaup 
            inputField.text = "";
        }
    }

    /// <summary>
    /// Attempts to teleport the player to the given location 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private string Teleport(string[] input)
    {
        if (input.Length < 2)
            return "Invalid location";

        string target = input[1].ToLower();

        // TODO: Make this not a brute force implementation 
        for (int i = 0; i < teleportPoints.Count; i++)
        {
            if(target == teleportPoints[i].key.ToLower())
            {
                player.transform.position = teleportPoints[i].point.position;
                return "Teleported to " + target;
            }
        }

        return "Invalid location";
    }

    /// <summary>
    /// Attempts to swap currently active memory 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private string Swap(string[] input)
    {
        if (input.Length < 2)
            return "Invalid Memory";

        int level = int.Parse(input[1].ToLower());

        MemorySwapper memory = GameObject.FindObjectOfType<MemorySwapper>();
        memory?.SwitchToMemory(level);

        return "Loading Memory";
    }

    [System.Serializable]
    private class TeleportPoint
    {
        [SerializeField] public string key;
        [SerializeField] public Transform point;
    }
}
