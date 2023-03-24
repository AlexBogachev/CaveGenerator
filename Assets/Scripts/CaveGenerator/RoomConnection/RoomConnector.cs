using System.Collections.Generic;
using UnityEngine;

public class RoomConnector
{
    List<Room> rooms = new List<Room>();

    public void HandleRooms()
    {
        Debug.Log("CONNECT ROOMS = " + rooms.Count);
    }

    public void AddRoom(Room room)
        =>rooms.Add(room);

    public void Clear()
        =>rooms.Clear();
}
