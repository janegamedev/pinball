using Janegamedev.UI;
using Janegamedev.Utilities;
using UnityEngine;

namespace Janegamedev.Audio
{
    public class MusicPlayer : Singleton<MusicPlayer>
    {
        [SerializeField]
        private AudioSettings audioSettings;
        
        [SerializeField]
        private DoubleAudioSource customAudioSource;
        
        [SerializeField]
        private AudioSource sfxSource;

        private AudioSettings.MusicTrackSetup MenuMusicTrack => audioSettings.MenuMusicTrack;
        private AudioSettings.MusicTrackSetup GameplayTrack => audioSettings.GameplayTrack;
        private AudioSettings.MusicTrackSetup GameOverTrack => audioSettings.GameOverTrack;

        private void Start()
        {
            UIController.Instance.StartScreen.OnTransitionInBegun += HandleStartScreenTransitionInBegun;
            UIController.Instance.StartScreen.OnTransitionOutBegun += HandleStartScreenTransitionOutBegun;
            UIController.Instance.GameOverScreen.OnTransitionInBegun += HandleGameOverScreenTransitionInBegun;
        }

        private void OnDestroy()
        {
            UIController.Instance.StartScreen.OnTransitionInBegun -= HandleStartScreenTransitionInBegun;
            UIController.Instance.StartScreen.OnTransitionOutBegun -= HandleStartScreenTransitionOutBegun;
            UIController.Instance.GameOverScreen.OnTransitionInBegun -= HandleGameOverScreenTransitionInBegun;
        }

        private void HandleStartScreenTransitionInBegun(UIScreen screen)
        {
            CrossFade(MenuMusicTrack);
        }
        
        private void HandleStartScreenTransitionOutBegun(UIScreen obj)
        {
            CrossFade(GameplayTrack);
        }

        private void HandleGameOverScreenTransitionInBegun(UIScreen screen)
        {
            CrossFade(GameOverTrack);
        }
        
        private void CrossFade(AudioSettings.MusicTrackSetup setup)
        {
            customAudioSource.CrossFade(setup.track, setup.maxVolume, setup.fadeTime);
        }

        public void PlaySFX(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }
            
            AudioSettings.SFXGroup group = audioSettings.GetSFXGroupByID(id);
            if (group != null)
            {
                sfxSource.PlayOneShot(group.GetRandomTrack(), group.maxVolume); 
            }
        }
    }
}