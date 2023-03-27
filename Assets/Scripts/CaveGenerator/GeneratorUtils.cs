﻿using UnityEngine;

public class GeneratorUtils
{
    public static bool IsInMapRange(int width, int height, int x, int y)
        => x >= 0 && y >= 0 && x < width && y < height;

    public static Vector3 MapPositionToWorldPosition((int x, int y) coord, int width, int height, float squareSize)
        => new Vector3(- width / 2 + squareSize/2.0f + coord.x, 2, - height / 2 + squareSize/2.0f + coord.y);
}
