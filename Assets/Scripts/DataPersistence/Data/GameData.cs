using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // For saving and loading Game levels
    public string levelName;
    public Dictionary<Vector2, GameObject> RoomDictionary0;
    public Dictionary<Vector2, float> AngleDictionary0;
    public Dictionary<Vector2, GameObject> RoomDictionary1;
    public Dictionary<Vector2, float> AngleDictionary1;
    public List<GameObject> initialObject;

    public GameData()
    {
        this.levelName = "Dungeon";
        this.initialObject = new List<GameObject>();
        this.RoomDictionary0 = new Dictionary<Vector2, GameObject>();
        this.AngleDictionary0 = new Dictionary<Vector2, float>();
        this.RoomDictionary1 = new Dictionary<Vector2, GameObject>();
        this.AngleDictionary1 = new Dictionary<Vector2, float>();
    }
}
