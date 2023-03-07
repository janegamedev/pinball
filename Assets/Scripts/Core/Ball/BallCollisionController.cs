using System.Collections.Generic;
using Janegamedev.Audio;
using Janegamedev.Obstacles;
using UnityEngine;

namespace Janegamedev.Core.Ball
{
    /// <summary>
    /// Controls the collision and trigger events of a ball object.
    /// </summary>
    [RequireComponent(typeof(Ball))]
    public class BallCollisionController : MonoBehaviour
    {
        private const float SFX_WAIT_TIME_MIN = 0.1f;
        private const float SFX_WAIT_TIME_MAX = 0.3f;
        
        public Ball Ball { get; private set; }
        private string CollisionSFX => Ball.CollisionSfxId;

        private readonly HashSet<GameObject> enteredColliders = new HashSet<GameObject>();

        private float lastSfxPlayTime;
        private float nextRandomWait;

        private void Awake()
        {
            Ball = GetComponent<Ball>();
        }

        private void OnEnable()
        {
            Reset();
        }

        #region Collision

        /// <summary>
        /// Handles collision event of the ball object.
        /// </summary>
        /// <param name="other">The collider that the ball collided with.</param>
        private void OnCollisionEnter(Collision other)
        {
            // If the component or the ball is not enabled, return and do nothing.
            if (!enabled || !Ball.IsEnabled)
            {
                return;
            }

            // Get the collidable component from the collided object.
            Component collidable = (Component)other.transform.GetComponentInParent<IBallCollidable>();
            
            // If the collidable component exists, call HandleBallCollision on each one.
            if (collidable != null)
            {
                IBallCollidable[] ballCollidables = collidable.GetComponents<IBallCollidable>();

                // Get the collision point and call HandleBallCollision for each collidable component.
                Vector3 collisionPoint = other.contacts[0].point;
                foreach (IBallCollidable ballCollidable in ballCollidables)
                {
                    ballCollidable?.HandleBallCollision(this, collisionPoint);
                }
            }

            // If enough time has passed, play the collision sound effect.
            if (Time.fixedTime >= lastSfxPlayTime + nextRandomWait)
            {
                lastSfxPlayTime = Time.fixedTime;
                nextRandomWait = Random.Range(SFX_WAIT_TIME_MIN, SFX_WAIT_TIME_MAX);
                MusicPlayer.Instance.PlaySFX(CollisionSFX);
            }
        }

        #endregion

        #region Trigger
        
        /// <summary>
        /// Handles trigger event of the ball object when entering a collider.
        /// </summary>
        /// <param name="other">The collider that the ball entered.</param>
        private void OnTriggerEnter(Collider other)
        {
            // If the component or the ball is not enabled, return and do nothing.
            if (!enabled || !Ball.IsEnabled)
            {
                return;
            }
            
            // Check if the collider is already in the list of entered colliders.
            // If it is, return and do nothing to prevent re-entering.
            if (enteredColliders.Contains(other.gameObject))
            {
                return;
            }

            // Add the collider to the list of entered colliders.
            enteredColliders.Add(other.gameObject);
            // Trigger the object and pass the CollisionEventType as Enter.
            TriggerObject(other, CollisionEventType.Enter);
        }

        /// <summary>
        /// Handles trigger event of the ball object when exiting a collider.
        /// </summary>
        /// <param name="other">The collider that the ball exited.</param>
        private void OnTriggerExit(Collider other)
        {
            // If the component or the ball is not enabled, return and do nothing.
            if (!enabled || !Ball.IsEnabled)
            {
                return;
            }
            
            // Check if the collider is in the list of entered colliders.
            // If it's not, return and do nothing.
            bool isEnteredCollider = enteredColliders.Contains(other.gameObject);
            
            if (!isEnteredCollider)
            {
                return;
            }
            
            // Remove the collider from the list of entered colliders
            enteredColliders.Remove(other.gameObject);
            // Trigger the object and pass the CollisionEventType as Exit.
            TriggerObject(other, CollisionEventType.Exit);
        }

        /// <summary>
        /// Triggers the ball triggerable object based on the collision event type.
        /// </summary>
        /// <param name="other">The collider that the ball collided with.</param>
        /// <param name="collisionEventType">The type of collision event (Enter or Exit).</param>
        private void TriggerObject(Collider other, CollisionEventType collisionEventType)
        {
            Component triggerable = (Component)other.transform.GetComponentInParent<IBallTriggerable>();

            if (triggerable == null)
            {
                return;
            }
            
            // Get all IBallTriggerable components attached to the triggerable object and call HandleBallTrigger method on each
            IBallTriggerable[] ballTriggerables = triggerable.GetComponents<IBallTriggerable>();

            foreach (IBallTriggerable ballTriggerable in ballTriggerables)
            {
                ballTriggerable?.HandleBallTrigger(this, collisionEventType);
            }
        }
        
        #endregion

        /// <summary>
        /// Resets the enteredColliders list.
        /// </summary>
        public void Reset()
        {
            enteredColliders.Clear();
        }
    }
}