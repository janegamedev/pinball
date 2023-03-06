using System;
using Janegamedev.Core;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    public class BallScoreZone : TriggerZone
    {
        public static event Action<BallScoreZone, Ball, int> OnAnyBallEnteredScoreZone;

        [SerializeField]
        private int teamId;
        
        public override void HandleBallTrigger(BallCollisionController controller, CollisionEventType type)
        {
            OnAnyBallEnteredScoreZone?.Invoke(this, controller.Ball, teamId);
        }
    }
}