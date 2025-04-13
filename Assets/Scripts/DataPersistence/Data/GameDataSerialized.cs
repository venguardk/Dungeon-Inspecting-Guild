using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameDataSerialized
{
    // This script is for converting GameData into a format whih can be placed within JSON format
    public string levelName;
    public List<SerializableGameObject> initialObject;
    public List<SerializableKeyValuePair<SerializableVector2, SerializableGameObject>> RoomDictionary0;
    public List<SerializableKeyValuePair<SerializableVector2, float>> AngleDictionary0;
    public List<SerializableKeyValuePair<SerializableVector2, SerializableGameObject>> RoomDictionary1;
    public List<SerializableKeyValuePair<SerializableVector2, float>> AngleDictionary1;

    // Converting Dictionary which are deserializable into List which are serializable, with serializable version of GameObject and Vector2 within them
    public GameDataSerialized(GameData gameData)
    {
        levelName = gameData.levelName;
        initialObject = new List<SerializableGameObject>();
        foreach (GameObject obj in gameData.initialObject)
        {
            initialObject.Add(new SerializableGameObject(obj));
        }

        RoomDictionary0 = new List<SerializableKeyValuePair<SerializableVector2, SerializableGameObject>>();
        foreach (var item in gameData.RoomDictionary0)
        {
            RoomDictionary0.Add(new SerializableKeyValuePair<SerializableVector2, SerializableGameObject>(
                new SerializableVector2(item.Key),
                new SerializableGameObject(item.Value)
            ));
        }

        AngleDictionary0 = new List<SerializableKeyValuePair<SerializableVector2, float>>();
        foreach (var item in gameData.AngleDictionary0)
        {
            AngleDictionary0.Add(new SerializableKeyValuePair<SerializableVector2, float>(new SerializableVector2(item.Key), item.Value));
        }

        RoomDictionary1 = new List<SerializableKeyValuePair<SerializableVector2, SerializableGameObject>>();
        foreach (var item in gameData.RoomDictionary1)
        {
            RoomDictionary1.Add(new SerializableKeyValuePair<SerializableVector2, SerializableGameObject>(
                new SerializableVector2(item.Key),
                new SerializableGameObject(item.Value)
            ));
        }

        AngleDictionary1 = new List<SerializableKeyValuePair<SerializableVector2, float>>();
        foreach (var item in gameData.AngleDictionary1)
        {
            AngleDictionary1.Add(new SerializableKeyValuePair<SerializableVector2, float>(new SerializableVector2(item.Key), item.Value));
        }
    }

    // Converting List to dictonaries, and converting DeserializableGameObject and DesrializableVector2 to normal GameObject and Vector2
    public GameData ToGameData()
    {
        GameData gameData = new GameData();

        gameData.levelName = levelName;

        gameData.initialObject = new List<GameObject>();
        foreach (var item in initialObject)
        {
            gameData.initialObject.Add(item.ToGameObject());
        }

        gameData.RoomDictionary0 = new Dictionary<Vector2, GameObject>();
        foreach (var item in RoomDictionary0)
        {
            gameData.RoomDictionary0.Add(item.key.ToVector2(), item.value.ToGameObject());
        }

        gameData.AngleDictionary0 = new Dictionary<Vector2, float>();
        foreach (var item in AngleDictionary0)
        {
            gameData.AngleDictionary0.Add(item.key.ToVector2(), item.value);
        }

        gameData.RoomDictionary1 = new Dictionary<Vector2, GameObject>();
        foreach (var item in RoomDictionary1)
        {
            gameData.RoomDictionary1.Add(item.key.ToVector2(), item.value.ToGameObject());
        }

        gameData.AngleDictionary1 = new Dictionary<Vector2, float>();
        foreach (var item in AngleDictionary1)
        {
            gameData.AngleDictionary1.Add(item.key.ToVector2(), item.value);
        }

        return gameData;
    }
}