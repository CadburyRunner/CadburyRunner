///<summary>
/// Author: Aidan
///
/// Basic obstacle that damages player when collider is triggered and conditions are met
///
///</summary>

using UnityEngine;
using CadburyRunner.Player;

namespace CadburyRunner.Obstacle
{
    public enum ObstacleType
    {
        Trip,
        Slam,
        Fall,
    }

    public class Obstacle : MonoBehaviour
	{
        [SerializeField] private ObstacleType m_type;

        public void OnTriggerEnter(Collider other)
        {
            //if player colliding
            if (other.CompareTag("Player"))
            {
                //gets player status component in parent
                PlayerStatus status = other.GetComponentInParent<PlayerStatus>();

                switch (m_type)
                {
                    case ObstacleType.Trip:

                        status.Trip();

                        break;
                    case ObstacleType.Slam:

                        status.Die(ObstacleType.Slam);

                        break;
                    case ObstacleType.Fall:

                        status.Die(ObstacleType.Fall);

                        break;
                }
            }
        }
#if UNITY_EDITOR
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
#endif
    }
}
