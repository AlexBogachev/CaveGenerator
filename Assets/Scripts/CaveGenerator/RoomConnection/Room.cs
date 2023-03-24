using System.Collections.Generic;

public class Room 
{
    public List<Room> ConnectedRooms { get; private set; }

    private List<(int x, int y)> tiles = new List<(int x, int y)>();
    private List<(int x, int y)> edgeTiles = new List<(int x, int y)>();
    

    public Room(List<(int x, int y)> tiles, int[,] map)
    {
        ConnectedRooms = new List<Room> ();

        this.tiles = tiles;

        foreach((int x, int y) tile in tiles)
        {
            for (int i = tile.x - 1; i <= tile.x + 1; i++)
                for (int j = tile.y - 1; j <= tile.y + 1; j++)
                    if ((i == tile.x || j == tile.y) && map[tile.x, tile.y] == 1)
                        edgeTiles.Add((i, j));
        }
    }

    public static void SetConnection(Room room0, Room room1)
    {
        room0.ConnectedRooms.Add(room1);
        room1.ConnectedRooms.Add(room0);
    }

    public bool IsConnected(Room room)
        =>ConnectedRooms.Contains(room);
}
