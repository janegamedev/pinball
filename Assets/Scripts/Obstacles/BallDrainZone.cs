using System;
using System.Collections.Generic;
using Janegamedev.Audio;
using Janegamedev.Core;
using Janegamedev.Core.Ball;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    /// <summary>
    /// A drain trigger zone that listens for ball trigger enter
    /// </summary>
    public class BallDrainZone : TriggerZone
    {
        private const string DRAIN_SFX_ID = "drain";
        
        public static event Action<BallDrainZone, Ball, int> OnAnyBallEnteredDrainZone;

        [SerializeField]
        private int teamId;

        private readonly HashSet<Ball> enteredBalls = new HashSet<Ball>();

        private void Start()
        {
            // Subscribe to new round started event
            GameState.OnNewRoundStarted += HandleNewRoundStarted;
        }

        private void OnDestroy()
        {
            // Unsubscribe from new round started event
            GameState.OnNewRoundStarted -= HandleNewRoundStarted;
        }

        /// <summary>
        /// Handle ball trigger enter event, and drain the ball
        /// </summary>
        /// <param name="controller">The ball collision controller</param>
        /// <param name="type">The type of the collision event</param>
        public override void HandleBallTrigger(BallCollisionController controller, CollisionEventType type)
        {
            if (type == CollisionEventType.Enter)
            {
                if (enteredBalls.Contains(controller.Ball))
                {
                    return;
                }

                enteredBalls.Add(controller.Ball);
                MusicPlayer.Instance.PlaySFX(DRAIN_SFX_ID);
                OnAnyBallEnteredDrainZone?.Invoke(this, controller.Ball, teamId);
            }
        }
        
        /// <summary>
        /// Handle new round started event, and reset the drain zone
        /// </summary>
        /// <param name="state">The game state</param>
        private void HandleNewRoundStarted(GameState state)
        {
            Reset();
        }

        /// <summary>
        /// Reset the drain zone
        /// </summary>
        public void Reset()
        {
            enteredBalls.Clear();
        }
    }
}