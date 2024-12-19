///<summary>
/// Author: Halen
///
/// Controls stuff related to the Character's animations.
///
///</summary>

using UnityEngine;

namespace CadburyRunner
{
    using Audio;
    using CadburyRunner.Obstacle;
    using Level;

	public class CharacterAnimationController : MonoBehaviour
	{
		private Animator m_anim;

        private void Awake()
        {
            m_anim = GetComponent<Animator>();
        }

        private void Start()
        {
            m_anim.SetFloat("MoveSpeed", LevelManager.Instance.CurrentLevelSpeed);
        }

        private void FixedUpdate()
        {
            m_anim.SetFloat("MoveSpeed", LevelManager.Instance.CurrentLevelSpeed);
        }

        public void SetGrounded(bool value)
        {
            m_anim.SetBool("IsGrounded", value);
        }

        public void Jump()
        {
            m_anim.Play("Jump");

            // play sound effect
            SFXController.Instance.PlaySoundClip("PlayerMove", "Jump", AudioTrack.PlayerMove);
        }

        public void Trip()
        {
            m_anim.Play("Trip");
        }

        public void SetSliding(bool value)
        {
            if (value)
            {
                m_anim.Play("Slide");

                // play sound effect
                SFXController.Instance.PlaySoundClip("PlayerMove", "Slide", AudioTrack.PlayerMove);
            }
            else
            {
                m_anim.SetTrigger("StopSliding");
            }
        }

        public void PlayFootstep()
        {
            SFXController.Instance.PlayRandomSoundClip("Footsteps", AudioTrack.Character);
        }

        public void Death(ObstacleType type, ObstacleSoundType sound)
        {
            // make sure animation plays even with timescale at 0
            m_anim.updateMode = AnimatorUpdateMode.UnscaledTime;

            // play a relevant sound effect
            string collectionName = string.Empty;
            string clipName = string.Empty;
            switch (sound)
            {
                case ObstacleSoundType.Hard:
                    collectionName = "Collision Hard";
                    break;
                case ObstacleSoundType.Metal:
                    collectionName = "Collision Metal";
                    break;
                case ObstacleSoundType.Tree:
                    collectionName = "Tree";
                    clipName = "TreeImpact";
                    break;
            }

            if (clipName == string.Empty)
                SFXController.Instance.PlayRandomSoundClip(collectionName, AudioTrack.Character);
            else
                SFXController.Instance.PlaySoundClip(collectionName, clipName, AudioTrack.Character);

            // play the correct death anim
            string name = type.ToString() + " Death";
            m_anim.Play(name);
        }
    }
}
