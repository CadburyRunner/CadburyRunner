///<summary>
/// Author:
///
///
///
///</summary>

using CadburyRunner.Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace CadburyRunner.Audio
{
	public class MusicPlayer : MonoBehaviour
	{
		[SerializeField] private string m_soundCollection;
		[SerializeField] private string m_soundName;
        [SerializeField] private bool playOnAwake = false;
        [SerializeField] private bool loop = false;
        [SerializeField] private AudioMixerGroup mixerGroup;

        private void Start()
        {
            if (playOnAwake)
            {
                PlayObstacleSound();
            }
        }

        [SerializeField]
        private enum SoundTypes
        {
            Music,
            SFX,
            Obstacle,
        };

        [SerializeField] private SoundTypes m_audioType;

        //private void Start()
        //{
        //    if (m_soundName == string.Empty)
        //        SFXController.Instance.PlayRandomSoundClip(m_soundCollection, AudioTrack.Music);
        //    else
        //        SFXController.Instance.PlaySoundClip(m_soundCollection, m_soundName, AudioTrack.Music);
        //}

        public void PlayObstacleSound()
        {
            if (m_audioType == SoundTypes.Obstacle)
            {
                if (loop == false)
                {
                    SFXController.Instance.PlaySoundClip(m_soundCollection, m_soundName, AudioTrack.Obstacle, mixerGroup, false);
                }
                else
                {
                    SFXController.Instance.PlaySoundClip(m_soundCollection, m_soundName, AudioTrack.Obstacle, mixerGroup, true);
                }
            }
        }
    }
}
