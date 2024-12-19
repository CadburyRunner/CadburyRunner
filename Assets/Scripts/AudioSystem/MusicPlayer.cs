///<summary>
/// Author:
///
///
///
///</summary>

using UnityEngine;
using UnityEngine.Audio;

namespace CadburyRunner.Audio
{
	public class MusicPlayer : MonoBehaviour
	{
		[SerializeField] private string m_soundCollection;
		[SerializeField] private string m_soundName;
        [SerializeField] private bool m_playOnStart = false;
        [SerializeField] private bool m_loop = false;
        [SerializeField] private AudioTrack m_track;

        private void Start()
        {
            if (m_playOnStart)
            {
                Play();
            }
        }

        public void Play()
        {
            if (m_soundName == string.Empty)
                SFXController.Instance.PlayRandomSoundClip(m_soundCollection, m_track, m_loop);
            else
                SFXController.Instance.PlaySoundClip(m_soundCollection, m_soundName, m_track, m_loop);
        }
    }
}
