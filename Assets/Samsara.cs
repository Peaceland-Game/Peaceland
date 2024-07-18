using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static FileIO;
using UnityEngine.SocialPlatforms.Impl;

// Description: The point of this script is to handle the storing and loading
//              of karmic data. This includes the peace meter value seen in 
//              the dialogue data base along with json file(s) that help us 
//              store data for article creation along with any level specific
//              data 


public class Samsara : MonoBehaviour
{
    [SerializeField] DialogueDatabase database;

    private FileIO fileIO;

    void OnEnable()
    {
        // Make the functions available to Lua
        /*Lua.RegisterFunction(nameof(KarmicEvent), this, 
            SymbolExtensions.GetMethodInfo(() => 
            KarmicEvent("memory", "theme", "description", 0, "headline")));*/
    }

    private void Awake()
    {
        fileIO = new FileIO();

        KarmicEvent("Memory 1", "theme", "description", 0, "headline");
    }

    /// <summary>
    /// Stores a karmic event into a json file
    /// </summary>
    public void KarmicEvent(string memory, string theme, string description, int value, string headline)
    {
        // Create a new PlayerData to override previous data 
        FileIO.DataBundle data = new FileIO.DataBundle();
        data.bundleName = memory;


        // Score
        FileIO.JSONStringHelper karmicMemory = new FileIO.JSONStringHelper();
        karmicMemory.name = "Karmic " + (theme + description).GetHashCode(); // Don't want to override other karmic events 
        karmicMemory.value = theme             + " " + 
                              description       + " " +
                              value.ToString()  + " " +
                              headline;


        // Form Packet data structures and store them into 
        // our new Packet
        FileIO.Packet toStoreData = new FileIO.Packet();
        toStoreData.packetName = "Karma";

        JSONIntHelper[] intHelpers = new JSONIntHelper[] { };
        JSONFloatHelper[] floatHelpers = new JSONFloatHelper[] { };
        JSONStringHelper[] stringHelpers = new JSONStringHelper[] { karmicMemory };
        toStoreData.intValues = intHelpers;
        toStoreData.floatValues = floatHelpers;
        toStoreData.stringValues = stringHelpers;

        // Data is already associated with a bundle 
        data.SetPacket(toStoreData);

        // Send data to FileIO 
        fileIO.StoreData(data, ""); // Store in root json folder 
    }
}
