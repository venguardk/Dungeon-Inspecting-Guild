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
    private GameData gameData;
    private OptionData optionData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance { get; private set; }

    //Main Save/Load systems based on How to make a Save & Load System in Unity - https://youtu.be/aUi9aijvpgs?si=GLtBO4zP_VGItJr-
    private void Awake()
    {
        if (instance != null) 
        {
            //Debug.LogError("More than one Data Persistence Manager in scene");
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            string downloadsPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "Downloads");
            this.dataHandler = new FileDataHandler(downloadsPath, fileName);
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
    public void NewGame()
    {
        this.optionData = new OptionData();
        this.gameData = new GameData();
        Debug.Log("NewGame");
    }

    public void ResetGame()
    {
        this.gameData = new GameData();
        Debug.Log("ResetGame");
    }

    public void ImportGame()
    {
        this.gameData = dataHandler.Load();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        Debug.Log("ImportGame");
    }
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
        dataHandler.Save(gameData);
        Debug.Log("ExportGame");
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

    // For saving options
    public void LoadOption()
    {
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

        Debug.Log("SaveOption");
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllPersistenceObjects();
        LoadGame();
        LoadOption();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
        SaveOption();
    }

    private List<IDataPersistence> FindAllPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
