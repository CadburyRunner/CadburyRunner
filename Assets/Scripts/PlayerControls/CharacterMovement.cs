///<summary>
/// Author: Jaxon Haldane
/// Handles what happens to the character when input is sent through.
/// does side to side movement,
/// jumping and sliding
///</summary>

using UnityEngine;
using CadburyRunner.Obstacle;
using System.Collections;
using CadburyRunner.Audio;
using UnityEditor;
using CadburyRunner.Level;

namespace CadburyRunner
{
	namespace Movement
	{
		public class CharacterMovement : MonoBehaviour
		{
            [Header("Physics Details")]
            [SerializeField] private float m_leftFull;
            [SerializeField] private float m_rightFull;
            [SerializeField, Range(0, 1)] private float m_input;
            [SerializeField] private float m_slidingTime;
            [SerializeField] private float m_jumpForce;
            [SerializeField] private float m_slideSpeedMultiplier;
            [SerializeField] LayerMask m_groundedMask;

            [Header("Collider Details")]
            [SerializeField] private Vector3 m_normalColliderCenter = Vector3.one;
            [SerializeField] private Vector3 m_normalColliderSize = Vector3.one;

            [SerializeField] private Vector3 m_slidingColliderCenter = Vector3.one;
            [SerializeField] private Vector3 m_slidingColliderSize = Vector3.one;

            private BoxCollider m_collider;
            private Rigidbody m_rb;
            private CharacterAnimationController m_anim;
            private bool m_isSliding;
            private bool m_isGrounded;

            public bool lose;

            private void Awake()
            {
                m_rb = GetComponent<Rigidbody>();
                m_collider = GetComponent<BoxCollider>();
                m_anim = GetComponentInChildren<CharacterAnimationController>();

                m_collider.center = m_normalColliderCenter;
                m_collider.size = m_normalColliderSize;
            }

#if UNITY_EDITOR
            void Update()
            {
                SideToSideMovement(m_input);
                if (Input.GetKeyDown(KeyCode.S))
                    Slide();

                if (Input.GetKeyDown(KeyCode.W))
                    Jump();

                if (lose)
                {
                    lose = false;
                    GameManager.Instance.OnLose();
                }
            }
#endif

            private void FixedUpdate()
            {
                transform.position = Vector3.Lerp(new Vector3(m_leftFull,transform.position.y,0), new Vector3(m_rightFull,transform.position.y,0), m_input);

                // check grounded
                Vector3 bottom = transform.position;
                bottom.y -= m_collider.bounds.extents.y;
                m_isGrounded = Physics.BoxCast(transform.position + m_collider.center, new Vector3(0.5f, 0.1f, 0.5f), 
                    Vector3.down, transform.rotation, m_collider.size.y / 2f, m_groundedMask, QueryTriggerInteraction.Ignore);
                m_anim.SetGrounded(m_isGrounded);
            }

            public void SideToSideMovement(float input)
            {
                m_input = input;
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
                    m_collider.center = m_normalColliderCenter;
                    m_collider.size = m_normalColliderSize;

                    // add force up
                    m_rb.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);

                    // reset grounded
                    m_isGrounded = false;

                    // play animation
                    m_anim.Jump();                    
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
                        m_rb.AddForce(Vector3.down * m_jumpForce, ForceMode.Impulse);
                    }
                    else
                    {
                        // if not in the air then switch collider to sliding collider
                        m_isSliding = true;
                        m_collider.center = m_slidingColliderCenter;
                        m_collider.size = m_slidingColliderSize;

                        // start the IsSliding coroutine
                        StartCoroutine(IsSliding());

                        // give extra speed
                        LevelManager.Instance.SetLevelSpeed(LevelMetrics.Speed * m_slideSpeedMultiplier);

                        m_anim.SetSliding(true);
                    }
                }
            }

            private IEnumerator IsSliding()
            {
                // wait till the slide is done
                yield return new WaitForSeconds(m_slidingTime);

                // reset the colliders and bool
                m_isSliding = false;
                m_collider.center = m_normalColliderCenter;
                m_collider.size = m_normalColliderSize;

                // stop the animation
                m_anim.SetSliding(false);

                // stop the coroutine
                StopCoroutine(IsSliding());
            }
        }
	}
}
