using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Gaia;

public class DebugHelper : MonoBehaviour {
    [Header("Free Cam")]
    [SerializeField] GameObject freeCam;
    [Header("Console")]
    [SerializeField] GameObject debugCanvas;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI logHistoryTextMesh;
    [Space]
    [SerializeField] List<TeleportPoint> teleportPoints = new();
    [SerializeField]
    private Dictionary<string, string> commandAliases = new();

    private GameObject player;
    private GameObject freeCamObj;
    private bool inDebugCamMode = false;
    private bool consoleIsActive = false;

    private void Awake() {
        player = GameObject.FindObjectOfType<FirstPersonController>()?.gameObject;
        commandAliases["tp"] = "Teleport";
        commandAliases["tfc"] = "ToggleFreeCam";
    }


    void Update() {
        // Toggles 
        //  ToggleDebugCamMode();
        //  ToggleDebugConsoleMode();
        HandleToggleDebugConsole();
        // Runtime logic 
        DebugConsoleLogic();
    }

    /// <summary>
    /// Toggle whether the debug console is active or not 
    /// </summary>
    private void ToggleDebugConsoleMode() {
        if (inDebugCamMode)
            return;

        if (Input.GetKeyUp(KeyCode.Tab)) {
            consoleIsActive = true;
            inputField.ActivateInputField();
            inputField.Select();
        }

        debugCanvas.SetActive(consoleIsActive);
    }

    private void HandleToggleDebugConsole() {
        if (Keyboard.current.backquoteKey.wasPressedThisFrame) {
            consoleIsActive = !consoleIsActive;
            debugCanvas.SetActive(consoleIsActive);

            if (consoleIsActive) {
                // Disable movement for both player and free cam
                if (!inDebugCamMode && player != null) {
                    FirstPersonController fpsController = player.GetComponent<FirstPersonController>();
                    if (fpsController != null) {
                        fpsController.enabled = false;
                        Debug.Log("disable player movement");
                    }
                }
                else if (freeCamObj != null) {
                    UnityEngine.Rendering.FreeCamera freeCamera = freeCamObj.GetComponentInChildren<UnityEngine.Rendering.FreeCamera>();
                    if (freeCamera != null) {
                        freeCamera.enabled = false;
                        Debug.Log("disable free cam movement");
                    }
                }

                // Set cursor properties for console interaction
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                // Focus on input field
                inputField.ActivateInputField();
                //inputField.Select();

            }
            else {
                // Re-enable movement for the active controller
                if (!inDebugCamMode && player != null) {
                    FirstPersonController fpsController = player.GetComponent<FirstPersonController>();
                    if (fpsController != null) {
                        Debug.Log("enable player movement");
                        fpsController.enabled = true;
                    }
                }
                else if (freeCamObj != null) {
                    UnityEngine.Rendering.FreeCamera freeCamera = freeCamObj.GetComponentInChildren<UnityEngine.Rendering.FreeCamera>();
                    if (freeCamera != null) {
                        Debug.Log("enable free cam movement");
                        freeCamera.enabled = true;
                    }
                }

                // Reset cursor properties
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            consoleIsActive = true;
            inputField.ActivateInputField();
            inputField.Select();
        }
    }

    ///// <summary>
    ///// Whether or not to set as free cam 
    ///// </summary>
    //private void ToggleDebugCamMode() {
    //    // Freeze cam toggleable 
    //    if (consoleIsActive)
    //        return;

    //    if (Input.GetKeyDown(KeyCode.G)) {

    //    }
    //}

    /// <summary>
    /// Logic that allows the user to ultilize several basic 
    /// commands to help in debugging 
    /// </summary>
    private void DebugConsoleLogic() {
        if (!consoleIsActive)
            return;

        if (Input.GetKeyUp(KeyCode.Return)) {
            string input = inputField.text.Trim();
            string output = ExecuteCommand(input);

            // Printing
            logHistoryTextMesh.text += $"\n> {input}";
            logHistoryTextMesh.text += $"\n{output}";

            // Cleanup
            inputField.text = "";
            inputField.Select();
        }
    }
    private string ExecuteCommand(string input) {
        // First, separate the command name from the arguments
        int parenthesisIndex = input.IndexOf('(');
        string commandName;
        string[] args;

        if (parenthesisIndex != -1) {
            // Command with parentheses
            commandName = input[..parenthesisIndex].Trim().ToLower();
            string argsString = input[(parenthesisIndex + 1)..].TrimEnd(')');
            args = argsString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(arg => arg.Trim())
                             .ToArray();
        }
        else {
            // Command without parentheses
            string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return "Empty command";

            commandName = parts[0].ToLower();
            args = parts.Skip(1).ToArray();
        }

        // Check for aliases
        if (commandAliases.TryGetValue(commandName, out string aliasedCommand))
            commandName = aliasedCommand;

        //Debug.Log(commandName);
        //foreach (var arg in args) {
        //    Debug.Log(arg);
        //}

        // Find the method
        MethodInfo method = GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(m => m.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));

        if (method == null)
            return "Unknown command";

        try {
            // Parse arguments
            ParameterInfo[] parameters = method.GetParameters();
            object[] parsedArgs = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++) {
                if (i < args.Length) {
                    parsedArgs[i] = Convert.ChangeType(args[i], parameters[i].ParameterType);
                }
                else {
                    parsedArgs[i] = parameters[i].ParameterType.IsValueType ? Activator.CreateInstance(parameters[i].ParameterType) : null;
                }
            }

            // Execute the method
            object result = method.Invoke(this, parsedArgs);
            return result?.ToString() ?? "";
        }
        catch (Exception ex) {
            return $"Error executing command: {ex.Message}";
        }
    }

    /// <summary>
    /// Attempts to teleport the player to the given location 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private string Teleport(string input) {
        if (input.Length < 2)
            return "Invalid location";



        string target = input.ToLower();

        // TODO: Make this not a brute force implementation 
        for (int i = 0; i < teleportPoints.Count; i++) {
            if (target == teleportPoints[i].key.ToLower()) {
                player.transform.position = teleportPoints[i].point.position;
                return "Teleported to " + target;
            }
        }

        return "Invalid location";
    }
    private void ToggleFreeCam() {
        inDebugCamMode = !inDebugCamMode;
        player.SetActive(!inDebugCamMode);

        if (inDebugCamMode) {
            // Spawn free cam 
            freeCamObj = Instantiate(freeCam, player.transform.position, player.transform.rotation);
        }
        else {
            // Teleport player character to free cam position 
            player.transform.position = freeCamObj.transform.position;

            // Delete free cam 
            Destroy(freeCamObj);
        }
    }
    private void Clear() {
        logHistoryTextMesh.text = "";
    }
    //private void Close() {
    //    HandleToggleDebugConsole();
    //}
    private string Help() {
        return "Available commands: Clear, Teleport, SetLuaVar, Swap";
    }
    public void SetLuaVar(string var, object value) {
        // Implement setting Lua variable logic here
        Debug.Log($"Set Lua variable '{var}' to {value}");
    }

    /// <summary>
    /// Attempts to swap currently active memory 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private string Swap(int level) {


        MemorySwapper memory = GameObject.FindObjectOfType<MemorySwapper>();
        if (memory) {
            if (memory.HasLevel(level)) {
                memory.SwitchToMemory(level);
            }
            else {
                return "Invalid memory index";
            }
        }
        else {
            return "Memory Swapper not found";
        }

        return "Loading Memory";
    }

    [System.Serializable]
    private class TeleportPoint {
        [SerializeField] public string key;
        [SerializeField] public Transform point;
    }
}
