using Janegamedev.Audio;
using Janegamedev.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Janegamedev.Obstacles
{
    public class Flipper : MonoBehaviour
    {
        private const string SFX_UP_ID = "flipperUp";
        private const string SFX_DOWN_ID = "flipperDown";
        
        [Header("Team")]
        [SerializeField]
        private int teamID = 0;
        [FormerlySerializedAs("flapperID")]
        [SerializeField]
        private int flipperID = 0;
        
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
            UpdateMotor(0, 0);
            BasePlayer.OnAnyFlipperActionReceived += HandleAnyFlipperActionReceived;
        }

        private void OnDestroy()
        {
            BasePlayer.OnAnyFlipperActionReceived -= HandleAnyFlipperActionReceived;
        }

        private void HandleAnyFlipperActionReceived(BasePlayer player, int teamId, int flapperId, bool active)
        {
            if (teamID == teamId && flipperID == flapperId)
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
            
            MusicPlayer.Instance.PlaySFX(active ? SFX_UP_ID : SFX_DOWN_ID);
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