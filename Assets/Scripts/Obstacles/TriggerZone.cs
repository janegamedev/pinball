using Janegamedev.Core;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    public abstract class TriggerZone : MonoBehaviour, IBallTriggerable
    {
        public abstract void HandleBallTrigger(BallCollisionController controller, CollisionEventType type);
    }
}