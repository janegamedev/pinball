using System;
using System.Collections.Generic;
using Janegamedev.Core;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    public class BallScoreZone : TriggerZone
    {
        public static event Action<BallScoreZone, Ball, int> OnAnyBallEnteredScoreZone;

        [SerializeField]
        private int teamId;

        private readonly HashSet<Ball> enteredBalls = new HashSet<Ball>();

        private void Start()
        {
            GameState.OnNewRoundStarted += HandleNewRoundStarted;
        }

        private void OnDestroy()
        {
            GameState.OnNewRoundStarted -= HandleNewRoundStarted;
        }

        public override void HandleBallTrigger(BallCollisionController controller, CollisionEventType type)
        {
            if (enteredBalls.Contains(controller.Ball))
            {
                return;
            }

            enteredBalls.Add(controller.Ball);
            OnAnyBallEnteredScoreZone?.Invoke(this, controller.Ball, teamId);
        }
        
        private void HandleNewRoundStarted(GameState state)
        {
            Reset();
        }

        public void Reset()
        {
            enteredBalls.Clear();
        }
    }
}