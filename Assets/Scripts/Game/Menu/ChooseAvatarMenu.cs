using Snakefun.Data;
using Snakefun.Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Snakefun.Game.Menu
{
    public class ChooseAvatarMenu : MonoBehaviour
    {
        private const string avatarPath = "Game/Avatars/Avatar_";

        [SerializeField] private AvatarItem avatarItem;
        [SerializeField] private Transform contentRoot;

        private Database database;

        private void OnEnable()
        {
            database = DatabaseController.Instance.Database;

            BuildAvatarTable();
        }

        private void BuildAvatarTable()
        {
            foreach (Transform item in contentRoot)
            {
                Destroy(item.gameObject);
            }

            for (int i = 0; i < 20; i++)
            {
                if (Resources.Load<Image>(avatarPath + i)?.TryGetComponent<Image>(out var avatar) == true)
                {
                    AvatarItem uiInstance = Instantiate(avatarItem, contentRoot);
                    uiInstance.Initialize(avatar, i, UpdateAvatar);
                }
                else
                {
                    break;
                }
            }
        }

        public void UpdateAvatar(int avatarId)
        {
            database.UpdateAvatar(avatarId);
        }
    }
}