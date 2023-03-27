using UnityEngine;

public static class GeneratorUtils
{
    public static bool IsInMapRange(int width, int height, int x, int y)
        => x >= 0 && y >= 0 && x < width && y < height;

    public static Vector3 MapPositionToWorldPosition((int x, int y) coord, int width, int height, float squareSize, float zOffset)
        => new Vector3(- width / 2 + squareSize/2.0f + coord.x, zOffset, - height / 2 + squareSize/2.0f + coord.y);
}
