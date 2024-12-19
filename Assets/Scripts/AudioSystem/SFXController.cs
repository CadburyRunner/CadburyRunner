///<summary>
/// Author: Halen
///
/// Singleton sound manager class.
///
///</summary>

using System.Collections.Generic;
using UnityEngine;

namespace CadburyRunner.Audio
{
    public enum AudioTrack
    {
        PlayerMove,
        Footstepts,
        Pickup,
        Obstacle,
        Music,
        LENGTH
    }

    public class SFXController : MonoBehaviour
    {
        [System.Serializable]
        public class SoundClipCollection
        {
            [SerializeField] private string m_collectionName;
            [SerializeField] private List<SoundClip> m_soundClips;
            public string CollectionName { get { return m_collectionName; } }
            public List<SoundClip> SoundClips { get { return m_soundClips; } }
        }

        [System.Serializable]
        public class SoundClip
        {
            [Tooltip("The name of the sound.")]
            [SerializeField] private string m_clipName;
            [Tooltip("The audio file.")]
            [SerializeField] private AudioClip m_audioClip;

            public string ClipName => m_clipName;
            public AudioClip AudioClip => m_audioClip;
        }

        #region Singleton
        public static SFXController Instance;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                AudioTrackSetup();
            }
            else if (Instance != this)
                Destroy(gameObject);
        }
        #endregion

        #region SourceSetup
        // constant source list/tracks
        private AudioSource[] m_audioSources = new AudioSource[0];

        private void AudioTrackSetup()
        {
            int trackCount = (int)AudioTrack.LENGTH;
            // remove all children
            while (transform.childCount != 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }

            // clear array
            m_audioSources = new AudioSource[trackCount];
            // create new children
            for (int i = 0; i < trackCount; i++)
            {
                GameObject newChild = new GameObject();
                newChild.name = ((AudioTrack)i).ToString() + " Source";
                newChild.transform.SetParent(transform);
                m_audioSources[i] = newChild.AddComponent<AudioSource>();
                m_audioSources[i].playOnAwake = false;
            }
        }
        #endregion

        [SerializeField] private List<SoundClipCollection> m_clipCollections;

        private void PlayAudioClip(SoundClip soundClip, AudioTrack track, bool loop)
        {
            m_audioSources[(int)track].clip = soundClip.AudioClip;
        }

        public void PlaySoundClip(string collectionName, string clipName, AudioTrack track, bool loop = false)
        {
            SoundClip soundClip = m_clipCollections.Find(collection => collection.CollectionName == collectionName).SoundClips.Find(clip => clip.ClipName == clipName);
            if (soundClip == null)
            {
                LogMissingClipError(collectionName, clipName);
                return;
            }

            PlayAudioClip(soundClip, track, loop);
        }

        /// <summary>
        /// Plays a random sound clip from a specified collection at a point in world space.
        /// </summary>
        public void PlayRandomSoundClip(string collectionName, AudioTrack track, bool loop = false)
        {
            SoundClipCollection collection = m_clipCollections.Find(collection => collection.CollectionName == collectionName);
            if (collection == null)
            {
                LogMissingCollectionError(collectionName);
                return;
            }

            List<SoundClip> soundClips = collection.SoundClips;
            if (soundClips.Count == 0)
            {
                LogMissingSoundClipsError(collectionName);
                return;
            }

            PlayAudioClip(soundClips[Random.Range(0, soundClips.Count)], track, loop);
        }

        private void LogMissingCollectionError(string collectionName)
        {
            Debug.LogError("The provided SoundClipCollection does not exist. Collection: " + collectionName + ".");
        }

        private void LogMissingClipError(string collectionName, string clipName)
        {
            Debug.LogError("The provided SoundClip or SoundClipCollection does not exist. Collection: " + collectionName + ", Clip: " + clipName + ".");
        }

        private void LogMissingSoundClipsError(string collectionName)
        {
            Debug.LogError("The provided SoundClipCollection does not contain any SoundClips. Collection: " + collectionName + ".");
        }
    }
}