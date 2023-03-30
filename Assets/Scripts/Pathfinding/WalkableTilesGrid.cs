using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Pathfinding
{
    public enum WalkableStatus
    {
        Walkable,
        NotWalkable
    }

    public class WalkableTilesGrid
    {
        [Inject]
        private IFactory<TileInfo, bool, PathTile> tilesFactory;

        public WalkableTilesGrid(SquareGrid squareGrid, PathTileCheckController pathTileCheckController) 
        {
            Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Subscribe(_ =>
                {
                    ShowTiles(squareGrid, pathTileCheckController);
                });
        }

        private void ShowTiles(SquareGrid squareGrid, PathTileCheckController pathTileCheckController)
        {
            var width = squareGrid.Squares.GetLength(0);
            var height = squareGrid.Squares.GetLength(1);

            for (int i = 0; i < width; i++)
                for(int j = 0; j < height; j++)
                {
                    var square  = squareGrid.Squares[i, j];
                    if(square.configuration == 0)
                    {
                        var position = square.GetCenter();
                        var positionInGrid = new Vector2Int(i,j);
                        var neighbours = new List<Vector2Int>();
                        for(int x = i-1; x <= i+1; x++)
                            for(int y = j - 1; y <= j+1; y++)
                            {
                                if(GeneratorUtils.IsInMapRange(width, height, x, y))
                                {
                                    var neighbour = squareGrid.Squares[x, y];
                                    if (neighbour.configuration == 0)
                                        neighbours.Add(new Vector2Int(x, y));
                                }
                            }
                        var info = new TileInfo(position, positionInGrid, neighbours);
                        var tile = tilesFactory.Create(info, true);
                        pathTileCheckController.AddTile(tile);
                    }
                }
        }
    }
}