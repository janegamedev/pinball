using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Janegamedev.Audio
{ 
    [ExecuteInEditMode]
    public class DoubleAudioSource : MonoBehaviour
    {
        [SerializeField]
        private AudioSource source0;
        
        [SerializeField]
        private AudioSource source1;

        private Coroutine fadeRoutine;
        private Coroutine curSourceFadeRoutine;
        private Coroutine newSourceFadeRoutine;

        private bool IsPlaying => source0.isPlaying || source1.isPlaying;
        private bool Source0IsPlaying => source0.isPlaying;

        public bool Loop
        {
            set
            {
                source0.loop = value;
                source1.loop = value;
            }
        }
        
        public void CrossFade(AudioClip clip, float maxVolume, float fadingTime, float delayBeforeCrossFade = 0)
        {
            fadeRoutine ??= StartCoroutine(Fade(clip, maxVolume, fadingTime, delayBeforeCrossFade));
        }
        
        private IEnumerator Fade(AudioClip clip, float maxVolume, float fadingTime, float delayBeforeCrossFade = 0)
        {
            if (delayBeforeCrossFade > 0)
            {
                yield return new WaitForSeconds(delayBeforeCrossFade);
            }

            AudioSource curActiveSource, newActiveSource;

            if (Source0IsPlaying)
            {
                curActiveSource = source0;
                newActiveSource = source1;
            }
            else
            {
                curActiveSource = source1;
                newActiveSource = source0;
            }

            newActiveSource.clip = clip;
            newActiveSource.Play();
            newActiveSource.volume = 0;

            if (curSourceFadeRoutine != null)
            {
                StopCoroutine(curSourceFadeRoutine);
            }

            if (newSourceFadeRoutine != null)
            {
                StopCoroutine(newSourceFadeRoutine);
            }

            curSourceFadeRoutine = StartCoroutine(FadeSource(curActiveSource, curActiveSource.volume, 0, fadingTime, () => curActiveSource.Stop()));
            newSourceFadeRoutine = StartCoroutine(FadeSource(newActiveSource, newActiveSource.volume, maxVolume, fadingTime));
            fadeRoutine = null;
        }

        private IEnumerator FadeSource(AudioSource sourceToFade, float startVolume, float endVolume, float duration, Action completionAction = null)
        {
            float volume = 0;
            Tween volumeTween = DOTween.To(x => volume = x,
                    startVolume,
                    endVolume,
                    duration)
                .OnUpdate(handleVolumeUpdated)
                .OnComplete(() => volumeTween = null);
            
            void handleVolumeUpdated()
            {
                sourceToFade.volume = volume;
            }

            yield return new WaitUntil(() => volumeTween == null);
            
            completionAction?.Invoke();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}