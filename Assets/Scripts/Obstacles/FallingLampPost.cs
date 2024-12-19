///<summary>
/// Author:
///
///
///
///</summary>

using UnityEngine;

namespace CadburyRunner.Obstacle
{
	public class FallingLampPost : MonoBehaviour
	{
		[SerializeField] private Animator m_lampPost;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
                m_lampPost.SetTrigger("Fall");
        }
    }
}
