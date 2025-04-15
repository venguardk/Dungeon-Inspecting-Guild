using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private string saveName;
    [SerializeField] private string optionName;
    private GameData gameData;
    private OptionData optionData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private FileDataHandler saveHandler;
    private FileDataHandler optionHandler;
    private string selectedProfileID = "";
    public static DataPersistenceManager instance { get; private set; }

    //Main Save/Load systems based on How to make a Save & Load System in Unity - https://youtu.be/aUi9aijvpgs?si=GLtBO4zP_VGItJr-
    //This script handles the game's save and load mechanic
    //Other scripts this script interacts with: LevelEditorManager, SceneLoadManager, FileDataHandler, HighContrastGrayScale, PlayerCOntrolsOption, GameLanguageManager, IDataPersistence, OptionData, ShareManager, GameDataSrialized, SerializableGameObject, SerializableKeyValuePair, SerializableVecotr2, GameData
    private void Awake()
    {
        if (instance != null) 
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            string downloadsPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "Downloads");
            this.dataHandler = new FileDataHandler(downloadsPath, fileName);
            this.saveHandler = new FileDataHandler(Application.persistentDataPath, saveName);
            this.optionHandler = new FileDataHandler(Application.persistentDataPath, optionName);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // Creates a new OptionData and GameData for save and load
    public void NewGame()
    {
        this.optionData = new OptionData();
        this.gameData = new GameData();
        Debug.Log("NewGame");
    }

    // Resets the components with GameData and save it
    public void ResetGame()
    {
        if(this.gameData == null)
        {
            this.dataPersistenceObjects = FindAllPersistenceObjects();
            LoadGame();
        }
        string levelName = this.gameData.levelName;

        this.gameData = new GameData();
        this.gameData.levelName = levelName;
        saveHandler.Save(this.gameData, selectedProfileID);
        
        Debug.Log("ResetGame");
    }

    // Take an JSON file and load it in to GameData
    public void ImportGame()
    {
        this.gameData = dataHandler.Load(selectedProfileID);
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        Debug.Log("ImportGame");
    }

    // Take the current GameData and export it into JSON file
    public void ExportGame()
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
        dataHandler.Save(gameData, selectedProfileID);
        Debug.Log("ExportGame");
    }

    // Take the current GameData and use the variables within it to run LoadData script within each scripts with IDataPersistence
    public void LoadGame()
    {
        this.gameData = saveHandler.Load(selectedProfileID);
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

    // Take the current GameData and use the variables within it to run SaveData script within each scripts with IDataPersistence
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
        saveHandler.Save(gameData, selectedProfileID);
        Debug.Log("SaveGame");
    }

    // Take the current OptionData and use the variables within it to run LoadData script within each scripts with IDataPersistence
    public void LoadOption()
    {
        this.optionData = optionHandler.LoadOption();
        if (this.optionData == null)
        {
            Debug.Log("No option data");
            SaveOption();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadOption(optionData);
        }

        Debug.Log("LoadOption");
    }

    // Take the current OptionData and use the variables within it to run SaveData script within each scripts with IDataPersistence
    public void SaveOption()
    {

        if (this.optionData == null)
        {
            Debug.Log("No option data");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveOption(ref optionData);
        }
        optionHandler.SaveOption(optionData);
        Debug.Log("SaveOption");
    }

    public void ChangeSelectedProfile(string profileID)
    {
        this.selectedProfileID = profileID;
        LoadGame();
    }

    public void DeleteProfileData(string profileID)
    {
        saveHandler.Delete(profileID);
        LoadGame();
    }

    // Load the data saved from the previous scene
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllPersistenceObjects();
        LoadGame();
        LoadOption();
    }

    // Save the current data for the next scene loaded
    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
        SaveOption();
    }

    // Search all scripts with IDataPersistence in scene
    private List<IDataPersistence> FindAllPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return saveHandler.LoadAllProfiles();
    }
}
