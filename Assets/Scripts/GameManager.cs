///<summary>
/// Author: Jaxon Haldane
///
/// Scene loading and menu methods
///
///</summary>

using UnityEngine;
using UnityEngine.SceneManagement;

namespace CadburyRunner
{
	public class GameManager : MonoBehaviour
	{
        public static GameManager Instance;
        [SerializeField] private Canvas pauseCanvas;
        [SerializeField] private Canvas loseCanvas;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void PauseTime() { Time.timeScale = 0; pauseCanvas.gameObject.SetActive(true); }
        public void UnPauseTime() { Time.timeScale = 1; pauseCanvas.gameObject.SetActive(false);}

        public void Quit() { Application.Quit(); }

        public void LoadScene(string name) { SceneManager.LoadSceneAsync(name); }

        public void OnLose()
        {
            loseCanvas.gameObject.SetActive(true);
        }
    }
}
