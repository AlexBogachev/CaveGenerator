using UnityEngine;
using Zenject;

public class CaveMesh : MonoBehaviour
{
    Mesh mesh;

    MeshFilter meshFilter;

    MeshRenderer meshRenderer;

    MeshGenerator meshGenerator;

    SquareGrid grid;

    [Inject]
    public void Constructor(MeshGenerator meshGenerator, SquareGrid grid)
    {
        this.meshGenerator = meshGenerator;
        this.grid = grid;

        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void UpdateMesh()
    {
        mesh.Clear();
        var meshData = meshGenerator.GetMeshData(grid);
        mesh.vertices = meshData.vert;
        mesh.triangles = meshData.tr;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }
}
