using TMPro;
using UnityEngine;

namespace Janegamedev.UI.Screens
{
    public class TeamResultElement : MonoBehaviour
    {
        private const string TEAM_NAME = "Player {0}";
        
        [SerializeField]
        private TextMeshProUGUI teamName;
        [SerializeField]
        private TextMeshProUGUI score;

        public void DisplayTeamScore(int teamId, long teamScore)
        {
            teamName.text = string.Format(TEAM_NAME, teamId + 1);
            score.text = teamScore.ToString();
        }
    }
}