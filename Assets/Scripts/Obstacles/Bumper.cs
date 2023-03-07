using Janegamedev.Core;
using Janegamedev.Core.Ball;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    /// <summary>
    /// Represents a bumper that applies an explosion force to a ball when a collision occurs.
    /// </summary>
    public class Bumper : CollisionObject
    {
        [SerializeField]
        private float explosionBounceForce = 100;
        [SerializeField]
        private float explosionRadius = 0.2f;
        
        /// <summary>
        /// Adds an explosion force to the ball that collided with this bumper.
        /// </summary>
        /// <param name="controller">The ball collision controller of the colliding ball.</param>
        /// <param name="collisionPoint">The point of collision.</param>
        protected override void PerformCollisionActions(BallCollisionController controller, Vector3 collisionPoint)
        {
            controller.Ball.AddExplosionForce(explosionBounceForce, collisionPoint, explosionRadius);
        }
    }
}