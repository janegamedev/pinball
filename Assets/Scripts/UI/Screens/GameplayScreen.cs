using System.Collections;
using Janegamedev.Audio;
using Janegamedev.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Janegamedev.UI.Screens
{
    public class GameplayScreen : UIScreen
    {
        private const string SCORE_LABEL_TEXT = "Score: {0}";
        
        [SerializeField]
        private TextMeshProUGUI totalRoundScore;
        [SerializeField]
        private Button menuButton;

        protected override void Awake()
        {
            base.Awake();
            GameState.OnTotalRoundScoreUpdated += HandleTotalRoundScoreUpdated;
            menuButton.onClick.AddListener(HandleMenuButtonPressed);
            SetScore(0);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GameState.OnTotalRoundScoreUpdated -= HandleTotalRoundScoreUpdated;
            menuButton.onClick.RemoveListener(HandleMenuButtonPressed);
        }

        protected override IEnumerator TransitionIn()
        {
            yield return base.TransitionIn();
            GameState.Instance.SetIgnoreInputs(false);
        }

        private void HandleTotalRoundScoreUpdated(GameState state, long score)
        {
            SetScore(score);
        }

        private void SetScore(long score)
        {
            totalRoundScore.text = string.Format(SCORE_LABEL_TEXT, score);
        }
        
        private void HandleMenuButtonPressed()
        {
            GameState.Instance.StopTheGame();
            UIController.Instance.OpenStartScreen();
            MusicPlayer.Instance.PlaySFX(BUTTON_PRESS_SFX);
        }
    }
}