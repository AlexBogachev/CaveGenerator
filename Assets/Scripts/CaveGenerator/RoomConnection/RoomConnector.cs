using System.Collections.Generic;
using UnityEngine;

public class RoomConnector
{
    PassageBuilder passageBuilder;

    private GeneratorValues values;

    private List<Room> rooms = new List<Room>();

    public RoomConnector(GeneratorValues values, PassageBuilder passageBuilder)
    {
        this.values = values;
        this.passageBuilder = passageBuilder;
    }

    public void HandleRooms()
    {
        Room closestRoom0 = null;
        Room closestRoom1 = null;
        var closestTile0 = (int.MaxValue, int.MaxValue);
        var closestTile1 = (int.MaxValue, int.MaxValue);

        rooms.Sort();

        foreach(Room r0 in rooms)
        {
            var closestRoomExists = false;
            var minDist = float.MaxValue;

            foreach (Room r1 in rooms)
            {
                if (r0 == r1)
                    continue;

                if (r0.IsConnected(r1))
                    break;

                foreach((int x, int y) tile0 in r0.Tiles)
                    foreach((int x, int y) tile1 in r1.Tiles)
                    {
                        var sqrDistance = Mathf.Pow((tile1.x - tile0.x), 2) + Mathf.Pow((tile1.y - tile0.y), 2);
                        if (sqrDistance < minDist)
                        {
                            minDist = sqrDistance;

                            closestRoom0 = r0;
                            closestRoom1 = r1;
                            closestTile0 = tile0;
                            closestTile1 = tile1;

                            closestRoomExists = true;
                        }
                    }
            }

            if (closestRoomExists)
                AddPassage(closestRoom0, closestTile0, closestRoom1, closestTile1);
        }
    }

    private void AddPassage(Room closestRoom0, (int, int) closestTile0, Room closestRoom1, (int, int) closestTile1)
    {
        Room.SetConnection(closestRoom0, closestRoom1);

        //var t0World = GeneratorUtils.MapPositionToWorldPosition(closestTile0, values.Width, values.Height);
        //var t1World = GeneratorUtils.MapPositionToWorldPosition(closestTile1, values.Width, values.Height);

        //Debug.DrawLine(t0World, t1World, Color.red, 100);

       passageBuilder.CreatePassage(closestTile0, closestTile1);
    }

    public void AddRoom(Room room)
        =>rooms.Add(room);

    public void Clear()
        =>rooms.Clear();
}
