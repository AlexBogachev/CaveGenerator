
using UnityEngine;

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

    public Vector3 GetCenter() 
    {
        var x = (TopRight.GetPosition().x + BottomLeft.GetPosition().x) / 2.0f;
        var y = (TopRight.GetPosition().y + BottomLeft.GetPosition().y) / 2.0f;
        var z = (TopRight.GetPosition().z + BottomLeft.GetPosition().z) / 2.0f;
        return new Vector3(x, y, z);
    }
}
