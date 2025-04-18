using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelEditorManager : MonoBehaviour, IDataPersistence
{
    //Main update function of this script is based on How to make a Level Editor in Unity - https://youtu.be/eWBDuEWUOwc?si=lxP03a4ICCOSW2Z_
    //This script handles the level editor and all the assets in it, as well as keeps track of the level's stats and layout
    //Other scripts this script interacts with: AssetManager, GameManager, AudioManager, CameraManager, PlayerManager, DataPersistenceManager

    public static LevelEditorManager instance;
    public AssetController[] assetButtons;
    public GameObject[] assets, assetImages;
    public int currentButtonPressed;
    [SerializeField] private int goldBudget, requiredThreatLevel;
    [SerializeField] private int requiredDartShooters, requiredSpikeTraps, requiredFlamethrowers, requiredEnemies;
    private int goldSpent, currentThreatLevel, currentDartShooters, currentSpikeTraps, currentFlamethrowers, currentEnemies;
    private bool isDragging = false;
    private Vector2 previousMousePosition = Vector2.zero;
    private string activeRoomName = "defaultRoom";

    public Tilemap Floor;
    public Tilemap Gap;
    public GameObject[] allAsset;
    public GameObject[] initialAsset;
    public Transform[] initialBase;
    private List<GameObject> initialObject = new List<GameObject>();
    private List<GameObject> deactivatedObject = new List<GameObject>();
    //private Dictionary<Vector2, GameObject> currentRoom.RoomDictionary0 = new Dictionary<Vector2, GameObject>();
    //private Dictionary<Vector2, GameObject> currentRoom.RoomDictionary1 = new Dictionary<Vector2, GameObject>();
    //private Dictionary<Vector2, float> currentRoom.AngleDictionary0 = new Dictionary<Vector2, float>();
    //private Dictionary<Vector2, float> currentRoom.AngleDictionary1 = new Dictionary<Vector2, float>();
    private Dictionary<string, RoomData> rooms = new();
    private RoomData currentRoom => rooms.GetActiveRoom(activeRoomName); //Need to check the extension code
    private string[] tagList = { "AddedItem", "Enemy", "Collectible", "Player", "Gap" };
    private string previousRoomName;
    private bool hasPreviousRoom = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        if (SceneLoadManager.sceneMovement == SceneManager.GetActiveScene().name || SceneLoadManager.sceneMovement == "" || SceneLoadManager.sceneMovement == "MainMenu")
        {

            DataPersistenceManager.instance.ResetGame();
        }
    }

    void Start()
    {

        StartCoroutine(RunAfterStart());
    }

    private IEnumerator RunAfterStart()
    {
        yield return new WaitForEndOfFrame();
        if (SceneLoadManager.sceneMovement == SceneManager.GetActiveScene().name || SceneLoadManager.sceneMovement == "" || SceneLoadManager.sceneMovement == "MainMenu")
        {
            LevelSave();
        }
        LevelLoad();
    }

    private void Update()
    {
        //This function handles the placement of assets in the level editor
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //Updating where the mouse is
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        Vector2 mouseCoordinate = new Vector2(Mathf.Ceil(worldPosition.x - 0.75f) + 0.5f, Mathf.Ceil(worldPosition.y - 0.75f) + 0.5f);

        if (Input.GetMouseButtonDown(0) && assetButtons[currentButtonPressed].clicked)
        { //If the left mouse button is clicked, spawn the asset
            isDragging = true;
            previousMousePosition = mouseCoordinate;
            GenerateAssetAt(mouseCoordinate);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

        }

        if (isDragging && mouseCoordinate != previousMousePosition) //If mouse is being clicked and dragged, continue to spawn the asset
        {
            previousMousePosition = mouseCoordinate;
            GenerateAssetAt(mouseCoordinate);
        }
        else if (Input.GetMouseButtonDown(1) && assetButtons[currentButtonPressed].clicked)
        { //If the right mouse button is clicked, cancel addition
            assetButtons[currentButtonPressed].clicked = false;
            Destroy(GameObject.FindGameObjectWithTag("AssetImage"));
        }

        if (Input.GetKeyDown(KeyCode.U)) CreateRoom("testRoom");
        if (Input.GetKeyDown(KeyCode.I)) SwitchRoom("testRoom");
        if (Input.GetKeyDown(KeyCode.O)) PrintRoomSummary();
        if (Input.GetKeyDown(KeyCode.P)) ValidateAllRooms();
        if (Input.GetKeyDown(KeyCode.B)) // B for "Back"
        {
            SwitchToPreviousRoom();
        }
    }

    private void GenerateAssetAt(Vector2 MouseCoordinate) //Add asset to current mouse position and adding to dictionary
    {

        if (assets[currentButtonPressed].GetComponent<AssetManager>().objType == 0 && currentRoom.RoomDictionary0.ContainsKey(MouseCoordinate) == false && CoordinateChecker(MouseCoordinate, Floor) == true && CoordinateChecker(MouseCoordinate, Gap) == false)
        {
            float rotation = GameObject.FindGameObjectWithTag("AssetImage").transform.rotation.eulerAngles.z; //Acquiring rotation from asset
            //Setting the asset so that it will be located in a grid position
            GameObject AddedObject = Instantiate(assets[currentButtonPressed], new Vector3(MouseCoordinate.x, MouseCoordinate.y, 0), Quaternion.Euler(0, 0, rotation)); //Spawn the asset at the mouse position

            currentRoom.RoomDictionary0.Add(MouseCoordinate, assets[currentButtonPressed]);
            currentRoom.AngleDictionary0.Add(MouseCoordinate, rotation);
        }
        else if (assets[currentButtonPressed].GetComponent<AssetManager>().objType == 1 && currentRoom.RoomDictionary1.ContainsKey(MouseCoordinate) == false && CoordinateChecker(MouseCoordinate, Gap) == true)
        {
            float rotation = GameObject.FindGameObjectWithTag("AssetImage").transform.rotation.eulerAngles.z; //Acquiring rotation from asset
            //Setting the asset so that it will be located in a grid position
            GameObject AddedObject = Instantiate(assets[currentButtonPressed], new Vector3(MouseCoordinate.x, MouseCoordinate.y, 0), Quaternion.Euler(0, 0, rotation)); //Spawn the asset at the mouse position
            currentRoom.RoomDictionary1.Add(MouseCoordinate, assets[currentButtonPressed]);
            currentRoom.AngleDictionary1.Add(MouseCoordinate, rotation);
        }
    }

    //HELPER FUNCTIONS FOR LEVEL EDITOR STATS
    public void AddGold(int gold)
    {
        goldSpent += gold;
    }

    public void MinusGold(int gold)
    {
        goldSpent -= gold;
    }

    public void AddThreatLevel(int threatLevel)
    {
        currentThreatLevel += threatLevel;
    }

    public void MinusThreatLevel(int threatLevel)
    {
        currentThreatLevel -= threatLevel;
    }

    public void AddThreatAssetCount(int assetID)
    {
        switch (assetID)
        {
            case 1: //Dartshooter
                currentDartShooters++;
                break;
            case 2: //Spikes
                currentSpikeTraps++;
                break;
            case 3: //Flamethrowers
                currentFlamethrowers++;
                break;
            case 4: //Enemies
                currentEnemies++;
                break;
            default:
                break;
        }
    }

    public void MinusThreatAssetCount(int assetID)
    {
        switch (assetID)
        {
            case 1: //Dartshooter
                currentDartShooters--;
                break;
            case 2: //Spikes
                currentSpikeTraps--;
                break;
            case 3: //Flamethrowers
                currentFlamethrowers--;
                break;
            case 4: //Enemies
                currentEnemies--;
                break;
            default:
                break;
        }
    }

    private void ResetCurrentThreatAssetsCount()
    {
        currentDartShooters = 0;
        currentSpikeTraps = 0;
        currentFlamethrowers = 0;
        currentEnemies = 0;
    }

    public void RemoveAsset(Vector2 position, int objType)
    {
        if (objType == 0)
        {
            foreach (KeyValuePair<Vector2, GameObject> items in currentRoom.RoomDictionary0)
            {
                Debug.Log(items.Key + " " + items.Value + " " + currentRoom.AngleDictionary0[items.Key]);
                if (items.Key == position)
                {
                    Debug.Log(items.Key == position);
                    Debug.Log(currentRoom.RoomDictionary0[position]);
                }

            }

            currentRoom.RoomDictionary0.Remove(position);
            currentRoom.AngleDictionary0.Remove(position);
        }
        else if (objType == 1)
        {
            currentRoom.RoomDictionary1.Remove(position);
            currentRoom.AngleDictionary1.Remove(position);
        }
    }

    public void DeactivateButton() //This function is called when the player clicks on the asset button in the level editor
    {
        assetButtons[currentButtonPressed].clicked = false;
        Destroy(GameObject.FindGameObjectWithTag("AssetImage"));
    }


    //FOR SAVING AND LOADING
    public void LevelSave()
    {
        DataPersistenceManager.instance.SaveGame();
    }

    public void LevelLoad()
    {
        DataPersistenceManager.instance.LoadGame();
        var allObjects = new List<GameObject>();
        foreach (GameObject activate in deactivatedObject)
        {
            activate.SetActive(true);
        }
        deactivatedObject.Clear();
        foreach (string tag in tagList)
        {
            GameObject[] sceneObjects = GameObject.FindGameObjectsWithTag(tag);
            allObjects.AddRange(sceneObjects);
        }
        foreach (GameObject obj in allObjects)
        {
            //if obj has goldCost or threatLevel, remove that
            if (obj.transform.parent == null)
            {
                AssetManager assetManager = obj.GetComponent<AssetManager>();
                if (assetManager != null)
                {
                    MinusGold(assetManager.goldCost);
                    MinusThreatLevel(assetManager.threatLevel);
                }
            }
            if (gameObject.CompareTag(tagList[3]))
            {
                PlayerManager playerManager = obj.GetComponent<PlayerManager>();
                playerManager.DestroyPlayer();
            }
            Destroy(obj);

        }
        foreach (KeyValuePair<Vector2, GameObject> loaded in currentRoom.RoomDictionary0)
        {
            //Debug.Log(loaded.Value.name);
            if (initialObject.Contains(loaded.Value))
            {
                if (loaded.Value == initialAsset[0])
                {

                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[0]);
                }
                else if (loaded.Value == initialAsset[1])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[1]);
                }
                else if (loaded.Value == initialAsset[2])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[2]);
                }
                else if (loaded.Value == initialAsset[3])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[3]);
                }
                else if (loaded.Value == initialAsset[4])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[4]);
                }
                else if (loaded.Value == initialAsset[5])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[5]);
                }
                else if (loaded.Value == initialAsset[6])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[6]);
                }
                else if (loaded.Value == initialAsset[7])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[7]);
                }
                else if (loaded.Value == initialAsset[8])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[8]);
                }
                else if (loaded.Value == initialAsset[9])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[9]);
                }
                else if (loaded.Value == initialAsset[10])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[10]);
                }
                else if (loaded.Value == initialAsset[11])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[11]);
                }
                else if (loaded.Value == initialAsset[12])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[12]);
                }

            }
            else
            {
                Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key])).SetActive(true);
            }


        }
        foreach (KeyValuePair<Vector2, GameObject> loaded in currentRoom.RoomDictionary1)
        {
            if (initialObject.Contains(loaded.Value))
            {
                if (loaded.Value == initialAsset[0])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[0]);
                }
                else if (loaded.Value == initialAsset[1])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[1]);
                }
                else if (loaded.Value == initialAsset[2])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[2]);
                }
                else if (loaded.Value == initialAsset[3])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[3]);
                }
                else if (loaded.Value == initialAsset[4])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[4]);
                }
                else if (loaded.Value == initialAsset[5])
                {
                    Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary0[loaded.Key]), initialBase[5]);
                }
            }
            else
            {
                Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, currentRoom.AngleDictionary1[loaded.Key])).SetActive(true);
            }
        }
    }

    //public void LoadData(GameData data)
    //{
    //    if (data.sceneName != SceneManager.GetActiveScene().name)
    //    {
    //        //SceneManager.LoadScene(data.sceneName);
    //    }
    //    ResetCurrentThreatAssetsCount(); //Resetting the current threat assets count
    //    this.initialObject.Clear();
    //    foreach (GameObject items in data.initialObject)
    //    {
    //        this.initialObject.Add(items);
    //    }
    //    this.currentRoom.RoomDictionary0.Clear();
    //    this.currentRoom.AngleDictionary0.Clear();
    //    foreach (KeyValuePair<Vector2, GameObject> items in data.currentRoom.RoomDictionary0)
    //    {
    //        this.currentRoom.RoomDictionary0.Add(items.Key, items.Value);
    //        this.currentRoom.AngleDictionary0.Add(items.Key, data.currentRoom.AngleDictionary0[items.Key]);
    //    }

    //    this.currentRoom.RoomDictionary1.Clear();
    //    this.currentRoom.AngleDictionary1.Clear();
    //    foreach (KeyValuePair<Vector2, GameObject> items in data.currentRoom.RoomDictionary1)
    //    {
    //        this.currentRoom.RoomDictionary1.Add(items.Key, items.Value);
    //        this.currentRoom.AngleDictionary1.Add(items.Key, data.currentRoom.AngleDictionary1[items.Key]);
    //    }
    //}
    public void LoadData(GameData data)
    {
        if (data.sceneName != SceneManager.GetActiveScene().name) return;

        ResetCurrentThreatAssetsCount();
        initialObject = new List<GameObject>(data.initialObject);
        rooms = data.Rooms;
    }

    //public void SaveData(ref GameData data)
    //{
    //    data.sceneName = SceneManager.GetActiveScene().name;
    //    data.initialObject.Clear();
    //    foreach (GameObject items in this.initialObject)
    //    {
    //        data.initialObject.Add(items);
    //    }
    //    data.currentRoom.RoomDictionary0.Clear();
    //    data.currentRoom.AngleDictionary0.Clear();
    //    foreach (KeyValuePair<Vector2, GameObject> items in this.currentRoom.RoomDictionary0)
    //    {
    //        data.currentRoom.RoomDictionary0.Add(items.Key, items.Value);
    //        data.currentRoom.AngleDictionary0.Add(items.Key, this.currentRoom.AngleDictionary0[items.Key]);
    //    }

    //    data.currentRoom.RoomDictionary1.Clear();
    //    data.currentRoom.AngleDictionary1.Clear();
    //    foreach (KeyValuePair<Vector2, GameObject> items in this.currentRoom.RoomDictionary1)
    //    {
    //        data.currentRoom.RoomDictionary1.Add(items.Key, items.Value);
    //        data.currentRoom.AngleDictionary1.Add(items.Key, this.currentRoom.AngleDictionary1[items.Key]);
    //    }
    //}
    public void SaveData(ref GameData data)
    {
        data.sceneName = SceneManager.GetActiveScene().name;
        data.initialObject = new List<GameObject>(initialObject);
        data.Rooms = rooms;
    }


    public void SaveOption(ref OptionData optionData)
    {
        return;
    }

    public void LoadOption(OptionData optionData)
    {
        return;
    }

    public void AddObject(GameObject added, Vector3 addedCoordinate, float rotation, int objType, bool initial)
    {
        Vector2 addedCoordinate2d = new Vector2(addedCoordinate.x, addedCoordinate.y);
        if (objType == 0 && currentRoom.RoomDictionary0.ContainsKey(addedCoordinate2d) == false)
        {
            currentRoom.RoomDictionary0.Add(addedCoordinate2d, added);
            currentRoom.AngleDictionary0.Add(addedCoordinate2d, rotation);

        }
        else if (objType == 1 && currentRoom.RoomDictionary1.ContainsKey(addedCoordinate2d) == false)
        {
            currentRoom.RoomDictionary1.Add(addedCoordinate2d, added);
            currentRoom.AngleDictionary1.Add(addedCoordinate2d, rotation);
        }
        if (initial == true)
        {
            initialObject.Add(added);
        }
    }

    public void DeacitaveObj(GameObject obj)
    {
        deactivatedObject.Add(obj);
    }

    private bool CoordinateChecker(Vector2 coordinate, Tilemap tilemap)
    {
        if (tilemap == null)
        {
            return false;
        }
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(coordinate);

        if (screenPosition.x >= 0 && screenPosition.x <= Screen.width && screenPosition.y >= 0 && screenPosition.y <= Screen.height)
        {
            Vector3Int gridPosition = tilemap.WorldToCell(coordinate);

            if (tilemap.HasTile(gridPosition))
            {
                return true;
            }
        }

        return false;
    }

    public bool FloorChecker(Vector2 coordinate)
    {
        return CoordinateChecker(coordinate, Floor);
    }
    public bool GapChecker(Vector2 coordinate)
    {
        return CoordinateChecker(coordinate, Gap);
    }

    public GameObject gameObjectConvertion(string name)
    {
        return allAsset.FirstOrDefault(obj => obj.name == name);
    }

    //FOR UI MANAGER
    public int GetGoldBudget()
    {
        return goldBudget;
    }
    public int GetRequiredThreatLevel()
    {
        return requiredThreatLevel;
    }

    public int GetGoldRemaining()
    {
        return goldBudget - goldSpent;
    }

    public int GetCurrentThreatLevel()
    {
        return currentThreatLevel;
    }

    public int GetDartsCount()
    {
        return currentDartShooters;
    }

    public int GetDartsRequirement()
    {
        return requiredDartShooters;
    }

    public int GetSpikesCount()
    {
        return currentSpikeTraps;
    }

    public int GetSpikesRequirement()
    {
        return requiredSpikeTraps;
    }

    public int GetFlamethrowersCount()
    {
        return currentFlamethrowers;
    }

    public int GetFlamethrowersRequirement()
    {
        return requiredFlamethrowers;
    }

    public int GetEnemiesCount()
    {
        return currentEnemies;
    }

    public int GetEnemiesRequirement()
    {
        return requiredEnemies;
    }

    public bool LevelValuesMet()
    {
        return goldSpent <= goldBudget
        && currentThreatLevel >= requiredThreatLevel
        && currentDartShooters >= requiredDartShooters
        && currentSpikeTraps >= requiredSpikeTraps
        && currentFlamethrowers >= requiredFlamethrowers
        && currentEnemies >= requiredEnemies;
    }

    #region Multi-Room Support

    public void CreateRoom(string roomName)
    {
        if (!rooms.ContainsKey(roomName))
        {
            rooms[roomName] = new RoomData();
            Debug.Log($"Created new room: {roomName}");
        }
        else
        {
            Debug.LogWarning($"Room {roomName} already exists.");
        }
        activeRoomName = roomName;
        LevelLoad();
    }

    public void SwitchToPreviousRoom()
    {
        if (hasPreviousRoom)
        {
            // Store current room before switching
            string currentBeforeSwitch = activeRoomName;

            // Switch to previous room
            SwitchRoom(previousRoomName);

            // Update previous room to be the one we just left
            previousRoomName = currentBeforeSwitch;

            Debug.Log($"Returned to previous room: {activeRoomName}");
        }
        else
        {
            Debug.LogWarning("No previous room available");
        }
    }

    // Modified SwitchRoom to track history
    public void SwitchRoom(string roomName)
    {
        if (rooms.ContainsKey(roomName))
        {
            // Only store history if we're actually changing rooms
            if (roomName != activeRoomName)
            {
                previousRoomName = activeRoomName;
                hasPreviousRoom = true;
            }

            activeRoomName = roomName;
            Debug.Log($"Switched to room: {roomName}");
            LevelLoad();
        }
        else
        {
            Debug.LogError($"Room '{roomName}' not found.");
        }
    }

    // Validate rooms and print result
    public bool ValidateAllRooms()
    {
        foreach (var kv in rooms)
        {
            var room = kv.Value;
            if (room == null ||
                room.RoomDictionary0 == null || room.AngleDictionary0 == null ||
                room.RoomDictionary1 == null || room.AngleDictionary1 == null)
            {
                Debug.LogError($"Room '{kv.Key}' is invalid.");
                return false;
            }
        }
        Debug.Log("All rooms are valid.");
        return true;
    }

    // Log contents of all rooms
    public void PrintRoomSummary()
    {
        foreach (var kv in rooms)
        {
            Debug.Log($"Room: {kv.Key} | Obj0: {kv.Value.RoomDictionary0.Count}, Obj1: {kv.Value.RoomDictionary1.Count}");
        }
    }

    public string GetActiveRoomName() => activeRoomName;

    #endregion

}
