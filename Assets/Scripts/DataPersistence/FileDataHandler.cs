using System.Collections;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;

public class FileDataHandler
{
    // This script is for handling save and load on files with the name dataFIleName and dataDirPath
    private string dataDirPath = "";

    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load(string profileID)
    {
        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
        GameDataSerialized loadedDataSerialized = null;
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedDataSerialized = JsonUtility.FromJson<GameDataSerialized>(dataToLoad);
                loadedData = loadedDataSerialized.ToGameData();
            }
            catch(Exception e)
            {
                Debug.Log("Error loading data from file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data, string profileID)
    {
        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.Combine(dataDirPath, profileID));
            GameDataSerialized dataSerialized = new GameDataSerialized(data);
            string dataToStore = JsonUtility.ToJson(dataSerialized, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error saving data to file: " + fullPath + "\n" + e);
        }
    }

    public OptionData LoadOption()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        OptionData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<OptionData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.Log("Error loading data from file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void SaveOption(OptionData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(dataDirPath);

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error saving data to file: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();

        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileID = dirInfo.Name;
            string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
            if (!File.Exists(fullPath))
            {
                continue;
            }

            GameData profileData = Load(profileID);
            if (profileData != null)
            {
                profileDictionary.Add(profileID, profileData);
            }
            else
            {
                Debug.Log("Null Data Loading. Profile ID: " + profileID);
            }
        }

        return profileDictionary;
    }

    public void Delete(string profileID)
    {
        if(profileID == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
        try
        {
            if (File.Exists(fullPath))
            {
                Debug.Log("Deleted");
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.Log("Error saving data to file, data not found at path: " + fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error deleting data from profileID: " + profileID + " at path:" + fullPath + "\n" + e);
        }
    }
}
