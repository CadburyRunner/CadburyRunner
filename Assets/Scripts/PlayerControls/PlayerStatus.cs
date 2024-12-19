///<summary>
/// Author: Aidan
///
///	Holds player information such as if they have tripped
///
///</summary>

using CadburyRunner.Level;
using CadburyRunner.Mobile;
using CadburyRunner.Obstacle;
using CadburyRunner.PlayerUI;
using CadburyRunner.ScoreSystem;
using UnityEngine;

namespace CadburyRunner.Player
{
	public class PlayerStatus : MonoBehaviour
	{
		[Header("Magnet Variables")]
		[SerializeField] private GameObject[] m_magnetObjects;
		[SerializeField] private SphereCollider m_pickupRadius;
		[SerializeField] private float m_pickupRadiusNormal;
		[SerializeField] private float m_pickupRadiusMagnet;
		private bool m_hasMagnet;
		private float m_magnetTime;

		[Header("Shield Variables")]
        [SerializeField] private GameObject[] m_shieldObjects;
        private bool m_hasShield = false;
		private float m_shieldTime;

		[Header("Point Multiplier")]
		private bool m_hasMultiplier = false;
		private float m_multiplierTime;
		[Header("Player HUD")]
        [SerializeField] private PlayerHUD m_playerHud;


		private bool m_tripped = false;
		private CharacterAnimationController m_anim;

        private void Start()
        {
			m_anim = GetComponentInChildren<CharacterAnimationController>();
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
          m_playerHud.HidePowerup(0);
					foreach (GameObject obj in m_magnetObjects)
						obj.SetActive(false);
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
					foreach (GameObject obj in m_shieldObjects)
						obj.SetActive(false);
          m_playerHud.HidePowerup(1);
        }
			}

			//check to remove multiplier powerup
			if (m_hasMultiplier)
			{
				if (m_multiplierTime >= 0)
				{
					m_multiplierTime -= Time.deltaTime;

				}
				else
				{
					m_hasMultiplier = false;
                    ScoreManager.Instance.ChangeMulti(1f);
                    m_playerHud.HidePowerup(2);
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
				m_anim.Trip();
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
			m_playerHud.ShowPowerup(0, time);
			foreach (GameObject obj in m_magnetObjects)
				obj.SetActive(true);
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
            foreach (GameObject obj in m_shieldObjects)
                obj.SetActive(true);
            m_playerHud.ShowPowerup(1, time);
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
                foreach (GameObject obj in m_shieldObjects)
                    obj.SetActive(false);
				m_playerHud.HidePowerup(1);
                return true;
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		public void MultiplierPickup(float time)
		{
			m_hasMultiplier = true;
			m_multiplierTime = time;
            m_playerHud.ShowPowerup(2, time);
            ScoreManager.Instance.ChangeMulti(2f);
		}

        public void Die(ObstacleType type)
		{
			m_hasMagnet = false;
			m_hasShield = false;
			m_hasMultiplier = false;
			
			m_anim.Death(type);

			// disable input
			gameObject.GetComponent<MobileController>().enabled = false;

			//show loss screen
            GameManager.Instance.OnLose();
        }

    }
}
