///<summary>
/// Author: Jaxon Haldane & Halen
///
/// Scene loading and menu methods
///
///</summary>

using CadburyRunner.Audio;
using CadburyRunner.Level;
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

        public void PauseTime() { Time.timeScale = 0; m_pauseCanvas.SetActive(true); SetPauseButton(false); }
        public void UnPauseTime() { Time.timeScale = 1; m_pauseCanvas.SetActive(false); SetPauseButton(true); }

        public void Restart() { LoadScene(SceneManager.GetActiveScene().name); Time.timeScale = 1; }
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        public void LoadScene(string name) 
        {
            StartCoroutine(AsyncLoad(name));
            Time.timeScale = 1;
        }

        private IEnumerator AsyncLoad(string name)
        {
            // stop playing all sounds if going to main menu            
            bool sceneIsNotMainMenu = name != "MainMenu";
            if (!sceneIsNotMainMenu)
                SFXController.Instance.StopPlaying();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // show the pause menu button if the scene is not the main menu
            SetPauseButton(sceneIsNotMainMenu);
        }

        public void OnLose()
        {
            Time.timeScale = 0;
            SetPauseButton(false);
            m_loseCanvas.gameObject.SetActive(true);
        }

        public void SetPauseButton(bool value)
        {
            m_pauseButton.SetActive(value);
        }
    }
}
