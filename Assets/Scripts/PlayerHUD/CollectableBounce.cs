///<summary>
/// Author: Aidan
///
/// Fancy animated effect
///
///</summary>

using UnityEngine;

namespace CadburyRunner
{
	public class CollectableBounce : MonoBehaviour
	{
        [SerializeField] private GameObject m_collectableObject;


        private float m_waveTime = 5f;

        private void Update()
        {
            //pretty much all of this is this equation:
            //https://www.desmos.com/calculator/irqlnelb1k
            float scale = Mathf.Pow((float)System.Math.E, -m_waveTime) * Mathf.Cos(2 * Mathf.PI * m_waveTime) + 1;

            m_collectableObject.transform.rotation = Quaternion.Euler(0, 0, 10f * Mathf.Sin(Time.time) + 23 - ((scale - 1) * 20f));

            m_collectableObject.transform.localScale = new Vector3 (scale, scale, scale);

            m_waveTime += Time.deltaTime * 2f;

        }


        public void Collected()
        {
            m_waveTime = 0f;
        }
    }
}
