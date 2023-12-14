using TMPro;
using UnityEngine;

namespace Snakefun.Game.Control
{
    public class GameStatistics : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private TextMeshProUGUI timerText;

        SnakeController snakeController;

        private void OnEnable()
        {
            snakeController = GameObject.FindGameObjectWithTag("Player").GetComponent<SnakeController>();
        }

        private void Update()
        {
            var minutes = Mathf.FloorToInt(snakeController.GameTime / 60f);
            var seconds = Mathf.FloorToInt(snakeController.GameTime % 60f);

            pointsText.text = $"Points: {snakeController.Points}";

            timerText.text = $"Time: {minutes}m {seconds}s";
        }
    }
}