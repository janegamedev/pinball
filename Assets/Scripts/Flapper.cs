using UnityEngine;

namespace Janegamedev
{
    public class Flapper : MonoBehaviour
    {
        [SerializeField]
        private HingeJoint joint;

        [SerializeField]
        private float targetVelocity = 100f;
        [SerializeField]
        private float motorForce = 100f;
        
        private JointMotor motor = new JointMotor();

        private void Start()
        {
            Relax();
        }

        [ContextMenu("Flap")]
        public void Flap()
        {
            UpdateMotor(targetVelocity, motorForce);
        }
        
        [ContextMenu("Relax")]
        public void Relax()
        {
            UpdateMotor(0, 0);
        }

        private void UpdateMotor(float velocity, float force)
        {
            // Sets it to negative as we need negative velocity to rotate counter-clockwise
            motor.targetVelocity = -velocity;
            motor.force = force;
            joint.motor = motor;
        }
    }
}