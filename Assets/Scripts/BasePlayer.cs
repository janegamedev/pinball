using System;
using UnityEngine;

namespace Janegamedev
{
    public class BasePlayer : MonoBehaviour
    {
        [SerializeField]
        private BaseInputListener inputListener;
        
        private Flapper[] Flappers => playerSide?.flappers;
        private BallScoreZone ScoreZone => playerSide.scoreZone;
        
        private TeamController.PlayerSide playerSide;

        private void Awake()
        {
            inputListener.OnAnyFlapperActionReceived += HandleAnyFlapperActionReceived;
            inputListener.OnFireActionTriggered += HandleFireActionTriggered;
        }

        private void OnDestroy()
        {
            inputListener.OnAnyFlapperActionReceived -= HandleAnyFlapperActionReceived;
            inputListener.OnFireActionTriggered -= HandleFireActionTriggered;
        }
        
        public void AssignSide(TeamController.PlayerSide side)
        {
            playerSide = side;
            inputListener.ActivateListener();
        }

        private void HandleAnyFlapperActionReceived(BaseInputListener listener, int flipperIndex, bool active)
        {
            Flappers?[flipperIndex].SetAction(active);
        }
        
        private void HandleFireActionTriggered(BaseInputListener obj)
        {
            
        }
    }
}