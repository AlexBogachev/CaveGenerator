using Assets.Scripts.Data;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Pathfinding
{
    public class PathfindingAlgorithm
    {
        private WalkableTilesGrid tileGrid;

        private List<PathTile> pathTiles = new List<PathTile>();

        public PathfindingAlgorithm(WalkableTilesGrid tileGrid,
                                    [Inject(Id = ZenjectIDs.PATH_CHECKED_TILES)] ReactiveCollection<PathTile> checkedTiles)
        {
            this.tileGrid = tileGrid;

            checkedTiles.ObserveCountChanged()
                .Subscribe(x =>
                {
                    pathTiles.ForEach(x => x.TileUpdated.OnNext(Color.white));
                    pathTiles.Clear();

                    if (x == 2)
                    {
                        UnityEngine.Debug.Log("PATH RENDER");
                        foreach (var tile in GetPath(checkedTiles.ToList()))
                        {
                            tile.TileUpdated.OnNext(Color.blue);
                            pathTiles.Add(tile);
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.Log("PATH NO RENDER");
                    }
                });
        }

        public List <PathTile>  GetPath(List<PathTile> checkedTiles)
        {
            var startTile = checkedTiles[0];
            var endTile = checkedTiles[1];

            var openSet = new Heap<PathTile>(tileGrid.Tiles.Count);
            var closedSet = new HashSet<PathTile>();

            openSet.Add(startTile);

            while (openSet.Count > 0)
            {  
                PathTile currentTile = openSet.PopFirst();
                closedSet.Add(currentTile);

                if(currentTile == endTile)
                {
                    return RetrivePath(startTile, endTile);
                }

                foreach(Vector2Int tileGridPos in currentTile.TileInfo.Neighbours)
                {
                    var neighbourTile = tileGrid.GetPathTileByGridCoords(tileGridPos);
                    if (closedSet.Contains(neighbourTile))
                        continue;

                    int newNeighbourDistance = currentTile.GCost + GetDistance(currentTile, neighbourTile);
                    if(newNeighbourDistance < neighbourTile.GCost || !openSet.Contains(neighbourTile))
                    {
                        neighbourTile.SetGCost(newNeighbourDistance);
                        neighbourTile.SetHCost(GetDistance(neighbourTile, endTile));
                        neighbourTile.SetParent(currentTile);

                        if(!openSet.Contains(neighbourTile))
                            openSet.Add(neighbourTile);
                    }
                }
            }
            return new List<PathTile>();
        }

        private int GetDistance(PathTile start, PathTile end)
        {
            var startInGrid = start.TileInfo.PositionInGrid;
            var endInGrid = end.TileInfo.PositionInGrid;
            var xDist = Mathf.Abs(endInGrid.x - startInGrid.x);
            var yDist = Mathf.Abs(endInGrid.y - startInGrid.y);

            if (xDist > yDist)
                return 14 * yDist + 10 * (xDist - yDist);
            else
                return 14 * xDist + 10 * (yDist - xDist);
        }

        private List<PathTile> RetrivePath(PathTile startTile, PathTile endTile)
        {
            var path = new List<PathTile>();
            var current = endTile;
            path.Add(endTile);
            while (current != startTile)
            {
                current = current.Parent;
                path.Add(current);
            }
            path.Reverse();
            return path;
        }
    }
}