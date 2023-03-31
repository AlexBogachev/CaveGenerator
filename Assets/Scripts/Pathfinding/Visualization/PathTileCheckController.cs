using Assets.Scripts.Pathfinding;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Pathfinding
{
    public class PathTileCheckController
    {
        private List<PathTile> allTiles;

        private ReactiveCollection<PathTile> checkedTiles;

        private Dictionary<Vector2Int, PathTile> tilesPositionsInGrid;

        public PathTileCheckController([Inject (Id = ZenjectIDs.PATH_CHECKED_TILES)]ReactiveCollection<PathTile> checkedTiles)
        {
            allTiles = new List<PathTile>();

            this.checkedTiles = checkedTiles;

            tilesPositionsInGrid = new Dictionary<Vector2Int, PathTile>();
        }

        public void AddTile(PathTile tile)
        {
            allTiles.Add(tile);
            tilesPositionsInGrid.Add(tile.TileInfo.PositionInGrid, tile);

            tile.TileChecked
                .Subscribe(x =>
                {
                    if (tile.IsTileChecked())
                    {
                        if (checkedTiles.Count == 2)
                        {
                            var tileToRemove = checkedTiles.Last();
                            checkedTiles.Remove(tileToRemove);
                            tileToRemove.TileChecked.OnNext(tileToRemove);
                        }
                        checkedTiles.Add(tile);
                    }
                    else
                        checkedTiles.Remove(tile);

                    foreach (var neighbour in tile.TileInfo.Neighbours.Where(x=>x!=tile.TileInfo.PositionInGrid))
                    {
                        var pathTile = tilesPositionsInGrid[neighbour];

                        if (tile.IsTileChecked())
                            pathTile.TileUpdated.OnNext(Color.grey);
                        else
                            pathTile.TileUpdated.OnNext(Color.white);
                    }
                });
        }

        public PathTile GetPathTileByGridCoords(Vector2Int coords)
            => tilesPositionsInGrid[coords];
    }
}

public class PathfindingAlgorithm
{
    public PathfindingAlgorithm([Inject(Id = ZenjectIDs.PATH_CHECKED_TILES)] ReactiveCollection<PathTile> checkedTiles)
    {
        checkedTiles.ObserveCountChanged()
            .Subscribe(x =>
            {
                if (x == 2)
                {
                    Debug.Log("RENDER PATH");
                }
                else
                {
                    Debug.Log("PATH NO RENDER");
                }
            });
    }
}