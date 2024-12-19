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
            [Header("Float variables")]
            [SerializeField] private float m_leftFull;
            [SerializeField] private float m_rightFull;
            [SerializeField, Range(0, 1)] private float m_input;
            [SerializeField] private float m_slidingTime;
            [SerializeField] private float forcePower;
            [SerializeField] private float m_slideSpeedMultiplier;

            [SerializeField] LayerMask m_groundedMask;

            [Header("References")]
            [SerializeField] private BoxCollider m_normalCollider;
            [SerializeField] private BoxCollider m_sliderCollider;

            private Rigidbody m_rb;
            private bool m_isSliding;
            private bool m_isGrounded;

            public bool lose;

            private void Start()
            {
                m_rb = GetComponent<Rigidbody>();
            }

            void Update()
            {
                

#if UNITY_EDITOR
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
#endif
            }

            private void FixedUpdate()
            {
                transform.position = Vector3.Lerp(new Vector3(m_leftFull,transform.position.y,0), new Vector3(m_rightFull,transform.position.y,0), m_input);

                // check grounded
                Vector3 bottom = transform.position;
                bottom.y -= m_normalCollider.bounds.extents.y;
                m_isGrounded = Physics.BoxCast(transform.position + m_normalCollider.center, new Vector3(0.5f, 0.1f, 0.5f), 
                    Vector3.down, transform.rotation, m_normalCollider.size.y, m_groundedMask, QueryTriggerInteraction.Ignore);
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
                    m_normalCollider.gameObject.SetActive(true);
                    m_sliderCollider.gameObject.SetActive(false);

                    // add force up
                    m_rb.AddForce(Vector3.up * forcePower, ForceMode.Impulse);

                    // reset grounded
                    m_isGrounded = false;

                    // play sound effect
                    SFXController.Instance.PlaySoundClip("PlayerMove", "Jump", AudioTrack.PlayerMove);
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
                        m_normalCollider.gameObject.SetActive(false);
                        m_sliderCollider.gameObject.SetActive(true);

                        // start the IsSliding coroutine
                        StartCoroutine(IsSliding());

                        // give extra speed
                        LevelManager.Instance.SetLevelSpeed(LevelMetrics.Speed * m_slideSpeedMultiplier);

                        // play sound effect
                        SFXController.Instance.PlaySoundClip("PlayerMove", "Slide", AudioTrack.PlayerMove);
                    }
                }
            }

            private IEnumerator IsSliding()
            {
                // wait till the slide is done
                yield return new WaitForSeconds(m_slidingTime);

                // reset the colliders and bool
                m_isSliding = false;
                m_normalCollider.gameObject.SetActive(true);
                m_sliderCollider.gameObject.SetActive(false);

                // stop the coroutine
                StopCoroutine(IsSliding());
            }

            public void GetHit(ObstacleType type)
            {

            }

#if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                Handles.color = Color.green;
                Handles.DrawWireCube(transform.position + Vector3.down * 0.05f, Vector3.one);
            }
#endif

        }
	}
}
