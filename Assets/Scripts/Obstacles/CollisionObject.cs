using Janegamedev.Audio;
using Janegamedev.Core;
using Janegamedev.Core.Ball;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    /// <summary>
    /// Base class for a collision object that listens for ball collision with it
    /// </summary>
    public abstract class CollisionObject : MonoBehaviour, IBallCollidable
    {
        [SerializeField]
        private int scoreOnCollision = 10;

        [SerializeField]
        private float cooldown;

        [SerializeField]
        private string sfxID = "bumper";

        private float lastCollisionTime;

        /// <summary>
        /// Handles ball collision with the object
        /// </summary>
        /// <param name="controller">The ball collision controller</param>
        /// <param name="collisionPoint">The point of collision</param>
        public virtual void HandleBallCollision(BallCollisionController controller, Vector3 collisionPoint)
        {
            // Check if enough time has passed since the last collision before handling the current collision
            if (Time.fixedTime >= lastCollisionTime + cooldown)
            {
                // Perform actions specific to the collision object when the ball collides with it
                PerformCollisionActions(controller, collisionPoint);
                // Add score to the game when the ball collides with the object
                AddScoreOnCollision();
                // Play a sound effect to indicate the collision
                MusicPlayer.Instance.PlaySFX(sfxID);
                // Update the time of the last collision
                lastCollisionTime = Time.fixedTime;
            }
        }

        /// <summary>
        /// Performs collision actions
        /// Must be implemented in the inherited class
        /// </summary>
        /// <param name="controller">The ball collision controller</param>
        /// <param name="collisionPoint">The point of collision</param>
        protected abstract void PerformCollisionActions(BallCollisionController controller, Vector3 collisionPoint);

        /// <summary>
        /// Adds the score on collision to the game state.
        /// </summary>
        private void AddScoreOnCollision()
        {
            GameState.Instance.AddScore(scoreOnCollision);
        }
    }
}