using System;
using UnityEngine;

namespace Janegamedev
{
    public abstract class BaseInputListener : MonoBehaviour
    {
        public event Action<BaseInputListener, int, bool> OnAnyFlapperActionReceived;
        public event Action<BaseInputListener> OnFireActionTriggered;

        public virtual void ActivateListener()
        {
            
        }
        
        protected void BroadcastFlapperAction(int index, bool activate)
        {
            OnAnyFlapperActionReceived?.Invoke(this, index, activate);
        }
        
        protected void BroadcastFireActionTriggered()
        {
            OnFireActionTriggered?.Invoke(this);
        }
    }
}