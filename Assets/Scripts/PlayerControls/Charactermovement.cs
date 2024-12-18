///<summary>
/// Author: Jaxon Haldane
/// Handles what happens to the character when input is sent through.
/// does side to side movement,
/// jumping and sliding
///</summary>

using UnityEngine;
using CadburyRunner.Obstacle;
using System.Collections;

namespace CadburyRunner
{
	namespace Movement
	{
		public class CharacterMovement : MonoBehaviour
		{
            [Header("Float variables")]
            [SerializeField] private float m_leftFull;
            [SerializeField] private float m_rightFull;
            [SerializeField, Range(0, 1)] private float m_Input;
            [SerializeField] private float m_slidingTime;
            [SerializeField] private float forcePower;

            [Header("References")]
            [SerializeField] private GameObject m_normalCollider;
            [SerializeField] private GameObject m_sliderCollider;

            private Rigidbody m_rb;
            private bool m_isSliding;
            private bool m_isGrounded;

            private void Start()
            {
                m_rb = GetComponent<Rigidbody>();
            }

            void Update()
            {
                m_isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f, LayerMask.NameToLayer("Ground"));
#if UNITY_EDITOR
                if (Input.GetKey(KeyCode.D))
                    m_Input += 1f * Time.deltaTime;
                if (Input.GetKey(KeyCode.A))
                    m_Input -= 1f * Time.deltaTime;
                SideToSideMovement(m_Input);
                if (Input.GetKeyDown(KeyCode.S))
                    Slide();

                if (Input.GetKeyDown(KeyCode.W))
                    Jump();
#endif
            }

            public void SideToSideMovement(float input)
            {
                transform.position = Vector3.Lerp(new Vector3(m_leftFull,transform.position.y,0), new Vector3(m_rightFull,transform.position.y,0), input);
            }

            public void Jump()
            {
                // if the player is grounded
                if (m_isGrounded)
                {
                    // stop all coroutines which is only the sliding one
                    StopAllCoroutines();

                    // reset to normal
                    m_isSliding = false;
                    m_normalCollider.SetActive(true);
                    m_sliderCollider.SetActive(false);

                    // add force up
                    m_rb.AddForce(Vector3.up * forcePower, ForceMode.Force);
                }
            }

            public void Slide()
            {
                // if character is not already sliding
                if (!m_isSliding)
                {
                    // if the character is in the air
                    if (!m_isGrounded)
                    {
                        // just pull the character back down fast
                        m_rb.AddForce(Vector3.down * forcePower, ForceMode.Force);
                    }
                    else
                    {
                        // if not in the air then switch collider to sliding collider
                        m_isSliding = true;
                        m_normalCollider.SetActive(false);
                        m_sliderCollider.SetActive(true);

                        // start the IsSliding coroutine
                        StartCoroutine(IsSliding());
                    }
                }
            }

            private IEnumerator IsSliding()
            {
                // wait till the slide is done
                yield return new WaitForSeconds(m_slidingTime);

                // reset the colliders and bool
                m_isSliding = false;
                m_normalCollider.SetActive(true);
                m_sliderCollider.SetActive(false);

                // stop the coroutine
                StopCoroutine(IsSliding());
            }

            public void GetHit(ObstacleType type)
            {

            }

        }
	}
}
