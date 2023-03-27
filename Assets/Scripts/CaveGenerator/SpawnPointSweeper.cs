using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Zenject;

public class SpawnPointSweeper
{
    private GeneratorValues values;

    private List<(int x, int y)> spawnPointsInPixelCoords;
    private List<Vector3> spawnPointsInWorldCoors;

    private int radius;

    public SpawnPointSweeper(GeneratorValues values, [Inject (Id = ZenjectIDs.SPAWN_POINTS)] List<Vector3> spawnPointsInWorldCoors)
    {
        this.values = values;
        this.spawnPointsInWorldCoors = spawnPointsInWorldCoors;
        spawnPointsInPixelCoords = new List<(int x, int y)>();
    }

    public void Sweep(PixelMap map)
    {
        SetValues(map);

        foreach((int x, int y) center in spawnPointsInPixelCoords)
        {
            for(int i = -radius; i <= radius;i++)
                for(int j = -radius; j <= radius; j++)
                {
                    if(i*i + j*j <= radius * radius)
                    {
                        var x = center.x + i;
                        var y = center.y + j;

                        var width = map.Map.GetLength(0);
                        var height = map.Map.GetLength(1);

                        if (GeneratorUtils.IsInMapRange(width, height, x, y))
                        {
                            if(i == 0 && j == 0)
                            {
                                Debug.Log("SWEEPER = " + center.x + " || " + center.y);
                                var pos = GeneratorUtils.MapPositionToWorldPosition((x, y), width, height, values.SquareSize, -values.WallHeight);
                                spawnPointsInWorldCoors.Add(pos);
                            }
                            map.Map[x, y] = 0;
                        }
                    } 
                }
        }
    }

    private void SetValues(PixelMap map)
    {
        radius = Mathf.CeilToInt(Mathf.Sqrt(values.RegionTreshhold) / 2.0f);
        spawnPointsInPixelCoords.Add((radius, Mathf.RoundToInt(map.Map.GetLength(1) / 2.0f)));
        spawnPointsInPixelCoords.Add((map.Map.GetLength(0) - radius, Mathf.RoundToInt(map.Map.GetLength(1) / 2.0f)));
    }
}
