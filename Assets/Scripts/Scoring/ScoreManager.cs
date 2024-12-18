///<summary>
/// Author: Aidan
///
/// Keeps track of score and can be accessed from anywhere
///
///</summary>

using UnityEngine;

namespace CadburyRunner.ScoreSystem
{
	public class ScoreManager : MonoBehaviour
	{
		private static ScoreManager Instance;

        private static float m_score;
        private static float m_scoreMulti = 1f;

        private static int m_collectableCount = 0;

        public static float Score => m_score;
        public static int CollectableCount => m_collectableCount;

        private void Awake()
        {
            //Assign singleton if one doesn't exist
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        private void Update()
        {
            //add score when enabled
            m_score += 10f * Time.deltaTime;
        }

        /// <summary>
        /// adds an amount of score to the points
        /// </summary>
        /// <param name="scoreToAdd"></param>
        public static void AddScore(float scoreToAdd)
        {
            if (Instance) m_score += scoreToAdd * m_scoreMulti;
        }

        /// <summary>
        /// adds an amount of score to the points, also adds one to the collectable tally
        /// </summary>
        /// <param name="scoreToAdd"></param>
        public static void AddScoreCollectable(float scoreToAdd)
        {
            if (Instance)
            {
                m_score += scoreToAdd * m_scoreMulti;
                m_collectableCount++;
            }
        }


        /// <summary>
        /// Sets all scoring values to zero
        /// </summary>
        public static void ResetScore()
        {
            m_score = 0f;
            m_scoreMulti = 1f;
            m_collectableCount = 0;
        }
        
        /// <summary>
        /// Removes instance and resets score
        /// </summary>
        public static void Kill()
        {
            if (Instance)
            {
                Destroy(Instance.gameObject);
                Instance = null;

                ResetScore();
            }
        }

        /// <summary>
        /// Sets whether the manager should constantly add score.
        /// </summary>
        /// <param name="AddScore">If true constantly add score</param>
        public static void SetEnabled(bool AddScore)
        {
            if (Instance) Instance.enabled = AddScore;
        }


        // Leave this out for now
        //public static void ChangeMulti()
        //{
        //
        //}
    }
}
