using TMPro;
using UnityEngine;

namespace Snakefun.Game.UI
{
    public class StatisticsRowItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI id;
        [SerializeField] private TextMeshProUGUI nickname;
        [SerializeField] private TextMeshProUGUI points;
        [SerializeField] private TextMeshProUGUI total;

        public void Initialize(int id, string nickname, int points, int total)
        {
            this.id.text = $"{id}";
            this.nickname.text = nickname;
            this.points.text = $"{points}";
            this.total.text = $"{total}";
        }
    }
}