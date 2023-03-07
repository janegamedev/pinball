using Janegamedev.Audio;
using Janegamedev.Core;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    public class GravityPad : TriggerZone
    {
        private const string GRAVITY_SFX = "wooo";

        [SerializeField]
        private float noGravityDuration = 3f;
        
        public override void HandleBallTrigger(BallCollisionController controller, CollisionEventType type)
        {
            if (type == CollisionEventType.Enter)
            {
                GravityController.Instance.DisableGravity(noGravityDuration);
                MusicPlayer.Instance.PlaySFX(GRAVITY_SFX);
            }
        }
    }
}