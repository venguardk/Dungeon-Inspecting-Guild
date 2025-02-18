using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditorManager : MonoBehaviour, IDataPersistence
{
    //Main update function of this script is based on How to make a Level Editor in Unity - https://youtu.be/eWBDuEWUOwc?si=lxP03a4ICCOSW2Z_
    public static LevelEditorManager instance;
    public AssetController[] assetButtons;
    public GameObject[] assets, assetImages;
    public int currentButtonPressed;
    [SerializeField] private int goldBudget, requiredThreatLevel;
    [SerializeField] private Transform assetParent;
    private int goldSpent, currentThreatLevel;

    public Tilemap Floor;
    public Tilemap Gap;
    private List<GameObject> initialObject;
    private Dictionary<Vector2, GameObject> RoomDictionary0;
    private Dictionary<Vector2, GameObject> RoomDictionary1;
    private Dictionary<Vector2, float> AngleDictionary0;
    private Dictionary<Vector2, float> AngleDictionary1;
    private string[] tagList = { "AddedItem", "Enemy", "Collectible", "Player", "Gap" };

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
    }

    private void Start()
    {
        initialObject = new List<GameObject>();
        RoomDictionary0 = new Dictionary<Vector2, GameObject>();
        RoomDictionary1 = new Dictionary<Vector2, GameObject>();
        AngleDictionary0 = new Dictionary<Vector2, float>();
        AngleDictionary1 = new Dictionary<Vector2, float>();
        var allInitialObjects = new List<GameObject>();
        foreach (string tag in tagList)
        {
            GameObject[] initialObjects = GameObject.FindGameObjectsWithTag(tag);
            allInitialObjects.AddRange(initialObjects);
        }

        if (allInitialObjects.Count > 0)
        {
            foreach (GameObject obj in allInitialObjects)
            {
                initialObject.Add(obj);
                //Separate the initial objects into each dictionary depending on their tags
                Vector2 initialCoordinate = new Vector2(obj.transform.position.x, obj.transform.position.y);
                RoomDictionary0.Add(initialCoordinate, obj);
                AngleDictionary0.Add(initialCoordinate, obj.transform.eulerAngles.z);
            }
        }
    }

    private void Update()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //Updating where the mouse is
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);


        if (Input.GetMouseButtonDown(0) && assetButtons[currentButtonPressed].clicked)
        { //If the left mouse button is clicked, spawn the asset

            Vector2 MouseCoordinate = new Vector2(Mathf.Ceil((worldPosition.x - 0.5f) / 0.96f) * 0.96f + 0.06f, Mathf.Ceil((worldPosition.y - 0.5f) / 0.96f) * 0.96f + 0.34f);
            if (assets[currentButtonPressed].GetComponent<AssetManager>().objType == 0 && RoomDictionary0.ContainsKey(MouseCoordinate) == false && coordinateChecker(MouseCoordinate, Floor) == true && coordinateChecker(MouseCoordinate, Gap) == false)
            {
                float rotation = GameObject.FindGameObjectWithTag("AssetImage").transform.rotation.eulerAngles.z; //Acquiring rotation from asset
                //Setting the asset so that it will be located in a grid position
                GameObject AddedObject = Instantiate(assets[currentButtonPressed], new Vector3(Mathf.Ceil((worldPosition.x - 0.5f) / 0.96f) * 0.96f + 0.06f, Mathf.Ceil((worldPosition.y - 0.5f) / 0.96f) * 0.96f + 0.34f, 0), Quaternion.Euler(0, 0, rotation)); //Spawn the asset at the mouse position
                RoomDictionary0.Add(MouseCoordinate, assets[currentButtonPressed]);
                AngleDictionary0.Add(MouseCoordinate, rotation);
            }
            else if (assets[currentButtonPressed].GetComponent<AssetManager>().objType == 1 && RoomDictionary1.ContainsKey(MouseCoordinate) == false && coordinateChecker(MouseCoordinate, Gap) == true)
            {
                float rotation = GameObject.FindGameObjectWithTag("AssetImage").transform.rotation.eulerAngles.z; //Acquiring rotation from asset
                //Setting the asset so that it will be located in a grid position
                GameObject AddedObject = Instantiate(assets[currentButtonPressed], new Vector3(Mathf.Ceil((worldPosition.x - 0.5f) / 0.96f) * 0.96f + 0.06f, Mathf.Ceil((worldPosition.y - 0.5f) / 0.96f) * 0.96f + 0.34f, 0), Quaternion.Euler(0, 0, rotation)); //Spawn the asset at the mouse position
                RoomDictionary1.Add(MouseCoordinate, assets[currentButtonPressed]);
                AngleDictionary1.Add(MouseCoordinate, rotation);
            }
        }
        else if (Input.GetMouseButtonDown(1) && assetButtons[currentButtonPressed].clicked)
        { //If the right mouse button is clicked, cancel addition
            assetButtons[currentButtonPressed].clicked = false;
            Destroy(GameObject.FindGameObjectWithTag("AssetImage"));
        }

        //For Debug
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (KeyValuePair<Vector2, GameObject> items in RoomDictionary0)
            {
                Debug.Log(items.Key + " " + items.Value + " " + AngleDictionary0[items.Key]);
            }
            foreach (KeyValuePair<Vector2, GameObject> items in RoomDictionary1)
            {
                Debug.Log(items.Key + " " + items.Value + " " + AngleDictionary1[items.Key]);
            }
        }
    }

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

    public void RemoveAsset(Vector2 position, int objType)
    {
        if (objType == 0)
        {
            RoomDictionary0.Remove(position);
            AngleDictionary0.Remove(position);
        }
        else if (objType == 1)
        {
            RoomDictionary1.Remove(position);
            AngleDictionary1.Remove(position);
        }
    }

    public void DeactivateButton()
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
        foreach (string tag in tagList)
        {
            GameObject[] initialObjects = GameObject.FindGameObjectsWithTag(tag);
            allObjects.AddRange(initialObjects);
        }
        foreach (GameObject obj in allObjects)
        {
            //if obj has goldCost or threatLevel, remove that
            if (!initialObject.Contains(obj))
            {
                AssetManager assetManager = obj.GetComponent<AssetManager>();
                if (assetManager != null)
                {
                    MinusGold(assetManager.goldCost);
                    MinusThreatLevel(assetManager.threatLevel);
                }
                Destroy(obj);
            }
        }
        foreach (KeyValuePair<Vector2, GameObject> loaded in RoomDictionary0)
        {
            if (initialObject.Contains(loaded.Value))
            {
                loaded.Value.transform.position = loaded.Key;
            }
            else
            {
                Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, AngleDictionary0[loaded.Key])).SetActive(true);
            }


        }
        foreach (KeyValuePair<Vector2, GameObject> loaded in RoomDictionary1)
        {
            if (initialObject.Contains(loaded.Value))
            {
                loaded.Value.transform.position = loaded.Key;
            }
            else
            {
                Instantiate(loaded.Value, new Vector3(loaded.Key.x, loaded.Key.y, 0), Quaternion.Euler(0, 0, AngleDictionary1[loaded.Key])).SetActive(true);
            }

        }
    }

    public void LoadData(GameData data)
    {
        this.RoomDictionary0.Clear();
        this.AngleDictionary0.Clear();
        foreach (KeyValuePair<Vector2, GameObject> items in data.RoomDictionary0)
        {
            this.RoomDictionary0.Add(items.Key, items.Value);
            this.AngleDictionary0.Add(items.Key, data.AngleDictionary0[items.Key]);
        }

        this.RoomDictionary1.Clear();
        this.AngleDictionary1.Clear();
        foreach (KeyValuePair<Vector2, GameObject> items in data.RoomDictionary1)
        {
            this.RoomDictionary1.Add(items.Key, items.Value);
            Debug.Log(items.Key + ", " + items.Value);
            this.AngleDictionary1.Add(items.Key, data.AngleDictionary1[items.Key]);
            
        }
    }

    public void SaveData(ref GameData data)
    {
        data.RoomDictionary0.Clear();
        data.AngleDictionary0.Clear();
        foreach (KeyValuePair<Vector2, GameObject> items in this.RoomDictionary0)
        {
            data.RoomDictionary0.Add(items.Key, items.Value);
            data.AngleDictionary0.Add(items.Key, this.AngleDictionary0[items.Key]);
        }

        data.RoomDictionary1.Clear();
        data.AngleDictionary1.Clear();
        foreach (KeyValuePair<Vector2, GameObject> items in this.RoomDictionary1)
        {
            data.RoomDictionary1.Add(items.Key, items.Value);
            data.AngleDictionary1.Add(items.Key, this.AngleDictionary1[items.Key]);
        }
    }

    public void AddObject(GameObject added, Vector3 addedCoordinate, float rotation, int objType)
    {
        Vector2 addedCoordinate2d = new Vector2(addedCoordinate.x, addedCoordinate.y);
        if (objType == 0 && RoomDictionary0.ContainsKey(addedCoordinate2d) == false)
        {
            RoomDictionary0.Add(addedCoordinate2d, added);
            AngleDictionary0.Add(addedCoordinate2d, rotation);
        }
        else if (objType == 1 && RoomDictionary1.ContainsKey(addedCoordinate2d) == false)
        {
            RoomDictionary1.Add(addedCoordinate2d, added);
            AngleDictionary1.Add(addedCoordinate2d, rotation);
        }


    }

    private bool coordinateChecker(Vector2 coordinate, Tilemap tilemap)
    {
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

    public bool LevelValuesMet()
    {
        return goldSpent <= goldBudget && currentThreatLevel >= requiredThreatLevel;
    }
}
