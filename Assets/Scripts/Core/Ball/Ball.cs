using System;
using UnityEngine;

namespace Janegamedev.Core
{
    public class Ball : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody mainRigidbody;
        [SerializeField]
        private Collider ballCollider;

        [SerializeField]
        private string collisionSfxId = "ballCollision";
        public string CollisionSfxId => collisionSfxId;

        public bool IsEnabled { get; private set; } = true;

        private void FixedUpdate()
        {
            // Dirty way to return the ball that's out of the bounds
            if (transform.position.y is < 0 or > 3)
            {
                Vector3 pos = transform.position;
                pos.y = 0.5f;
                transform.position = pos;
            }
        }

        public void AddImpulseForce(Vector3 dir)
        {
            mainRigidbody.AddForce(dir, ForceMode.Impulse);
        }

        public void AddExplosionForce(float explosionForce,
            Vector3 explosionPosition,
            float explosionRadius)
        {
            mainRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
        }

        public void DisableBall()
        {
            ActivateBall(false);
        }

        public void EnableBall()
        {
            ActivateBall(true);
        }

        private void ActivateBall(bool active)
        {
            IsEnabled = active;
            mainRigidbody.isKinematic = !active;
            ballCollider.enabled = active;
        }
    }
}