///<summary>
/// Author: Halen
///
/// Handles the generation of the level.
///
///</summary>

using System.Collections;
using UnityEngine;

namespace CadburyRunner.Level
{
	public class LevelManager : MonoBehaviour
	{
		#region Singleton
		public static LevelManager Instance;
        private void Awake()
        {
			if (!Instance)
				Instance = this;
			else
			{
				Destroy(gameObject);
				DontDestroyOnLoad(gameObject);
			}

        }
        #endregion

        [SerializeField] private LevelChunk[] m_possibleLevelChunks;

		private LevelChunk m_previousChunk;
		private LevelChunk m_currentChunk;
		private LevelChunk m_nextChunk;

		private float m_currentLevelSpeed;
		public float CurrentLevelSpeed => m_currentLevelSpeed;

		bool m_returningToNormalSpeed;

        private void Start()
        {
			m_currentLevelSpeed = 0;
			m_returningToNormalSpeed = true;
			GenerateNewChunk();
			m_nextChunk.GetComponent<Collider>().enabled = false;
			GenerateNewChunk();
			GenerateNewChunk();
        }

        private void Update()
        {
            if (m_returningToNormalSpeed)
			{
				m_currentLevelSpeed = Mathf.MoveTowards(m_currentLevelSpeed, LevelMetrics.Speed, LevelMetrics.Acceleration * Time.deltaTime);

				// if reaches target, stop
				if (m_currentLevelSpeed == LevelMetrics.Speed)
					m_returningToNormalSpeed = false;
			}
        }

        public void GenerateNewChunk()
		{ 
			// if there is a current chunk, spawn at the end of that chunk
			Vector3 spawnPosition = Vector3.zero;
			if (m_nextChunk)
				spawnPosition = m_nextChunk.EndPoint;

			// create new chunk
			LevelChunk newChunk = Instantiate(m_possibleLevelChunks[Random.Range(0, m_possibleLevelChunks.Length)], spawnPosition, Quaternion.identity);
			newChunk.Init();

            // set up the old chunks correctly
            // old is destroyed -> current becomes old -> next becomes current -> new becomes next
            if (m_previousChunk)
                Destroy(m_previousChunk.gameObject);
			if (m_currentChunk)
			{
				m_previousChunk = m_currentChunk;
				m_previousChunk.gameObject.name = "Previous Chunk";
			}
			if (m_nextChunk)
			{
				m_currentChunk = m_nextChunk;
				m_currentChunk.gameObject.name = "Current Chunk";
			}
			m_nextChunk = newChunk;;
			m_nextChunk.gameObject.name = "Next Chunk";
        }

		public void SetLevelSpeed(float speed)
		{
			// disallow speeds less than 0
			if (speed < 0)
			{
				Debug.LogError("Level Speed cannot be negative!");
				return;
			}

			// speed is set to 0 when the player loses
			if (speed == 0)
			{
				m_currentLevelSpeed = 0;
				return;
			}

			m_currentLevelSpeed -= 0.01f;

			StartCoroutine(approachLevelSpeed(speed));
		}

		private IEnumerator approachLevelSpeed(float target)
		{
			while (m_currentLevelSpeed != target)
			{
				m_currentLevelSpeed = Mathf.MoveTowards(m_currentLevelSpeed, target, LevelMetrics.Decceleration * Time.deltaTime);
				yield return null;
			}
			m_returningToNormalSpeed = true;
		}
    }
}
