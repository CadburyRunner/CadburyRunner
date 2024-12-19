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
        private float[] m_powerupMaxTimes;

        [SerializeField] private Color m_startColour;
        [SerializeField] private Color m_endColour;

        

        private int m_prevCollectables = 0;

        private void Awake()
        {
            m_scoreText.text = "00000";
            m_collectableText.text = "00 X";
            m_powerupTimes = new float[m_powerupUI.Length];
            m_powerupMaxTimes = new float[m_powerupUI.Length];
        }

        private void Update()
        {
            m_scoreText.text = ((int)ScoreManager.Instance.Score).ToString();
            m_collectableText.text = ScoreManager.Instance.CollectableCount + " X";

            if (m_prevCollectables != ScoreManager.Instance.CollectableCount) m_bounceScript.Collected();

            m_prevCollectables = ScoreManager.Instance.CollectableCount;

            //go through all active powerups
            for(int i = 0; i < m_powerupUI.Length; i++)
            {
                if (!m_powerupUI[i].activeSelf) continue;

                m_powerupTimes[i] -= Time.deltaTime;

                m_progressCircle[i].fillAmount = m_powerupTimes[i] / m_powerupMaxTimes[i];

                m_progressCircle[i].color = Color.Lerp(m_endColour, m_startColour, m_powerupTimes[i] / m_powerupMaxTimes[i]);
            }

        }
        /// <summary>
        /// shows specified int powerup
        /// </summary>
        public void ShowPowerup(int powerup, float time)
        {
            m_powerupUI[powerup].SetActive(true);
            m_powerupTimes[powerup] = time;
            m_powerupMaxTimes[powerup] = time;
        }

        /// <summary>
        /// shows specified int powerup
        /// </summary>
        public void HidePowerup(int powerup)
        {
            m_powerupUI[powerup].SetActive(false);
        }
    }
}
