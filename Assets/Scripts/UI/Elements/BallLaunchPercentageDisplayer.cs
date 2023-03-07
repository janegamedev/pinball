using System;
using Janegamedev.Audio;
using Janegamedev.Core;
using UnityEngine;

namespace Janegamedev.UI.Elements
{
    public class BallLaunchPercentageDisplayer : MonoBehaviour
    {
        private const string FORCE_INCREASE_SFX = "launchForce";
        
        [SerializeField]
        private SpriteRenderer[] forceArrows = new SpriteRenderer[5];
        [SerializeField]
        private SpriteSet spriteSet;

        private float step;
        private float nextThreshold;
        private int nextIndex;

        private void Awake()
        {
            step = 1f / forceArrows.Length;
            BallController.OnLaunchForcePercentageUpdated += HandleLaunchForcePercentageUpdated;
            GameState.OnNewRoundStarted += HandleNewRoundStarted;
        }
        
        private void Start()
        {
            Reset();
        }

        private void OnDestroy()
        {
            BallController.OnLaunchForcePercentageUpdated -= HandleLaunchForcePercentageUpdated;
            GameState.OnNewRoundStarted -= HandleNewRoundStarted;
        }

        private void HandleLaunchForcePercentageUpdated(BallController ballController, float percentage)
        {
            if (percentage >= nextThreshold)
            {
                SetIcon(forceArrows[nextIndex], spriteSet.On);
                nextThreshold += step;
                nextIndex++;
                MusicPlayer.Instance.PlaySFX(FORCE_INCREASE_SFX);
            }
        }

        private void Reset()
        {
            foreach (SpriteRenderer spriteRenderer in forceArrows)
            {
                SetIcon(spriteRenderer, spriteSet.Off);
            }

            nextThreshold = step;
            nextIndex = 0;
        }

        private void SetIcon(SpriteRenderer spriteRenderer, Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
        
        private void HandleNewRoundStarted(GameState gameState)
        {
            Reset();
        }
    }
}