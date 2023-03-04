using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Janegamedev
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField]
        private Ball ball;
        [SerializeField]
        private Transform spawnPosition;
        [SerializeField]
        private float spawnForce = 10;

        private readonly HashSet<Ball> spawnedBalls = new HashSet<Ball>();

        private void Awake()
        {
            BallScoreZone.OnAnyBallEnteredScoreZone += HandleAnyBallEnteredScoreZone;
        }

        private void OnDestroy()
        {
            BallScoreZone.OnAnyBallEnteredScoreZone -= HandleAnyBallEnteredScoreZone;
        }

        [ContextMenu("Spawn ball")]
        public void SpawnBall()
        {
            Ball spawnedBall = Instantiate(ball, spawnPosition.position, quaternion.identity);
            spawnedBall.AddForce(spawnPosition.up * spawnForce);
            spawnedBalls.Add(spawnedBall);
        }
        
        private void HandleAnyBallEnteredScoreZone(BallScoreZone zone, Ball scoredBall)
        {
            spawnedBalls.Remove(scoredBall);
            scoredBall.BroadcastBallScored();
            Destroy(scoredBall.gameObject);
        }
    }
}
