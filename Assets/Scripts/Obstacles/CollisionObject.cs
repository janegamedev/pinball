using Janegamedev.Audio;
using Janegamedev.Core;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    public abstract class CollisionObject : MonoBehaviour, IBallCollidable
    {
        [SerializeField]
        private int scoreOnCollision = 10;

        [SerializeField]
        private float cooldown;

        [SerializeField]
        private string sfxID = "bumper";

        private float lastCollisionTime;

        public virtual void HandleBallCollision(BallCollisionController controller, Vector3 collisionPoint)
        {
            if (Time.fixedTime >= lastCollisionTime + cooldown)
            {
                PerformCollisionActions(controller, collisionPoint);
                AddScoreOnCollision();
                MusicPlayer.Instance.PlaySFX(sfxID);
                lastCollisionTime = Time.fixedTime;
            }
        }

        protected abstract void PerformCollisionActions(BallCollisionController controller, Vector3 collisionPoint);

        private void AddScoreOnCollision()
        {
            GameState.Instance.AddScore(scoreOnCollision);
        }
    }
}