using System.Collections.Generic;
using UnityEngine;

public enum DebugElement
{
    Point, 
    Line
}

public static class DebugDraw
{
    public static List<GameObject> debugPoints = new List<GameObject>();
    public static List<GameObject> debugLines = new List<GameObject>();

    public static void DrawPoint(Vector3 pos, float scale, Color color)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.transform.position = new Vector3(pos.x, pos.y, pos.z);
        obj.transform.localScale = new Vector3(scale, scale, scale);
        obj.GetComponent<MeshRenderer>().material.color = color;

        debugPoints.Add(obj);
    }

    public static void DrawLine(Vector3 p0, Vector3 p1, float zOffset, Color color)
    {
        GameObject obj = new GameObject("LR", typeof(LineRenderer));
        LineRenderer line = obj.GetComponent<LineRenderer>();
        line.SetPosition(0, UpdateZOffset(p0));
        line.SetPosition(1, UpdateZOffset(p1));
        line.startColor = color;
        line.endColor = color;
        line.startWidth *= 0.05f;
        line.endWidth *= 0.05f;

        debugLines.Add(obj);

        Vector3 UpdateZOffset(Vector3 v3)
            => v3 += new Vector3(0.0f, 0.0f, zOffset);
    }

    public static void Clear(params DebugElement[] elements)
    {
        foreach (DebugElement element in elements)
            Clear(GetElements(element));
    }

    public static void Clear(List<GameObject>objects)
    {
        objects.ForEach(x => Object.Destroy(x));
        objects.Clear();
    }

    private static List<GameObject> GetElements(DebugElement element)
    {
        switch (element)
        {
            case DebugElement.Point:
                return debugPoints;
            case DebugElement.Line:
                return debugLines;
            default:
                return new List<GameObject>();
        }
    }
}
