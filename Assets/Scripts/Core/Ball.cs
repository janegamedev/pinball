using UnityEngine;

namespace Janegamedev.Core
{
    public class Ball : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody mainRigidbody;

        public bool IsEnabled { get; private set; } = true;
        
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
            IsEnabled = false;
            mainRigidbody.isKinematic = true;
        }

        public void EnableBall()
        {
            IsEnabled = true;
            mainRigidbody.isKinematic = false;
        }
    }
}