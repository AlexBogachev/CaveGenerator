public class BlackWhiteMap
{
    public int[,] Map { get; private set; }

    public int[,] UpdateMap(int width, int height)
    {
        Map = new int[width, height];
        return Map;
    }

    public (int w, int h) GetDimensions()
        => (Map.GetLength(0), Map.GetLength(1));

    public void Reset()
        => Map = new int[0, 0];

}
