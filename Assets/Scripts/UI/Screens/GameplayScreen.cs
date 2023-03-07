using System.Collections;
using Janegamedev.Audio;
using Janegamedev.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Janegamedev.UI.Screens
{
    /// <summary>
    /// Controls the gameplay screen which displays the player's score and provides a button to return to the main menu.
    /// </summary>
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
            // Enables the player inputs
            GameState.Instance.SetIgnoreInputs(false);
        }

        /// <summary>
        /// Handles an update to the total round score and sets the score text accordingly.
        /// </summary>
        private void HandleTotalRoundScoreUpdated(GameState state, long score)
        {
            SetScore(score);
        }

        /// <summary>
        /// Sets the score text with the provided score.
        /// </summary>
        private void SetScore(long score)
        {
            totalRoundScore.text = string.Format(SCORE_LABEL_TEXT, score);
        }
        
        /// <summary>
        /// Handles a click of the menu button,
        /// stopping the game and returning to the main menu.
        /// </summary>
        private void HandleMenuButtonPressed()
        {
            GameState.Instance.StopTheGame();
            UIController.Instance.ActivateStartScreen();
            MusicPlayer.Instance.PlaySFX(BUTTON_PRESS_SFX);
        }
    }
}