///<summary>
/// Author: Aidan
///
///
///
///</summary>

using UnityEditor;
using UnityEngine;

namespace CadburyRunner.Obstacle
{
	public class Obstacle : MonoBehaviour
	{
		public enum ObstacleType
		{
			Trip,
			Slam,
			Fall,
		}

        [SerializeField] private ObstacleType m_type;

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //deal damage

                
            }
        }

        #region Scrapped

        //[System.Serializable]
        //public class HitBox
        //{
        //	[SerializeField] Vector3 m_position;
        //	[SerializeField] Vector3 m_size;
        //	[SerializeField] ObstacleType m_type;
        //
        //	public Vector3 Position => m_position;
        //	public Vector3 GetSize() { return m_size; }
        //	public	ObstacleType GetOType() { return m_type; }
        //}


        //[SerializeField] private HitBox[] m_hitboxes;

        //private void OnDrawGizmosSelected()
        //{
        //	Gizmos.color = Color.red;
        //	Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        //
        //    foreach (HitBox hit in m_hitboxes)
        //	{
        //		Gizmos.DrawWireCube(hit.GetPos(), hit.GetSize());
        //		Handles.Label(transform.position + hit.GetPos(), $"{hit.GetOType()}");
        //	}
        //}
        #endregion
    }
}
