///<summary>
/// Author: Emmanuel
///
/// Handles picking up items.
///
///</summary>

using UnityEngine;
using CadburyRunner.ScoreSystem;
using CadburyRunner.Audio;
using CadburyRunner.Player;

namespace CadburyRunner.Pickup
{
	public class Pickup : MonoBehaviour
	{
        [SerializeField] private GameObject m_model;
        [SerializeField] private int m_pointValue;

        private void Start()
        {
            Instantiate(m_model, transform);
        }

        public void CollectPickup()
        {
            ScoreManager.Instance.AddScoreCollectable(m_pointValue);   // Add the "m_pointValue" int from the chocolate bar ScriptableObject to the players score.
            
            AudioSystem.Instance.PlaySound(2, 2); // Play the Pickup sound.

            Destroy(gameObject); // Destroy the pickup.
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //other.GetComponentInParent<PlayerStatus>();
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
