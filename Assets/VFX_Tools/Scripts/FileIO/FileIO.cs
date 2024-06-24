using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Analytics;
using static FileIO;
using System.Linq;

/// <summary>
/// For generalized bundle loading and storing. Not
/// for any level or bundle in particuluar. Also holds 
/// data structures to make the process more convient.
/// 
/// When storing data it expects it in the proper data
/// structures beforehand. 
/// </summary>
public class FileIO
{
    /// <summary>
    /// Load a json file if possible. Consider that the
    /// starting directory to be from jsons folder 
    /// </summary>
    /// <returns></returns>
    public DataBundle LoadData(string bundleName, string path = "")
    {
        string basePath = Directory.GetCurrentDirectory() + "\\jsons\\";
        string finalPath = basePath + path + bundleName + ".json";

        if (!File.Exists(finalPath))
        {
            Debug.LogWarning("Filepath " + finalPath + " does not exist");
            return null;
        }

        string jsonString = File.ReadAllText(finalPath);
        return JsonUtility.FromJson<DataBundle>(jsonString);
    }


    /// <summary>
    /// Store a bundle's data using the bundle's name and the end of the given path 
    /// as a json directory
    /// </summary>
    /// <param name="data"></param>
    public void StoreData(DataBundle data, string path = "")
    {
        string basePath = Directory.GetCurrentDirectory() + "\\jsons\\";
        string finalPath = basePath + path + data.bundleName + ".json";

        if (!File.Exists(finalPath))
            File.CreateText(finalPath);

        File.WriteAllText(finalPath, JsonUtility.ToJson(data, true));
    }



    #region FileIOStructs

    /// <summary>
    /// Represents a whole json file connected to a single bundle  
    /// </summary>
    [System.Serializable]
    public class DataBundle
    {
        public string bundleName;
        public Packet[] dataPackets;

        /// <summary>
        /// Get the level data by passing in its name. If level 
        /// does not exists returns null 
        /// </summary>
        /// <param name="packetName"></param>
        /// <returns></returns>
        public Packet GetPacket(string packetName)
        {
            foreach (Packet packet in dataPackets)
            {
                if (packet.packetName == packetName)
                    return packet;
            }

            return null;
        }

        /// <summary>
        /// Store a packet connected to a bundles's JSON. If packet name
        /// does not exist adds new data to the bundle's JSON. 
        /// </summary>
        /// <param name="levelName"></param>
        /// <param name="data"></param>
        public void SetPacket(string packetName, FileIO.Packet data)
        {
            // Make sure dataPackets is not null 
            if (dataPackets == null)
                dataPackets = new Packet[0];

            for (int i = 0; i < dataPackets.Length; i++)
            {
                // Have we found the right level? 
                if (dataPackets[i].packetName == packetName)
                {
                    dataPackets[i] = data;
                }
            }

            // Add data as a new entry 
            FileIO.Packet[] nextData = new FileIO.Packet[dataPackets.Length + 1];
            for (int i = 0; i < dataPackets.Length; i++)
            {
                nextData[i] = dataPackets[i];
            }
            nextData[dataPackets.Length] = data;

            dataPackets = nextData;
        }
    }

    /// <summary>
    /// Holds the raw data tied to a particuluar object or state. 
    /// Each array holds structs of a type and their particuluar 
    /// name. 
    /// </summary>
    [System.Serializable]
    public class Packet
    {
        public string packetName;
        public JSONIntHelper[] intValues;
        public JSONFloatHelper[] floatValues;
        public JSONStringHelper[] stringValues;

        public int GetInt(string variableName)
        {
            foreach (JSONIntHelper intData in intValues)
            {
                if (intData.name == variableName)
                    return intData.value;
            }

            Debug.Assert(false, "Invalid variable name: " + variableName);

            return -1;
        }

        public float GetFloat(string variableName)
        {
            foreach (JSONFloatHelper floatData in floatValues)
            {
                if (floatData.name == variableName)
                    return floatData.value;
            }

            Debug.Assert(false, "Invalid variable name: " + variableName);

            return -1.0f;
        }

        public string GetString(string variableName)
        {
            foreach (JSONStringHelper stringData in stringValues)
            {
                if (stringData.name == variableName)
                    return stringData.value;
            }

            Debug.Assert(false, "Invalid variable name: " + variableName);

            return "";
        }
    }


    [System.Serializable]
    public class JSONIntHelper
    {
        public string name;
        public int value;
    }

    [System.Serializable]
    public class JSONFloatHelper
    {
        public string name;
        public float value;
    }


    [System.Serializable]
    public class JSONStringHelper
    {
        public string name;
        public string value;
    }

    #endregion

}