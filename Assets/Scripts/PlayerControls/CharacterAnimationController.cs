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
            SFXController.Instance.PlayRandomSoundClip("Footsteps", AudioTrack.Footsteps);
        }

        public void Death(ObstacleType type)
        {
            m_anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            string name = type.ToString() + " Death";
            m_anim.Play(name);
        }
    }
}
