using Janegamedev.Core;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    public interface IBallCollidable
    {
        void HandleBallCollision(BallCollisionController controller, Vector3 collisionPoint);
    }
}