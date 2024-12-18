///<summary>
/// Author:
///
///
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

        private void Awake()
        {
            m_scoreText.text = "00000";
            m_collectableText.text = "00 X";
        }

        private void Update()
        {
            m_scoreText.text = ((int)ScoreManager.Score).ToString();
            m_collectableText.text = ScoreManager.CollectableCount + " X";
        }
    }
}
