using UnityEngine;
using UnityEngine.InputSystem;

namespace Janegamedev.Core.Player
{
    /// <summary>
    /// A player input listener that listens to input actions and broadcasts them to other objects.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputListener : BaseInputListener
    {
        [SerializeField]
        private PlayerInput playerInput;
        
        private InputAction fireInputAction;
        private InputAction flipperLeftInputAction;
        private InputAction flipperRightInputAction;

        private void Awake()
        {
            flipperLeftInputAction = playerInput.actions.FindAction("FlipperLeft");
            flipperLeftInputAction.started += HandleFlipperLeftInputActionStarted;
            flipperLeftInputAction.canceled += HandleFlipperLeftInputActionCancelled;
            
            flipperRightInputAction = playerInput.actions.FindAction("FlipperRight");
            flipperRightInputAction.started += HandleFlipperRightInputActionStarted;
            flipperRightInputAction.canceled += HandleFlipperRightInputActionCancelled;
            
            fireInputAction = playerInput.actions.FindAction("Fire");
            fireInputAction.started += HandleFireInputActionStarted;
        }

        private void OnDestroy()
        {
            flipperLeftInputAction.started -= HandleFlipperLeftInputActionStarted;
            flipperLeftInputAction.canceled -= HandleFlipperLeftInputActionCancelled;
            
            flipperRightInputAction.started -= HandleFlipperRightInputActionStarted;
            flipperRightInputAction.canceled -= HandleFlipperRightInputActionCancelled;
            
            fireInputAction.started -= HandleFireInputActionStarted;
        }

        /// <summary>
        /// Activates the input listener for a given control scheme.
        /// </summary>
        /// <param name="controlScheme">The control scheme to activate.</param>
        public override void ActivateListener(string controlScheme)
        {
            base.ActivateListener(controlScheme);
            
            // Set control scheme and keyboard as a desired device
            playerInput.SwitchCurrentControlScheme(controlScheme, Keyboard.current);
        }
        
        #region Flappers

        /// <summary>
        /// Handles the start of the flipper left input action and broadcasts it to other objects.
        /// </summary>
        private void HandleFlipperLeftInputActionStarted(InputAction.CallbackContext context)
        {
            BroadcastFlipperAction(0, true);
        }
        
        /// <summary>
        /// Handles the cancel of the flipper left input action and broadcasts it to other objects.
        /// </summary>
        private void HandleFlipperLeftInputActionCancelled(InputAction.CallbackContext context)
        {
            BroadcastFlipperAction(0, false);
        }
        
        /// <summary>
        /// Handles the start of the flipper right input action and broadcasts it to other objects.
        /// </summary>
        private void HandleFlipperRightInputActionStarted(InputAction.CallbackContext context)
        {
            BroadcastFlipperAction(1, true);
        }
        
        /// <summary>
        /// Handles the cancel of the flipper right input action and broadcasts it to other objects.
        /// </summary>
        private void HandleFlipperRightInputActionCancelled(InputAction.CallbackContext context)
        {
            BroadcastFlipperAction(1, false);
        }
        
        #endregion

        #region Fire

        /// <summary>
        /// Handles the start of the fire input action and broadcasts it to other objects.
        /// </summary>
        private void HandleFireInputActionStarted(InputAction.CallbackContext context)
        {
            BroadcastFireActionTriggered();
        }
        
        #endregion
    }
}