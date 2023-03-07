using Janegamedev.Core;
using Janegamedev.Core.Ball;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    public interface IBallCollidable
    {
        void HandleBallCollision(BallCollisionController controller, Vector3 collisionPoint);
    }
}