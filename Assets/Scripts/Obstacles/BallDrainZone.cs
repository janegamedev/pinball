using System;
using System.Collections.Generic;
using Janegamedev.Audio;
using Janegamedev.Core;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    public class BallDrainZone : TriggerZone
    {
        private const string DRAIN_SFX_ID = "drain";
        
        public static event Action<BallDrainZone, Ball, int> OnAnyBallEnteredDrainZone;

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
            MusicPlayer.Instance.PlaySFX(DRAIN_SFX_ID);
            OnAnyBallEnteredDrainZone?.Invoke(this, controller.Ball, teamId);
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