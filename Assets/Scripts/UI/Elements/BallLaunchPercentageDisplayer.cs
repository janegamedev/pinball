using System;
using Janegamedev.Audio;
using Janegamedev.Core;
using Janegamedev.Core.Ball;
using UnityEngine;

namespace Janegamedev.UI.Elements
{
    /// <summary>
    /// Displays the launch force percentage with force arrows sprites
    /// </summary>
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
            // Initializes the step size and subscribes to events.
            step = 1f / forceArrows.Length;
            BallController.OnLaunchForcePercentageUpdated += HandleLaunchForcePercentageUpdated;
            GameState.OnNewRoundStarted += HandleNewRoundStarted;
        }
        
        private void Start()
        {
            // Resets the arrows.
            Reset();
        }

        private void OnDestroy()
        {
            // Unsubscribes from events.
            BallController.OnLaunchForcePercentageUpdated -= HandleLaunchForcePercentageUpdated;
            GameState.OnNewRoundStarted -= HandleNewRoundStarted;
        }

        /// <summary>
        /// Handles the event of launch force percentage being updated.
        /// </summary>
        private void HandleLaunchForcePercentageUpdated(BallController ballController, float percentage)
        {
            // If the percentage meets or exceeds the next threshold,
            // updates the arrow and plays the force increase sound effect.
            if (percentage >= nextThreshold)
            {
                SetIcon(forceArrows[nextIndex], spriteSet.On);
                nextThreshold += step;
                nextIndex++;
                MusicPlayer.Instance.PlaySFX(FORCE_INCREASE_SFX);
            }
        }

        /// <summary>
        /// Resets the arrows to their default state.
        /// </summary>
        private void Reset()
        {
            foreach (SpriteRenderer spriteRenderer in forceArrows)
            {
                SetIcon(spriteRenderer, spriteSet.Off);
            }

            nextThreshold = step;
            nextIndex = 0;
        }

        /// <summary>
        /// Sets the icon of a sprite renderer.
        /// </summary>
        private void SetIcon(SpriteRenderer spriteRenderer, Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
        
        /// <summary>
        /// Handles the event of a new round starting.
        /// </summary>
        private void HandleNewRoundStarted(GameState gameState)
        {
            Reset();
        }
    }
}