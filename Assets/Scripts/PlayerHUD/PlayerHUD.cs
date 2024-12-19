///<summary>
/// Author: Aidan
///
/// Logic for displaying player info on the hud
///
///</summary>

using UnityEngine;
using CadburyRunner.ScoreSystem;
using TMPro;
using UnityEngine.UI;
using CadburyRunner.Level;
using UnityEngine.PlayerLoop;


namespace CadburyRunner.PlayerUI
{
	public class PlayerHUD : MonoBehaviour
	{
        [SerializeField] private TextMeshProUGUI m_scoreText;
        [SerializeField] private TextMeshProUGUI m_collectableText;

        [SerializeField] private CollectableBounce m_bounceScript;

        [SerializeField] private GameObject[] m_powerupUI;
        [SerializeField] private Image[] m_progressCircle;
        private float[] m_powerupTimes;

        [SerializeField] private Color m_startColour;
        [SerializeField] private Color m_endColour;

        private void Awake()
        {
            m_scoreText.text = "00000";
            m_collectableText.text = "00 X";
            m_powerupTimes = new float[m_powerupUI.Length];
        }

        private void Update()
        {
            //go through all active powerups
            for (int i = 0; i < m_powerupUI.Length; i++)
            {
                if (!m_powerupUI[i].activeSelf) continue;

                m_powerupTimes[i] -= Time.deltaTime;

                m_progressCircle[i].fillAmount = m_powerupTimes[i] / LevelMetrics.PowerupTime;

                m_progressCircle[i].color = Color.Lerp(m_endColour, m_startColour, m_powerupTimes[i] / LevelMetrics.PowerupTime);
            }

        }
        /// <summary>
        /// shows specified int powerup
        /// </summary>
        public void ShowPowerup(int powerup)
        {
            m_powerupUI[powerup].SetActive(true);
            m_powerupTimes[powerup] = LevelMetrics.PowerupTime;
        }

        /// <summary>
        /// shows specified int powerup
        /// </summary>
        public void HidePowerup(int powerup)
        {
            m_powerupUI[powerup].SetActive(false);
        }

        public void UpdateScore(float score)
        {
            m_scoreText.text = ((int)score).ToString();
        }

        public void UpdateCollectibleCount(int count)
        {
            m_collectableText.text = count + " X";
            m_bounceScript.Collected();
        }
    }
}
