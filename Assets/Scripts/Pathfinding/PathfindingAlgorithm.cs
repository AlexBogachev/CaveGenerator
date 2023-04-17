using Assets.Scripts.Data;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Pathfinding
{
    public class PathfindingAlgorithm
    {
        private WalkableTilesGrid tileGrid;

        private List<PathTile> pathTiles = new List<PathTile>();

        public PathfindingAlgorithm(WalkableTilesGrid tileGrid)
        {
            this.tileGrid = tileGrid;
        }

        public async UniTask<List<Vector3>> GetPath(PathTile startTile, PathTile endTile)
        {
            var openSet = new Heap<PathTile>(tileGrid.Tiles.Count);
            var closedSet = new HashSet<PathTile>();

            openSet.Add(startTile);

            Stopwatch sw = Stopwatch.StartNew();

            var path = await UniTask.RunOnThreadPool(() =>
            {
                while (openSet.Count > 0)
                {
                    PathTile currentTile = openSet.PopFirst();
                    closedSet.Add(currentTile);

                    if (currentTile == endTile)
                    {
                        sw.Stop();
                        UnityEngine.Debug.Log("TIME = " + sw.ElapsedMilliseconds);
                        return RetrivePath(startTile, endTile);
                    }

                    foreach (Vector2Int tileGridPos in currentTile.TileInfo.Neighbours)
                    {
                        var neighbourTile = tileGrid.GetPathTileByGridCoords(tileGridPos);
                        if (closedSet.Contains(neighbourTile))
                            continue;

                        int newNeighbourDistance = currentTile.GCost + GetDistance(currentTile, neighbourTile);
                        if (newNeighbourDistance < neighbourTile.GCost || !openSet.Contains(neighbourTile))
                        {
                            neighbourTile.SetGCost(newNeighbourDistance);
                            neighbourTile.SetHCost(GetDistance(neighbourTile, endTile));
                            neighbourTile.SetParent(currentTile);

                            if (!openSet.Contains(neighbourTile))
                                openSet.Add(neighbourTile);
                        }
                    }
                }

                return new List<Vector3>();
            });

            return path;
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

        private List<Vector3> RetrivePath(PathTile startTile, PathTile endTile)
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
            return path.Select(x=>x.TileInfo.Position).ToList();
        }
    }

    public class AsyncAwaitTest
    {
        public async void Msg()
        {
            UnityEngine.Debug.Log("MSG");
        }
        public async void Strt()
        {
            await UniTask.RunOnThreadPool(() => StartWhile());
        }

        public async UniTask<bool> StartWhile()
        {
            return await LongWhile();
        }

        public UniTask<bool> LongWhile()
        {
            int n = 0;
            while (true)
            {
                n++;
                if(n == 10000)
                {
                    return new UniTask<bool>(true);
                }
                UnityEngine.Debug.Log("n = " + n);
            }          
        }
    }
}