using System.Collections;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;

public class FileDataHandler
{
    private string dataDirPath = "";

    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
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

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(dataDirPath);
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
}
