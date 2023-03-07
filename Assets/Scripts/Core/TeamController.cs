using System.Collections.Generic;
using System.Linq;
using Janegamedev.Obstacles;
using Janegamedev.Utilities;
using UnityEngine;

namespace Janegamedev.Core
{
    public class TeamController : Singleton<TeamController>
    {
        [SerializeField]
        private Team[] playerSides = new Team[2];

        public List<Team> Teams => teamByIdSet.Values.ToList();

        private readonly HashSet<BasePlayer> spawnedPlayers = new HashSet<BasePlayer>();
        private readonly Dictionary<int, Team> teamByIdSet = new Dictionary<int, Team>();

        protected override void Awake()
        {
            base.Awake();
            GameState.OnGameStarted += HandleGameStarted;
            GameState.OnNewRoundStarted += HandleNewRoundStarted;
            BallScoreZone.OnAnyBallEnteredScoreZone += HandleAnyBallEnteredScoreZone;
            BallController.OnBothBallsScored += HandleBothBallsScored;

            foreach (Team team in playerSides)
            {
                teamByIdSet.Add(team.teamId, team);
            }
        }
        
        private void OnDestroy()
        {
            GameState.OnGameStarted -= HandleGameStarted;
            GameState.OnNewRoundStarted -= HandleNewRoundStarted;
            BallScoreZone.OnAnyBallEnteredScoreZone -= HandleAnyBallEnteredScoreZone;
            BallController.OnBothBallsScored -= HandleBothBallsScored;
        }

        private void HandleGameStarted(GameState gameState)
        {
            SpawnPlayers();
        }

        private void SpawnPlayers()
        {
            DestroyPlayer();

            for (int i = 0; i < playerSides.Length; i++)
            {
                Team playerTeam = playerSides[i];
                BasePlayer templateByType = GameState.Instance.GameSettings.GetPlayerTemplateByType(playerTeam.playerType);

                if (templateByType == null)
                {
                    continue;
                }

                BasePlayer player = Instantiate(templateByType, transform);
                player.AssignTeam(i, playerTeam.controlScheme);
                spawnedPlayers.Add(player);
            }
        }

        private void DestroyPlayer()
        {
            foreach (BasePlayer spawnedPlayer in spawnedPlayers)
            {
                Destroy(spawnedPlayer.gameObject);
            }

            foreach (Team team in teamByIdSet.Values)
            {
                team.ResetGame();
            }
            
            spawnedPlayers.Clear();
        }
        
        private void HandleAnyBallEnteredScoreZone(BallScoreZone scoreZone, Ball ball, int teamId)
        {
            if (teamByIdSet.TryGetValue(teamId, out Team team))
            {
                team.IncrementScoredBalls();
            }
        }
        
        private void HandleBothBallsScored(BallController controller)
        {
            int totalScore = GameState.Instance.TotalRoundScore;

            Team winningTeam = teamByIdSet.FirstOrDefault(x => x.Value.ScoredBalls > 1).Value;

            if (winningTeam == null)
            {
                int splitTotalScore = Mathf.FloorToInt(totalScore / 2f);
                foreach (Team team in teamByIdSet.Values)
                {
                    team.AddScore(splitTotalScore);
                }
            }
            else
            {
                winningTeam.AddScore(totalScore);
            }
        }
        
        private void HandleNewRoundStarted(GameState state)
        {
            foreach (Team team in teamByIdSet.Values)
            {
                team.ResetRound();
            }
        }

        public bool HasWinnerTeam()
        {
            List<Team> sortedTeam = Teams;
            sortedTeam.Sort();
            
            for (int i = 1; i < 2; i++)
            {
                if (sortedTeam[i].Score == sortedTeam[i - 1].Score)
                {
                    return false;
                }
            }

            return true;
        }
    }
}