using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Janegamedev.Obstacles;
using Janegamedev.UI;
using Unity.Mathematics;
using UnityEngine;

namespace Janegamedev.Core
{
    public class BallLauncher : MonoBehaviour
    {
        public event Action<BallLauncher, int> OnBothBallsScored;
        public static event Action<BallLauncher, float> OnLaunchForcePercentageUpdated; 

        [SerializeField]
        private Ball ballPrefab;
        [SerializeField]
        private Transform[] spawnPositions;
        [SerializeField]
        private Vector2 spawnForceMinMax = new Vector2(10f, 100f);
        [SerializeField]
        private float launchWaitDuration;

        private readonly HashSet<Ball> spawnedBalls = new HashSet<Ball>();
        private readonly HashSet<Ball> scoredBalls = new HashSet<Ball>();
        private Dictionary<Transform, Ball> spawnedNotLaunchedBalls = new Dictionary<Transform, Ball>();
        private Coroutine launchRoutine;
        private float launchForcePercentage;
        
        private void Awake()
        {
            BallScoreZone.OnAnyBallEnteredScoreZone += HandleAnyBallEnteredScoreZone;
            BasePlayer.OnAnyPlayerFireRequest += HandleAnyPlayerFireRequest;
            
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
            
            BallScoreZone.OnAnyBallEnteredScoreZone -= HandleAnyBallEnteredScoreZone;
            BasePlayer.OnAnyPlayerFireRequest -= HandleAnyPlayerFireRequest;
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
            for (int i = 0; i < spawnPositions.Length; i++)
            {
                PlaceBall(i);
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
        
        private void PlaceBall(int teamIndex)
        {
            Transform teamLaunchPos = spawnPositions[teamIndex];
            if (spawnedNotLaunchedBalls.ContainsKey(teamLaunchPos))
            {
                return;
            }
            
            Ball ball = PlaceBall(teamLaunchPos.position);
            spawnedNotLaunchedBalls.Add(teamLaunchPos, ball);
        }

        private Ball PlaceBall(Vector3 spawnLocation)
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
            float force = Mathf.Lerp(spawnForceMinMax.x, spawnForceMinMax.y, launchForcePercentage);
            ball.AddForce(Vector3.forward * force);
            spawnedBalls.Add(ball);
        }
        
        private void HandleAnyPlayerFireRequest(BasePlayer player, int teamIndex)
        {
            LaunchBall(teamIndex);
        }
        
        private void HandleAnyBallEnteredScoreZone(BallScoreZone zone, Ball scoredBall, int teamId)
        {
            scoredBalls.Add(scoredBall);
            scoredBall.MainRigidbody.isKinematic = true;

            if (scoredBalls.Count == spawnedBalls.Count)
            {
                OnBothBallsScored?.Invoke(this, teamId);
                StartRound();
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
    }
}
