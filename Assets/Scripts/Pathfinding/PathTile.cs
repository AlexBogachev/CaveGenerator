using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace Assets.Scripts.Pathfinding
{
    public class PathTile
    {
        public static float TILE_LENGTH;
        public static float TILE_DIAGONAL;

        public TileInfo TileInfo { get; private set; }
        public Subject<Color> TileUpdated { get; private set; }
        public Subject<PathTile> TileChecked { get; private set; }

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
                        Debug.Log("COLOR WHITE");
                        tileUpdated.OnNext(Color.white);
                    }
                        
                    else
                    {
                        Debug.Log("COLOR RED");
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

        public void SetWalkableStatus(bool isWalkable)
            =>this.isWalkable = isWalkable;

        public bool IsTileChecked()
            => isChecked;

        public class Factory:PlaceholderFactory<TileInfo, bool, PathTile> { }
    }
}