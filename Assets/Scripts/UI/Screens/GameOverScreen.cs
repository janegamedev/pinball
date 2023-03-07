using System.Collections;
using System.Collections.Generic;
using Janegamedev.Audio;
using Janegamedev.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Janegamedev.UI.Screens
{
    public class GameOverScreen : UIScreen
    {
        private const string GAME_OVER_SFX = "gameOver";
        
        [SerializeField]
        private TeamResultElement[] teamResultElements;
        [SerializeField]
        private Button menuButton;

        private void Start()
        {
            menuButton.onClick.AddListener(HandleMenuButtonPressed);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            menuButton.onClick.RemoveListener(HandleMenuButtonPressed);
        }

        protected override IEnumerator TransitionIn()
        {
            List<Team> teams = TeamController.Instance.Teams;
            teams.Sort();

            for (int i = 0; i < teams.Count; i++)
            {
                Team team = teams[i];
                teamResultElements[i].DisplayTeamScore(team.TeamId, team.Score);
            }
            
            yield return base.TransitionIn();
            
            MusicPlayer.Instance.PlaySFX(GAME_OVER_SFX);
        }

        private void HandleMenuButtonPressed()
        {
            UIController.Instance.OpenStartScreen();
            MusicPlayer.Instance.PlaySFX(BUTTON_PRESS_SFX);
        }
    }
}