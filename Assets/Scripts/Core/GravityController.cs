using System.Collections;
using Janegamedev.Utilities;
using UnityEngine;

namespace Janegamedev.Core
{
    public class GravityController : Singleton<GravityController>
    {
        private const float NORMAL_GRAVITY = -9.8f;

        private Coroutine noGravityRoutine;

        private void Start()
        {
            GameState.OnNewRoundStarted += HandleNewRoundStarted;
        }

        private void OnDestroy()
        {
            GameState.OnNewRoundStarted -= HandleNewRoundStarted;
        }

        private void HandleNewRoundStarted(GameState state)
        {
            StopGravityRoutine();
            EnableGravity();
        }

        public void DisableGravity(float duration)
        {
            StopGravityRoutine();
            noGravityRoutine = StartCoroutine(GravityRoutine(duration));
        }

        private void StopGravityRoutine()
        {
            if (noGravityRoutine == null)
            {
                return;
            }
            StopCoroutine(noGravityRoutine);
            noGravityRoutine = null;
        }

        private IEnumerator GravityRoutine(float duration)
        {
            SetGravityZ(-NORMAL_GRAVITY);
            yield return new WaitForSeconds(duration);
            EnableGravity();
        }

        private void EnableGravity()
        {
            SetGravityZ(NORMAL_GRAVITY);
        }

        private void SetGravityZ(float z)
        {
            Physics.gravity = new Vector3(0, NORMAL_GRAVITY, z);
        }
    }
}