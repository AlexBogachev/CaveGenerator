using UnityEngine;
using Zenject;

public class CaveGeneratorVisualizer : MonoBehaviour
{
    GeneratorValues values;

    PixelMap map;

    SquareGrid squareGrid;

    [Inject]
    private void Constructor(PixelMap map, SquareGrid grid, GeneratorValues values)
    {
        this.map = map;
        this.values = values;
        this.squareGrid = grid;
    }

    private void OnDrawGizmos()
    {
        var width = squareGrid.Squares.GetLength(0);
        var height = squareGrid.Squares.GetLength(1);

        var squares = squareGrid.Squares;

        for(int i = 0; i < width; i++)
            for(int j = 0; j < height; j++)
            {
                //var xPos = i - width / 2 + 0.5f;
                //var yPos = j - height / 2 + 0.5f;
                var square = squareGrid.Squares[i, j];

                Gizmos.color = square.TopLeft.IsActive()? Color.red : Color.white;
                Gizmos.DrawCube(new Vector3(square.TopLeft.GetPosition().x, 0.0f, square.TopLeft.GetPosition().z), Vector3.one*0.25f);

                Gizmos.color = square.TopRight.IsActive() ? Color.red : Color.white;
                Gizmos.DrawCube(new Vector3(square.TopRight.GetPosition().x, 0.0f, square.TopRight.GetPosition().z), Vector3.one * 0.25f);

                Gizmos.color = square.BottomRight.IsActive() ? Color.red : Color.white;
                Gizmos.DrawCube(new Vector3(square.BottomRight.GetPosition().x, 0.0f, square.BottomRight.GetPosition().z), Vector3.one * 0.25f);

                Gizmos.color = square.BottomLeft.IsActive() ? Color.red : Color.white;
                Gizmos.DrawCube(new Vector3(square.BottomLeft.GetPosition().x, 0.0f, square.BottomLeft.GetPosition().z), Vector3.one * 0.25f);

                Gizmos.color = Color.grey;
                Gizmos.DrawCube(new Vector3(square.CenterLeft.GetPosition().x, 0.0f, square.CenterLeft.GetPosition().z), Vector3.one * 0.25f);
                Gizmos.DrawCube(new Vector3(square.CenterTop.GetPosition().x, 0.0f, square.CenterTop.GetPosition().z), Vector3.one * 0.25f);
                Gizmos.DrawCube(new Vector3(square.CenterRight.GetPosition().x, 0.0f, square.CenterRight.GetPosition().z), Vector3.one * 0.25f);
                Gizmos.DrawCube(new Vector3(square.CenterBottom.GetPosition().x, 0.0f, square.CenterBottom.GetPosition().z), Vector3.one * 0.25f);
            }
    }
}
