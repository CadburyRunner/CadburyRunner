using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingController : MonoBehaviour
{
    private CharacterController m_characterController;

    [SerializeField] private float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Check if cjharacter is grounded
        if (!m_characterController.isGrounded)
        {
            //Add our gravity Vecotr
            move += Physics.gravity;
        }

        //Apply our move Vector , remeber to multiply by Time.delta
        m_characterController.Move(move * speed * Time.deltaTime);
    }
}
