using DG.Tweening;
using Janegamedev.UI;
using UnityEngine;

namespace Janegamedev.Camera
{
    /// <summary>
    /// Class that manages the Pinball game camera.
    /// </summary>
    public class PinballCamera : MonoBehaviour
    {
        [SerializeField]
        private float menuOrthoSize = 5f;
        [SerializeField]
        private float inGameOrthoSize = 27f;
        [SerializeField]
        private float transitionDuration = 2f;
        [SerializeField]
        private Ease transitionEase = Ease.Linear;

        private UnityEngine.Camera gameCamera;
        private Tween sizeTween;

        private void Awake()
        {
            // Get the reference of the main camera
            gameCamera = UnityEngine.Camera.main;
            gameCamera.orthographicSize = menuOrthoSize;

            // Subscribe to the relevant events of the UIController
            if (UIController.Instance != null)
            {
                UIController.Instance.StartScreen.OnTransitionInBegun += HandleMenuScreenTransitionInBegun;
                UIController.Instance.StartScreen.OnTransitionOutBegun += HandleMenuScreenTransitionOutBegun;
                UIController.Instance.GameOverScreen.OnTransitionInBegun += HandleGameOverScreenTransitionInBegun;
            }
        }
        
        private void OnDestroy()
        {
            // Kill the existing tween and unsubscribe from the UIController events
            sizeTween?.Kill();
            
            if (UIController.Instance != null)
            {
                UIController.Instance.StartScreen.OnTransitionInBegun -= HandleMenuScreenTransitionInBegun;
                UIController.Instance.StartScreen.OnTransitionOutBegun -= HandleMenuScreenTransitionOutBegun;
                UIController.Instance.GameOverScreen.OnTransitionInBegun -= HandleGameOverScreenTransitionInBegun;
            }
        }

        /// <summary>
        /// Handles the beginning of a menu screen transition in by tweening the orthographic size of the game camera to the menuOrthoSize.
        /// </summary>
        /// <param name="screen">The menu screen that is transitioning in.</param>
        private void HandleMenuScreenTransitionInBegun(UIScreen screen)
        {
            TweenOrthoSize(menuOrthoSize);
        }
        
        /// <summary>
        /// Handles the beginning of a menu screen transition out by tweening the orthographic size of the game camera to the inGameOrthoSize.
        /// </summary>
        /// <param name="screen">The menu screen that is transitioning out.</param>
        private void HandleMenuScreenTransitionOutBegun(UIScreen screen)
        {
            TweenOrthoSize(inGameOrthoSize);
        }
        
        /// <summary>
        /// Handles the beginning of a game over screen transition in by tweening the orthographic size of the game camera to the menuOrthoSize.
        /// </summary>
        /// <param name="screen">The game over screen that is transitioning in.</param>
        private void HandleGameOverScreenTransitionInBegun(UIScreen screen)
        {
            TweenOrthoSize(menuOrthoSize);
        }

        /// <summary>
        /// Tweens the orthographic size of the game camera to the given size using the DOTween library.
        /// </summary>
        /// <param name="size">The size to tween to.</param>
        private void TweenOrthoSize(float size)
        {
            // Kill any existing sizeTween before starting a new one
            sizeTween?.Kill();
            // Tween the orthographic size using the DOTween library and set the ease type and duration
            sizeTween = gameCamera.DOOrthoSize(size, transitionDuration).SetEase(transitionEase);
        }
    }
}