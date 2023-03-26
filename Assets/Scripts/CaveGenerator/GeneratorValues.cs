using UnityEngine;

public class GeneratorValues : MonoBehaviour
{
    public int Width;
    public int Height;

    [Range (0,100)]
    public int FillPercent;

    [Range (1,10)]
    public int SmoothRate;

    [Range(0, 20)]
    public int BorderSize;

    [Range(0, 10)]
    public float WallHeight;

    [Range(0, 500)]
    public int RegionTreshhold;

    [Range(0, 20)]
    public int PassageRadius;
}
