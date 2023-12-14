using Snakefun.Data;
using Snakefun.Game.Control;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Snakefun.Game.Menu
{
    public class RegistrationMenu : MonoBehaviour
    {
        [Header("Login")]
        [SerializeField] private TMP_InputField nicknameLogin;
        [SerializeField] private TMP_InputField passwordLogin;
        [SerializeField] private Toggle remeberUser;

        [Header("Registration")]
        [SerializeField] private TMP_InputField nicknameRegister;
        [SerializeField] private TMP_InputField passwordRegister;
        [SerializeField] private TMP_InputField passwordConfirmRegister;

        private Database database;

        private void OnEnable()
        {
            database = DatabaseController.Instance.Database;

            Debug.Log(database.CurrentDatabaseModel.Nickname);
            Debug.Log(PlayerPrefs.GetString("nickname"));
            if (!string.IsNullOrEmpty(database.CurrentDatabaseModel.Nickname))
            {
                SceneController.Instance.LoadMenuScene();
            }
        }

        public void LoginUser()
        {
            var success = database.TryLoginUser(nicknameLogin.text, passwordLogin.text, remeberUser.isOn);

            if (success)
            {
                SceneController.Instance.LoadMenuScene();
            }
        }

        public void RegisterUser()
        {
            var isPasswordSimilar = passwordRegister.text == passwordConfirmRegister.text;
            var success = database.TryRegisterUser(nicknameRegister.text, passwordRegister.text, Role.Player, isPasswordSimilar);

            if (isPasswordSimilar && success)
            {
                SceneController.Instance.LoadMenuScene();
            }
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}