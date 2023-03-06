using Janegamedev.Core;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    public class Bumper : CollisionObject
    {
        [SerializeField]
        private float explosionBounceForce = 100;
        [SerializeField]
        private float explosionRadius = 0.2f;
        
        protected override void PerformCollisionActions(BallCollisionController controller, Vector3 collisionPoint)
        {
            controller.Ball.MainRigidbody.AddExplosionForce(explosionBounceForce, collisionPoint, explosionRadius);
        }
    }
}