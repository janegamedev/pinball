using Janegamedev.Core;
using Janegamedev.Core.Ball;

namespace Janegamedev.Obstacles
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
}