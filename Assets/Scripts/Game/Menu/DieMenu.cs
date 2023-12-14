using Snakefun.Data;
using Snakefun.Game.Control;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Snakefun.Game.Menu
{
    public class DieMenu : MonoBehaviour
    {
        [SerializeField] private GameObject body;
        [SerializeField] private TextMeshProUGUI points;
        [SerializeField] private TextMeshProUGUI time;
        [SerializeField] private TextMeshProUGUI total;
        [SerializeField] private Button restartGameButton;
        [SerializeField] private Button loadMenuButton;

        private SnakeController snakeController;

        private void OnEnable()
        {
            snakeController = GameObject.FindGameObjectWithTag("Player").GetComponent<SnakeController>();
            snakeController.OnDie += ShowResults;

            restartGameButton.onClick.AddListener(SceneController.Instance.LoadGameScene);
            loadMenuButton.onClick.AddListener(SceneController.Instance.LoadMenuScene);
        }

        private void OnDisable()
        {
            snakeController.OnDie -= ShowResults;

            restartGameButton.onClick.RemoveAllListeners();
            loadMenuButton.onClick.RemoveAllListeners();
        }

        private void ShowResults()
        {
            var minutes = Mathf.FloorToInt(snakeController.GameTime / 60f);
            var seconds = Mathf.FloorToInt(snakeController.GameTime % 60f);

            var total = Mathf.FloorToInt((snakeController.Points * 1.2f) + snakeController.GameTime);

            points.text = $"Points: {snakeController.Points}";
            time.text = $"Time: {minutes}m {seconds}s";
            this.total.text = $"Total: {total}";

            TrySetNewRecords(snakeController.Points, Mathf.FloorToInt(snakeController.GameTime), total);

            body.SetActive(true);
        }

        private void TrySetNewRecords(int points, int time, int total)
        {
            var database = DatabaseController.Instance.Database;

            database.TrySetNewHighScore(points);
            database.TrySetNewHighTime(time);
            database.TrySetNewTotalScore(total);
        }
    }
}