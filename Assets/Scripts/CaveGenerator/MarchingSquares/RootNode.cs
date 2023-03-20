using UnityEngine;

public class RootNode : INode
{
    public Node TopNode { get; private set; }
    public Node RightNode { get; private set; }


    private Vector3 position;
    private int vertexPosition = -1;

    private bool isActive;

    private float size;

    public RootNode(Vector3 position, bool isActive, float size)
    {
        this.position = position;
        this.isActive = isActive;
        this.size = size;

        TopNode = new Node(position + Vector3.forward * size * 0.5f);
        RightNode = new Node(position + Vector3.right * size * 0.5f);
    }

    public Vector3 GetPosition()
        => position;

    public void SetPosition(Vector3 newPosition)
        => position = newPosition;

    public void SetVertexPosition(int vertexPosition)
        =>this.vertexPosition = vertexPosition;

    public int GetVertexPosition()
        => vertexPosition;


    public void SetActive(bool isActive)
        => this.isActive = isActive;
    public bool IsActive()
        => isActive;

    public float GetSize()
        => size;

    
}
