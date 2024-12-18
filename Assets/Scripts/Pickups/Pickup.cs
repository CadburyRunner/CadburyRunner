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
    enum PickupType
    {
        Points,
        Shield,
        Magnet
    }

	public class Pickup : MonoBehaviour
	{
        [SerializeField] private GameObject m_model;
        [SerializeField] private int m_pointValue;
        [SerializeField] private PickupType m_type = 0;
        [SerializeField, Min(0)] private float m_powerupTime = 10;

        private void Start()
        {
            Instantiate(m_model, transform);
        }

        public void CollectPickup()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerStatus player = other.GetComponentInParent<PlayerStatus>();

                switch (m_type)
                {
                    case PickupType.Shield:
                        player.ShieldPickup(m_powerupTime);
                        break;
                    case PickupType.Magnet:
                        player.MagnetPickup(m_powerupTime);
                        break;
                }
                
                ScoreManager.Instance.AddScoreCollectable(m_pointValue);   // Add the "m_pointValue" int from the chocolate bar ScriptableObject to the players score.

                AudioSystem.Instance.PlaySound(2, 2); // Play the Pickup sound.

                Destroy(gameObject); // Destroy the pickup.
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
