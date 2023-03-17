using Zenject;
using UnityEngine;
using System;
using System.Collections.Generic;

public class CaveGeneratorInstaller : MonoInstaller
{
    [SerializeField]
    private GeneratorValues values;

    private int[,] map = new int[0, 0];

    private List<Type> singleTypes = new List<Type>()
    {
        typeof(CaveGenerator),
        typeof(PixelMap),
        typeof(SquareGrid),
        typeof(MeshGenerator)
    };
    

    public override void InstallBindings()
    {
        Container.Bind<int[,]>()
            .WithId(ZenjectIDs.BLACK_WHITE_MAP)
            .FromInstance(map)
            .AsSingle()
            .NonLazy();

        foreach(Type type in singleTypes)
            Container.Bind(type)
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
    }
}