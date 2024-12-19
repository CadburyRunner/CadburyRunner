///<summary>
/// Author: Aidan
///
/// Get object to look at camera
///
///</summary>

using UnityEngine;

namespace CadburyRunner
{
	public class Billboard : MonoBehaviour
	{
        private void LateUpdate()
        {
            transform.LookAt(Camera.main.transform.position); 
        }
    }
}
