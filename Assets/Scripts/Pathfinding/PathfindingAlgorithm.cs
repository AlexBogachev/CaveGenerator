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

        public PathfindingAlgorithm(WalkableTilesGrid tileGrid,
                                    [Inject(Id = ZenjectIDs.PATH_CHECKED_TILES)] ReactiveCollection<PathTile> checkedTiles)
        {
            this.tileGrid = tileGrid;

            checkedTiles.ObserveCountChanged()
                .Subscribe(x =>
                {
                    if (x == 2)
                    {
                        Debug.Log("PATH RENDER");
                        foreach (var tile in GetPath(checkedTiles.ToList()))
                        {
                            tile.TileUpdated.OnNext(Color.blue);
                        }
                    }
                    else
                    {
                        Debug.Log("PATH NO RENDER");
                    }
                });
        }

        public List <PathTile>  GetPath(List<PathTile> checkedTiles)
        {
            var startTile = checkedTiles[0];
            var endTile = checkedTiles[1];

            var openSet = new List<PathTile>();
            var closedSet = new HashSet<PathTile>();

            openSet.Add(startTile);
            int n = 0;
            while (openSet.Count > 0)
            {
                if (n > 10000)
                {
                    Debug.Log("OVERFLOW");
                    return new List<PathTile>();
                }
                    
                PathTile currentTile = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                    if (openSet[i].FCost < currentTile.FCost || (openSet[i].FCost == currentTile.FCost && openSet[i].HCost < currentTile.HCost))
                    {
                        currentTile = openSet[i];
                    }
                openSet.Remove(currentTile);
                closedSet.Add(currentTile);

                if(currentTile == endTile)
                {
                    //Debug.Log("CUR = " + currentTile.TileInfo.Position + " || END = " + currentTile.TileInfo.Position);
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

                        //Debug.Log("PARENT = " + currentTile.TileInfo.Position);
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