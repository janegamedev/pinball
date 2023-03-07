using System.Collections;
using DG.Tweening;
using Janegamedev.Audio;
using Janegamedev.Core;
using TMPro;
using UnityEngine;

namespace Janegamedev.UI.Screens
{
    /// <summary>
    /// Screen that shows up at the start of a new round with a countdown before gameplay starts.
    /// </summary>
    public class NewRoundScreen : UIScreen
    {
        private const string ROUND_START_SFX = "roundStart";
        private const string COUNTDOWN_SFX = "countdown";
        private const string ROUND_LABEL_TEXT = "Round {0}";
        private const float COUNTDOWN_DELAY = 2f;
        private const float TEXT_FADE_DURATION = 0.25f;
        
        [SerializeField]
        private TextMeshProUGUI roundLabel;
        [SerializeField]
        private TextMeshProUGUI additionalTimeLabel;
        [SerializeField]
        private TextMeshProUGUI countdownLabel;
        [SerializeField]
        private float countdownTime = 3f;
        
        private Coroutine countdownRoutine;
        private bool additionalTime;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            // Stops the countdown coroutine if it's running.
            if (countdownRoutine != null)
            {
                StopCoroutine(countdownRoutine);
            }
        }

        protected override IEnumerator TransitionIn()
        {
            // Checks if additional round is enabled and enables additionalTimeLabel based on that
            additionalTime = GameState.Instance.IsAdditionalRound;
            additionalTimeLabel.gameObject.SetActive(additionalTime);
            
            // Sets game state to ignore inputs
            GameState.Instance.SetIgnoreInputs(true);
            
            // Sets the alpha of countdown label to 0 instantly and clears the text
            countdownLabel.DOFade(0, 0);
            countdownLabel.text = string.Empty;
            
            // Sets the text of round label
            roundLabel.text = string.Format(ROUND_LABEL_TEXT, GameState.Instance.RoundIndex);
            
            yield return base.TransitionIn();
            
            // Plays round start sfx
            MusicPlayer.Instance.PlaySFX(ROUND_START_SFX);
            // Starts countdown routine
            countdownRoutine ??= StartCoroutine(CountdownRoutine());
        }

        /// <summary>
        /// Coroutine that runs the countdown and updates the countdown label.
        /// </summary>
        private IEnumerator CountdownRoutine()
        {
            // Fade in round label and additional time label if there is extra time.
            Tween fadeTween = roundLabel.DOFade(1, TEXT_FADE_DURATION);

            if (additionalTime)
            {
                additionalTimeLabel.DOFade(1, TEXT_FADE_DURATION);
            }
            
            yield return new WaitForSeconds(COUNTDOWN_DELAY);
            
            // Kill previous fade tween and start new fade tween for countdown label.
            fadeTween?.Kill();
            fadeTween = DOTween.Sequence()
                .Append(roundLabel.DOFade(0, TEXT_FADE_DURATION))
                .Join(additionalTimeLabel.DOFade(0, TEXT_FADE_DURATION))
                .Append(countdownLabel.DOFade(1f, TEXT_FADE_DURATION))
                .OnComplete(() => fadeTween = null);
            
            yield return new WaitUntil(() => fadeTween == null);
            
            // Play countdown sound effect.
            MusicPlayer.Instance.PlaySFX(COUNTDOWN_SFX);

            // Start countdown timer.
            float tweenPercentage = 0f;
            Tween forceTween = DOTween.To(x => tweenPercentage = x,
                    0f,
                    1f,
                    countdownTime)
                .OnUpdate(HandleTweenUpdate)
                .OnComplete(() => forceTween = null);
            
            // Updates the countdown label based on the tween progress.
            void HandleTweenUpdate()
            {
                float timeLeft = Mathf.Lerp(0, countdownTime, 1f - tweenPercentage);
                countdownLabel.text = Mathf.Ceil(timeLeft).ToString();
            }

            // Wait until countdown timer completes.
            yield return new WaitUntil(() => forceTween == null);
            
            // Start new round after countdown finishes.
            UIController.Instance.ActivateGameplayScreen();

            // Set countdown routine to null.
            countdownRoutine = null;
        }
    }
}