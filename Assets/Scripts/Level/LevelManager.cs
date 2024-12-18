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
			}

        }
        #endregion

        [SerializeField] private LevelChunk[] m_possibleLevelChunks;

		[SerializeField] private GameObject[] m_possiblePickups;
		public GameObject[] Pickups => m_possiblePickups;

		private GameObject m_startingChunk;

		private LevelChunk m_previousChunk;
		private LevelChunk m_currentChunk;
		private LevelChunk m_nextChunk;

		private float m_currentLevelSpeed;
		public float CurrentLevelSpeed => m_currentLevelSpeed;

		bool m_approachingSetSpeed;

        private void Start()
        {
			Init();
        }

        public void Init()
        {
			m_startingChunk = GameObject.FindWithTag("StartChunk");
			m_currentLevelSpeed = 0;
			m_approachingSetSpeed = false;
			GenerateNewChunk();
			m_nextChunk.GetComponent<Collider>().enabled = false;
			GenerateNewChunk();
			GenerateNewChunk();
        }

        private void Update()
        {
            if (!m_approachingSetSpeed && Mathf.Abs(LevelMetrics.Speed - m_currentLevelSpeed) > 0.01f)
			{
				m_currentLevelSpeed = Mathf.MoveTowards(m_currentLevelSpeed, LevelMetrics.Speed, LevelMetrics.Acceleration * Time.deltaTime);
			}
        }

        public void GenerateNewChunk()
		{ 
			// if there is a current chunk, spawn at the end of that chunk
			Vector3 spawnPosition = Vector3.zero;
			if (m_nextChunk)
				spawnPosition = m_nextChunk.EndPoint;

			// create new chunk
			LevelChunk chunkChoice = m_possibleLevelChunks[Random.Range(0, m_possibleLevelChunks.Length)];
            LevelChunk newChunk = Instantiate(chunkChoice, spawnPosition, Quaternion.identity);
			newChunk.Init();

			// set up the old chunks correctly
			// old is destroyed -> current becomes old -> next becomes current -> new becomes next
			if (m_previousChunk)
			{
				// destroy the starting chunk if it exists
				if (m_startingChunk)
					Destroy(m_startingChunk);

				Destroy(m_previousChunk.gameObject);
			}
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
			m_nextChunk.gameObject.name = "Next Chunk (" + chunkChoice.name + ")";
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

            StopCoroutine(approachLevelSpeed(speed));
            m_approachingSetSpeed = true;
            StartCoroutine(approachLevelSpeed(speed));
		}

		private IEnumerator approachLevelSpeed(float target)
		{
			while (Mathf.Abs(target -m_currentLevelSpeed) > 0.01f)
			{
				m_currentLevelSpeed = Mathf.MoveTowards(m_currentLevelSpeed, target, LevelMetrics.Decceleration * Time.deltaTime);
				yield return null;
			}

			m_approachingSetSpeed = false;
			StopCoroutine(approachLevelSpeed(target));
		}
    }
}
