using Assets.Scripts.Data;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Pathfinding
{
    public class PathTile : IHeapItem<PathTile>
    {
        public static float TILE_LENGTH;
        public static float TILE_DIAGONAL;

        public int GCost { get; private set; }
        public int HCost { get; private set; }
        public int FCost
        {
            get
            {
                return GCost + HCost;
            }
        }

        public PathTile Parent { get; private set; }

        public TileInfo TileInfo { get; private set; }
        public Subject<Color> TileUpdated { get; private set; }
        public Subject<PathTile> TileChecked { get; private set; }

        private int heapIndex;
        public int HeapIndex { get => heapIndex; set => heapIndex = value; }

        private PathTile parent;

        private bool isWalkable;
        private bool isChecked;

        public PathTile(TileInfo tileInfo, bool isWalkable,
                       GeneratorValues values,
                       [Inject(Id = ZenjectIDs.PATH_TILE_CHECKED)] Subject<PathTile> tileChecked,
                       [Inject(Id = ZenjectIDs.PATH_TILE_UPDATED)] Subject<Color> tileUpdated)
        {
            TILE_LENGTH = values.SquareSize;
            TILE_DIAGONAL = Mathf.Sqrt(2 * TILE_LENGTH * TILE_LENGTH);

            TileInfo = tileInfo;
            TileChecked = tileChecked;
            TileUpdated = tileUpdated;

            tileChecked
                .Subscribe(x =>
                {
                    if (isChecked)
                    {
                        tileUpdated.OnNext(Color.white);
                    }
                        
                    else
                    {
                        tileUpdated.OnNext(Color.red);
                    }

                    isChecked = !isChecked;

                });
        }

        public void ForceCheck(bool isChecked, IEnumerable<PathTile> neighbours)
        {
            this.isChecked = isChecked;
            if (!isChecked)
            {
                TileUpdated.OnNext(Color.white);
                foreach (var neighbour in neighbours)
                    neighbour.TileUpdated.OnNext(Color.white);
            }
            else
            {
                TileUpdated.OnNext(Color.red);
                foreach (var neighbour in neighbours)
                    neighbour.TileUpdated.OnNext(Color.grey);
            }
        }

        public void SetGCost(int gCost)
            => GCost = gCost;

        public void SetHCost(int hCost)
            => HCost = hCost;

        public void SetParent(PathTile parent)
            => Parent = parent;

        public void SetWalkableStatus(bool isWalkable)
            =>this.isWalkable = isWalkable;

        public bool IsTileChecked()
            => isChecked;

        public int CompareTo(PathTile other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if(compare == 0)
                compare = HCost.CompareTo(other.HCost);
            return -compare;
        }

        public class Factory:PlaceholderFactory<TileInfo, bool, PathTile> { }
    }
}