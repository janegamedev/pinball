using System;
using UnityEngine;

namespace Janegamedev.Core
{
    [Serializable]
    public class Team : IComparable<Team>
    {
        public static event Action<Team> OnAnyTeamScoreUpdated;

        [SerializeField]
        private GameSettings.PlayerType playerType;
        public GameSettings.PlayerType PlayerType => playerType;
        [SerializeField]
        private string controlScheme = string.Empty;
        public string ControlScheme => controlScheme;
        [SerializeField]
        private int teamId = 0;
        public int TeamId => teamId;

        public int ScoredBalls { get; private set; }

        private long score;
        public long Score
        {
            get => score;
            private set
            {
                score = value;
                OnAnyTeamScoreUpdated?.Invoke(this);
            }
        }

        public void IncrementScoredBalls()
        {
            ScoredBalls++;
        }

        public void AddScore(long scoreToAdd)
        {
            Score = score + scoreToAdd;
        }

        public void ResetRound()
        {
            ScoredBalls = 0;
        }

        public void ResetGame()
        {
            Score = 0;
        }

        public int CompareTo(Team other)
        {
            if (other == null)
            {
                return 1;
            }

            if (Score > other.Score)
            {
                return -1;
            }

            return Score < other.Score ? 1 : 0;
        }
    }
}