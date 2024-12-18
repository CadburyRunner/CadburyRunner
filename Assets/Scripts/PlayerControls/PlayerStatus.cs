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
		[SerializeField] private SphereCollider m_pickupRadius;
		[SerializeField] private float m_pickupRadiusNormal;
		[SerializeField] private float m_pickupRadiusMagnet;
		private bool m_hasMagnet;
		private float m_magnetTime;

		private bool m_hasShield = false;


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

			if (m_hasMagnet)
			{
				if (m_magnetTime >= 0)
				{
					m_magnetTime -= Time.deltaTime;
				}
				else 
				{
					m_hasMagnet = false;
                    m_pickupRadius.radius = m_pickupRadiusNormal;
                }
			}
        }

        public void Trip()
		{
			if (m_tripped)
			{
				//if player has already tripped die
				Die(ObstacleType.Trip);
			}
			else
			{
				//if player hasn't tripped set tripped to true and lower speed
				m_tripped = true;
				LevelManager.Instance.SetLevelSpeed(1f);
			}
		}


		public void MagnetPickup(float time)
		{
			m_hasMagnet = true;
			m_magnetTime = time;
			m_pickupRadius.radius = m_pickupRadiusMagnet;
		}

        public void Die(ObstacleType type)
		{
            switch (type)
            {
                case ObstacleType.Trip:
					//do trip logic
                    break;
                case ObstacleType.Slam:
					//do slam logic
                    break;
                case ObstacleType.Fall:
					//do fall logic
                    break;
            }
			//show loss screen
            GameManager.Instance.OnLose();
        }

    }
}
