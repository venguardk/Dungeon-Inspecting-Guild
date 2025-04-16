using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameDataSerialized
{
    // This script is for converting GameData into a format whih can be placed within JSON format
    public string levelName;
    public string sceneName;
    public List<SerializableGameObject> initialObject = new();
    public List<SerializableKeyValuePair<string, SerializableRoomData>> Rooms = new();

    // Converting Dictionary which are deserializable into List which are serializable, with serializable version of GameObject and Vector2 within them
    public GameDataSerialized(GameData gameData)
    {
        levelName = gameData.levelName;
        sceneName = gameData.sceneName;
        foreach (var obj in gameData.initialObject)
            initialObject.Add(new SerializableGameObject(obj));

        foreach (var room in gameData.Rooms)
            Rooms.Add(new SerializableKeyValuePair<string, SerializableRoomData>(room.Key, new SerializableRoomData(room.Value)));
    }

    // Converting List to dictonaries, and converting DeserializableGameObject and DesrializableVector2 to normal GameObject and Vector2
    public GameData ToGameData()
    {
        GameData gameData = new GameData();

        gameData.levelName = levelName;
        gameData.sceneName = sceneName;

        foreach (var item in initialObject)
            gameData.initialObject.Add(item.ToGameObject());

        foreach (var room in Rooms)
            gameData.Rooms[room.key] = room.value.ToRoomData();


        return gameData;
    }
}