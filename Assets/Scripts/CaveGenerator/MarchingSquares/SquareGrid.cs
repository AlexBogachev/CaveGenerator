
using UnityEngine;

public class SquareGrid
{
    public NodesSquare[,] Squares;

    private float squareSize;

    public SquareGrid(GeneratorValues values) 
    {
        squareSize = values.SquareSize;
    }

    public void UpdateSquareGrid(int[,] pixels)
    {
        var width = pixels.GetLength(0);
        var height = pixels.GetLength(1);

        RootNode[,] nodes = new RootNode[width, height];
        for(int i = 0; i<width;i++)
            for (int j = 0; j < height; j++)
            {
                var pos = new Vector3(-width * 0.5f + i * squareSize + squareSize * 0.5f, 0.0f, -height * 0.5f + j * squareSize + squareSize * 0.5f);
                bool isActive = pixels[i, j] == 1;
                RootNode node = new RootNode(pos, isActive, squareSize);
                nodes[i,j] = node;
            }

        Squares = new NodesSquare[width - 1, height - 1]; 
        for (int i = 0; i < width - 1; i++)
            for (int j = 0; j < height - 1; j++)
            {
                NodesSquare square = new NodesSquare(nodes[i, j + 1], nodes[i + 1, j + 1], nodes[i + 1, j], nodes[i, j]);
                Squares[i, j] = square;
            }
    }
}
