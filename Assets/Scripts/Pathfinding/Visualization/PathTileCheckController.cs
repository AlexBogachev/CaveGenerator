using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Pathfinding
{
    public class PathTileCheckController
    {
        private List<PathTile> allTiles;

        private Dictionary<Vector2Int, PathTile> tilesPositionsInGrid;
        private Dictionary<PathTile, List<PathTile>> tileAndNeighbours;

        public PathTileCheckController()
        {
            allTiles = new List<PathTile>();

            tilesPositionsInGrid = new Dictionary<Vector2Int, PathTile>();
            tileAndNeighbours = new Dictionary<PathTile, List<PathTile>>();
        }

        public void AddTile(PathTile tile)
        {
            allTiles.Add(tile);
            tilesPositionsInGrid.Add(tile.TileInfo.PositionInGrid, tile);


            tile.TileChecked
                .Subscribe(x =>
                {
                    foreach (var neighbour in tile.TileInfo.Neighbours.Where(x=>x!=tile.TileInfo.PositionInGrid))
                    {
                        var pathTile = tilesPositionsInGrid[neighbour];

                        if(tile.IsTileChecked())
                            pathTile.TileUpdated.OnNext(Color.grey);
                        else
                            pathTile.TileUpdated.OnNext(Color.white);
                    }
                });
        }
    }
}