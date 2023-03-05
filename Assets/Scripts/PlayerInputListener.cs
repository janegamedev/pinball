using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Janegamedev
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputListener : BaseInputListener
    {
        public static event Action<PlayerInputListener, PlayerInput> OnAnyPlayerInputJoinRequest;
        
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
            fireInputAction.canceled += HandleFireInputActionCancelled;
        }

        public override void ActivateListener()
        {
            base.ActivateListener();
            playerInput.SwitchCurrentControlScheme(playerInput.defaultControlScheme, Keyboard.current);
            OnAnyPlayerInputJoinRequest?.Invoke(this, playerInput);
        }

        private void OnDestroy()
        {
            flapperLeftInputAction.started -= HandleFlapperLeftInputActionStarted;
            flapperLeftInputAction.canceled -= HandleFlapperLeftInputActionCancelled;
            
            flapperRightInputAction.started -= HandleFlapperRightInputActionStarted;
            flapperRightInputAction.canceled -= HandleFlapperRightInputActionCancelled;
            
            fireInputAction.started -= HandleFireInputActionStarted;
            fireInputAction.canceled -= HandleFireInputActionCancelled;
        }

        #region Flappers

        private void HandleFlapperLeftInputActionStarted(InputAction.CallbackContext context)
        {
            BroadcastFlapperAction(0, true);
        }
        
        private void HandleFlapperLeftInputActionCancelled(InputAction.CallbackContext context)
        {
            BroadcastFlapperAction(0, false);
            Debug.Log("HandleFlapperLeftInputActionCancelled");
        }
        
        private void HandleFlapperRightInputActionStarted(InputAction.CallbackContext context)
        {
            BroadcastFlapperAction(1, true);
        }
        
        private void HandleFlapperRightInputActionCancelled(InputAction.CallbackContext context)
        {
            BroadcastFlapperAction(1, false);
            Debug.Log("HandleFlapperRightInputActionCancelled");
        }
        
        #endregion

        #region Fire

        private void HandleFireInputActionStarted(InputAction.CallbackContext context)
        {
            BroadcastFireActionTriggered();
        }
        
        private void HandleFireInputActionCancelled(InputAction.CallbackContext context)
        {
            Debug.Log("Fire cancelled");
        }
        
        #endregion
    }
}