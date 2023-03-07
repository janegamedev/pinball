using Janegamedev.Core;
using TMPro;
using UnityEngine;

namespace Janegamedev.UI.Elements
{
    /// <summary>
    /// Displays the score of a specific team.
    /// </summary>
    public class PlayerScoreDisplayer : MonoBehaviour
    {
        [SerializeField]
        private int teamIndex;
        [SerializeField]
        private TextMeshProUGUI playersScore;

        private void Start()
        {
            // Register for the event
            Team.OnAnyTeamScoreUpdated += HandleAnyPlayerScoreUpdated;
        }

        private void OnDestroy()
        {
            // Unregister from the event
            Team.OnAnyTeamScoreUpdated -= HandleAnyPlayerScoreUpdated;
        }

        /// <summary>
        /// Updates the score of the team if it matches the team index.
        /// </summary>
        /// <param name="team">The team whose score has been updated.</param>
        private void HandleAnyPlayerScoreUpdated(Team team)
        {
            if (teamIndex == team.TeamId)
            {
                playersScore.text = team.Score.ToString();
            }
        }
    }
}