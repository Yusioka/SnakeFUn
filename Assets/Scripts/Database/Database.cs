using Mono.Data.Sqlite;
using Snakefun.Game.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Snakefun.Data
{
    public class Database
    {
        private readonly IDbConnection dbConnection;

        private DatabaseModel databaseModel = new();

        public DatabaseModel CurrentDatabaseModel
        {
            get => databaseModel;

            set
            {
                databaseModel = value;                
            }
        }

        public Database(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
            InitializeDatabase();
            CurrentDatabaseModel = GetUserDataByNickname(PlayerPrefs.GetString("nickname"));
        }

        public bool TryRegisterUser(string nickname, string password, Role role, bool isPasswordSimilar)
        {
            try
            {
                ValidatenNickname(nickname);
                ValidatePassword(password, isPasswordSimilar);

                var dbCommandCheckUser = dbConnection.CreateCommand();
                dbCommandCheckUser.CommandText = "SELECT COUNT(*) FROM Players WHERE nickname=@nickname";
                dbCommandCheckUser.Parameters.Add(new SqliteParameter("@nickname", nickname));

                int count = Convert.ToInt32(dbCommandCheckUser.ExecuteScalar());

                if (count > 0)
                {
                    throw new InvalidOperationException("User already exists!");
                }

                var dbCommandInsertUser = dbConnection.CreateCommand();
                dbCommandInsertUser.CommandText = "INSERT INTO Players (nickname, password, role) VALUES (@nickname, @password, @role)";
                dbCommandInsertUser.Parameters.Add(new SqliteParameter("@nickname", nickname));
                dbCommandInsertUser.Parameters.Add(new SqliteParameter("@password", password));
                dbCommandInsertUser.Parameters.Add(new SqliteParameter("@role", $"{(int)role}"));
                dbCommandInsertUser.ExecuteNonQuery();

                CurrentDatabaseModel = GetUserDataByNickname(nickname);

                return true;
            }
            catch (ArgumentException e)
            {
                ShowMessage(e.Message);
            }
            catch (InvalidOperationException e)
            {
                ShowMessage(e.Message);
            }
            catch (SqliteException e)
            {
                ShowMessage("Error registering user in database: " + e.Message);
            }
            catch (Exception e)
            {
                ShowMessage("An unexpected error occurred: " + e.Message);
            }

            return false;
        }

        public bool TryLoginUser(string nickname, string password, bool rememberUser)
        {
            try
            {
                bool isPasswordSimilar = true;
                ValidatenNickname(nickname);
                ValidatePassword(password, isPasswordSimilar);

                var dbCommandCheckUser = dbConnection.CreateCommand();
                dbCommandCheckUser.CommandText = "SELECT COUNT(*) FROM Players WHERE nickname=@nickname AND password=@password";
                dbCommandCheckUser.Parameters.Add(new SqliteParameter("@nickname", nickname));
                dbCommandCheckUser.Parameters.Add(new SqliteParameter("@password", password));

                int count = Convert.ToInt32(dbCommandCheckUser.ExecuteScalar());

                if (count <= 0)
                {
                    throw new InvalidOperationException("Invalid Username or Password.");
                }

                CurrentDatabaseModel = GetUserDataByNickname(nickname);

                if (rememberUser)
                {
                    PlayerPrefs.SetString("nickname", CurrentDatabaseModel.Nickname);
                }
                else
                {
                    PlayerPrefs.SetString("nickname", "");
                }

                return true;
            }

            catch (ArgumentException e)
            {
                ShowMessage(e.Message);
            }
            catch (InvalidOperationException e)
            {
                ShowMessage(e.Message);
            }
            catch (SqliteException e)
            {
                ShowMessage("Error registering user in database: " + e.Message);
            }
            catch (Exception e)
            {
                ShowMessage("An unexpected error occurred: " + e.Message);
            }

            return false;
        }

        public DatabaseModel GetUserDataByNickname(string nickname)
        {
            if (nickname != null)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "SELECT * FROM Players WHERE nickname=@nickname";
                dbCommand.Parameters.Add(new SqliteParameter("@nickname", nickname));

                var reader = dbCommand.ExecuteReader();

                if (reader.Read())
                {
                    DatabaseModel user = new()
                    {
                        ID = reader.GetInt32(0),
                        Nickname = reader.GetString(1),
                        Password = reader.GetString(2),
                        Role = (Role)reader.GetInt32(3),
                        AvatarID = reader.GetInt32(4),
                        HighScore = reader.GetInt32(5),
                        HighTime = reader.GetInt32(6),
                        TotalScore = reader.GetInt32(7),
                    };

                    reader.Close();

                    return user;
                }
            }

            return new DatabaseModel();
        }

        public List<DatabaseModel> GetAllPlayersSortedByParameter(string type, ref bool desc)
        {
            var PlayersList = new List<DatabaseModel>();
            var dbCommand = dbConnection.CreateCommand();

            if (desc)
            {
                dbCommand.CommandText = $"SELECT * FROM Players ORDER BY {type} DESC";
            }
            else
            {
                dbCommand.CommandText = $"SELECT * FROM Players ORDER BY {type}";
            }

            desc = !desc;

            var reader = dbCommand.ExecuteReader();

            while (reader.Read())
            {
                DatabaseModel user = new()
                {
                    ID = reader.GetInt32(0),
                    Nickname = reader.GetString(1),
                    Password = reader.GetString(2),
                    Role = (Role)reader.GetInt32(3),
                    AvatarID = reader.GetInt32(4),
                    HighScore = reader.GetInt32(5),
                    HighTime = reader.GetInt32(6),
                    TotalScore = reader.GetInt32(7),
                };

                PlayersList.Add(user);
            }

            reader.Close();

            return PlayersList;
        }

        public bool UpdatePassword(string newPassword)
        {
            try
            {
                bool isPasswordSimilar = true;
                ValidatePassword(newPassword, isPasswordSimilar);

                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "UPDATE Players SET password=@NewPassword WHERE id=@UserId";
                dbCommand.Parameters.Add(new SqliteParameter("@NewPassword", newPassword));
                dbCommand.Parameters.Add(new SqliteParameter("@UserId", CurrentDatabaseModel.ID));

                dbCommand.ExecuteNonQuery();

                databaseModel.Password = newPassword;
                ShowMessage("Success! Your new password is: " + newPassword);
                return true;
            }
            catch (ArgumentException e)
            {
                ShowMessage(e.Message);
            }
            catch (SqliteException e)
            {
                ShowMessage(e.Message);
            }

            return false;
        }

        public bool UpdateAvatar(int avatarID)
        {
            try
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "UPDATE Players SET avatarId=@NewAvatarId WHERE id=@UserId";
                dbCommand.Parameters.Add(new SqliteParameter("@NewAvatarId", avatarID));
                dbCommand.Parameters.Add(new SqliteParameter("@UserId", CurrentDatabaseModel.ID));

                dbCommand.ExecuteNonQuery();

                databaseModel.AvatarID = avatarID;
                return true;
            }
            catch (ArgumentException e)
            {
                ShowMessage(e.Message);
            }
            catch (SqliteException e)
            {
                ShowMessage(e.Message);
            }

            return false;
        }

        public bool ChangeRole(Role role)
        {
            try
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "UPDATE Players SET role=@Role WHERE id=@UserId";
                dbCommand.Parameters.Add(new SqliteParameter("@Role", role));
                dbCommand.Parameters.Add(new SqliteParameter("@UserId", CurrentDatabaseModel.ID));

                dbCommand.ExecuteNonQuery();

                databaseModel.Role = role;
                ShowMessage("Successful role change! Your new role is: " + role);
                return true;
            }
            catch (ArgumentException e)
            {
                ShowMessage(e.Message);
            }
            catch (SqliteException e)
            {
                ShowMessage(e.Message);
            }

            return false;
        }

        public void TrySetNewHighScore(int score)
        {
            if (CurrentDatabaseModel.HighScore < score)
            {
                databaseModel.HighScore = score;

                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "UPDATE Players SET highScore=@highScore WHERE id=@UserId";
                dbCommand.Parameters.Add(new SqliteParameter("@highScore", CurrentDatabaseModel.HighScore));
                dbCommand.Parameters.Add(new SqliteParameter("@UserId", CurrentDatabaseModel.ID));

                dbCommand.ExecuteNonQuery();

                ShowMessage("Congratulations! New High Score: " + CurrentDatabaseModel.HighScore);
            }
        }

        public void TrySetNewHighTime(int time)
        {
            if (CurrentDatabaseModel.HighTime < time)
            {
                databaseModel.HighTime = time;

                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "UPDATE Players SET highTime=@highTime WHERE id=@UserId";
                dbCommand.Parameters.Add(new SqliteParameter("@highTime", CurrentDatabaseModel.HighTime));
                dbCommand.Parameters.Add(new SqliteParameter("@UserId", CurrentDatabaseModel.ID));

                dbCommand.ExecuteNonQuery();

                ShowMessage("Congratulations! New Time Record: " + CurrentDatabaseModel.HighTime);
            }
        }

        public void TrySetNewTotalScore(int score)
        {
            if (CurrentDatabaseModel.TotalScore < score)
            {
                databaseModel.TotalScore = score;

                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "UPDATE Players SET totalScore=@totalScore WHERE id=@UserId";
                dbCommand.Parameters.Add(new SqliteParameter("@totalScore", CurrentDatabaseModel.TotalScore));
                dbCommand.Parameters.Add(new SqliteParameter("@UserId", CurrentDatabaseModel.ID));

                dbCommand.ExecuteNonQuery();

                ShowMessage("Congratulations! New Total Score: " + CurrentDatabaseModel.TotalScore);
            }
        }

        public void Logout()
        {
            CurrentDatabaseModel = new();

            PlayerPrefs.SetString("nickname", null);
        }

        public void DeleteUserByNickname(string nickname)
        {
            if (nickname != null)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "DELETE FROM Players WHERE nickname=@nickname";
                dbCommand.Parameters.Add(new SqliteParameter("@nickname", nickname));

                dbCommand.ExecuteNonQuery();
            }
        }

        public void ClearUserData()
        {
            var dbCommandClearData = dbConnection.CreateCommand();
            dbCommandClearData.CommandText = "DELETE FROM Players";
            dbCommandClearData.ExecuteNonQuery();
            ShowMessage("All user data was cleared.");
        }

        private void InitializeDatabase()
        {
            var dbCommandCreateTable = dbConnection.CreateCommand();
            dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Players (id INTEGER PRIMARY KEY, nickname TEXT, password TEXT, role INTEGER DEFAULT 0, avatarId INTEGER DEFAULT 0, highScore INTEGER DEFAULT 0, highTime INTEGER DEFAULT 0, totalScore INTEGER DEFAULT 0)";
            dbCommandCreateTable.ExecuteNonQuery();
        }

        public void ClearCurrentUserData()
        {
            CurrentDatabaseModel = new();
        }

        private void ValidatenNickname(string username)
        {
            string pattern = "^[\\S]{5,20}$";

            if (!Regex.IsMatch(username, pattern))
            {
                throw new ArgumentException("Invalid username. It should consist of 5-20 alphanumeric characters.");
            }
        }

        private void ValidatePassword(string password, bool isPasswordSimilar)
        {
            string pattern = "^[\\S]{4,20}$";

            if (!Regex.IsMatch(password, pattern))
            {
                throw new ArgumentException("Invalid password. It should be 4-20 characters long.");
            }

            if (!isPasswordSimilar)
            {
                throw new ArgumentException("Invalid password. Repeat the password you wrote.");
            }
        }

        private void ShowMessage(string text)
        {
            try
            {
                if (UnityEngine.Object.FindObjectOfType<MessageUI>().TryGetComponent<MessageUI>(out var message))
                {
                    message.ShowMessage(text);
                    message.gameObject.SetActive(true);
                }
            }
            catch (NullReferenceException)
            {

            }
        }
    }
}