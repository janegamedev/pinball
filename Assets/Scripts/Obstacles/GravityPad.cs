using Janegamedev.Audio;
using Janegamedev.Core;
using Janegamedev.Core.Ball;
using UnityEngine;
using UnityEngine.Serialization;

namespace Janegamedev.Obstacles
{
    /// <summary>
    /// A trigger zone that switches gravity when a ball enters it for a specified duration.
    /// </summary>
    public class GravityPad : TriggerZone
    {
        private const string GRAVITY_SFX = "wooo";
        
        [SerializeField]
        private float gravitySwitchDuration = 3f;
        
        /// <summary>
        /// Handles the event when a ball enters or exits the gravity pad.
        /// </summary>
        /// <param name="controller">The ball collision controller.</param>
        /// <param name="type">The type of collision event (enter or exit).</param>
        public override void HandleBallTrigger(BallCollisionController controller, CollisionEventType type)
        {
            if (type == CollisionEventType.Enter)
            {
                GravityController.Instance.SwitchGravity(gravitySwitchDuration);
                MusicPlayer.Instance.PlaySFX(GRAVITY_SFX);
            }
        }
    }
}