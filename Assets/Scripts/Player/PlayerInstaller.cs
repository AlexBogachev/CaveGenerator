using System.Collections.Generic;
using Zenject;

namespace Assets.Scripts.Player
{
    public class PlayerInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<List<(int, int)>>()
                .WithId(ZenjectIDs.SPAWN_POINTS)
                .FromInstance(new List<(int, int)>())
                .AsSingle()
                .NonLazy();
                
        }
    }
}