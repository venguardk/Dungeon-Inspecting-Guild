using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData_1
{
    public Dictionary<Vector2, GameObject> RoomDictionary0;
    public Dictionary<Vector2, GameObject> RoomDictionary1;

    public LevelData_1()
    {
       this.RoomDictionary0 = new Dictionary<Vector2, GameObject>();
       this.RoomDictionary1 = new Dictionary<Vector2, GameObject>();
    }
}
