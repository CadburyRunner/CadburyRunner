///<summary>
/// Author: Jaxon Haldane
/// Handles what happens to the character when input is sent through.
/// does side to side movement,
/// jumping and sliding
///</summary>

using UnityEngine;
using CadburyRunner.Obstacle;

namespace CadburyRunner
{
	namespace Movement
	{
		public class CharacterMovement : MonoBehaviour
		{
            [SerializeField] private Vector3 m_leftFull;
            [SerializeField] private Vector3 m_rightFull;
            [SerializeField, Range(0, 1)] private float m_Input;

            void Update()
            {
                SideToSideMovement(m_Input);
            }

            public void SideToSideMovement(float input)
            {
                transform.position = Vector3.Lerp(m_leftFull, m_rightFull, input);
            }

            public void GetHit(ObstacleType type)
            {

            }

        }
	}
}
