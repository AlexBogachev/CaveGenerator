public class MaxSquareOnPlaneGetter
{
    private enum Direction
    {
        Width,
        Height
    }

    private float width;
    private float height;
    private float Epsilon = 1.0f;

    public MaxSquareOnPlaneGetter(GeneratorValues values) 
    {
        width = (values.Width + values.BorderSize*2) * values.SquareSize;
        height = (values.Height + values.BorderSize*2) * values.SquareSize;
        GetMaxSquare(width, height);
    }

    public float GetMaxSquare()
        =>GetMaxSquare(width, height);

   public float GetMaxSquare(float width, float height)
    {
        var maxDirection = width > height ? Direction.Width : Direction.Height;

        float remainder;
        if (maxDirection == Direction.Width)
        {
            remainder = width % height;
            if (remainder <= Epsilon)
                return height;
            else
                return GetMaxSquare(remainder, height);
        }
        else
        {
            remainder = height % width;
            if (remainder <= Epsilon)
                return width;
            else
                return GetMaxSquare(remainder, width);
        }
    }
}
