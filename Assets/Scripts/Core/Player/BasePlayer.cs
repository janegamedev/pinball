using System;
using UnityEngine;

namespace Janegamedev.Core.Player
{
    /// <summary>
    /// Base class for players in the game.
    /// Currently used by human player, but later in the game AI player could inherit from this class if needed
    /// </summary>
    public class BasePlayer : MonoBehaviour
    {
        public static event Action<BasePlayer, int> OnAnyPlayerFireRequest;
        public static event Action<BasePlayer, int, int, bool> OnAnyFlipperActionReceived;
        
        [SerializeField]
        private BaseInputListener inputListener;

        private int teamId = 0;

        private void Awake()
        {
            // Registers input listener and handlers for flipper and fire actions.
            inputListener.OnAnyFlipperActionReceived += HandleAnyFlipperActionReceived;
            inputListener.OnFireActionTriggered += HandleFireActionTriggered;
        }

        private void OnDestroy()
        {
            // Unregisters input listener and handlers for flipper and fire actions.
            inputListener.OnAnyFlipperActionReceived -= HandleAnyFlipperActionReceived;
            inputListener.OnFireActionTriggered -= HandleFireActionTriggered;
        }
        
        /// <summary>
        /// Assigns team ID and activates input listener.
        /// </summary>
        /// <param name="id">The team ID to assign.</param>
        /// <param name="controlScheme">The control scheme to use for the player.</param>
        public void AssignTeam(int id, string controlScheme)
        {
            teamId = id;
            inputListener.ActivateListener(controlScheme);
        }

        /// <summary>
        /// Handles any flipper action received by broadcasting to listeners.
        /// </summary>
        /// <param name="listener">The input listener that received the flipper action.</param>
        /// <param name="flipperIndex">The index of the flipper that was activated or deactivated.</param>
        /// <param name="active">Whether the flipper was activated or deactivated.</param>
        private void HandleAnyFlipperActionReceived(BaseInputListener listener, int flipperIndex, bool active)
        {
            OnAnyFlipperActionReceived?.Invoke(this, teamId, flipperIndex, active);
        }
        
        /// <summary>
        /// Handles fire action triggered by broadcasting to listeners.
        /// </summary>
        /// <param name="listener">The input listener that triggered the fire action.</param>
        private void HandleFireActionTriggered(BaseInputListener listener)
        {
            OnAnyPlayerFireRequest?.Invoke(this, teamId);
        }
    }
}