using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FileIO;

/// <summary>
/// Used to store and load data using FileIo. Loading
/// data is still fairly generalized but is more 
/// human readable than the FileIO. Functions are 
/// explicitly made for each game mode in this 
/// script where data is turned into FIleIO data
/// structures. 
/// </summary>
public class DataStoreLoad
{
    private FileIO fileIO;
    private FileIO.DataBundle currentData;


    public DataStoreLoad()
    {
        // Initialize 
        fileIO = new FileIO();
        currentData = null;
    }

    #region GAMEMODE_STORING

    /// <summary>
    /// Store the data recorded for the some arbitrary gamemode 
    /// </summary>
    /// <param name="score"></param>
    /// <param name="collisions"></param>
    /// <param name="time"></param>
    public void StoreSample(string bundleName, string path, float score, int collisions, float time)
    {
        // Create a new PlayerData to override previous data 
        FileIO.DataBundle data = new FileIO.DataBundle();
        data.bundleName = bundleName;


        // Score
        FileIO.JSONFloatHelper scoreStore = new FileIO.JSONFloatHelper();
        scoreStore.name = "score";
        scoreStore.value = score;

        // Collisions 
        FileIO.JSONIntHelper collisionsStore = new FileIO.JSONIntHelper();
        collisionsStore.name = "collisions";
        collisionsStore.value = collisions;

        // Time 
        FileIO.JSONFloatHelper timeStore = new FileIO.JSONFloatHelper();
        timeStore.name = "time";
        timeStore.value = time;


        // Form Packet data structures and store them into 
        // our new Packet
        FileIO.Packet toStoreData = new FileIO.Packet();
        toStoreData.packetName = "SampleLevel";

        JSONIntHelper[] intHelpers = new JSONIntHelper[] { collisionsStore };
        JSONFloatHelper[] floatHelpers = new JSONFloatHelper[] { scoreStore, timeStore };
        JSONStringHelper[] stringHelpers = new JSONStringHelper[] { };
        toStoreData.intValues = intHelpers;
        toStoreData.floatValues = floatHelpers;
        toStoreData.stringValues = stringHelpers;

        // Data is already associated with a bundle 
        data.SetPacket("SampleData", toStoreData);

        // Send data to FileIO 
        fileIO.StoreData(data, path);
    }

    #endregion

    #region GETTERS

    /// <summary>
    /// Get a specific variable's value for a bundle in a specific packet 
    /// </summary>
    /// <returns></returns>
    public float GetFloat(string bundleName, string packetName, string floatName)
    {
        if (currentData == null || currentData.bundleName != bundleName)
            currentData = fileIO.LoadData(bundleName);

        Packet packet = currentData.GetPacket(packetName);

        return packet.GetFloat(floatName);
    }

    /// <summary>
    /// Get a specific variable's value for a player in a specific level
    /// </summary>
    /// <returns></returns>
    public int GetInt(string bundleName, string packetName, string intName)
    {
        if (currentData == null || currentData.bundleName != bundleName)
            currentData = fileIO.LoadData(bundleName);

        Packet packet = currentData.GetPacket(packetName);

        return packet.GetInt(intName);
    }

    /// <summary>
    /// Get a specific variable's value for a player in a specific level
    /// </summary>
    /// <returns></returns>
    public string GetString(string bundleName, string packetName, string stringName)
    {
        if (currentData == null || currentData.bundleName != bundleName)
            currentData = fileIO.LoadData(bundleName);

        Packet packet = currentData.GetPacket(packetName);

        return packet.GetString(stringName);
    }

    #endregion
}