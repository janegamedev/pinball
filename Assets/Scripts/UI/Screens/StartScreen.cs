using Janegamedev.Audio;
using Janegamedev.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Janegamedev.UI.Screens
{
    /// <summary>
    /// Initial screen of the game
    /// Allows to select balls to play with
    /// </summary>
    public class StartScreen : UIScreen
    {
        [SerializeField]
        private Button playGameButton;
        [SerializeField]
        private Button quitGameButton;

        protected override void Awake()
        {
            base.Awake();
            playGameButton.onClick.AddListener(HandlePlayGameButtonPressed);
            quitGameButton.onClick.AddListener(HandleQuitGameButtonPressed);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            playGameButton.onClick.RemoveListener(HandlePlayGameButtonPressed);
            quitGameButton.onClick.RemoveListener(HandleQuitGameButtonPressed);
        }

        private void HandlePlayGameButtonPressed()
        {
            Active = false;
            GameState.Instance.StartGame();
            MusicPlayer.Instance.PlaySFX(BUTTON_PRESS_SFX);
        }

        private void HandleQuitGameButtonPressed()
        {
            Application.Quit();
        }
    }
}