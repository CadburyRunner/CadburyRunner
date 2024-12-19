///<summary>
/// Author: Aidan
///
/// Logic for displaying player info on the hud
///
///</summary>

using UnityEngine;
using CadburyRunner.ScoreSystem;
using TMPro;


namespace CadburyRunner.PlayerHUD
{


	public class PlayerHUD : MonoBehaviour
	{

        [SerializeField] private TextMeshProUGUI m_scoreText;
        [SerializeField] private TextMeshProUGUI m_collectableText;

        [SerializeField] private CollectableBounce m_bounceScript;

        private int m_prevCollectables = 0;

        private void Awake()
        {
            m_scoreText.text = "00000";
            m_collectableText.text = "00 X";
        }

        private void Update()
        {
            m_scoreText.text = ((int)ScoreManager.Instance.Score).ToString();
            m_collectableText.text = ScoreManager.Instance.CollectableCount + " X";

            if (m_prevCollectables != ScoreManager.Instance.CollectableCount) m_bounceScript.Collected();

            m_prevCollectables = ScoreManager.Instance.CollectableCount;
        }
    }
}
