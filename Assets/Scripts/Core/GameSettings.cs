using System;
using Janegamedev.Core.Player;
using UnityEngine;

namespace Janegamedev.Core
{
    [CreateAssetMenu(menuName = "Settings/Game", fileName = "GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        public enum PlayerType
        {
            Human,
            AI
        }

        [Serializable]
        public class PlayerTypePrefab
        {
            public PlayerType playerType;
            public BasePlayer playerTemplate;
        }

        [SerializeField]
        private PlayerTypePrefab[] playerTypePrefabSet = new PlayerTypePrefab[2];

        [Header("Gameplay")]
        [SerializeField]
        private int maxRounds = 3;
        public int MaxRounds => maxRounds;

        public BasePlayer GetPlayerTemplateByType(PlayerType playerType)
        {
            foreach (PlayerTypePrefab playerTypePrefab in playerTypePrefabSet)
            {
                if (playerTypePrefab.playerType == playerType)
                {
                    return playerTypePrefab.playerTemplate;
                }
            }

            Debug.LogError($"PlayerTypePrefab with {playerType} type doesn't exist in the game settings");
            return null;
        }
    }
}