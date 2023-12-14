using Snakefun.Data;
using Snakefun.Game.Control;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Snakefun.Game.Menu
{
    public class PlayerMenu : MonoBehaviour
    {
        private const string avatarPath = "Game/Avatars/Avatar_";

        [SerializeField] private TextMeshProUGUI nickname;
        [SerializeField] private TextMeshProUGUI currentPassword;
        [SerializeField] private Image avatar;
        [SerializeField] private TMP_InputField newPasswordField;
        [SerializeField] private Toggle roleToggle;

        private Database database;

        private void OnEnable()
        {
            database = DatabaseController.Instance.Database;
            roleToggle.isOn = database.CurrentDatabaseModel.Role == Role.Developer;

            UpdateUI();
        }

        private void Update()
        {
            if (Resources.Load<Image>(avatarPath + database.CurrentDatabaseModel.AvatarID)?.TryGetComponent<Image>(out var avatar) == true)
            {
                this.avatar.sprite = avatar.sprite;
            }
        }

        public void Logout()
        {
            database.Logout();

            SceneController.Instance.LoadAuthorizationScene();
        }

        public void ChangePassword()
        {
            var canChangePassword = database.UpdatePassword(newPasswordField.text);

            if (canChangePassword)
            {
                UpdateUI();
            }
        }

        public void ChangeRole()
        {
            var canChangeRole = database.ChangeRole(roleToggle.isOn ? Role.Developer : Role.Player);

            if (canChangeRole)
            {
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            var currentDatabaseModel = database.CurrentDatabaseModel;

            var avatar = Resources.Load<Image>(avatarPath + database.CurrentDatabaseModel.AvatarID);
            nickname.text = "Nickname: " + currentDatabaseModel.Nickname;
            currentPassword.text = "Password: " + currentDatabaseModel.Password;
            this.avatar.sprite = avatar.sprite;
            newPasswordField.text = "";
        }
    }
}