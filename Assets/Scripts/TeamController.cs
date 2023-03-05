using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Janegamedev
{
    public class TeamController : MonoBehaviour
    {
        [Serializable]
        public class PlayerSide
        {
            public BasePlayer player;
            public Flapper[] flappers = new Flapper[2];
            public BallScoreZone scoreZone;
        }

        [SerializeField]
        private PlayerSide[] playerSides = new PlayerSide[2];

        private HashSet<PlayerInputListener> joinedInputListeners = new HashSet<PlayerInputListener>();
        
        private void Awake()
        {
            PlayerInputListener.OnAnyPlayerInputJoinRequest += HandleAnyPlayerInputJoinRequest;
        }

        private void Start()
        {
            SpawnPlayers();
        }

        private void OnDestroy()
        {
            PlayerInputListener.OnAnyPlayerInputJoinRequest -= HandleAnyPlayerInputJoinRequest;
        }
        
        private void SpawnPlayers()
        {
            foreach (PlayerSide playerSide in playerSides)
            {
                playerSide.player.AssignSide(playerSide);
            }
        }
        
        private void HandleAnyPlayerInputJoinRequest(PlayerInputListener inputListener, PlayerInput playerInput)
        {
            joinedInputListeners.Add(inputListener);
        }
    }
}