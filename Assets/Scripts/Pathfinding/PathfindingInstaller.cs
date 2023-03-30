using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Pathfinding
{
    public class PathfindingInstaller : MonoInstaller
    {
        [SerializeField]
        private PathTileView pathTileViewPrefab;

        [SerializeField]
        private Transform pathTilesParent;

        public override void InstallBindings()
        {
            Container.Bind<WalkableTilesGrid>()
                .AsSingle()
                .NonLazy();

            Container.Bind<PathTileCheckController>()
                .AsSingle()
                .NonLazy();

            Container.BindIFactory<TileInfo, bool, PathTile>()
                .FromSubContainerResolve()
                .ByMethod(CreateTile);
        }

        private void CreateTile (DiContainer subContainer, TileInfo info, bool isWalkable)
        {
            subContainer.Bind<PathTile>()
                 .AsCached()
                 .WithArguments(info, isWalkable)
                 .NonLazy();

            subContainer.Bind<PathTileView>()
                .FromMethod(() => CreateView(subContainer, info.Position))
                .AsCached()
                .NonLazy();

            subContainer.Bind<Subject<PathTile>>()
                .WithId(ZenjectIDs.PATH_TILE_CHECKED)
                .FromInstance(new Subject<PathTile>())
                .AsCached()
                .NonLazy();

            subContainer.Bind<Subject<Color>>()
                .WithId(ZenjectIDs.PATH_TILE_UPDATED)
                .FromInstance(new Subject<Color>())
                .AsCached()
                .NonLazy();
                
        }

        private PathTileView CreateView(DiContainer subContainer, Vector3 position)
        {
            PathTileView view = Instantiate(pathTileViewPrefab, position + Vector3.down*4.5f, Quaternion.identity);
            view.transform.SetParent(pathTilesParent);
            subContainer.Inject(view);
            return view;
        }
    }
}