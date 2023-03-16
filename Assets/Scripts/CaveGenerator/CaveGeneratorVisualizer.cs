using UnityEngine;
using Zenject;

public class CaveGeneratorVisualizer : MonoBehaviour
{
    GeneratorValues values;
    BlackWhiteMap map;

    [Inject]
    private void Constructor(BlackWhiteMap map, GeneratorValues values)
    {
        this.map = map;
        this.values = values;
    }

    private void OnDrawGizmos()
    {
        var width = values.Width;
        var height = values.Height;

        for(int i = 0; i < width; i++)
            for(int j = 0; j < height; j++)
            {
                var xPos = i - width / 2 + 0.5f;
                var yPos = j - height / 2 + 0.5f;
                Gizmos.color = map.Map[i, j] == 1? Color.red : Color.white;
                Gizmos.DrawCube(new Vector3(xPos, 0.0f, yPos), Vector3.one);
            }
    }
}
