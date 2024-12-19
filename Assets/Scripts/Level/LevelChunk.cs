///<summary>
/// Author: Halen
///
/// Defines a chunk of level that can be generated.
///
///</summary>

using UnityEditor;
using UnityEngine;

namespace CadburyRunner.Level
{
	public class LevelChunk : MonoBehaviour
	{
		[SerializeField] private Transform m_endPoint;

		[Header("Pickup Spawning")]
		[Tooltip("Appears Pink in the scene view.")]
		[SerializeField, Range(0, 1)] private float m_pickupSpawnChance = 0.8f;
		[SerializeField] private Transform[] m_pickupPositions;

		[Header("Obstacle Spawning")]
		[Tooltip("Appears Blue in the scene view.")]
		[SerializeField, Range(0, 1)] private float m_obstacleSpawnChance = 0.33f;
		[SerializeField] private Transform[] m_obstaclePositions;
		[SerializeField] private GameObject[] m_possibleObstacles;

		public Vector3 EndPoint => m_endPoint.position;
		
		public void Init()
		{
			// spawn pickups
			for (int i = 0; i < m_pickupPositions.Length; i++)
			{
				// roll to create pickup
				// if get within spawn chance, spawn a choc bar
				// if get outside of spawn chance, spawn a powerup
				if (Random.value <= m_pickupSpawnChance)
					Instantiate(LevelManager.Instance.Pickups[0], m_pickupPositions[i]);
				else
					Instantiate(LevelManager.Instance.Pickups[Random.Range(1, LevelManager.Instance.Pickups.Length)], m_pickupPositions[i]);
			}

			// spawn obstacles
			for (int i = 0; i < m_obstaclePositions.Length; i++)
			{
				// roll to create obstacle
				if (Random.value <= m_obstacleSpawnChance)
					Instantiate(m_possibleObstacles[Random.Range(0, m_possibleObstacles.Length)], m_obstaclePositions[i].transform);
			}
		}

        private void Update()
        {
			transform.Translate(new Vector3(0, 0, LevelManager.Instance.CurrentLevelSpeed * -Time.deltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
			{
				LevelManager.Instance.GenerateNewChunk();
			}
        }

#if UNITY_EDITOR
		private void OnDrawGizmos()
        {
			Handles.color = Color.magenta;
			// draw pickup positions
			for (int i = 0; i < m_pickupPositions.Length; i++)
			{
                Handles.DrawWireCube(m_pickupPositions[i].position, Vector3.one);
            }

            Handles.color = Color.blue;
			// draw obstacle positions
			for (int i = 0; i < m_obstaclePositions.Length; i++)
            {
				Handles.DrawWireCube(m_obstaclePositions[i].position, Vector3.one);
            }
        }
#endif
	}
}
