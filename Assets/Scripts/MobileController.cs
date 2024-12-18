///<summary>
/// Author: Jackson
///
///	Handles the mobile inputs and gives an output
///
///</summary>

using UnityEngine;
using UnityEngine.Events;

namespace CadburyRunner.Mobile
{
	public class MobileController : MonoBehaviour
	{
        [Min(0)]
		[SerializeField] private Vector2 m_tiltThreashold = new(10f, 10f);
        [Min(0)]
        [SerializeField] private float m_tiltSensitivity = 1;
        private Vector3 m_gyroEuler;
		[SerializeField] private float m_swipeThreashold = 15f;

        [SerializeField] private UnityEvent<float> m_tiltLeftEvent;
        [SerializeField] private UnityEvent<float> m_tiltRightEvent;
        [SerializeField] private UnityEvent<float> m_tiltForwardEvent;
        [SerializeField] private UnityEvent<float> m_tiltBackwardEvent;
        [SerializeField] private UnityEvent<float> m_swipeLeftEvent;
        [SerializeField] private UnityEvent<float> m_swipeRightEvent;
        [SerializeField] private UnityEvent<float> m_swipeUpEvent;
        [SerializeField] private UnityEvent<float> m_swipeDownEvent;

        private Vector3 fp;   //First touch position
        private Vector3 lp;   //Last touch position
        private float dragDistance;  //minimum distance for a swipe to be registered

        void Awake()
        {
            dragDistance = Screen.height * m_swipeThreashold / 100; //dragDistance is 15% height of the screen
            Input.gyro.enabled = true;
        }

        void Update()
        {
            if (Input.touchCount == 1) // user is touching the screen with a single touch
            {
                Touch touch = Input.GetTouch(0); // get the touch
                if (touch.phase == TouchPhase.Began) //check for the first touch
                {
                    fp = touch.position;
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
                {
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    lp = touch.position;  //last touch position. Ommitted if you use list

                    //Check if drag distance is greater than 20% of the screen height
                    if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                    {//It's a drag
                     //check if the drag is vertical or horizontal
                        if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                        {   //If the horizontal movement is greater than the vertical movement...
                            if ((lp.x > fp.x))  //If the movement was to the right)
                            {   //Right swipe
                                Debug.Log("Right Swipe");
                                m_swipeRightEvent.Invoke(1);
                            }
                            else
                            {   //Left swipe
                                Debug.Log("Left Swipe");
                                m_swipeLeftEvent.Invoke(-1);
                            }
                        }
                        else
                        {   //the vertical movement is greater than the horizontal movement
                            if (lp.y > fp.y)  //If the movement was up
                            {   //Up swipe
                                Debug.Log("Up Swipe");
                                m_swipeUpEvent.Invoke(1);
                            }
                            else
                            {   //Down swipe
                                Debug.Log("Down Swipe");
                                m_swipeDownEvent.Invoke(-1);
                            }
                        }
                    }
                    else
                    {   //It's a tap as the drag distance is less than 20% of the screen height
                        Debug.Log("Tap");
                    }
                }
            }
            //Get the gyro angles in deg
            m_gyroEuler = Input.gyro.attitude.eulerAngles;
            //Check if the axis is greater then the threashold
            if (m_gyroEuler.y > m_tiltThreashold.y && m_gyroEuler.y < 360.0f - m_tiltThreashold.y)
            {
                //Check which side we're on
                if (m_gyroEuler.y <= 180)
                {
                    Debug.Log("Tilt Forward");
                    m_tiltForwardEvent.Invoke(m_gyroEuler.y - m_tiltThreashold.y * m_tiltSensitivity);
                }
                else
                {
                    Debug.Log("Tilt Back");
                    m_tiltBackwardEvent.Invoke(-(360 - m_gyroEuler.y - m_tiltThreashold.y) * m_tiltSensitivity);
                }
            }
            //repeat
            if (m_gyroEuler.x > m_tiltThreashold.x && m_gyroEuler.x < 360.0f - m_tiltThreashold.x)
            {
                if(m_gyroEuler.x <= 180)
                {
                    Debug.Log("Tilt Right");
                    m_tiltRightEvent.Invoke(m_gyroEuler.x - m_tiltThreashold.x * m_tiltSensitivity);
                }
                else
                {
                    Debug.Log("Tilt Left");
                    m_tiltLeftEvent.Invoke(-(360 - m_gyroEuler.x - m_tiltThreashold.x) * m_tiltSensitivity);
                }
            }
        }
        protected void OnGUI()
        {
            GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude.eulerAngles);
        }
    }
}
