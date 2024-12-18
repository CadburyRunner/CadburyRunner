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
			GameManager.Instance.LoadScene("RunningLevel");
		}

		public void Quit()
		{
			GameManager.Instance.Quit();
		}
	}
}
