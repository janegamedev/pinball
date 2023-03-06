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
        private InputAction flapperLeftInputAction;
        private InputAction flapperRightInputAction;

        private void Awake()
        {
            flapperLeftInputAction = playerInput.actions.FindAction("FlapperLeft");
            flapperLeftInputAction.started += HandleFlapperLeftInputActionStarted;
            flapperLeftInputAction.canceled += HandleFlapperLeftInputActionCancelled;
            
            flapperRightInputAction = playerInput.actions.FindAction("FlapperRight");
            flapperRightInputAction.started += HandleFlapperRightInputActionStarted;
            flapperRightInputAction.canceled += HandleFlapperRightInputActionCancelled;
            
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
            flapperLeftInputAction.started -= HandleFlapperLeftInputActionStarted;
            flapperLeftInputAction.canceled -= HandleFlapperLeftInputActionCancelled;
            
            flapperRightInputAction.started -= HandleFlapperRightInputActionStarted;
            flapperRightInputAction.canceled -= HandleFlapperRightInputActionCancelled;
            
            fireInputAction.started -= HandleFireInputActionStarted;
        }

        #region Flappers

        private void HandleFlapperLeftInputActionStarted(InputAction.CallbackContext context)
        {
            BroadcastFlapperAction(0, true);
        }
        
        private void HandleFlapperLeftInputActionCancelled(InputAction.CallbackContext context)
        {
            BroadcastFlapperAction(0, false);
        }
        
        private void HandleFlapperRightInputActionStarted(InputAction.CallbackContext context)
        {
            BroadcastFlapperAction(1, true);
        }
        
        private void HandleFlapperRightInputActionCancelled(InputAction.CallbackContext context)
        {
            BroadcastFlapperAction(1, false);
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