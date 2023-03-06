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

        [SerializeField]
        private BallLauncher ballLauncher;

        private int MaxRounds => gameSettings.MaxRounds;
        
        public int RoundIndex { get; private set; }
        public bool IgnoreInputs { get; private set; }

        private int totalRoundScore = 0;

        protected override void Awake()
        {
            base.Awake();
            ballLauncher.OnBothBallsScored += HandleBothBallsScored;
        }

        private void OnDestroy()
        {
            ballLauncher.OnBothBallsScored -= HandleBothBallsScored;
        }

        public void StartGame()
        {
            OnGameStarted?.Invoke(this);
            StartNewRound();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EndRound(true);
            }
        }

        private void StartNewRound()
        {
            RoundIndex++;
            SetNewRoundScore(0);
            UIController.Instance.DisplayNewRoundInfo();
            OnNewRoundStarted?.Invoke(this);
        }

        private void EndRound(bool ignoreMaxRounds = false)
        {
            if (RoundIndex < MaxRounds || ignoreMaxRounds)
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
        
        private void HandleBothBallsScored(BallLauncher launcher, int winnerIndex)
        {
            EndRound();
        }

        public void SetIgnoreInputs(bool ignore)
        {
            IgnoreInputs = ignore;
        }

        public void AddScore(int score)
        {
            SetNewRoundScore(totalRoundScore + score);
        }

        private void SetNewRoundScore(int newScore)
        {
            totalRoundScore = newScore;
            OnTotalRoundScoreUpdated?.Invoke(this, totalRoundScore);
        }
    }
}