using Janegamedev.Core;
using Janegamedev.Core.Ball;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    /// <summary>
    /// Represents a bumper that applies a directional force to a ball when a collision occurs.
    /// </summary>
    public class DirectionalBumper : CollisionObject
    {
        [SerializeField]
        private float force = 100f;
        [SerializeField]
        private Vector3 direction;
        
        /// <summary>
        /// Adds a directional force to the ball that collided with this bumper.
        /// </summary>
        /// <param name="controller">The ball collision controller of the colliding ball.</param>
        /// <param name="collisionPoint">The point of collision.</param>
        protected override void PerformCollisionActions(BallCollisionController controller, Vector3 collisionPoint)
        {
            controller.Ball.AddImpulseForce(direction * force);
        }
    }
}