using System.Collections;
using System.Collections.Generic;
using Janegamedev.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Janegamedev.UI.Screens
{
    public class GameOverScreen : UIScreen
    {
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
                teamResultElements[i].DisplayTeamScore(team.teamId, team.Score);
            }
            
            return base.TransitionIn();
        }

        private void HandleMenuButtonPressed()
        {
            UIController.Instance.OpenStartScreen();
        }
    }
}