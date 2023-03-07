using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Janegamedev.Audio;
using Janegamedev.Core.Player;
using Janegamedev.Obstacles;
using Janegamedev.UI;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Janegamedev.Core.Ball
{
    ///<summary>
    /// The BallController is responsible for handling the creation, launching, and tracking of balls in the game.
    ///</summary>
    public class BallController : MonoBehaviour
    {
        private const string BALL_RELEASE_SFX = "ballRelease";

        public static event Action<BallController> OnBothBallsScored;
        public static event Action<BallController, float> OnLaunchForcePercentageUpdated; 

        [SerializeField]
        private Ball[] ballPrefabs;
        [SerializeField]
        private Transform[] spawnPositions;
        [SerializeField]
        private Vector2 spawnForceMinMax = new Vector2(10f, 100f);
        [SerializeField]
        private float launchWaitDuration;

        private readonly Dictionary<Transform, Ball> spawnedNotLaunchedBalls = new Dictionary<Transform, Ball>();
        private readonly HashSet<Ball> spawnedBalls = new HashSet<Ball>();
        private readonly HashSet<Ball> scoredBalls = new HashSet<Ball>();
        private Coroutine launchRoutine;
        private float launchForcePercentage;
        
        private void Awake()
        {
            // Add necessary event handlers and initialize values
            BallDrainZone.OnAnyBallEnteredDrainZone += HandleAnyBallEnteredDrainZone;
            BasePlayer.OnAnyPlayerFireRequest += HandleAnyPlayerFireRequest;
            GameState.OnGameEnded += HandleGameEnded;
            
            if(UIController.Instance != null)
            {
                UIController.Instance.GameplayScreen.OnTransitionInFinished += HandleGameplayScreenTransitionInFinished;
            }
        }

        private void OnDestroy()
        {
            // Remove event handlers and stop coroutines on destruction
            if (launchRoutine != null)
            {
                StopCoroutine(launchRoutine);
                launchRoutine = null;
            }
            
            BallDrainZone.OnAnyBallEnteredDrainZone -= HandleAnyBallEnteredDrainZone;
            BasePlayer.OnAnyPlayerFireRequest -= HandleAnyPlayerFireRequest;
            GameState.OnGameEnded -= HandleGameEnded;
            
            if (UIController.Instance != null)
            {
                UIController.Instance.GameplayScreen.OnTransitionInFinished -= HandleGameplayScreenTransitionInFinished;
            }
        }

        /// <summary>
        /// Handles the completion of the transition-in animation for the gameplay screen.
        /// </summary>
        /// <param name="screen">The gameplay screen.</param>
        private void HandleGameplayScreenTransitionInFinished(UIScreen screen)
        {
            if (launchRoutine != null)
            {
                StopCoroutine(launchRoutine);
                launchRoutine = null;
            }
                
            StartRound();
        }

        /// <summary>
        /// Starts a new round.
        /// </summary>
        private void StartRound()
        {
            DestroyBalls();
            launchRoutine ??= StartCoroutine(LaunchRoutine());
        }

        /// <summary>
        /// Spawns and launches balls for each spawn position.
        /// </summary>
        private IEnumerator LaunchRoutine()
        {
            // Spawn a random ball for each spawn position
            Ball randomBall = ballPrefabs[Random.Range(0, ballPrefabs.Length)];
            for (int i = 0; i < spawnPositions.Length; i++)
            {
                PlaceBall(randomBall, i);
            }
            
            // Tween to gradually increase launch force percentage
            Tween forceTween = DOTween.To(x => launchForcePercentage = x,
                    0f,
                    1f,
                    launchWaitDuration)
                .OnUpdate(HandlePercentageUpdated)
                .OnComplete(() => forceTween = null);

            // Callback for handling launch force percentage update
            void HandlePercentageUpdated()
            {
                OnLaunchForcePercentageUpdated?.Invoke(this, launchForcePercentage);
            }

            // Wait until forceTween is completed
            yield return new WaitUntil(() => forceTween == null);
            
            // Launch each ball at each spawn position
            for (int i = 0; i < spawnPositions.Length; i++)
            {
                LaunchBall(i);
            }

            // Set launchRoutine to null indicating that the launch routine has finished
            launchRoutine = null;
        }
        
        /// <summary>
        /// Places a ball at a specified spawn location and disables it.
        /// </summary>
        /// <param name="ballPrefab">The ball prefab to instantiate.</param>
        /// <param name="teamIndex">The index of the team.</param>
        private void PlaceBall(Ball ballPrefab, int teamIndex)
        {
            Transform teamLaunchPos = spawnPositions[teamIndex];
            if (spawnedNotLaunchedBalls.ContainsKey(teamLaunchPos))
            {
                return;
            }
            
            Ball ball = PlaceBall(ballPrefab, teamLaunchPos.position);
            ball.DisableBall();
            spawnedNotLaunchedBalls.Add(teamLaunchPos, ball);
        }

        /// <summary>
        /// Instantiates a ball at a specified location.
        /// </summary>
        /// <param name="ballPrefab">The ball prefab to instantiate.</param>
        /// <param name="spawnLocation">The location to instantiate the ball.</param>
        /// <returns>The instantiated ball.</returns>
        private Ball PlaceBall(Ball ballPrefab, Vector3 spawnLocation)
        {
            return Instantiate(ballPrefab, spawnLocation, quaternion.identity);
        }
        
        /// <summary>
        /// Launches a ball for a specified team.
        /// </summary>
        /// <param name="teamIndex">The index of the team.</param>
        private void LaunchBall(int teamIndex)
        {
            Transform teamLaunchPos = spawnPositions[teamIndex];
            if (spawnedNotLaunchedBalls.TryGetValue(teamLaunchPos, out Ball ball))
            {
                LaunchBall(ball);
                spawnedNotLaunchedBalls.Remove(teamLaunchPos);
            }
        }

        /// <summary>
        /// Launches a ball.
        /// </summary>
        /// <param name="ball">The ball to launch.</param>
        private void LaunchBall(Ball ball)
        {
            ball.EnableBall();
            float force = Mathf.Lerp(spawnForceMinMax.x, spawnForceMinMax.y, launchForcePercentage);
            ball.AddImpulseForce(Vector3.forward * force);
            spawnedBalls.Add(ball);
            MusicPlayer.Instance.PlaySFX(BALL_RELEASE_SFX);
        }

        /// <summary>
        /// Handles any player request to fire a ball.
        /// </summary>
        private void HandleAnyPlayerFireRequest(BasePlayer player, int teamIndex)
        {
            LaunchBall(teamIndex);
        }
        
        /// <summary>
        /// Adds the scored ball to the list of scored balls and disables it.
        /// If all spawned balls are scored, invokes the OnBothBallsScored event.
        /// </summary>
        /// <param name="zone">The BallDrainZone where the ball entered.</param>
        /// <param name="scoredBall">The ball that scored.</param>
        /// <param name="teamId">The team that scored the ball.</param>
        private void HandleAnyBallEnteredDrainZone(BallDrainZone zone, Ball scoredBall, int teamId)
        {
            scoredBalls.Add(scoredBall);
            scoredBall.DisableBall();

            if (scoredBalls.Count == spawnedBalls.Count)
            {
                OnBothBallsScored?.Invoke(this);
            }
        }

        /// <summary>
        /// Destroys all spawned balls and clears the lists of spawned and scored balls.
        /// </summary>
        private void DestroyBalls()
        {
            foreach (Ball ball in spawnedBalls)
            {
                Destroy(ball.gameObject);
            }
                
            spawnedBalls.Clear();
            scoredBalls.Clear();
        }
        
        /// <summary>
        /// Handles the end of the game by destroying all spawned balls.
        /// </summary>
        /// <param name="state">The state of the game.</param>
        private void HandleGameEnded(GameState state)
        {
            DestroyBalls();
        }
    }
}
