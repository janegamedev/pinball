using System;
using Janegamedev.UI;
using Janegamedev.Utilities;
using UnityEngine;

namespace Janegamedev.Core
{
    public class GameState : Singleton<GameState>
    {
        public static event Action<GameState> OnGameStarted;
        public static event Action<GameState> OnNewRoundStarted;
        public static event Action<GameState> OnGameEnded;
        public static event Action<GameState, int> OnTotalRoundScoreUpdated;

        [SerializeField]
        private GameSettings gameSettings;
        public GameSettings GameSettings => gameSettings;

        private int MaxRounds => gameSettings.MaxRounds;
        public bool IsAdditionalRound => RoundIndex >= MaxRounds && !TeamController.Instance.HasWinnerTeam();
        
        public int RoundIndex { get; private set; }
        public bool IgnoreInputs { get; private set; }
        public int TotalRoundScore { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            BallController.OnBothBallsScored += HandleBothBallsScored;
        }

        private void OnDestroy()
        {
            BallController.OnBothBallsScored -= HandleBothBallsScored;
        }

        public void StartGame()
        {
            RoundIndex = 0;
            OnGameStarted?.Invoke(this);
            StartNewRound();
        }

        private void StartNewRound()
        {
            RoundIndex++;
            SetNewRoundScore(0);
            UIController.Instance.DisplayNewRoundInfo();
            OnNewRoundStarted?.Invoke(this);
        }

        private void EndRound()
        {
            if (RoundIndex < MaxRounds || !TeamController.Instance.HasWinnerTeam())
            {
                StartNewRound();
                return;
            }
            
            EndGame();
        }

        private void EndGame()
        {
            UIController.Instance.ShowResultScreen();
            OnGameEnded?.Invoke(this);
        }
        
        private void HandleBothBallsScored(BallController controller)
        {
            EndRound();
        }

        public void SetIgnoreInputs(bool ignore)
        {
            IgnoreInputs = ignore;
        }

        public void AddScore(int score)
        {
            SetNewRoundScore(TotalRoundScore + score);
        }

        private void SetNewRoundScore(int newScore)
        {
            TotalRoundScore = newScore;
            OnTotalRoundScoreUpdated?.Invoke(this, TotalRoundScore);
        }
    }
}