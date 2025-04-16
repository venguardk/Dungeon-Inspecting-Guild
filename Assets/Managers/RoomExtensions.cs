using System.Collections.Generic;
using UnityEngine;

public static class RoomExtensions
{
    public static RoomData GetActiveRoom(this Dictionary<string, RoomData> rooms, string roomName)
    {
        if (!rooms.ContainsKey(roomName))
        {
            rooms[roomName] = new RoomData();
        }
        return rooms[roomName];
    }
}
