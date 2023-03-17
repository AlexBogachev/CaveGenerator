using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneratorValues : MonoBehaviour
{
    public int Width;
    public int Height;

    [Range (0,100)]
    public int FillPercent;

    [Range (1,10)]
    public int SmoothRate;
}
