using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Pathfinding
{
    public class PathTileView: MonoBehaviour
    {
        [Inject]
        private void Constructor(PathTile tile, 
                                 [Inject (Id = ZenjectIDs.PATH_TILE_CHECKED)] Subject<PathTile> tileChecked,
                                 [Inject(Id = ZenjectIDs.PATH_TILE_UPDATED)] Subject<Color> tileUpdated)
        {
            var meshRenderer = GetComponent<MeshRenderer>();

            gameObject.OnMouseDownAsObservable()
                .Subscribe(x =>
                {
                    tileChecked.OnNext(tile);
                    //if (!Input.GetKeyDown(KeyCode.LeftControl))
                    //    tileChecked.OnNext(WalkableStatus.Walkable);
                    //else
                    //    tileChecked.OnNext(WalkableStatus.NotWalkable);
                })
                .AddTo(this);

            tileUpdated
                .Subscribe(x =>
                {
                    meshRenderer.material.color = x;
                })
                .AddTo(this);
        }
    }
}