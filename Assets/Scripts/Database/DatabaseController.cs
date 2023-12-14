using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

namespace Snakefun.Data
{
    public class DatabaseController : MonoBehaviour
    {
        public static DatabaseController Instance { get; private set; }
        public Database Database { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            Database = new(CreateAndOpenDatabase());

            DontDestroyOnLoad(gameObject);

        //    Database.ClearUserData();
        }

        private IDbConnection CreateAndOpenDatabase()
        {
            // Create and open the SQLite database connection
            IDbConnection dbConnection = new SqliteConnection("URI=file:GeneralDatabase.sqlite");
            dbConnection.Open();

            return dbConnection;
        }
    }
}