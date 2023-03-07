using System;
using Janegamedev.Obstacles;
using UnityEngine;

namespace Janegamedev.Core
{
    public class BasePlayer : MonoBehaviour
    {
        public static event Action<BasePlayer, int> OnAnyPlayerFireRequest;
        public static event Action<BasePlayer, int, int, bool> OnAnyFlipperActionReceived;
        
        [SerializeField]
        private BaseInputListener inputListener;

        private int teamId = 0;

        private void Awake()
        {
            inputListener.OnAnyFlipperActionReceived += HandleAnyFlipperActionReceived;
            inputListener.OnFireActionTriggered += HandleFireActionTriggered;
        }

        private void OnDestroy()
        {
            inputListener.OnAnyFlipperActionReceived -= HandleAnyFlipperActionReceived;
            inputListener.OnFireActionTriggered -= HandleFireActionTriggered;
        }
        
        public void AssignTeam(int id, string controlScheme)
        {
            teamId = id;
            inputListener.ActivateListener(controlScheme);
        }

        private void HandleAnyFlipperActionReceived(BaseInputListener listener, int flipperIndex, bool active)
        {
            OnAnyFlipperActionReceived?.Invoke(this, teamId, flipperIndex, active);
        }
        
        private void HandleFireActionTriggered(BaseInputListener obj)
        {
            OnAnyPlayerFireRequest?.Invoke(this, teamId);
        }
    }
}