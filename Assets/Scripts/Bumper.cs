using UnityEngine;

namespace Janegamedev
{
    public class Bumper : CollisionObject
    {
        [SerializeField]
        private float explosionBounceForce = 100;
        [SerializeField]
        private float explosionRadius = 0.2f;
        
        public override void HandleBallCollision(BallCollisionController controller, Vector3 collisionPoint)
        {
            controller.Ball.MainRigidbody.AddExplosionForce(explosionBounceForce, collisionPoint, explosionRadius);
        }
    }
}