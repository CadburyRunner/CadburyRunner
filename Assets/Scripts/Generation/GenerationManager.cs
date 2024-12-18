///<summary>
/// Author: Halen
///
/// Handles the generation of the level.
///
///</summary>

using UnityEngine;

namespace CadburyRunner.Generation
{
	public class GenerationManager : MonoBehaviour
	{
		#region Singleton
		public static GenerationManager Instance;
        private void Awake()
        {
			if (!Instance)
				Instance = this;
			else
				Destroy(gameObject);

			DontDestroyOnLoad(gameObject);
        }
        #endregion

        [SerializeField] private LevelChunk[] m_possibleLevelChunks;

		private LevelChunk m_previousChunk;
		private LevelChunk m_currentChunk;
		private LevelChunk m_nextChunk;

		int m_chunkCount;

        private void Start()
        {
			GenerateNewChunk();
			m_nextChunk.GetComponent<Collider>().enabled = false;
			GenerateNewChunk();
			GenerateNewChunk();
        }

        public void GenerateNewChunk()
		{ 
			// if there is a current chunk, spawn at the end of that chunk
			Vector3 spawnPosition = Vector3.zero;
			if (m_nextChunk)
				spawnPosition = m_nextChunk.EndPoint;

			// create new chunk
			LevelChunk newChunk = Instantiate(m_possibleLevelChunks[Random.Range(0, m_possibleLevelChunks.Length - 1)], spawnPosition, Quaternion.identity);
			newChunk.Init();
			newChunk.gameObject.name = "Chunk " + (m_chunkCount + 1).ToString();

            // set up the old chunks correctly
            // old is destroyed -> current becomes old -> next becomes current -> new becomes next
            if (m_previousChunk)
                Destroy(m_previousChunk.gameObject);
			if (m_currentChunk)
				m_previousChunk = m_currentChunk;
			if (m_nextChunk)
				m_currentChunk = m_nextChunk;
			m_nextChunk = newChunk;

			m_chunkCount++;
        }
    }
}
