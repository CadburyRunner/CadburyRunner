///<summary>
/// Author:
///
///
///
///</summary>

using UnityEngine;

namespace CadburyRunner
{
	public class MainMenu : MonoBehaviour
	{
		public void StartGame()
		{
			GameManager.Instance.LoadScene("PlayerControllerTesting");
		}

		public void Quit()
		{
			GameManager.Instance.Quit();
		}
	}
}
