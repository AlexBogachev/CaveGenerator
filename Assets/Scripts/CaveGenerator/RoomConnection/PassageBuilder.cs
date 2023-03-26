using Codice.Client.BaseCommands;
using System.Collections.Generic;
using UnityEngine;

public class PassageBuilder
{
    private PixelMap map;

    private GeneratorValues values;

    public PassageBuilder(PixelMap map, GeneratorValues values)
    {
        this.map = map;
        this.values = values;
    }

    public List<(int x, int y)> CreatePassage((int x, int y) from, (int x, int y) to)
    {
        var passageLine = new List<(int x, int y)>();

        float x = from.x;
        float y = from.y;

        var dx = to.x - x;
        var dy = to.y - y;

        var step = Mathf.Sign(dx);
        var gradientStep = Mathf.Sign(dy);

        var inverted = false;
        var longest = Mathf.Abs(dx);
        var shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Mathf.Sign(dy);
            gradientStep = Mathf.Sign(dx);
        }

        var gradientAccumulation = longest / 2;
        for(int i = 0; i < longest; i++)
        {
            //passageLine.Add(((int)x, (int)y));
            var tile = ((int)x, (int)y);
            AddThicknessToPassage(tile, passageLine);

            if (inverted)
                y += step;
            else
                x += step;

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest)
            {
                if (inverted)
                    x += gradientStep;
                else
                    y += gradientStep;

                gradientAccumulation -= longest;
            }
        }
        return passageLine;
    }

    public void AddThicknessToPassage((int x, int y) tile, List<(int x, int y)> passage)
    {
        var radius = values.PassageRadius;
        for(int i = -radius; i<=radius;i++)
            for(int j = -radius; j <= radius; j++)
            {
                var x = tile.x + i;
                var y = tile.y + j;

                if (GeneratorUtils.IsInMapRange(values.Width, values.Height, x, y))
                {
                    passage.Add((x, y));
                    map.Map[x, y] = 0;
                }
                    

            }
    }
}
