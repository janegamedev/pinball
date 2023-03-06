using DG.Tweening;
using Janegamedev.UI;
using UnityEngine;

namespace Janegamedev.Camera
{
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
            gameCamera = UnityEngine.Camera.main;
            gameCamera.orthographicSize = menuOrthoSize;

            if (UIController.Instance != null)
            {
                UIController.Instance.StartScreen.OnTransitionInBegun += HandleMenuScreenTransitionInBegun;
                UIController.Instance.StartScreen.OnTransitionOutBegun += HandleMenuScreenTransitionOutBegun;
            }
        }
        
        private void OnDestroy()
        {
            sizeTween?.Kill();
            
            if (UIController.Instance != null)
            {
                UIController.Instance.StartScreen.OnTransitionInBegun -= HandleMenuScreenTransitionInBegun;
                UIController.Instance.StartScreen.OnTransitionOutBegun -= HandleMenuScreenTransitionOutBegun;
            }
        }

        private void HandleMenuScreenTransitionInBegun(UIScreen screen)
        {
            TweenOrthoSize(menuOrthoSize);
        }
        
        private void HandleMenuScreenTransitionOutBegun(UIScreen screen)
        {
            TweenOrthoSize(inGameOrthoSize);
        }

        private void TweenOrthoSize(float size)
        {
            sizeTween?.Kill();
            sizeTween = gameCamera.DOOrthoSize(size, transitionDuration).SetEase(transitionEase);
        }
    }
}