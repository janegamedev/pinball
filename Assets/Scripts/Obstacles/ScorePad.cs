using System;
using Janegamedev.Audio;
using Janegamedev.Core;
using UnityEngine;

namespace Janegamedev.Obstacles
{
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