using System.Collections;
using Janegamedev.Utilities;
using UnityEngine;

namespace Janegamedev.Core
{
    /// <summary>
    /// This class controls the gravity of the game.
    /// </summary>
    public class GravityController : Singleton<GravityController>
    {
        private Coroutine gravityRoutine;
        private Vector3 originalGravity;
        private Vector3 switchedGravity;

        private void Start()
        {
            // Sets up the original gravity vector and the switched gravity vector
            originalGravity = Physics.gravity;
            switchedGravity = originalGravity;
            switchedGravity.z = -switchedGravity.z;
            
            // Subscribes to the events
            GameState.OnNewRoundStarted += HandleNewRoundStarted;
        }

        private void OnDestroy()
        {
            // Unsubscribes from the events.
            GameState.OnNewRoundStarted -= HandleNewRoundStarted;
        }

        /// <summary>
        /// Handles new round started
        /// Resets the gravity to the original gravity vector
        /// </summary>
        private void HandleNewRoundStarted(GameState state)
        {
            StopGravityRoutine();
            ResetGravity();
        }

        /// <summary>
        /// Switches gravity for a specified duration.
        /// </summary>
        /// <param name="duration">The duration in seconds to disable gravity.</param>
        public void SwitchGravity(float duration)
        {
            StopGravityRoutine();
            gravityRoutine = StartCoroutine(GravitySwitchRoutine(duration));
        }

        /// <summary>
        /// Stops the gravityRoutine coroutine if it is running.
        /// </summary>
        private void StopGravityRoutine()
        {
            if (gravityRoutine == null)
            {
                return;
            }
            StopCoroutine(gravityRoutine);
            gravityRoutine = null;
        }

        /// <summary>
        /// Switches the gravity to the switchedGravity vector for a specified duration.
        /// </summary>
        /// <param name="duration">The duration in seconds to switch the gravity.</param>
        private IEnumerator GravitySwitchRoutine(float duration)
        {
            SetGravity(switchedGravity);
            yield return new WaitForSeconds(duration);
            ResetGravity();
        }

        /// <summary>
        /// Resets the gravity to the original gravity vector.
        /// </summary>
        private void ResetGravity()
        {
            SetGravity(originalGravity);
        }

        /// <summary>
        /// Sets the gravity to the specified gravity vector.
        /// </summary>
        /// <param name="gravity">The gravity vector to set.</param>
        private void SetGravity(Vector3 gravity)
        {
            Physics.gravity = gravity;
        }
    }
}