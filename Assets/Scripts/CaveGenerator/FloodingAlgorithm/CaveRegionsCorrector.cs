using System.Collections.Generic;
using UnityEngine;

public class CaveRegionsCorrector
{
    private PixelMap map;

    GeneratorValues values;

    public CaveRegionsCorrector(PixelMap map, GeneratorValues values) 
    { 
        this.values = values;
    }

    public void ProcessMap(PixelMap map)
    {
        this.map = map;

        var wallsRegions = GetRegions(1);
        Debug.Log("WR = " + wallsRegions.Count);
        var emptyRegions = GetRegions(0);
        Debug.Log("ER = " + emptyRegions.Count);

        var threshhold = values.RegionTreshhold;

        InverseRegion(wallsRegions, 0);
        InverseRegion(emptyRegions, 1);

        void InverseRegion(List<List<(int x, int y)>> regions, int inversedValue)
        {
            regions.ForEach(x =>
            {
                if (x.Count <= threshhold)
                    x.ForEach(y => map.Map[y.x, y.y] = inversedValue);
            });
        }
    }

    private List<List<(int x, int y)>> GetRegions(int regionType)
    {
        var regions = new List<List<(int x, int y)>>();
        var flags = new int[values.Width, values.Height];

        for(int i = 0; i<values.Width;i++)
            for(int j = 0; j<values.Height; j++)
            {
                if (flags[i,j] == 0 && map.Map[i,j] == regionType)
                {
                    var region = GetRegionTiles(i, j);
                    regions.Add(region);
                    region.ForEach(x => flags[x.x, x.y] = 1);
                }
            }

        return regions;
    }

    public List<(int x, int y)> GetRegionTiles(int firstX, int firstY)
    {
        var tiles = new List<(int x, int y)>();
        var flags = new int[values.Width, values.Height];
        var tileType = map.Map[firstX, firstY];

        var queue  = new Queue<(int x, int y)>();
        queue.Enqueue((firstX, firstY));
        flags[firstX, firstY] = 1;

        while (queue.Count > 0)
        {
            (int x, int y) coord = queue.Dequeue();
            tiles.Add(coord);
            for(int i = coord.x - 1; i <= coord.x + 1; i++)
                for (int j = coord.y - 1; j <= coord.y + 1; j++)
                {
                    if (GeneratorUtils.IsInMapRange(values.Width, values.Height, i, j) && 
                       (i == coord.x || j == coord.y) &&
                       map.Map[i,j] == tileType &&
                       flags[i,j] == 0)
                    {
                        flags[i,j]  = 1;
                        queue.Enqueue((i,j));
                    }
                }
        }

        return tiles;
    }
}
