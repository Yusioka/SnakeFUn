using Snakefun.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Snakefun.Game.UI
{
    public class PlayerProfile : MonoBehaviour
    {
        private const string avatarPath = "Game/Avatars/Avatar_";

        [SerializeField] private TextMeshProUGUI nickname;
        [SerializeField] private Image avatar;

        private Database database;

        private void OnEnable()
        {
            database = DatabaseController.Instance.Database;
            UpdateUI();
        }

        private void Update()
        {
            if (Resources.Load<Image>(avatarPath + database.CurrentDatabaseModel.AvatarID)?.TryGetComponent<Image>(out var avatar) == true)
            {
                this.avatar.sprite = avatar.sprite;
            }
        }

        private void UpdateUI()
        {
            var currentDatabaseModel = database.CurrentDatabaseModel;

            var avatar = Resources.Load<Image>(avatarPath + database.CurrentDatabaseModel.AvatarID);

            nickname.text = currentDatabaseModel.Nickname;
            this.avatar.sprite = avatar.sprite;
        }
    }
}