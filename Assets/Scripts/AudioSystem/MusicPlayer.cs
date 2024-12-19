///<summary>
/// Author:
///
///
///
///</summary>

using CadburyRunner.Audio;
using UnityEngine;

namespace CadburyRunner.Audio
{
	public class MusicPlayer : MonoBehaviour
	{
		[SerializeField] private string m_soundCollection;
		[SerializeField] private string m_soundName;

        private void Start()
        {
            if (m_soundName == string.Empty)
                SFXController.Instance.PlayRandomSoundClip(m_soundCollection, AudioTrack.Music);
            else
                SFXController.Instance.PlaySoundClip(m_soundCollection, m_soundName, AudioTrack.Music);
        }
    }
}
