using System.Collections.Generic;
using Janegamedev.Utilities;
using UnityEngine;

namespace Janegamedev.UI
{
    /// <summary>
    /// Handles UI navigation and user input events.
    /// Is a singleton and can be referenced as UIController.Instance.
    /// </summary>
    public class UIController : Singleton<UIController>
    {
        [SerializeField]
        private UIScreen startScreen;
        public UIScreen StartScreen => startScreen;
        [SerializeField]
        private UIScreen roundScreen;
        [SerializeField]
        private UIScreen gameplayScreen;
        public UIScreen GameplayScreen => gameplayScreen;
        [SerializeField]
        private UIScreen gameOverScreen;
        public UIScreen GameOverScreen => gameOverScreen;

        private UIScreen currentScreen;
        private UIScreen nextScreen;
        private readonly HashSet<UIScreen> registeredScreens = new HashSet<UIScreen>();

        protected override void Awake()
        {
            base.Awake();
            RegisterUIScreens(
                startScreen, 
                roundScreen, 
                gameplayScreen,
                gameOverScreen);
        }

        private void Start()
        {
            startScreen.Active = true;
        }

        private void OnDestroy()
        {
            foreach (UIScreen registeredScreen in registeredScreens)
            {
                if (registeredScreen != null)
                {
                    UnregisterUIScreen(registeredScreen, onDestroy: true);
                }
            }
            
            registeredScreens.Clear();
        }

        /// <summary>
        /// Helper to clean up initialization code for the UIController.
        /// </summary>
        private void RegisterUIScreens(params UIScreen[] screens)
        {
            foreach (UIScreen uiScreen in screens)
            {
                RegisterUIScreen(uiScreen);
            }
        }
        
        /// <summary>
        /// Registers a UIScreen to this UIController to handle transition events
        /// </summary>
        /// <param name="screen">The UIScreen to register</param>
        private void RegisterUIScreen(UIScreen screen)
        {
            if (screen == null)
            {
                return;
            }

            if (registeredScreens.Contains(screen))
            {
                Debug.LogError($"Tried to register an already-registered UIScreen: {screen.name}");
                return;
            }
            
            registeredScreens.Add(screen);
            screen.OnTransitionInFinished += HandleScreenTransitionInFinished;
            screen.OnTransitionOutFinished += HandleScreenTransitionOutFinished;
            screen.OnActiveStatusChanged += HandleScreenActiveStatusChanged;
        }

        /// <summary>
        /// Unregisters a UIScreen to this UIController to stop handling transition events
        /// </summary>
        /// <param name="screen">The UIScreen to unregister</param>
        /// <param name="onDestroy">If true, we're calling this from OnDestroy</param>
        private void UnregisterUIScreen(UIScreen screen, bool onDestroy = false)
        {
            if (screen == null)
            {
                return;
            }

            if (!onDestroy)
            {
                registeredScreens.Remove(screen);
            }
            
            screen.OnTransitionInFinished -= HandleScreenTransitionInFinished;
            screen.OnTransitionOutFinished -= HandleScreenTransitionOutFinished;
            screen.OnActiveStatusChanged -= HandleScreenActiveStatusChanged;
        }

        private void HandleScreenTransitionInFinished(UIScreen screen)
        {
            // Screen was transitioned in without using the UIController - handle this case in case
            if (screen != currentScreen)
            {
                currentScreen.BeginTransitionOut();
                currentScreen = screen;
            }
        }

        private void HandleScreenTransitionOutFinished(UIScreen screen)
        {
            // Current screen has transitioned out
            // If there is a nextScreen, transition that in and assign as the currentScreen
            if (screen == currentScreen)
            {
                currentScreen = nextScreen;
                if (currentScreen != null)
                {
                    screen.BeginTransitionIn();
                }
            }
        }
        
        /// <summary>
        /// Handler for a change in a UIScreen's Active property.
        /// DO NOT change (set a new value for) the UIScreen's Active property again in here,
        /// or you will trigger the event for this handler again and create an infinite recursion!
        /// </summary>
        private void HandleScreenActiveStatusChanged(UIScreen uiScreen)
        {
            if (uiScreen.Active)
            {
                if (currentScreen != null)
                {
                    nextScreen = uiScreen;
                    currentScreen.BeginTransitionOut();
                }
                else
                {
                    nextScreen = null;
                    currentScreen = uiScreen;
                    currentScreen.BeginTransitionIn();
                }
            }
            else
            {
                if (currentScreen == uiScreen)
                {
                    currentScreen.BeginTransitionOut();
                    nextScreen = null;
                    currentScreen = null;
                }
            }
        }

        /// <summary>
        /// Activates the start screen and deactivates all other screens.
        /// </summary>
        public void ActivateStartScreen()
        {
            ActivateScreen(startScreen);
        }
        
        /// <summary>
        /// Activates the round screen and deactivates all other screens.
        /// </summary>
        public void ActivateNewRoundScreen()
        {
            ActivateScreen(roundScreen);
        }

        /// <summary>
        /// Activates the gameplay screen and deactivates all other screens.
        /// </summary>
        public void ActivateGameplayScreen()
        {
            ActivateScreen(gameplayScreen);
        }

        /// <summary>
        /// Activates the game over screen and deactivates all other screens.
        /// </summary>
        public void ActivateGameOverScreen()
        {
            ActivateScreen(gameOverScreen);
        }

        /// <summary>
        /// Activates the given screen and deactivates all other registered screens.
        /// </summary>
        /// <param name="activate">The screen to activate.</param>
        private void ActivateScreen(UIScreen activate)
        {
            // Deactivates all registered screens.
            foreach (UIScreen screen in registeredScreens)
            {
                screen.Active = false;
            }

            activate.Active = true;
        }
        
    }
}