// === SerializableRoomData.cs ===
using System.Collections.Generic;

[System.Serializable]
public class SerializableRoomData
{
    public List<SerializableKeyValuePair<SerializableVector2, SerializableGameObject>> RoomDictionary0 = new();
    public List<SerializableKeyValuePair<SerializableVector2, float>> AngleDictionary0 = new();
    public List<SerializableKeyValuePair<SerializableVector2, SerializableGameObject>> RoomDictionary1 = new();
    public List<SerializableKeyValuePair<SerializableVector2, float>> AngleDictionary1 = new();

    public SerializableRoomData(RoomData room)
    {
        foreach (var item in room.RoomDictionary0)
            RoomDictionary0.Add(new SerializableKeyValuePair<SerializableVector2, SerializableGameObject>(new SerializableVector2(item.Key), new SerializableGameObject(item.Value)));
        foreach (var item in room.AngleDictionary0)
            AngleDictionary0.Add(new SerializableKeyValuePair<SerializableVector2, float>(new SerializableVector2(item.Key), item.Value));
        foreach (var item in room.RoomDictionary1)
            RoomDictionary1.Add(new SerializableKeyValuePair<SerializableVector2, SerializableGameObject>(new SerializableVector2(item.Key), new SerializableGameObject(item.Value)));
        foreach (var item in room.AngleDictionary1)
            AngleDictionary1.Add(new SerializableKeyValuePair<SerializableVector2, float>(new SerializableVector2(item.Key), item.Value));
    }

    public RoomData ToRoomData()
    {
        var room = new RoomData();
        foreach (var item in RoomDictionary0)
            room.RoomDictionary0[item.key.ToVector2()] = item.value.ToGameObject();
        foreach (var item in AngleDictionary0)
            room.AngleDictionary0[item.key.ToVector2()] = item.value;
        foreach (var item in RoomDictionary1)
            room.RoomDictionary1[item.key.ToVector2()] = item.value.ToGameObject();
        foreach (var item in AngleDictionary1)
            room.AngleDictionary1[item.key.ToVector2()] = item.value;
        return room;
    }
}