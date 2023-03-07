using UnityEngine;

namespace Janegamedev.Core.Ball
{
    /// <summary>
    /// Represents the ball in the pinball game
    /// </summary>
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
            // Dirty way to move the ball back onto the playfield if it goes out of bounds
            if (transform.position.y is < 0 or > 3)
            {
                Vector3 pos = transform.position;
                pos.y = 0.5f;
                transform.position = pos;
            }
        }

        /// <summary>
        /// Adds an impulse force to the ball in the given direction.
        /// </summary>
        /// <param name="dir">The direction in which to add the force.</param>
        public void AddImpulseForce(Vector3 dir)
        {
            mainRigidbody.AddForce(dir, ForceMode.Impulse);
        }

        /// <summary>
        /// Adds an explosion force to the ball at the given position, with the given force and radius.
        /// </summary>
        /// <param name="explosionForce">The force of the explosion.</param>
        /// <param name="explosionPosition">The position of the explosion.</param>
        /// <param name="explosionRadius">The radius of the explosion.</param>
        public void AddExplosionForce(float explosionForce,
            Vector3 explosionPosition,
            float explosionRadius)
        {
            mainRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
        }

        /// <summary>
        /// Disables the ball, preventing it from moving and colliding
        /// </summary>
        public void DisableBall()
        {
            ActivateBall(false);
        }

        /// <summary>
        /// Enables the ball, allowing it to move and collide
        /// </summary>
        public void EnableBall()
        {
            ActivateBall(true);
        }

        /// <summary>
        /// Activates or deactivates the ball, depending on the value of the active parameter.
        /// </summary>
        /// <param name="active">Whether to activate or deactivate the ball.</param>
        private void ActivateBall(bool active)
        {
            IsEnabled = active;
            mainRigidbody.isKinematic = !active;
            ballCollider.enabled = active;
        }
    }
}