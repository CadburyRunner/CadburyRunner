///<summary>
/// Author: Aidan
///
///	Holds player information such as if they have tripped
///
///</summary>

using CadburyRunner.Audio;
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

		[Header("Barking Sounds")]
		[SerializeField] private Vector2 m_barkDelayMinMax;
		private float m_dogBarkTimer;

        private void Start()
        {
			m_anim = GetComponentInChildren<CharacterAnimationController>();
			m_pickupRadius.radius = m_pickupRadiusNormal;
			SetBarkTimer();
        }

		private void Update()
		{
			//if player has recently tripped, recover speed until it is at current speed
			if (m_tripped)
			{
				if (LevelManager.Instance.CurrentLevelSpeed == LevelMetrics.Speed)
				{
					m_tripped = false;
				}
			}

			// check to remove magnet powerup
			if (m_hasMagnet)
			{
				if (m_magnetTime <= 0)
				{
					SetMagnetObjects(false);
					m_playerHud.HidePowerup(0);
				}
				else
				{
					m_magnetTime -= Time.deltaTime;
				}
			}

			// check to remove shield powerup
			if (m_hasShield)
			{
				if (m_shieldTime <= 0)
				{
					SetShieldObjects(false);
					m_playerHud.HidePowerup(1);
				}
				else
				{
					m_shieldTime -= Time.deltaTime;
				}
			}

			//check to remove multiplier powerup
			if (m_hasMultiplier)
			{
				if (m_multiplierTime <= 0)
				{
					SetMultiplierObjects(false);
					m_playerHud.HidePowerup(2);
				}
				else
				{
					m_multiplierTime -= Time.deltaTime;
				}
			}

			// dog barks
			if (m_dogBarkTimer <= 0)
			{
				SFXController.Instance.PlayRandomSoundClip("Dog Bark", AudioTrack.Character);
				SetBarkTimer();
			}
			else
				m_dogBarkTimer -= Time.deltaTime;
        }

        public void Trip(ObstacleSoundType soundType)
		{
			if (ShieldCheck())
				return;
			
			if (m_tripped)
			{
				//if player has already tripped die
				Die(ObstacleType.Trip, soundType);
			}
			else
			{
				//if player hasn't tripped set tripped to true and lower speed
				m_tripped = true;
				m_anim.Trip();
				LevelManager.Instance.SetLevelSpeed(LevelMetrics.Speed / 8f);
			}
		}

        #region GivePowerups
        /// <summary>
        /// initiate magnet pickup
        /// </summary>
        /// <param name="time">how long pickup will last for</param>
        public void GiveMagnetPickup()
		{
			SetMagnetObjects(true);
			m_magnetTime = LevelMetrics.PowerupTime;
			m_playerHud.ShowPowerup(0);
			m_pickupRadius.radius = m_pickupRadiusMagnet;
		}

		/// <summary>
		/// initiate shield pickup
		/// </summary>
		/// <param name="time">how long pickup will last for</param>
		public void GiveShieldPickup()
		{
			SetShieldObjects(true);
			m_shieldTime = LevelMetrics.PowerupTime;
            m_playerHud.ShowPowerup(1);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		public void GiveMultiplierPickup()
		{
			SetMultiplierObjects(true);
			m_multiplierTime = LevelMetrics.PowerupTime;
            m_playerHud.ShowPowerup(2);
		}
        #endregion

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

        public void Die(ObstacleType type, ObstacleSoundType sound)
		{
			m_hasMagnet = false;
			m_hasShield = false;
			m_hasMultiplier = false;
			
			m_anim.Death(type, sound);

			// disable input
			gameObject.GetComponent<MobileController>().enabled = false;

			//show loss screen
            GameManager.Instance.OnLose();
        }

		private void SetMagnetObjects(bool value)
		{
			m_hasMagnet = value;
			foreach (GameObject obj in m_magnetObjects)
			{
				obj.SetActive(value);
			}
            m_pickupRadius.radius = value ? m_pickupRadiusMagnet : m_pickupRadiusNormal;
        }

        private void SetShieldObjects(bool value)
		{
			m_hasShield = false;
			foreach (GameObject obj in m_shieldObjects)
            {
                obj.SetActive(value);
            }
        }

		private void SetMultiplierObjects(bool value)
		{
			m_hasMultiplier = value;
			ScoreManager.Instance.ChangeMulti(value ? 2f : 1f);
		}

		private void SetBarkTimer()
		{
            m_dogBarkTimer = Random.Range(m_barkDelayMinMax.x, m_barkDelayMinMax.y);
        }

    }
}
