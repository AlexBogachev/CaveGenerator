public class PixelMap
{
    public int[,] Map { get; private set; }

    public int[,] UpdateMap(int width, int height)
    {
        Map = new int[width, height];
        return Map;
    }

    public void SetNewMap(int[,] newMap)
        => Map = newMap;

    public (int w, int h) GetDimensions()
        => (Map.GetLength(0), Map.GetLength(1));

    public void Reset()
        => Map = new int[0, 0];

}
