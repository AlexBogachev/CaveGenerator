using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Pathfinding
{
    public class PathTile
    {
        public TileInfo TileInfo { get; private set; }

        public Subject<Color> TileUpdated { get; private set; }
        public Subject<PathTile> TileChecked { get; private set; }



        private PathTile parent;

        private bool isWalkable;
        private bool isChecked;

        public PathTile(TileInfo tileInfo, bool isWalkable,
                       [Inject(Id = ZenjectIDs.PATH_TILE_CHECKED)] Subject<PathTile> tileChecked,
                       [Inject(Id = ZenjectIDs.PATH_TILE_UPDATED)] Subject<Color> tileUpdated)
        {
            TileInfo = tileInfo;
            TileChecked = tileChecked;
            TileUpdated = tileUpdated;

            tileChecked
                .Subscribe(x =>
                {
                    if (isChecked)
                        tileUpdated.OnNext(Color.white);
                    else
                    {
                        tileUpdated.OnNext(Color.red);

                        //if(x == WalkableStatus.Walkable)
                        //    tileUpdated.OnNext(Color.red);
                        //else
                        //    tileUpdated.OnNext(Color.black);
                    }

                    isChecked = !isChecked;

                });
        }

        public void SetWalkableStatus(bool isWalkable)
            =>this.isWalkable = isWalkable;

        public bool IsTileChecked()
            => isChecked;

        public class Factory:PlaceholderFactory<TileInfo, bool, PathTile> { }
    }
}