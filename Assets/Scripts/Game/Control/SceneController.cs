using UnityEngine;
using UnityEngine.SceneManagement;

namespace Snakefun.Game.Control
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        public void LoadAuthorizationScene()
        {
            SceneManager.LoadSceneAsync(0);
        }

        public void LoadMenuScene()
        {
            SceneManager.LoadSceneAsync(1);
        }

        public void LoadGameScene()
        {
            SceneManager.LoadSceneAsync(2);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}