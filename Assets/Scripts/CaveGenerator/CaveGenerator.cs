using UniRx;
using UnityEngine;

public class CaveGenerator
{
    GeneratorValues values;

    public CaveGenerator(PixelMap map, SquareGrid squareGrid, GeneratorValues values)
    {;
        this.values = values;
        map.UpdateMap(values.Width, values.Height);
        GenerateMap(map);
        squareGrid.UpdateSquareGrid(map.Map);

        UserInput.OnClick
            .Subscribe(x =>
            {
                GenerateMap(map);
                squareGrid.UpdateSquareGrid(map.Map);
            });
    }

    private void GenerateMap(PixelMap m)
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
}
