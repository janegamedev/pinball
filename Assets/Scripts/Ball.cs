using System;
using UnityEngine;

namespace Janegamedev
{
    [RequireComponent(typeof(BallCollisionController))]
    public class Ball : MonoBehaviour
    {
        public static event Action<Ball> OnAnyBallScored;
        
        [SerializeField]
        private Rigidbody mainRigidbody;
        public Rigidbody MainRigidbody => mainRigidbody;
        
        [SerializeField]
        private BallCollisionController collisionController;
        
        public void AddForce(Vector3 dir)
        {
            mainRigidbody.AddForce(dir, ForceMode.Impulse);
        }

        public void BroadcastBallScored()
        {
            OnAnyBallScored?.Invoke(this);
        }
    }
}