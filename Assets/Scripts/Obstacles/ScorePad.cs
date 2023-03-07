using System;
using Janegamedev.Audio;
using Janegamedev.Core;
using Janegamedev.Core.Ball;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    /// <summary>
    /// Represents a score pad that provides points to the total score when the ball collides with it.
    /// </summary>
    public class ScorePad : TriggerZone
    {
        private const string SCORE_PAD_SFX = "score";
        
        public enum ScoreType
        {
            Addition,
            Multiply
        }

        [SerializeField]
        private ScoreType scoreType;
        [SerializeField]
        private int amount;
        
        /// <summary>
        /// Handles ball triggers on this score pad.
        /// </summary>
        /// <param name="controller">The ball collision controller.</param>
        /// <param name="type">The collision event type.</param>
        public override void HandleBallTrigger(BallCollisionController controller, CollisionEventType type)
        {
            if (type == CollisionEventType.Enter)
            {
                switch (scoreType)
                {
                    case ScoreType.Addition:
                        GameState.Instance.AddScore(amount);
                        break;
                    case ScoreType.Multiply:
                        GameState.Instance.MultiplyScore(amount);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                MusicPlayer.Instance.PlaySFX(SCORE_PAD_SFX);
            }
        }
    }
}