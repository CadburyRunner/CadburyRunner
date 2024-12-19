///<summary>
/// Author: Halen
///
/// Makes an object move forward when the Player enters its trigger.
///
///</summary>

using CadburyRunner.Audio;
using UnityEngine;

namespace CadburyRunner.Obstacle
{
	public class ForwardCar : MonoBehaviour
	{
		[SerializeField, Min(0)] private float m_speed;

        private bool m_isMoving = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                m_isMoving = true;
                SFXController.Instance.PlaySoundClip("Car", "Honk", AudioTrack.Obstacle);
            }
        }

        private void Update()
        {
            if (m_isMoving)
                transform.position += m_speed * Time.deltaTime * transform.forward;
        }
    }
}
