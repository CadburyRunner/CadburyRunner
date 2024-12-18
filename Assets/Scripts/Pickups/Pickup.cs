///<summary>
/// Author: Emmanuel
///
/// Handles picking up items.
///
///</summary>

using UnityEngine;

namespace CadburyRunner.Pickup
{
	public class Pickup : MonoBehaviour
	{
        [SerializeField] private ChocolateBar m_chocolatePickup;
        private AudioSystem.AudioSystem m_aSystem = null;

        private int pickupType = 0;

        private void Awake()
        {
            InitializePickup();
        }


        // Initializes all necessary variables and references for the pickup.
        public void InitializePickup()
        {
            m_aSystem = FindObjectOfType<CadburyRunner.AudioSystem.AudioSystem>();
            if (m_chocolatePickup != null)
            {
                pickupType = 0;
            }

        }

        public void CollectPickup()
        {
            // TODO: Add point value to score.
            // -----------------------------------

            Debug.Log("Add " + m_chocolatePickup.GetPointValue() + " points to the players score.");

            if (m_aSystem != null) { m_aSystem.PlaySound(2, 2); }
            m_aSystem = null;
            m_chocolatePickup = null;
            Destroy(this.gameObject);
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
            if (Input.GetKeyUp(KeyCode.Space))
            {
                CollectPickup();
            }
            // -----------------------------------
        }

    }
}
