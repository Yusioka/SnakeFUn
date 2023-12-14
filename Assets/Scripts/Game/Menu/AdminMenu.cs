using Snakefun.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Snakefun.Game.Menu
{
    public class AdminMenu : MonoBehaviour
    {
        [SerializeField] private AdminRowItem adminRowItem;
        [SerializeField] private Transform contentRoot;

        private Database database;

        private List<DatabaseModel> databaseModels = new();
        private List<DatabaseModel> DatabaseModels
        {
            get => databaseModels;

            set
            {
                if (databaseModels != value)
                {
                    databaseModels = value;
                    BuildAdminTable();
                }
            }
        }

        private bool idSort;
        private bool usernameSort;
        private bool highScoreSort;
        private bool totalScoreSort;

        private void OnEnable()
        {
            database = DatabaseController.Instance.Database;

            idSort = false;
            usernameSort = false;
            highScoreSort = false;
            totalScoreSort = true;

            SortByID();
        }

        private void BuildAdminTable()
        {
            foreach (Transform item in contentRoot)
            {
                Destroy(item.gameObject);
            }

            foreach (var databaseModel in DatabaseModels)
            {
                if (databaseModel.ID == database.CurrentDatabaseModel.ID)
                    continue;

                var isDelButtonActive = databaseModel.Role != Role.Developer;

                AdminRowItem uiInstance = Instantiate(adminRowItem, contentRoot);
                uiInstance.Initialize(databaseModel.ID, databaseModel.Nickname, databaseModel.Password, databaseModel.AvatarID, databaseModel.Role, isDelButtonActive);
            }
        }

        public void SortByID()
        {
            DatabaseModels = database.GetAllPlayersSortedByParameter("ID", ref idSort);
        }
        public void SortByNickname()
        {
            DatabaseModels = database.GetAllPlayersSortedByParameter("Nickname", ref usernameSort);
        }
        public void SortByPassword()
        {
            DatabaseModels = database.GetAllPlayersSortedByParameter("Password", ref highScoreSort);
        }
        public void SortByAvatarId()
        {
            DatabaseModels = database.GetAllPlayersSortedByParameter("AvatarID", ref totalScoreSort);
        }
        public void SortByRole()
        {
            DatabaseModels = database.GetAllPlayersSortedByParameter("Role", ref totalScoreSort);
        }
    }
}