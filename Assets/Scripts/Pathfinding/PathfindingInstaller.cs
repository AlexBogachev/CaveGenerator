using System;
using System.Collections.Generic;
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

        private List<Type> simpleTypes = new List<Type>()
        {
            typeof(WalkableTilesGrid),
            //typeof(PathTileCheckController),
            typeof(PathfindingAlgorithm)
        };

        public override void InstallBindings()
        {
            foreach (Type type in simpleTypes)
                Container.Bind(type)
                    .AsSingle()
                    .NonLazy();

            Container.Bind<ReactiveCollection<PathTile>>()
                .WithId(ZenjectIDs.PATH_CHECKED_TILES)
                .FromInstance(new ReactiveCollection<PathTile>())
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

            //subContainer.Bind<PathTileView>()
            //    .FromMethod(() => CreateView(subContainer, info.Position))
            //    .AsCached()
            //    .NonLazy();

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