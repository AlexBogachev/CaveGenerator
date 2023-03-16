using Zenject;
using UnityEngine;

public class CaveGeneratorInstaller : MonoInstaller
{
    [SerializeField]
    private GeneratorValues values;

    int[,] map = new int[0, 0];
    public override void InstallBindings()
    {
        Container.Bind<int[,]>()
            .WithId(ZenjectIDs.BLACK_WHITE_MAP)
            .FromInstance(map)
            .AsSingle()
            .NonLazy();

        Container.Bind<CaveGenerator>()
            .AsSingle()
            .NonLazy();

        Container.Bind<CaveGeneratorVisualizer>()
            .FromNewComponentOnNewGameObject()
            .AsSingle()
            .NonLazy();

        Container.Bind<BlackWhiteMap>()
            .AsSingle()
            .NonLazy();

        Container.Bind<GeneratorValues>()
            .FromComponentInNewPrefab(values)
            .AsSingle()
            .NonLazy();
    }
}