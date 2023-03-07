using Janegamedev.Core;
using TMPro;
using UnityEngine;

namespace Janegamedev.UI.Elements
{
    public class PlayerScoreDisplayer : MonoBehaviour
    {
        [SerializeField]
        private int teamIndex;
        [SerializeField]
        private TextMeshProUGUI playersScore;

        private void Start()
        {
            Team.OnAnyTeamScoreUpdated += HandleAnyPlayerScoreUpdated;
        }

        private void OnDestroy()
        {
            Team.OnAnyTeamScoreUpdated -= HandleAnyPlayerScoreUpdated;
        }

        private void HandleAnyPlayerScoreUpdated(Team team)
        {
            if (teamIndex == team.teamId)
            {
                playersScore.text = team.Score.ToString();
            }
        }
    }
}