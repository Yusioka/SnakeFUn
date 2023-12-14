using Snakefun.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Snakefun.Game.UI
{
    public class StatisticsMenu : MonoBehaviour
    {
        [SerializeField] private StatisticsRowItem statisticsRowItem;
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
                    BuildStatisticsTable();
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

        private void BuildStatisticsTable()
        {
            foreach (Transform item in contentRoot)
            {
                Destroy(item.gameObject);
            }

            foreach (var databaseModel in DatabaseModels)
            {
                StatisticsRowItem uiInstance = Instantiate(statisticsRowItem, contentRoot);
                uiInstance.Initialize(databaseModel.ID, databaseModel.Nickname, databaseModel.HighScore, databaseModel.TotalScore);
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

        public void SortByHighScore()
        {
            DatabaseModels = database.GetAllPlayersSortedByParameter("HighScore", ref highScoreSort);
        }

        public void SortByTotalScore()
        {
            DatabaseModels = database.GetAllPlayersSortedByParameter("TotalScore", ref totalScoreSort);
        }
    }
}