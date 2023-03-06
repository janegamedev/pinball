using UnityEngine;

namespace Janegamedev.Core
{
    [RequireComponent(typeof(BallCollisionController))]
    public class Ball : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody mainRigidbody;
        public Rigidbody MainRigidbody => mainRigidbody;
        
        [SerializeField]
        private BallCollisionController collisionController;
        
        public void AddForce(Vector3 dir)
        {
            mainRigidbody.AddForce(dir, ForceMode.Impulse);
        }
    }
}