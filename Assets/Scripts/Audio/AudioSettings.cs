using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Janegamedev.Audio
{
    [CreateAssetMenu(menuName = "Settings/Audio", fileName = "AudioSettings", order = 0)]
    public class AudioSettings : ScriptableObject
    {
        [Serializable]
        public class MusicTrackSetup
        {
            public AudioClip track;
            public float maxVolume = 1f;
            public float fadeTime = 1f;
        }
        
        [Header("Music")]
        [SerializeField]
        private MusicTrackSetup menuMusicTrack;
        public MusicTrackSetup MenuMusicTrack => menuMusicTrack;
        [SerializeField]
        private MusicTrackSetup gameplayTrack;
        public MusicTrackSetup GameplayTrack => gameplayTrack;
        [SerializeField]
        private MusicTrackSetup gameOverTrack;
        public MusicTrackSetup GameOverTrack => gameOverTrack;

        [Serializable]
        public class SFXGroup
        {
            public string id;
            public AudioClip[] tracks;
            public float maxVolume = 1f;

            public AudioClip GetRandomTrack()
            {
                return tracks[Random.Range(0, tracks.Length)];
            }
        }

        [Header("SFX")]
        [SerializeField]
        private SFXGroup[] sfxGroups;

        private Dictionary<string, SFXGroup> sfxGroupsByID = new Dictionary<string, SFXGroup>();

        public Dictionary<string, SFXGroup> SfxGroupsByID
        {
            get
            {
                if (sfxGroupsByID == null || sfxGroupsByID.Count < 1)
                {
                    InitializeSfxGroupsDict();
                }

                return sfxGroupsByID;
            }
        }

        public SFXGroup GetSFXGroupByID(string id)
        {
            if (SfxGroupsByID.TryGetValue(id, out SFXGroup group))
            {
                return group;
            }

            return null;
        }

        private void InitializeSfxGroupsDict()
        {
            foreach (SFXGroup group in sfxGroups)
            {
                if (sfxGroupsByID.ContainsKey(group.id))
                {
                    continue;
                }
                
                sfxGroupsByID.Add(group.id, group);
            }
        }
    }
}