using TMPro;
using UnityEngine;

namespace Janegamedev.UI.Screens
{
    /// <summary>
    /// Component that displays the name and score of a team in a result screen.
    /// </summary>
    public class TeamResultElement : MonoBehaviour
    {
        private const string TEAM_NAME = "Player {0}";
        
        [SerializeField]
        private TextMeshProUGUI teamName;
        [SerializeField]
        private TextMeshProUGUI score;

        /// <summary>
        /// Displays the name and score of a team.
        /// </summary>
        /// <param name="teamId">The id of the team.</param>
        /// <param name="teamScore">The score of the team.</param>
        public void DisplayTeamScore(int teamId, long teamScore)
        {
            teamName.text = string.Format(TEAM_NAME, teamId + 1);
            score.text = teamScore.ToString();
        }
    }
}