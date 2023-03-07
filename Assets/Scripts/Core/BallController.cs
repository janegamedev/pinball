using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Janegamedev.Audio;
using Janegamedev.Obstacles;
using Janegamedev.UI;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Janegamedev.Core
{
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

        private void HandleGameplayScreenTransitionInFinished(UIScreen screen)
        {
            if (launchRoutine != null)
            {
                StopCoroutine(launchRoutine);
                launchRoutine = null;
            }
                
            StartRound();
        }

        private void StartRound()
        {
            DestroyBalls();
            launchRoutine ??= StartCoroutine(LaunchRoutine());
        }

        private IEnumerator LaunchRoutine()
        {
            Ball randomBall = ballPrefabs[Random.Range(0, ballPrefabs.Length)];
            for (int i = 0; i < spawnPositions.Length; i++)
            {
                PlaceBall(randomBall, i);
            }
            
            Tween forceTween = DOTween.To(x => launchForcePercentage = x,
                    0f,
                    1f,
                    launchWaitDuration)
                .OnUpdate(HandlePercentageUpdated)
                .OnComplete(() => forceTween = null);

            void HandlePercentageUpdated()
            {
                OnLaunchForcePercentageUpdated?.Invoke(this, launchForcePercentage);
            }

            yield return new WaitUntil(() => forceTween == null);
            
            for (int i = 0; i < spawnPositions.Length; i++)
            {
                LaunchBall(i);
            }

            launchRoutine = null;
        }
        
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

        private Ball PlaceBall(Ball ballPrefab, Vector3 spawnLocation)
        {
            return Instantiate(ballPrefab, spawnLocation, quaternion.identity);
        }
        
        private void LaunchBall(int teamIndex)
        {
            Transform teamLaunchPos = spawnPositions[teamIndex];
            if (spawnedNotLaunchedBalls.TryGetValue(teamLaunchPos, out Ball ball))
            {
                LaunchBall(ball);
                spawnedNotLaunchedBalls.Remove(teamLaunchPos);
            }
        }

        private void LaunchBall(Ball ball)
        {
            ball.EnableBall();
            float force = Mathf.Lerp(spawnForceMinMax.x, spawnForceMinMax.y, launchForcePercentage);
            ball.AddImpulseForce(Vector3.forward * force);
            spawnedBalls.Add(ball);
            MusicPlayer.Instance.PlaySFX(BALL_RELEASE_SFX);
        }
        
        private void HandleAnyPlayerFireRequest(BasePlayer player, int teamIndex)
        {
            LaunchBall(teamIndex);
        }
        
        private void HandleAnyBallEnteredDrainZone(BallDrainZone zone, Ball scoredBall, int teamId)
        {
            scoredBalls.Add(scoredBall);
            scoredBall.DisableBall();

            if (scoredBalls.Count == spawnedBalls.Count)
            {
                OnBothBallsScored?.Invoke(this);
            }
        }

        private void DestroyBalls()
        {
            foreach (Ball ball in spawnedBalls)
            {
                Destroy(ball.gameObject);
            }
                
            spawnedBalls.Clear();
            scoredBalls.Clear();
        }
        
        private void HandleGameEnded(GameState state)
        {
            DestroyBalls();
        }
    }
}
