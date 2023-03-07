using System;
using Janegamedev.Core.Ball;
using Janegamedev.UI;
using Janegamedev.Utilities;
using UnityEngine;

namespace Janegamedev.Core
{
    /// <summary>
    /// Manages the state and loop of the game, including rounds, scores and game settings.
    /// </summary>
    public class GameState : Singleton<GameState>
    {
        public static event Action<GameState> OnGameStarted;
        public static event Action<GameState> OnNewRoundStarted;
        public static event Action<GameState> OnGameEnded;
        public static event Action<GameState, long> OnTotalRoundScoreUpdated;

        [SerializeField]
        private GameSettings gameSettings;
        public GameSettings GameSettings => gameSettings;

        private int MaxRounds => gameSettings.MaxRounds;
        public bool IsAdditionalRound => RoundIndex > MaxRounds && !TeamController.Instance.HasWinnerTeam();
        
        public int RoundIndex { get; private set; }
        public bool IgnoreInputs { get; private set; }
        public long TotalRoundScore { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            BallController.OnBothBallsScored += HandleBothBallsScored;
        }

        private void OnDestroy()
        {
            BallController.OnBothBallsScored -= HandleBothBallsScored;
        }

        /// <summary>
        /// Starts the game, resets the round index and triggers OnGameStarted and StartNewRound.
        /// </summary>
        public void StartGame()
        {
            RoundIndex = 0;
            OnGameStarted?.Invoke(this);
            StartNewRound();
        }

        /// <summary>
        /// Starts a new round
        /// </summary>
        private void StartNewRound()
        {
            // Increments the round index
            RoundIndex++;
            // Resets the total round score
            SetNewRoundScore(0);
            // Displays new round info
            UIController.Instance.ActivateNewRoundScreen();
            OnNewRoundStarted?.Invoke(this);
        }

        /// <summary>
        /// Ends the current round
        /// </summary>
        private void EndRound()
        {
            // Starts a new round if there are still rounds left or if there is no winning team. 
            if (RoundIndex < MaxRounds || !TeamController.Instance.HasWinnerTeam())
            {
                StartNewRound();
                return;
            }
            
            // Otherwise, ends the game.
            EndGame();
        }

        /// <summary>
        /// Ends the game by showing the result screen and triggering OnGameEnded.
        /// </summary>
        private void EndGame()
        {
            UIController.Instance.ActivateGameOverScreen();
            BroadcastGameEnded();
        }
        
        /// <summary>
        /// Handles the event when both balls are scored by ending the round.
        /// </summary>
        private void HandleBothBallsScored(BallController controller)
        {
            EndRound();
        }

        /// <summary>
        /// Sets whether inputs should be ignored.
        /// </summary>
        public void SetIgnoreInputs(bool ignore)
        {
            IgnoreInputs = ignore;
        }

        /// <summary>
        /// Stops the game by triggering BroadcastGameEnded.
        /// </summary>
        public void StopTheGame()
        {
            BroadcastGameEnded();
        }

        /// <summary>
        /// Broadcasts the event that the game has ended.
        /// </summary>
        private void BroadcastGameEnded()
        {
            OnGameEnded?.Invoke(this);
        }

        #region Score
        
        /// <summary>
        /// Adds the given score to the total round score
        /// </summary>
        /// <param name="score">The score to add.</param>
        public void AddScore(int score)
        {
            SetNewRoundScore(TotalRoundScore + score);
        }

        /// <summary>
        /// Multiplies the total round score by the given amount
        /// </summary>
        /// <param name="amount">The amount to multiply by.</param>
        public void MultiplyScore(int amount)
        {
            SetNewRoundScore(TotalRoundScore * amount);
        }

        /// <summary>
        /// Sets the total round score to the given new score and
        /// broadcasts the event that the total round score has been updated.
        /// </summary>
        /// <param name="newScore">The new score to set.</param>
        private void SetNewRoundScore(long newScore)
        {
            TotalRoundScore = newScore;
            OnTotalRoundScoreUpdated?.Invoke(this, TotalRoundScore);
        }
        
        #endregion
    }
}