///<summary>
/// Author: Emmanuel
///
/// Handles picking up items.
///
///</summary>

using UnityEngine;
using CadburyRunner.ScoreSystem;

namespace CadburyRunner.Pickup
{
	public class Pickup : MonoBehaviour
	{
        [SerializeField] private GameObject m_model;
        [SerializeField] private int m_pointValue;
        private AudioSystem.AudioSystem m_aSystem = null;

        private void Awake() { InitializePickup(); }
  
        // Initializes all necessary variables and references for the pickup.
        public void InitializePickup()
        {
            m_aSystem = FindObjectOfType<CadburyRunner.AudioSystem.AudioSystem>();
        }

        public void CollectPickup()
        {
            ScoreManager.AddScoreCollectable(m_pointValue);   // Add the "m_pointValue" int from the chocolate bar ScriptableObject to the players score.

            if (m_aSystem != null) { m_aSystem.PlaySound(2, 2); } // Play the Pickup sound.
            m_aSystem = null;

            Destroy(m_model); // Destroy the pickup.
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CollectPickup();
            }
        }

        private void Update()
        {
            // TESTING & DEBUGGING
            // -----------------------------------
            //if (Input.GetKeyUp(KeyCode.Space))
            //{
            //    CollectPickup();
            //}
            // -----------------------------------
        }

    }
}
