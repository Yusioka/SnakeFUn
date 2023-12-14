using Snakefun.Data;
using Snakefun.Game.Control;
using UnityEngine;
using UnityEngine.UI;

namespace Snakefun.Game.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button endGameButton;
        [SerializeField] private Button adminMenuButton;

        private void OnEnable()
        {
            startGameButton.onClick.AddListener(SceneController.Instance.LoadGameScene);
            endGameButton.onClick.AddListener(SceneController.Instance.QuitGame);

            var isUserDeveloper = DatabaseController.Instance.Database.CurrentDatabaseModel.Role == Role.Developer;

            adminMenuButton.gameObject.SetActive(isUserDeveloper);
        }

        private void OnDisable()
        {
            startGameButton.onClick.RemoveAllListeners();
            endGameButton.onClick.RemoveAllListeners();
        }
    }
}