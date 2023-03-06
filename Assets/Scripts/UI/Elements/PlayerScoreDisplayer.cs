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

        private void HandleAnyPlayerScoreUpdated(int index, int score)
        {
            if (teamIndex == index)
            {
                playersScore.text = score.ToString();
            }
        }
    }
}