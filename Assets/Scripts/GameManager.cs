///<summary>
/// Author: Jaxon Haldane & Halen
///
/// Scene loading and menu methods
///
///</summary>

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CadburyRunner
{
	public class GameManager : MonoBehaviour
	{
        public static GameManager Instance;
        [SerializeField] private GameObject m_pauseCanvas;
        [SerializeField] private Canvas m_loseCanvas;
        [SerializeField] private GameObject m_pauseButton;

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

        public void PauseTime() { Time.timeScale = 0; m_pauseCanvas.SetActive(true); }
        public void UnPauseTime() { Time.timeScale = 1; m_pauseCanvas.SetActive(false);}

        public void Restart() { LoadScene(SceneManager.GetActiveScene().name); }
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        public void LoadScene(string name) 
        {
            StartCoroutine(AsyncLoad(name, name != "MainMenu"));
        }

        private IEnumerator AsyncLoad(string name, bool showPauseMenu = false)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            if (showPauseMenu)
                ShowPauseButton();
        }

        public void OnLose()
        {
            m_loseCanvas.gameObject.SetActive(true);
        }

        public void ShowPauseButton()
        {
            m_pauseButton.SetActive(true);
        }
    }
}
