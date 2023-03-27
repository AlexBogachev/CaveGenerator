using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpawnPointSweeper
{
    private GeneratorValues values;

    private List<(int x, int y)> spawnPoints;

    private int radius;

    public SpawnPointSweeper(GeneratorValues values, [Inject (Id = ZenjectIDs.SPAWN_POINTS)] List<(int x, int y)> spawnPoints )
    {
        this.values = values;
        this.spawnPoints = spawnPoints;
    }

    public void Sweep(PixelMap map)
    {
        SetValues(map);

        foreach((int x, int y) center in spawnPoints)
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
                            map.Map[x, y] = 0;
                        }
                    } 
                }
        }
    }

    private void SetValues(PixelMap map)
    {
        radius = Mathf.CeilToInt(Mathf.Sqrt(values.RegionTreshhold) / 2.0f);
        spawnPoints = new List<(int x, int y)>()
        {
            (radius ,Mathf.RoundToInt(map.Map.GetLength(1)/2.0f)),
            (map.Map.GetLength(0) - radius, Mathf.RoundToInt(map.Map.GetLength(1)/2.0f))
        };
    }
}
