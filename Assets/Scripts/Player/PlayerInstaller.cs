using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player
{
    public class PlayerInstaller: MonoInstaller
    {
        [SerializeField]
        private Vehicle vehiclePrefab;

        public override void InstallBindings()
        {
            Container.Bind<List<Vector3>>()
                .WithId(ZenjectIDs.SPAWN_POINTS)
                .FromInstance(new List<Vector3>())
                .AsSingle()
                .NonLazy();

            Container.BindFactory<Vector3, Vehicle, Vehicle.Factory>()
                .FromSubContainerResolve()
                .ByMethod(InstallVehicle)
                .AsSingle();

            Container.Bind<VehicleFactory>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }

        private void InstallVehicle(DiContainer subContainer, Vector3 position)
        {
            subContainer.Bind<Vehicle>()
                .FromComponentInNewPrefab(vehiclePrefab)
                .AsSingle()
                .WithArguments(position)
                .NonLazy();

            subContainer.Bind<ReactiveProperty<VehicleStatus>>()
                .WithId(ZenjectIDs.VEHICLE_STATUS)
                .FromInstance(new ReactiveProperty<VehicleStatus>())
                .AsSingle()
                .NonLazy();

            subContainer.Bind<PathHandler>()
                .AsSingle()
                .NonLazy();
        }
    }
}