using UnityEngine;
using UnityEngine.InputSystem;

namespace Janegamedev.Core
{
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

        public override void ActivateListener(string controlScheme)
        {
            base.ActivateListener(controlScheme);
            playerInput.SwitchCurrentControlScheme(controlScheme, Keyboard.current);
        }

        private void OnDestroy()
        {
            flipperLeftInputAction.started -= HandleFlipperLeftInputActionStarted;
            flipperLeftInputAction.canceled -= HandleFlipperLeftInputActionCancelled;
            
            flipperRightInputAction.started -= HandleFlipperRightInputActionStarted;
            flipperRightInputAction.canceled -= HandleFlipperRightInputActionCancelled;
            
            fireInputAction.started -= HandleFireInputActionStarted;
        }

        #region Flappers

        private void HandleFlipperLeftInputActionStarted(InputAction.CallbackContext context)
        {
            BroadcastFlipperAction(0, true);
        }
        
        private void HandleFlipperLeftInputActionCancelled(InputAction.CallbackContext context)
        {
            BroadcastFlipperAction(0, false);
        }
        
        private void HandleFlipperRightInputActionStarted(InputAction.CallbackContext context)
        {
            BroadcastFlipperAction(1, true);
        }
        
        private void HandleFlipperRightInputActionCancelled(InputAction.CallbackContext context)
        {
            BroadcastFlipperAction(1, false);
        }
        
        #endregion

        #region Fire

        private void HandleFireInputActionStarted(InputAction.CallbackContext context)
        {
            BroadcastFireActionTriggered();
        }
        
        #endregion
    }
}