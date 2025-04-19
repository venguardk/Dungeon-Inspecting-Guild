using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // For saving and loading Game levels
    public string levelName;
    public string sceneName;
    //public Dictionary<Vector2, GameObject> RoomDictionary0;
    //public Dictionary<Vector2, float> AngleDictionary0;
    //public Dictionary<Vector2, GameObject> RoomDictionary1;
    //public Dictionary<Vector2, float> AngleDictionary1;
    public List<GameObject> initialObject = new();
    public Dictionary<string, RoomData> Rooms = new();

    public GameData()
    {
        this.levelName = "Dungeon";
        this.sceneName = "Prototype 3";
        this.initialObject = new List<GameObject>();
        this.Rooms = new Dictionary<string, RoomData>();
        //this.RoomDictionary0 = new Dictionary<Vector2, GameObject>();
        //this.AngleDictionary0 = new Dictionary<Vector2, float>();
        //this.RoomDictionary1 = new Dictionary<Vector2, GameObject>();
        //this.AngleDictionary1 = new Dictionary<Vector2, float>();
    }
}
