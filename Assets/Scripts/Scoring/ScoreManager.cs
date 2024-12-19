///<summary>
/// Author: Aidan
///
/// Keeps track of score and can be accessed from anywhere
///
///</summary>

using CadburyRunner.Level;
using CadburyRunner.PlayerUI;
using UnityEngine;

namespace CadburyRunner.ScoreSystem
{
	public class ScoreManager : MonoBehaviour
	{
        #region Singleton
        public static ScoreManager Instance;

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
            }
        }
        #endregion

        [SerializeField] private PlayerHUD m_playerHUD;

        private float m_score;
        private float m_scoreMulti = 1f;

        private int m_collectableCount = 0;

        public float Score => m_score;
        public int CollectableCount => m_collectableCount;

        private void Update()
        {
            //add score when enabled
            m_score += LevelManager.Instance.CurrentLevelSpeed * Time.deltaTime * m_scoreMulti;
            m_playerHUD.UpdateScore(m_score);
        }

        /// <summary>
        /// adds an amount of score to the points
        /// </summary>
        /// <param name="scoreToAdd"></param>
        public void AddScore(float scoreToAdd)
        {
            m_score += scoreToAdd * m_scoreMulti;
            m_playerHUD.UpdateScore(m_score);
        }

        /// <summary>
        /// adds an amount of score to the points, also adds one to the collectable tally
        /// </summary>
        /// <param name="scoreToAdd"></param>
        public void AddScoreCollectable(float scoreToAdd)
        {
            m_score += scoreToAdd * m_scoreMulti;
            m_collectableCount++;
            m_playerHUD.UpdateScore(m_score);
            m_playerHUD.UpdateCollectibleCount(m_collectableCount);
        }

        /// <summary>
        /// Sets all scoring values to zero
        /// </summary>
        public void ResetScore()
        {
            m_score = 0f;
            m_scoreMulti = 1f;
            m_collectableCount = 0;
        }
        
        /// <summary>
        /// Removes instance and resets score
        /// </summary>
        public void Kill()
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
        public void SetEnabled(bool AddScore)
        {
            Instance.enabled = AddScore;
        }


        /// <summary>
        /// change multiplier to set value
        /// </summary>
        public void ChangeMulti(float value)
        {
            Instance.m_scoreMulti = value;
        }
    }
}
