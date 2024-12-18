///<summary>
/// Author: Emmanuel
///
/// Handles all audio.
///
///</summary>

using UnityEngine;
using UnityEngine.Audio;

namespace CadburyRunner.AudioSystem
{
	public class AudioSystem : MonoBehaviour
	{
		[SerializeField] private AudioClip[] m_soundClips;			// Array of all AudioClips.
		[SerializeField] private AudioSource[] m_soundSources;      // Array of all AudioSources.

		// Plays the specified sound using the "soundIndex" int passed through.
		public void PlaySound(int soundIndex, int sourceIndex)
		{
			AudioSource soundSource = m_soundSources[sourceIndex];	// Find the necessary AudioSource.
			if (soundSource.isPlaying) { soundSource.Stop(); }		// If the AudioSource is already playing, stop it so we can change the AudioClip without issues.
			soundSource.clip = m_soundClips[soundIndex];			// Update the current AudioClip for the necessary AudioSource to the new necessary Audio Clip.
			soundSource.Play();										// Make the AudioSource start playing its new sound.
		}

        private void Update()
        {
			// TESTING & DEBUGGING
			// -----------------------------------
            if (Input.GetKeyUp(KeyCode.DownArrow))
			{
				PlaySound(0, 0);
			}

            if (Input.GetKeyUp(KeyCode.UpArrow))
			{
				PlaySound(1, 0);
			}
			// -----------------------------------
        }
    }
}
