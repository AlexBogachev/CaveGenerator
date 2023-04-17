using Assets.Scripts.Pathfinding;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player
{
    public class PathHandler
    {
        private Vehicle vehicle;

        private PathfindingAlgorithm algorithm;

        private WalkableTilesGrid tilesGrid;

        private ReactiveProperty<VehicleStatus> status;

        private List<Vector3> path = new List<Vector3>();

        private int currentIndex;

        public PathHandler(Vehicle vehicle, PathfindingAlgorithm algorithm, WalkableTilesGrid tilesGrid,
                          [Inject(Id = ZenjectIDs.VEHICLE_STATUS)] ReactiveProperty<VehicleStatus> status,
                          [Inject(Id = ZenjectIDs.GROUND_TOUCHED)] Subject<Vector3> groundTouched)
        {
            this.vehicle = vehicle;
            this.algorithm = algorithm; 
            this.tilesGrid = tilesGrid;
            this.status = status;

            status.Value = VehicleStatus.Idle;

            groundTouched
                .Subscribe(x =>
                {
                    vehicle.StopAllCoroutines();
                    status.Value = VehicleStatus.SearchingForTarget;
                    currentIndex = 1;
                    StartMove(vehicle.Position, x);
                })
                .AddTo(vehicle);
        }

        public async UniTaskVoid StartMove(Vector3 start, Vector3 end)
        {
            var startTile = tilesGrid.GetPathTileByCoord(start);
            var endTile = tilesGrid.GetPathTileByCoord(end);

            path = await algorithm.GetPath(startTile, endTile);

            if (TryGetNext(ref currentIndex, path, out Vector3 nextPoint))
            {
                vehicle.StartCoroutine(RotateToPoint(nextPoint));
            }
        }

        private IEnumerator RotateToPoint(Vector3 pos)
        {
            var correction = new Vector3(pos.x, vehicle.Position.y, pos.z);
            var lookAt = correction - vehicle.Position;
            var fromToRotation = Quaternion.LookRotation(lookAt, vehicle.transform.up);

            while (Quaternion.Angle(vehicle.Rotation, fromToRotation) > 1.0f && status.Value == VehicleStatus.SearchingForTarget)
            {
                vehicle.Rotation = Quaternion.Lerp(vehicle.Rotation, fromToRotation, Time.deltaTime * vehicle.Speed * 2.5f);
                yield return null;
            }
            vehicle.Rotation = fromToRotation;
            status.Value = VehicleStatus.Moving;
            vehicle.StartCoroutine(MoveToPoint(correction));
        }

        private IEnumerator MoveToPoint(Vector3 correction)
        {
            while (vehicle.Position != correction && status.Value == VehicleStatus.Moving)
            {
                vehicle.Position = Vector3.MoveTowards(vehicle.Position, correction, Time.deltaTime * vehicle.Speed);
                yield return null;
            }

            if (TryGetNext(ref currentIndex, path, out Vector3 nextPoint))
                vehicle.StartCoroutine(RotateToPoint(nextPoint));
            else
                status.Value = VehicleStatus.Idle;
        }

        private bool TryGetNext(ref int currentIndex, List<Vector3> points, out Vector3 nextPoint)
        {
            var maxIndex = points.Count - 2;
            if (currentIndex <= maxIndex)
            {
                nextPoint = points[currentIndex];
                currentIndex++;
                return true;
            }
            else
            {
                nextPoint = Vector3.positiveInfinity;
                return false;
            }
                
        }
    }
}