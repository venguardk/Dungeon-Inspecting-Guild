using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    public static DataPersistenceManager instance { get; private set; }

    //Main Save/Load systems based on How to make a Save & Load System in Unity - https://youtu.be/aUi9aijvpgs?si=GLtBO4zP_VGItJr-
    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("More than one Data Persistence Manager in scene");
        }
        instance = this;
    }

    private void Start()
    {
        this.dataPersistenceObjects = FindAllPersistenceObjects();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        if (this.gameData == null)
        {
            Debug.Log("No game data");
            SaveGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        Debug.Log("LoadGame");
    }
    public void SaveGame()
    {

        if (this.gameData == null)
        {
            Debug.Log("No game data");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        Debug.Log("SaveGame");
    }

    private List<IDataPersistence> FindAllPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
