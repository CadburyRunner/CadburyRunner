using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Charactermovement : MonoBehaviour
{
    [SerializeField] private Vector3 m_leftFull;
    [SerializeField] private Vector3 m_rightFull;
    [SerializeField, Range(0,1)] private float m_Input;
    private bool m_isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SideToSideMovement(m_Input);
    }

    public void SideToSideMovement(float input)
    {
        transform.position = Vector3.Lerp(m_leftFull, m_rightFull, input);
    }
}
