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
        public static MobileController Instance { get; private set; }
        [SerializeField] private bool m_debugging = false;

        [Min(0)]
        [Tooltip("The angle in degrees for a tilt input to be registered.")]
		[SerializeField] private Vector2 m_tiltThreashold = new(0.5f, 0.5f);
        private Vector3 m_inputedRotation;
        [Min(0)]
        [SerializeField] private float m_tiltSensitivity = 1;
        [Tooltip("The distance in pixels that a swipe has to be to count as a swipe input.")]
		[SerializeField] private float m_swipeThreashold = 15f;
        [Tooltip("The time to wait before reading a tap input as a held input.")]
        [SerializeField] private float m_holdDelay = 0.3f;
        private float m_timeHeld = 0f;

        [SerializeField] private UnityEvent<float> m_swipeUpEvent;
        [SerializeField] private UnityEvent<float> m_swipeDownEvent;
        [SerializeField] private UnityEvent<float> m_swipeLeftEvent;
        [SerializeField] private UnityEvent<float> m_swipeRightEvent;
        [SerializeField] private UnityEvent<float> m_tapLeftEvent;
        [SerializeField] private UnityEvent<float> m_tapRightEvent;
        [SerializeField] private UnityEvent m_tapEvent;
        [SerializeField] private UnityEvent<float> m_tiltLeftEvent;
        [SerializeField] private UnityEvent<float> m_tiltRightEvent;
        [SerializeField] private UnityEvent<float> m_tiltForwardEvent;
        [SerializeField] private UnityEvent<float> m_tiltBackwardEvent;
        [SerializeField] private UnityEvent<float> m_tiltRecalibrationEvent;

        private Vector3 fp;   //First touch position
        private Vector3 lp;   //Last touch position
        private float dragDistance;  //minimum distance for a swipe to be registered

        protected MobileController SetInstance()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning($"Theres 2 of me (Mobile Controller)! Deleting self");
                Destroy(this);
            }
            return Instance;
        }
        void Awake()
        {
            dragDistance = Screen.height * m_swipeThreashold / 100; //dragDistance is 15% height of the screen
            Input.gyro.enabled = true;
            SetInstance();
            Recalibrate();
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
                    m_timeHeld = 0;
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
                                if(m_debugging) Debug.Log("Right Swipe");
                                m_swipeRightEvent.Invoke(1);
                            }
                            else
                            {   //Left swipe
                                if(m_debugging) Debug.Log("Left Swipe");
                                m_swipeLeftEvent.Invoke(-1);
                            }
                        }
                        else
                        {   //the vertical movement is greater than the horizontal movement
                            if (lp.y > fp.y)  //If the movement was up
                            {   //Up swipe
                                if(m_debugging) Debug.Log("Up Swipe");
                                m_swipeUpEvent.Invoke(1);
                            }
                            else
                            {   //Down swipe
                                if(m_debugging) Debug.Log("Down Swipe");
                                m_swipeDownEvent.Invoke(-1);
                            }
                        }
                    }
                    else
                    {   //It's a tap as the drag distance is less than 20% of the screen height
                        if(m_debugging) Debug.Log("Tap");
                        m_tapEvent.Invoke();
                    }
                }
                m_timeHeld += Time.deltaTime;
                if (m_timeHeld >= m_holdDelay)
                {
                    //Tap inputs - run constantly
                    if (touch.position.x < Screen.width / 2)
                    {
                        if(m_debugging) Debug.Log("Left tap");
                        m_tapLeftEvent.Invoke(-1);
                    }
                    else
                    {
                        if(m_debugging) Debug.Log("Right tap");
                        m_tapRightEvent.Invoke(-1);
                    }
                }
            }

            //Get the gyro angles in deg
            Vector3 newRate = Input.gyro.rotationRateUnbiased * Time.deltaTime;
            m_inputedRotation += newRate;
            //Check if the axis is greater then the threashold
            if (Mathf.Abs(newRate.x) > m_tiltThreashold.x)
            {
                if (m_inputedRotation.x < 0)
                {
                    if(m_debugging) Debug.Log("Tilt Forward");
                    m_tiltForwardEvent.Invoke(m_inputedRotation.x * m_tiltSensitivity);
                }
                else if(m_inputedRotation.x > 0)
                {
                    if(m_debugging) Debug.Log("Tilt Back");
                    m_tiltBackwardEvent.Invoke(m_inputedRotation.x * m_tiltSensitivity);
                }
            }
            //repeat
            if (Mathf.Abs(newRate.y) > m_tiltThreashold.y)
            {
                //Check which side we're on
                if (m_inputedRotation.y > 0)
                {
                    if(m_debugging) Debug.Log("Tilt Right");
                    m_tiltRightEvent.Invoke(m_inputedRotation.y * m_tiltSensitivity);
                }
                else if (m_inputedRotation.y < 0)
                {
                    if(m_debugging) Debug.Log("Tilt Left");
                    m_tiltLeftEvent.Invoke(m_inputedRotation.y * m_tiltSensitivity);
                }
            }
        }
        static public void Recalibrate()
        {
            Instance.m_inputedRotation.y = 0.5f / Instance.m_tiltSensitivity;
            Instance.m_tiltRecalibrationEvent.Invoke(Instance.m_inputedRotation.y);
        }
    }
}
