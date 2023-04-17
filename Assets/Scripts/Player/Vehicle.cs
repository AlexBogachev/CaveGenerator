using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player
{

    public enum VehicleStatus
    {
        Idle,
        SearchingForTarget,
        Moving
    }

    public class Vehicle: MonoBehaviour
    {
        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return transform.rotation;
            }
            set
            {
                transform.rotation = value;
            }
        }

        public float Speed { get; private set; }

        private BoxCollider boxCollider;

        private Rigidbody rigidBody;

        [Inject]
        private void Constructor(Vector3 spawnPoint)
        {
            Speed = 5.0f;

            boxCollider = GetComponent<BoxCollider>();
            rigidBody = GetComponent<Rigidbody>();

            var spawnPosition = spawnPoint + new Vector3(0.0f, boxCollider.size.y, 0.0f);
            transform.position = spawnPosition;

            var tokenSource = new CancellationTokenSource();
            //groundTouched
            //    .Subscribe(touch =>
            //    {
            //        //tokenSource.Cancel();
            //        //StartMove(touch, tokenSource.Token);

            //        StopAllCoroutines();
            //        StartCoroutine(RotateToPoint(touch));

            //    })
            //    .AddTo(this);
        }

        private async UniTaskVoid StartMove(Vector3 touchPosition, CancellationToken token)
        {
            var(isCanceled, isFinished) = await MoveToPointAsync(touchPosition, token).SuppressCancellationThrow();
            if (isCanceled)
            {
                Debug.Log("CANCELED");
                return;
            }
        }

        private async UniTask<bool> MoveToPointAsync(Vector3 pos, CancellationToken token)
        {
            var correction = new Vector3(pos.x, transform.position.y, pos.z);
            var lookAt = correction - transform.position;

            var (isCanceled, isFinished) =  await RotateToPointAsync(lookAt, token).SuppressCancellationThrow();
            if (isCanceled)
                return false;

            while (transform.position != correction)
            {
                transform.position = Vector3.MoveTowards(transform.position, correction, Time.deltaTime * Speed);
                await UniTask.WaitForFixedUpdate();
            }
            return true;
        }

        private async UniTask<bool> RotateToPointAsync(Vector3 lookAt, CancellationToken token)
        {

            var fromToRotation = Quaternion.LookRotation(lookAt, transform.up);

            while (transform.rotation != fromToRotation)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, fromToRotation, Time.deltaTime * Speed * 2.5f);
                await UniTask.WaitForFixedUpdate();
            }
            return true;
        }

       

        

        public class Factory:PlaceholderFactory<Vector3, Vehicle> { }
    }
}
