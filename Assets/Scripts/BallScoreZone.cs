using System;

namespace Janegamedev
{
    public class BallScoreZone : TriggerZone
    {
        public static event Action<BallScoreZone, Ball> OnAnyBallEnteredScoreZone;
        
        public override void HandleBallTrigger(BallCollisionController controller, CollisionEventType type)
        {
            OnAnyBallEnteredScoreZone?.Invoke(this, controller.Ball);
        }
    }
}