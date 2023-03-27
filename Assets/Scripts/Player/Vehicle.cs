using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player
{
    public class Vehicle: MonoBehaviour
    {
        private BoxCollider boxCollider;

        private Rigidbody rigidBody;

        [Inject]
        private void Constructor(Vector3 spawnPoint)
        {
            boxCollider = GetComponent<BoxCollider>();
            rigidBody = GetComponent<Rigidbody>();

            var spawnPosition = spawnPoint + new Vector3(0.0f, boxCollider.size.y, 0.0f);
            Debug.Log("POS= " + spawnPosition);
            transform.position = spawnPosition;
        }

        public class Factory:PlaceholderFactory<Vector3, Vehicle> { }
    }
}
