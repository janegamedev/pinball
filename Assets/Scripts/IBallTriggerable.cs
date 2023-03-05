using UnityEngine;

namespace Janegamedev
{
    public enum CollisionEventType
    {
        Enter,
        Exit
    }
    
    public interface IBallTriggerable
    {
        void HandleBallTrigger(BallCollisionController controller, CollisionEventType type);
    }

    public interface IBallCollidable
    {
        void HandleBallCollision(BallCollisionController controller, Vector3 collisionPoint);
    }
}