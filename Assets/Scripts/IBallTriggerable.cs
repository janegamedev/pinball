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
}