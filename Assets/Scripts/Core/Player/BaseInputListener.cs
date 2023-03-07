using System;
using UnityEngine;

namespace Janegamedev.Core.Player
{
    /// <summary>
    /// Base abstract class for input listeners.
    /// Handles events for flipper actions and fire actions.
    /// </summary>
    public abstract class BaseInputListener : MonoBehaviour
    {
        public event Action<BaseInputListener, int, bool> OnAnyFlipperActionReceived;
        public event Action<BaseInputListener> OnFireActionTriggered;

        /// <summary>
        /// Activates the input listener with a given control scheme.
        /// </summary>
        /// <param name="controlScheme">The control scheme to activate the listener with.</param>
        public virtual void ActivateListener(string controlScheme) { }
        
        /// <summary>
        /// Broadcasts a flipper action to subscribers.
        /// </summary>
        /// <param name="index">The index of the flipper to broadcast.</param>
        /// <param name="activate">Whether to activate or deactivate the flipper.</param>
        protected void BroadcastFlipperAction(int index, bool activate)
        {
            OnAnyFlipperActionReceived?.Invoke(this, index, activate);
        }
        
        /// <summary>
        /// Broadcasts a fire action to subscribers if the game is not currently ignoring inputs.
        /// </summary>
        protected void BroadcastFireActionTriggered()
        {
            if (GameState.Instance.IgnoreInputs)
            {
                return;
            }
            
            OnFireActionTriggered?.Invoke(this);
        }
    }
}