using System;
using UnityEngine;

namespace Janegamedev.Core
{
    public abstract class BaseInputListener : MonoBehaviour
    {
        public event Action<BaseInputListener, int, bool> OnAnyFlipperActionReceived;
        public event Action<BaseInputListener> OnFireActionTriggered;

        public virtual void ActivateListener(string controlScheme) { }
        
        protected void BroadcastFlipperAction(int index, bool activate)
        {
            OnAnyFlipperActionReceived?.Invoke(this, index, activate);
        }
        
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