using Janegamedev.Audio;
using Janegamedev.Core;
using Janegamedev.Core.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Janegamedev.Obstacles
{
    /// <summary>
    /// Class that controls a flipper in the game.
    /// </summary>
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

        /// <summary>
        /// Handles any flipper action received by a player and sets the action accordingly
        /// </summary>
        /// <param name="player">The player whose action is being handled</param>
        /// <param name="teamId">The ID of the team that the flipper belongs to</param>
        /// <param name="flapperId">The ID of the flipper being activated</param>
        /// <param name="active">The state of the flipper (active or not)</param>
        private void HandleAnyFlipperActionReceived(BasePlayer player, int teamId, int flapperId, bool active)
        {
            if (teamID == teamId && flipperID == flapperId)
            {
                SetAction(active);
            }
        }

        /// <summary>
        /// Sets the action of the flipper based on the given state
        /// </summary>
        /// <param name="active">The state of the flipper (active or not)</param>
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

        /// <summary>
        /// Updates the motor of the hinge joint attached to the flipper with the given velocity and force
        /// </summary>
        /// <param name="velocity">The target velocity of the motor</param>
        /// <param name="force">The force of the motor</param>
        private void UpdateMotor(float velocity, float force)
        {
            // Sets it to negative as we need negative velocity to rotate counter-clockwise
            motor.targetVelocity = -velocity;
            motor.force = force;
            joint.motor = motor;
        }
    }
}