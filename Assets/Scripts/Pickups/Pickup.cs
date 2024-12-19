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
        Magnet,
        Multiplier
    }

	public class Pickup : MonoBehaviour
	{
        [SerializeField] private GameObject m_model;
        [SerializeField] private int m_pointValue;
        [SerializeField] private PickupType m_type = 0;
        [SerializeField] private ParticleSystem m_pickupParticles;

        private void Start()
        {
            Instantiate(m_model, transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerStatus player = other.GetComponentInParent<PlayerStatus>();
                int pickupType = 0;

                switch (m_type)
                {
                    case PickupType.Shield:
                        player.GiveShieldPickup();
                        pickupType = 1;
                        break;
                    case PickupType.Magnet:
                        player.GiveMagnetPickup();
                        pickupType = 1;
                        break;
                    case PickupType.Multiplier:
                        pickupType = 1;
                        player.GiveMultiplierPickup();
                        break;

                }

                if (pickupType == 0)
                {
                    SFXController.Instance.PlaySoundClip("Pickup", "Chocolate", AudioTrack.Pickup);
                    ScoreManager.Instance.AddScoreCollectable(m_pointValue);   // Add the "m_pointValue" int from the chocolate bar ScriptableObject to the players score.
                                                                               // Also adds 1 point to the collectables list.
                }
                else
                {
                    SFXController.Instance.PlaySoundClip("Pickup", "Powerup", AudioTrack.Pickup);
                    ScoreManager.Instance.AddScore(m_pointValue); // Add the "m_pointValue" int from the chocolate bar ScriptableObject to the players score.
                }

                if(m_pickupParticles != null)
                {
                    Instantiate(m_pickupParticles, transform.position, Quaternion.identity);
                }
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
