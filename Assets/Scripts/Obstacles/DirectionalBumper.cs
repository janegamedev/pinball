using Janegamedev.Core;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    public class DirectionalBumper : CollisionObject
    {
        [SerializeField]
        private float force = 100f;
        [SerializeField]
        private Vector3 direction;
        
        protected override void PerformCollisionActions(BallCollisionController controller, Vector3 collisionPoint)
        {
            controller.Ball.AddImpulseForce(direction * force);
        }
    }
}