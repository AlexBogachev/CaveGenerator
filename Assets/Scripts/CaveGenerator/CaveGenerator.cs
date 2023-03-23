using UniRx;
using UnityEngine;

public class CaveGenerator
{
    GeneratorValues values;

    public CaveGenerator(PixelMap map, CaveRegionsCorrector regions,SquareGrid squareGrid, CaveMesh formMesh, WallsMesh wallsMesh,GeneratorValues values)
    {;
        this.values = values;
        map.UpdateMap(values.Width, values.Height);
        GenerateMap(map, regions);
        squareGrid.UpdateSquareGrid(map.Map);
        formMesh.UpdateMesh();
        wallsMesh.UpdateMesh();

        UserInput.OnClick
            .Subscribe(x =>
            {
                map.UpdateMap(values.Width, values.Height);
                GenerateMap(map, regions);
                squareGrid.UpdateSquareGrid(map.Map);
                formMesh.UpdateMesh();
                wallsMesh.UpdateMesh();
            });
    }

    private void GenerateMap(PixelMap m, CaveRegionsCorrector regions)
    {
        var hash = Time.deltaTime.ToString().GetHashCode();
        var random = new System.Random(hash);

        var width = values.Width;
        var height = values.Height;

        var mapArray = m.Map;

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                    mapArray[i, j] = 1;
                else
                    mapArray[i, j] = random.Next(0, 100) < values.FillPercent ? 1:0 ;
            }

        for (int k = 0; k < values.SmoothRate; k++)
            SmoothMap(m);

        regions.ProcessMap(m);

        m.SetNewMap(GetMapWithBorder(mapArray));
    }

    private void SmoothMap(PixelMap m)
    {
        var width = values.Width;
        var height = values.Height;

        var mapArray = m.Map;

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                int surroundedWalls = GetSurroundedWalls(i,j);
                if (surroundedWalls < 4)
                    mapArray[i, j] = 0;
                else if (surroundedWalls > 4)
                    mapArray[i, j] = 1;
            }


        int GetSurroundedWalls(int mapX, int mapY)
        {
            int walls = 0;
            int prevX = mapX - 1;
            int nextX = mapX + 1;
            int prevY = mapY - 1;
            int nextY = mapY + 1;
            for (int x = prevX; x <= nextX; x++)
                for (int y = prevY; y <= nextY; y++)
                {
                    if (x == mapX && y == mapY)
                        continue;
                    if (prevX < 0 || prevY < 0 || nextX >= width || nextY >= height)
                        walls++;
                    else
                    {
                        walls += mapArray[x, y];
                    }
                }
            return walls;
        }
    }

    private int[,] GetMapWithBorder(int[,] mapArray)
    {
        var width = mapArray.GetLength(0);
        var height = mapArray.GetLength(1);

        int borderSize = values.BorderSize;
        if (borderSize > 0)
        {
            var borderMap = new int[mapArray.GetLength(0) + borderSize * 2, mapArray.GetLength(1) + borderSize * 2];
            for (int i = 0; i < borderMap.GetLength(0); i++)
                for (int j = 0; j < borderMap.GetLength(1); j++)
                {
                    if (i >= borderSize && j >= borderSize && i < width + borderSize && j < height + borderSize)
                    {
                        borderMap[i, j] = mapArray[i - borderSize, j - borderSize];
                    }
                    else
                        borderMap[i, j] = 1;
                }
            return borderMap;
        }
        return mapArray;
    }
}

public class GeneratorUtils
{
    public static bool IsInMapRange(int width, int height, int x, int y)
        => x >= 0 && y >= 0 && x < width && y < height;
}
