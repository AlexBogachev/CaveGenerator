using UnityEngine;

public class Node: INode
{
    private Vector3 position;
    private int vertexPosition = -1;

    public Node(Vector3 position)
    {
        this.position = position;
    }

    public Vector3 GetPosition()
        => position;

    public void SetPosition(Vector3 newPosition)
       => position = newPosition;


    public int GetVertexPosition()
        => vertexPosition;

    public void SetVertexPosition(int vertexPosition)
    => this.vertexPosition = vertexPosition;
}
