using System.Collections;
using DG.Tweening;
using Janegamedev.Core;
using TMPro;
using UnityEngine;

namespace Janegamedev.UI.Screens
{
    public class NewRoundScreen : UIScreen
    {
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
            if (countdownRoutine != null)
            {
                StopCoroutine(countdownRoutine);
            }
        }

        protected override IEnumerator TransitionIn()
        {
            additionalTime = GameState.Instance.IsAdditionalRound;
            additionalTimeLabel.gameObject.SetActive(additionalTime);
            GameState.Instance.SetIgnoreInputs(true);
            countdownLabel.DOFade(0, 0);
            countdownLabel.text = string.Empty;
            roundLabel.text = string.Format(ROUND_LABEL_TEXT, GameState.Instance.RoundIndex);
            yield return base.TransitionIn();
            countdownRoutine ??= StartCoroutine(CountdownRoutine());
        }

        private IEnumerator CountdownRoutine()
        {
            Tween fadeTween = roundLabel.DOFade(1, TEXT_FADE_DURATION);

            if (additionalTime)
            {
                additionalTimeLabel.DOFade(1, TEXT_FADE_DURATION);
            }
            
            yield return new WaitForSeconds(COUNTDOWN_DELAY);
            
            fadeTween?.Kill();
            fadeTween = DOTween.Sequence()
                .Append(roundLabel.DOFade(0, TEXT_FADE_DURATION))
                .Join(additionalTimeLabel.DOFade(0, TEXT_FADE_DURATION))
                .Append(countdownLabel.DOFade(1f, TEXT_FADE_DURATION))
                .OnComplete(() => fadeTween = null);
            
            yield return new WaitUntil(() => fadeTween == null);

            float tweenPercentage = 0f;
            Tween forceTween = DOTween.To(x => tweenPercentage = x,
                    0f,
                    1f,
                    countdownTime)
                .OnUpdate(HandleTweenUpdate)
                .OnComplete(() => forceTween = null);

            void HandleTweenUpdate()
            {
                float timeLeft = Mathf.Lerp(0, countdownTime, 1f - tweenPercentage);
                countdownLabel.text = Mathf.Ceil(timeLeft).ToString();
            }

            yield return new WaitUntil(() => forceTween == null);
            
            UIController.Instance.StartNewRound();

            countdownRoutine = null;
        }
    }
}