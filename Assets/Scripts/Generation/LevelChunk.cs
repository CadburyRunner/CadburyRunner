///<summary>
/// Author: Halen
///
/// Defines a chunk of level that can be generated.
///
///</summary>

using UnityEngine;

namespace CadburyRunner.Generation
{
	public class LevelChunk : MonoBehaviour
	{
		[SerializeField] private Transform[] m_obstaclePositions;
		[SerializeField] private GameObject[] m_possibleObstacles;

		[SerializeField] private Transform m_endPoint;

		public Vector3 EndPoint => m_endPoint.position;
		
		public void Init()
		{
			for (int i = 0; i < m_obstaclePositions.Length - 1 ; i++)
			{
				Instantiate(m_possibleObstacles[Random.Range(0, m_possibleObstacles.Length - 1)], transform);
			}
		}

        private void Update()
        {
			transform.Translate(new Vector3(0, 0, GenerationMetrics.LevelSpeed) * -Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
			{
				GenerationManager.Instance.GenerateNewChunk();
			}
        }
    }
}
