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
    [SerializeField] bool writeToFile;

    private FileIO fileIO;

    void OnEnable()
    {
        // Make the functions available to Lua
        Lua.RegisterFunction(nameof(KarmicEvent), this, 
            SymbolExtensions.GetMethodInfo(() => 
            KarmicEvent("memory", "theme", "description", 0, "headline")));
    }

    private void Awake()
    {
        fileIO = new FileIO();
    }

    /// <summary>
    /// Stores a karmic event into a json file
    /// </summary>
    public void KarmicEvent(string memory, string theme, string description, double value, string headline)
    {
        // NOTE: This function does NOT override repeated events
        //       which is fine for a player making their way 
        //       through the game but it can lead to a memory leak
        //       if continously called when working in editor 

        if (!writeToFile)
            return;

        // Create a new databundle to override previous data 
        FileIO.DataBundle data = fileIO.LoadData(memory); //new FileIO.DataBundle();
        if(data == null) 
            data = new FileIO.DataBundle();

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
        FileIO.Packet memoryKarmaPacket = data.GetPacket("Karma"); //new FileIO.Packet();
        if(memoryKarmaPacket == null)
        {
            // Generate karma packet if necessary 
            memoryKarmaPacket = new FileIO.Packet();
            memoryKarmaPacket.packetName = "Karma";
        }

        // Generate our arrays 
        JSONStringHelper[] stringHelpers = new JSONStringHelper[] { karmicMemory };

        // Combine our new data to previous data 
        var z = new JSONStringHelper[memoryKarmaPacket.stringValues.Length + stringHelpers.Length];
        memoryKarmaPacket.stringValues.CopyTo(z, 0);
        stringHelpers.CopyTo(z, memoryKarmaPacket.stringValues.Length);

        memoryKarmaPacket.stringValues = z;

        // Data is already associated with a bundle 
        data.SetPacket(memoryKarmaPacket);

        // Send data to FileIO 
        fileIO.StoreData(data, ""); // Store in root json folder 
    }
}
