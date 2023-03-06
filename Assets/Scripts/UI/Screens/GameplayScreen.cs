using System.Collections;
using Janegamedev.Core;
using TMPro;
using UnityEngine;

namespace Janegamedev.UI.Screens
{
    public class GameplayScreen : UIScreen
    {
        private const string SCORE_LABEL_TEXT = "Score: {0}";
        
        [SerializeField]
        private TextMeshProUGUI totalRoundScore;

        protected override void Awake()
        {
            base.Awake();
            GameState.OnTotalRoundScoreUpdated += HandleTotalRoundScoreUpdated;
            SetScore(0);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GameState.OnTotalRoundScoreUpdated -= HandleTotalRoundScoreUpdated;
        }

        protected override IEnumerator TransitionIn()
        {
            yield return base.TransitionIn();
            GameState.Instance.SetIgnoreInputs(false);
        }

        private void HandleTotalRoundScoreUpdated(GameState state, int score)
        {
            SetScore(score);
        }

        private void SetScore(int score)
        {
            totalRoundScore.text = string.Format(SCORE_LABEL_TEXT, score);
        }
    }
}