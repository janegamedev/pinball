using System;
using System.Collections.Generic;
using UnityEngine;

namespace Janegamedev.Core
{
    public class TeamController : MonoBehaviour
    {
        [Serializable]
        public class Team
        {
            public GameSettings.PlayerType playerType;
            public string controlScheme = string.Empty;
        }
        
        [SerializeField]
        private Team[] playerSides = new Team[2];
        
        private readonly HashSet<BasePlayer> spawnedPlayers = new HashSet<BasePlayer>();

        private void Awake()
        {
            GameState.OnGameStarted += HandleGameStarted;
        }

        private void OnDestroy()
        {
            GameState.OnGameStarted -= HandleGameStarted;
        }

        private void HandleGameStarted(GameState gameState)
        {
            SpawnPlayers();
        }

        private void SpawnPlayers()
        {
            DestroyPlayer();

            for (int i = 0; i < playerSides.Length; i++)
            {
                Team playerTeam = playerSides[i];
                BasePlayer templateByType = GameState.Instance.GameSettings.GetPlayerTemplateByType(playerTeam.playerType);

                if (templateByType == null)
                {
                    continue;
                }

                BasePlayer player = Instantiate(templateByType, transform);
                player.AssignTeam(i, playerTeam.controlScheme);
                spawnedPlayers.Add(player);
            }
        }

        private void DestroyPlayer()
        {
            foreach (BasePlayer spawnedPlayer in spawnedPlayers)
            {
                Destroy(spawnedPlayer.gameObject);
            }
            
            spawnedPlayers.Clear();
        }
    }
}