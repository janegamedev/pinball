using System;
using UnityEngine;

namespace Janegamedev
{
    public class BallScoreZone : MonoBehaviour, IBallTriggerable
    {
        public static event Action<BallScoreZone, Ball> OnAnyBallEnteredScoreZone;
        
        public void HandleBallTrigger(BallCollisionController controller, CollisionEventType type)
        {
            OnAnyBallEnteredScoreZone?.Invoke(this, controller.Ball);
        }
    }
}