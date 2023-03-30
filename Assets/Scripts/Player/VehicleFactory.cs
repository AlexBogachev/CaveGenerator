using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player
{
    public class VehicleFactory


    {
        public VehicleFactory([Inject(Id = ZenjectIDs.CAVE_CREATED)] Subject<Unit> caveCreated,
                              [Inject(Id = ZenjectIDs.SPAWN_POINTS)] List<Vector3> spawnPoints,
                              Vehicle.Factory factory)
        {
            caveCreated
                    .Subscribe(_ =>
                    {
                        factory.Create(spawnPoints[0]);
                    });
        }
    }
}