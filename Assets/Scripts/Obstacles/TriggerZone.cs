using Janegamedev.Core;
using Janegamedev.Core.Ball;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    /// <summary>
    /// Base class for a trigger zone that listens for ball trigger enter or exit
    /// </summary>
    public abstract class TriggerZone : MonoBehaviour, IBallTriggerable
    {
        /// <summary>
        /// Handle ball trigger event when a ball enters or exits the trigger zone.
        /// </summary>
        /// <param name="controller">The ball collision controller.</param>
        /// <param name="type">The type of collision event.</param>
        public abstract void HandleBallTrigger(BallCollisionController controller, CollisionEventType type);
    }
}