using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class GroundMesh : MonoBehaviour, IPointerClickHandler
{
    private Mesh mesh;

    private MeshFilter meshFilter;

    private MeshRenderer meshRenderer;

    private MeshGenerator meshGenerator;

    private MeshCollider meshCollider;

    private GeneratorValues values;

    private Subject<Vector3> groundTouched;

    [Inject]
    public void Constructor(MeshGenerator meshGenerator, GeneratorValues values,
                            [Inject (Id = ZenjectIDs.GROUND_TOUCHED)] Subject<Vector3> groundTouched)
    {
        this.meshGenerator = meshGenerator;
        this.values = values;
        this.groundTouched = groundTouched;

        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var touch = eventData.pointerCurrentRaycast.worldPosition;
        groundTouched.OnNext(touch);
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
