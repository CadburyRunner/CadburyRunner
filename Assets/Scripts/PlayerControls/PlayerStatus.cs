///<summary>
/// Author: Aidan
///
///	Holds player information such as if they have tripped
///
///</summary>

using CadburyRunner.Level;
using CadburyRunner.Obstacle;
using UnityEngine;

namespace CadburyRunner.Player
{
	public class PlayerStatus : MonoBehaviour
	{

		private bool m_tripped = false;

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

        private void Update()
        {
			//if player has recently tripped recover speed until it is at current speed
			if (m_tripped)
			{
				if(LevelManager.Instance.CurrentLevelSpeed == LevelMetrics.Speed)
				{
					m_tripped = false;
				}
			}
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
