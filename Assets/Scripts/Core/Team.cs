using System;
using UnityEngine;

namespace Janegamedev.Core
{
    /// <summary>
    /// Represents a team in the game, with a player type, control scheme, and score data.
    /// </summary>
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

        /// <summary>
        /// Increment the number of balls scored by this team
        /// </summary>
        public void IncrementScoredBalls()
        {
            ScoredBalls++;
        }

        /// <summary>
        /// Add a given score to the team's total score
        /// </summary>
        /// <param name="scoreToAdd">The score to be added</param>
        public void AddScore(long scoreToAdd)
        {
            Score = score + scoreToAdd;
        }

        #region Reset
        
        /// <summary>
        /// Reset the number of balls scored by the team for the current round to 0
        /// </summary>
        public void ResetRound()
        {
            ScoredBalls = 0;
        }

        /// <summary>
        /// Reset the team's total score to 0
        /// </summary>
        public void ResetGame()
        {
            Score = 0;
        }
        
        #endregion

        /// <summary>
        /// Compare the team by the team's score with another team
        /// </summary>
        /// <param name="other">The team to be compared to</param>
        /// <returns>1 if this team's score is less than the other team's score,
        /// -1 if this team's score is greater than the other team's score,
        /// and 0 if both scores are equal</returns>
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