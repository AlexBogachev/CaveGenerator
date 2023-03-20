public class NodesSquare
{
    public RootNode TopLeft;
    public RootNode TopRight;
    public RootNode BottomRight;
    public RootNode BottomLeft;

    public Node CenterLeft;
    public Node CenterTop;
    public Node CenterRight;
    public Node CenterBottom;

    public int configuration;

    public NodesSquare(RootNode topLeft, RootNode topRight, RootNode bottomRight, RootNode bottomLeft)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomRight = bottomRight;
        BottomLeft = bottomLeft;

        CenterLeft = bottomLeft.TopNode;
        CenterTop = topLeft.RightNode;
        CenterRight = bottomRight.TopNode;
        CenterBottom = bottomLeft.RightNode;

        if (TopLeft.IsActive())
            configuration += 8;
        if (TopRight.IsActive())
            configuration += 4;
        if (BottomRight.IsActive())
            configuration += 2;
        if(bottomLeft.IsActive())
            configuration += 1; 
    }
}
