using Zenject;
using UnityEngine;
using System;
using System.Collections.Generic;
using UniRx;

public class CaveGeneratorInstaller : MonoInstaller
{
    [SerializeField]
    private GeneratorValues values;

    [SerializeField]
    private TopMesh caveMeshPrefab;

    [SerializeField]
    private WallsMesh wallsMeshPrefab;

    [SerializeField]
    private GroundMesh groundMeshPrefab;

    private int[,] map = new int[0, 0];

    private List<Type> singleTypes = new List<Type>()
    {
        typeof(CaveGenerator),
        typeof(PixelMap),
        typeof(SquareGrid),
        typeof(MeshGenerator),
        typeof(CaveRegionsThreshholdHandler),
        typeof(RoomConnector),
        typeof(PassageBuilder),
        typeof(MaxSquareOnPlaneGetter),
        typeof(SpawnPointSweeper)
    };

    public override void InstallBindings()
    {
        Container.Bind<int[,]>()
            .WithId(ZenjectIDs.BLACK_WHITE_MAP)
            .FromInstance(map)
            .AsSingle()
            .NonLazy();

        foreach(Type type in singleTypes)
            Container.BindInterfacesAndSelfTo(type)
                .AsSingle()
                .NonLazy();

        Container.Bind<CaveGeneratorVisualizer>()
            .FromNewComponentOnNewGameObject()
            .AsSingle()
            .NonLazy();

        Container.Bind<GeneratorValues>()
            .FromComponentInNewPrefab(values)
            .AsSingle()
            .NonLazy();

        Container.Bind<TopMesh>()
            .FromComponentInNewPrefab(caveMeshPrefab)
            .AsSingle()
            .NonLazy();

        Container.Bind<WallsMesh>()
            .FromComponentInNewPrefab(wallsMeshPrefab)
            .AsSingle()
            .NonLazy();

        Container.Bind<GroundMesh>()
            .FromComponentInNewPrefab(groundMeshPrefab)
            .AsSingle()
            .NonLazy();

        Container.Bind<Subject<Unit>>()
            .WithId(ZenjectIDs.CAVE_CREATED)
            .FromInstance(new Subject<Unit>())
            .AsCached()
            .NonLazy();
    }
}