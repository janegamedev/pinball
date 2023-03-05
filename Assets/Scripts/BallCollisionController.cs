using System.Collections.Generic;
using UnityEngine;

namespace Janegamedev
{
    public class BallCollisionController : MonoBehaviour
    {
        public Ball Ball { get; private set; }

        private readonly HashSet<GameObject> enteredColliders = new HashSet<GameObject>();

        private void Awake()
        {
            Ball = GetComponent<Ball>();
        }

        private void OnEnable()
        {
            Reset();
        }

        #region Collision

        private void OnCollisionEnter(Collision other)
        {
            if (!enabled)
            {
                return;
            }
            
            Component collidable = (Component)other.transform.GetComponentInParent<IBallCollidable>();
            
            if (collidable != null)
            {
                IBallCollidable[] ballCollidables = collidable.GetComponents<IBallCollidable>();

                Vector3 collisionPoint = other.contacts[0].point;
                foreach (IBallCollidable ballCollidable in ballCollidables)
                {
                    ballCollidable?.HandleBallCollision(this, collisionPoint);
                }
            }
        }

        #endregion

        #region Trigger
        
        private void OnTriggerEnter(Collider other)
        {
            // Check if already entered the same collider
            // Made to prevent the child of this component to trigger OnTriggerEnter which results in entering more than once
            if (enteredColliders.Contains(other.gameObject))
            {
                return;
            }
            
            enteredColliders.Add(other.gameObject);
            TriggerObject(other, CollisionEventType.Enter);
        }

        private void OnTriggerExit(Collider other)
        {
            bool isEnteredCollider = enteredColliders.Contains(other.gameObject);
            
            if (!isEnteredCollider)
            {
                return;
            }
            
            enteredColliders.Remove(other.gameObject);
            TriggerObject(other, CollisionEventType.Exit);
        }

        private void TriggerObject(Collider other, CollisionEventType collisionEventType)
        {
            Component triggerable = (Component)other.transform.GetComponentInParent<IBallTriggerable>();

            if (triggerable == null)
            {
                return;
            }
            
            IBallTriggerable[] ballTriggerables = triggerable.GetComponents<IBallTriggerable>();

            foreach (IBallTriggerable ballTriggerable in ballTriggerables)
            {
                ballTriggerable?.HandleBallTrigger(this, collisionEventType);
            }
        }
        
        #endregion

        public void Reset()
        {
            enteredColliders.Clear();
        }
    }
}