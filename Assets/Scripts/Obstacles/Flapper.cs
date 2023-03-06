using Janegamedev.Core;
using UnityEngine;

namespace Janegamedev.Obstacles
{
    public class Flapper : MonoBehaviour
    {
        [Header("Team")]
        [SerializeField]
        private int teamID = 0;
        [SerializeField]
        private int flapperID = 0;
        
        [Header("Joint")]
        [SerializeField]
        private HingeJoint joint;
        [SerializeField]
        private float targetVelocity = 100f;
        [SerializeField]
        private float motorForce = 100f;
        
        private JointMotor motor = new JointMotor();

        private void Start()
        {
            SetAction(false);
            BasePlayer.OnAnyFlapperActionReceived += HandleAnyFlapperActionReceived;
        }

        private void HandleAnyFlapperActionReceived(BasePlayer player, int teamId, int flapperId, bool active)
        {
            if (teamID == teamId && flapperID == flapperId)
            {
                SetAction(active);
            }
        }

        private void SetAction(bool active)
        {
            if (active)
            {
                UpdateMotor(targetVelocity, motorForce);
            }
            else
            {
                UpdateMotor(0, 0);
            }
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