using UnityEngine;

public interface INode
{
    Vector3 GetPosition();
    void SetPosition(Vector3 newPosition);
    int GetVertexPosition();
    void SetVertexPosition(int vertexPosition);
}
