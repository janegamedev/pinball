using UnityEngine;

namespace Janegamedev
{
    public class FrictionPad : TriggerZone
    {
        public override void HandleBallTrigger(BallCollisionController controller, CollisionEventType type)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AccelerationPad : TriggerZone
    {
        public override void HandleBallTrigger(BallCollisionController controller, CollisionEventType type)
        {
            throw new System.NotImplementedException();
        }
    }

    public abstract class TriggerZone : MonoBehaviour, IBallTriggerable
    {
        public abstract void HandleBallTrigger(BallCollisionController controller, CollisionEventType type);
    }

    public abstract class CollisionObject : MonoBehaviour, IBallCollidable
    {
        public abstract void HandleBallCollision(BallCollisionController controller, Vector3 collisionPoint);
    }
}