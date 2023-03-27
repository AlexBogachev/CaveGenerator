using UnityEngine;
using Zenject;

public class GroundMesh : MonoBehaviour
{
    Mesh mesh;

    MeshFilter meshFilter;

    MeshRenderer meshRenderer;

    MeshGenerator meshGenerator;

    MeshCollider meshCollider;

    GeneratorValues values;

    [Inject]
    public void Constructor(MeshGenerator meshGenerator, GeneratorValues values)
    {
        this.meshGenerator = meshGenerator;
        this.values = values;

        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    public void UpdateMesh()
    {
        mesh.Clear();
        var meshData = meshGenerator.GetGroundMesh();
        mesh.vertices = meshData.vert;
        mesh.triangles = meshData.tr;
        mesh.uv = GetUVs(mesh.vertices);
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    private Vector2[] GetUVs(Vector3[] vertices)
    {
        var tiling = 20.0f;
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertice = vertices[i];
            var uvX = Mathf.InverseLerp(-(values.Width + values.BorderSize * 2) / 2, (values.Width + values.BorderSize * 2) / 2, vertice.x) * tiling;
            var uvY = Mathf.InverseLerp(-(values.Height + values.BorderSize * 2) / 2, (values.Height + values.BorderSize * 2) / 2, vertice.z) * tiling;
            uvs[i] = new Vector2(uvX, uvY);
        }
        return uvs;
    }
}
