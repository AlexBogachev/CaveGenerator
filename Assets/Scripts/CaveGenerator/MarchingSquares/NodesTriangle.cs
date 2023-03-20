using System.Linq;

public struct NodesTriangle
{
    public INode nodeA;
    public INode nodeB;
    public INode nodeC;

    public int[] nodes;

    public NodesTriangle(INode nodeA, INode nodeB, INode nodeC)
    {
        this.nodeA = nodeA;
        this.nodeB = nodeB;
        this.nodeC = nodeC;

        nodes = new int[3];
        nodes[0] = nodeA.GetVertexPosition();
        nodes[1] = nodeB.GetVertexPosition();
        nodes[2] = nodeC.GetVertexPosition();
    }

    public bool Contains(int vertex)
        => nodes.Contains(vertex);
}

