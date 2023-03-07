using System.Collections.Generic;
using System.Linq;
using Janegamedev.Core.Ball;
using Janegamedev.Core.Player;
using Janegamedev.Obstacles;
using Janegamedev.Utilities;
using UnityEngine;

namespace Janegamedev.Core
{
    /// <summary>
    /// The class is responsible for managing the teams and players in the game.
    /// </summary>
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
            // Add the event handlers
            GameState.OnGameStarted += HandleGameStarted;
            GameState.OnNewRoundStarted += HandleNewRoundStarted;
            BallDrainZone.OnAnyBallEnteredDrainZone += HandleAnyBallEnteredDrainZone;
            BallController.OnBothBallsScored += HandleBothBallsScored;

            // Add all the teams to the dictionary
            foreach (Team team in playerSides)
            {
                teamByIdSet.Add(team.TeamId, team);
            }
        }
        
        private void OnDestroy()
        {
            // Remove the event handlers
            GameState.OnGameStarted -= HandleGameStarted;
            GameState.OnNewRoundStarted -= HandleNewRoundStarted;
            BallDrainZone.OnAnyBallEnteredDrainZone -= HandleAnyBallEnteredDrainZone;
            BallController.OnBothBallsScored -= HandleBothBallsScored;
        }

        /// <summary>
        /// Handles the start of the game by spawning the players.
        /// </summary>
        /// <param name="gameState">The current game state.</param>
        private void HandleGameStarted(GameState gameState)
        {
            SpawnPlayers();
        }

        /// <summary>
        /// Spawns the players based on the player settings and assigns them to their respective teams.
        /// </summary>
        private void SpawnPlayers()
        {
            // Destroy the players first
            DestroyPlayer();

            // Iterate through each player side and instantiate the corresponding player template.
            for (int i = 0; i < playerSides.Length; i++)
            {
                Team playerTeam = playerSides[i];
                BasePlayer templateByType = GameState.Instance.GameSettings.GetPlayerTemplateByType(playerTeam.PlayerType);

                // If there is no template for this player type, skip to the next one.
                if (templateByType == null)
                {
                    continue;
                }

                // Instantiate the player template and assign them to the team.
                BasePlayer player = Instantiate(templateByType, transform);
                player.AssignTeam(i, playerTeam.ControlScheme);
                spawnedPlayers.Add(player);
            }
        }

        /// <summary>
        /// Destroys all the currently spawned players and resets each team's game state.
        /// </summary>
        private void DestroyPlayer()
        {
            // Destroy all the spawned players.
            foreach (BasePlayer spawnedPlayer in spawnedPlayers)
            {
                Destroy(spawnedPlayer.gameObject);
            }

            // Reset each team's game state.
            foreach (Team team in teamByIdSet.Values)
            {
                team.ResetGame();
            }
            
            spawnedPlayers.Clear();
        }
        
        /// <summary>
        /// Handles when a ball enters the drain zone by incrementing the corresponding team's scored balls count.
        /// </summary>
        /// <param name="drainZone">The ball drain zone.</param>
        /// <param name="ball">The ball that entered the drain zone.</param>
        /// <param name="teamId">The ID of the team that the ball belonged to.</param>
        private void HandleAnyBallEnteredDrainZone(BallDrainZone drainZone, Ball.Ball ball, int teamId)
        {
            // Increment the scored balls count for the corresponding team.
            if (teamByIdSet.TryGetValue(teamId, out Team team))
            {
                team.IncrementScoredBalls();
            }
        }
        
        /// <summary>
        /// Handles when both balls have been scored by determining the winning team and adding the round score to their total score.
        /// </summary>
        /// <param name="controller">The ball controller.</param>
        private void HandleBothBallsScored(BallController controller)
        {
            // Determine the winning team and add the round score to their total score.
            long totalScore = GameState.Instance.TotalRoundScore;

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
        
        /// <summary>
        /// Handles the start of a new round by resetting each team's round state.
        /// </summary>
        /// <param name="state">The current game state.</param>
        private void HandleNewRoundStarted(GameState state)
        {
            foreach (Team team in teamByIdSet.Values)
            {
                team.ResetRound();
            }
        }

        /// <summary>
        /// Determines whether there is a winning team.
        /// </summary>
        /// <returns>True if there is a winning team, false otherwise.</returns>
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