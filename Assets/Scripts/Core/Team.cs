using System;

namespace Janegamedev.Core
{
    [Serializable]
    public class Team : IComparable<Team>
    {
        public static event Action<Team> OnAnyTeamScoreUpdated;

        public GameSettings.PlayerType playerType;
        public string controlScheme = string.Empty;
        public int teamId = 0;

        private int scoredBalls;
        public int ScoredBalls => scoredBalls;

        private int score;
        public int Score
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
            scoredBalls++;
        }

        public void AddScore(int scoreToAdd)
        {
            Score = score + scoreToAdd;
        }

        public void ResetRound()
        {
            scoredBalls = 0;
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