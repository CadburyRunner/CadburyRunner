///<summary>
/// Author: Aidan
///
///	Holds player information such as if they have tripped
///
///</summary>

using CadburyRunner.Level;
using CadburyRunner.Obstacle;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CadburyRunner.Player
{
	public class PlayerStatus : MonoBehaviour
	{
		[Header("Magnet Variables")]
		[SerializeField] private SphereCollider m_pickupRadius;
		[SerializeField] private float m_pickupRadiusNormal;
		[SerializeField] private float m_pickupRadiusMagnet;
		[SerializeField] private GameObject m_magnetObject;
		private bool m_hasMagnet;
		private float m_magnetTime;
		[Header("Shield Variables")]
        [SerializeField] private GameObject m_shieldObject;
        private bool m_hasShield = false;
		private float m_shieldTime;


		private bool m_tripped = false;

        private void Start()
        {
			m_pickupRadius.radius = m_pickupRadiusNormal;
        }

        private void Update()
        {
			//if player has recently tripped, recover speed until it is at current speed
			if (m_tripped)
			{
				if(LevelManager.Instance.CurrentLevelSpeed == LevelMetrics.Speed)
				{
					m_tripped = false;
				}
			}

			// check to remove magnet powerup
			if (m_hasMagnet)
			{
				if (m_magnetTime >= 0)
				{
					m_magnetTime -= Time.deltaTime;
				}
				else 
				{
					m_hasMagnet = false;
					m_magnetObject.SetActive(false);
                    m_pickupRadius.radius = m_pickupRadiusNormal;
                }
			}

			// check to remove shield powerup
			if (m_hasShield)
			{
				if(m_shieldTime >= 0)
				{
					m_shieldTime -= Time.deltaTime;
				}
				else
				{
					m_hasShield = false;
					m_shieldObject.SetActive(false);
				}
			}
        }

        public void Trip()
		{
			if (ShieldCheck())
				return;
			
			if (m_tripped)
			{
				//if player has already tripped die
				Die(ObstacleType.Trip);
			}
			else
			{
				//if player hasn't tripped set tripped to true and lower speed
				m_tripped = true;
				LevelManager.Instance.SetLevelSpeed(LevelMetrics.Speed / 8f);
			}
		}

		/// <summary>
		/// initiate magnet pickup
		/// </summary>
		/// <param name="time">how long pickup will last for</param>
		public void MagnetPickup(float time)
		{
			m_hasMagnet = true;
			m_magnetTime = time;
			m_magnetObject.SetActive(true);
			m_pickupRadius.radius = m_pickupRadiusMagnet;
		}
		/// <summary>
		/// initiate shield pickup
		/// </summary>
		/// <param name="time">how long pickup will last for</param>
		public void ShieldPickup(float time)
		{
			m_hasShield = true;
			m_shieldTime = time;
            m_shieldObject.SetActive(true);
        }
		/// <summary>
		/// Check if the shield is active and resets it
		/// </summary>
		/// <returns>true if the shield is active</returns>
		private bool ShieldCheck()
		{
			if (m_hasShield)
			{
				m_hasShield = false;
				m_shieldTime = 0;
                m_shieldObject.SetActive(false);
                return true;
			}
			return false;
		}

        public void Die(ObstacleType type)
		{
			switch (type)
			{
				case ObstacleType.Trip:
					break;
				case ObstacleType.Slam:
					break;
				case ObstacleType.Fall:
					break;
			}
			//show loss screen
            GameManager.Instance.OnLose();
        }

    }
}
