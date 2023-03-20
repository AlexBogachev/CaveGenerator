using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshGenerator
{
    private GeneratorValues values;

    private List<Vector3> vertices = new List<Vector3>();
    private List<NodesTriangle> triangles = new List<NodesTriangle>();

    private Dictionary<int, List<NodesTriangle>> nodesAndConnectedTraingles = new Dictionary<int, List<NodesTriangle>>();

    private List<List<int>> allOutlines = new List<List<int>>();
    private HashSet<int> checkedVertices = new HashSet<int>();

    public MeshGenerator(GeneratorValues values)
    {
        this.values = values;
    }

    public (Vector3[] vert, int[]tr) GetMeshData(SquareGrid grid) 
    {
        ClearData();

        foreach(NodesSquare square in grid.Squares)
            TriangulateSquare(square);

        var intTriangles = triangles.SelectMany(x => x.nodes).ToArray();
        return (vertices.ToArray(), intTriangles);
    }

    public (Vector3[] vert, int[] tr) GetWallsMesh()
    {
        CalculateWallOutlines();

        List<Vector3> wallVertices = new List<Vector3>();
        List<int> wallTrinagles = new List<int>();

        var wallHeight = values.WallHeight;

        foreach(List<int> outline in allOutlines)
            for(int i = 0; i < outline.Count - 1; i++)
            {
                int startIndex = wallVertices.Count;

                wallVertices.Add(vertices[outline[i]]); // l
                wallVertices.Add(vertices[outline[i+1]]); // r
                wallVertices.Add(vertices[outline[i]] - Vector3.up * wallHeight); // bl
                wallVertices.Add(vertices[outline[i +1]] - Vector3.up * wallHeight); // br

                wallTrinagles.Add(startIndex + 0);
                wallTrinagles.Add(startIndex + 2);
                wallTrinagles.Add(startIndex + 3);

                wallTrinagles.Add(startIndex + 3);
                wallTrinagles.Add(startIndex + 1);
                wallTrinagles.Add(startIndex + 0);
            }

        return (wallVertices.ToArray(), wallTrinagles.ToArray());
    }

    private void CalculateWallOutlines()
    {
        for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
        {
            if (!checkedVertices.Contains(vertexIndex))
            {
                var nextOutlineIndex = GetConnectedOutlineVertex(vertexIndex);
                if (nextOutlineIndex != -1)
                {
                    checkedVertices.Add(vertexIndex);
                    List<int>outlines = new List<int> () { vertexIndex };
                    allOutlines.Add(outlines);
                    BypassOutline(nextOutlineIndex, allOutlines.Count - 1);
                    outlines.Add(vertexIndex);
                }
            }
        }
    }

    private void BypassOutline(int vertexIndex, int outlineIndex)
    {
        allOutlines[outlineIndex].Add(vertexIndex);
        checkedVertices.Add(vertexIndex);
        var nextIndex = GetConnectedOutlineVertex(vertexIndex);

        if (nextIndex != -1)
            BypassOutline(nextIndex, outlineIndex);
    }

    private int GetConnectedOutlineVertex(int vertexA)
    {
        List<NodesTriangle> connectedTriangles = nodesAndConnectedTraingles[vertexA];
        foreach (NodesTriangle triangle in connectedTriangles)
            foreach (int vertexB in triangle.nodes.Where(x => x != vertexA && !checkedVertices.Contains(x)))
                if (IsOutlineEdge(vertexA, vertexB))
                    return vertexB;
        return -1;
    }

    private bool IsOutlineEdge(int vertexA, int vertexB)
    {
        List<NodesTriangle> connectedTriangles = nodesAndConnectedTraingles[vertexA];
        int sharedTriangles = 0;
        foreach(NodesTriangle triangle in connectedTriangles)
        {
            if (triangle.Contains(vertexB))
            {
                sharedTriangles++;
                if (sharedTriangles > 1)
                    break;
            }
        }
        return sharedTriangles == 1;
    }

    
    private void TriangulateSquare(NodesSquare square)
    {
        switch (square.configuration)
        {
            case 0:
                break;

            // 1 active
            case 1:
                UpdateData(square.CenterLeft, square.CenterBottom, square.BottomLeft);
                break;
            case 2:
                UpdateData(square.BottomRight, square.CenterBottom, square.CenterRight);
                break;
            case 4:
                UpdateData(square.TopRight, square.CenterRight, square.CenterTop);
                break;
            case 8:
                UpdateData(square.TopLeft, square.CenterTop, square.CenterLeft);
                break;

            // 2 active
            case 3:
                UpdateData(square.CenterRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
                break;
            case 6:
                UpdateData(square.CenterTop, square.TopRight, square.BottomRight, square.CenterBottom);
                break;
            case 9:
                UpdateData(square.TopLeft, square.CenterTop, square.CenterBottom, square.BottomLeft);
                break;
            case 12:
                UpdateData(square.TopLeft, square.TopRight, square.CenterRight, square.CenterLeft);
                break;
            case 5:
                UpdateData(square.CenterTop, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft, square.CenterLeft);
                break;
            case 10:
                UpdateData(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
                break;

            // 3 active
            case 7:
                UpdateData(square.CenterTop, square.TopRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
                break;
            case 11:
                UpdateData(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.BottomLeft);
                break;
            case 13:
                UpdateData(square.TopLeft, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft);
                break;
            case 14:
                UpdateData(square.TopLeft, square.TopRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
                break;

            // 4 active
            case 15:
                UpdateData(square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);

                checkedVertices.Add(square.TopLeft.GetVertexPosition());
                checkedVertices.Add(square.TopRight.GetVertexPosition());
                checkedVertices.Add(square.BottomRight.GetVertexPosition());
                checkedVertices.Add(square.BottomLeft.GetVertexPosition());

                break;
        }
    }

    private void UpdateData(params INode[] nodes)
    {
        foreach(INode node in nodes)
            if (node.GetVertexPosition() == -1)
            {
                node.SetVertexPosition(vertices.Count);
                vertices.Add(node.GetPosition());
            }

        if (nodes.Count() >= 3)
            AddTriangle(nodes[0], nodes[1], nodes[2]);
        if (nodes.Count() >= 4)
            AddTriangle(nodes[0], nodes[2], nodes[3]);
        if (nodes.Count() >= 5)
            AddTriangle(nodes[0], nodes[3], nodes[4]);
        if (nodes.Count() >= 6)
            AddTriangle(nodes[0], nodes[4], nodes[5]);

        void AddTriangle(INode a, INode b, INode c)
        {
            NodesTriangle triangle = new NodesTriangle(a, b, c);
            triangles.Add(triangle);

            foreach(int vertexPosition in triangle.nodes)
            {
                if (nodesAndConnectedTraingles.ContainsKey(vertexPosition))
                    nodesAndConnectedTraingles[vertexPosition].Add(triangle);
                else
                    nodesAndConnectedTraingles.Add(vertexPosition, new List<NodesTriangle>() { triangle });
            }
        }
    }

    private void ClearData()
    {
        vertices.Clear();
        triangles.Clear();
        nodesAndConnectedTraingles.Clear();
        allOutlines.Clear();
        checkedVertices.Clear();
    }
}