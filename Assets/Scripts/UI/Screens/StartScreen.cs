using Janegamedev.Audio;
using Janegamedev.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Janegamedev.UI.Screens
{
    /// <summary>
    /// Initial screen of the game
    /// Allows to start and quit the game
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

        /// <summary>
        /// Handles the "Play Game" button press event.
        /// Deactivates the current UI and starts the game.
        /// </summary>
        private void HandlePlayGameButtonPressed()
        {
            Active = false;
            GameState.Instance.StartGame();
            MusicPlayer.Instance.PlaySFX(BUTTON_PRESS_SFX);
        }

        /// <summary>
        /// Handles the "Quit Game" button press event.
        /// Quits the application.
        /// </summary>
        private void HandleQuitGameButtonPressed()
        {
            Application.Quit();
        }
    }
}