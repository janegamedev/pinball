using Janegamedev.Core;

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