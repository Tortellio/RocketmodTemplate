using I18N.West;
using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RocketmodTemplate.Databases
{
    public class DatabaseManager
    {
        #region Methods
        public void Example(string example)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(yes => ExampleThread(example));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        

        private void ExampleThread(string example)
        {
            /* NonQuery example
             * ExecuteQuery(EQueryType.NonQuery,
                $"INSERT INTO `{Main.Instance.Configuration.Instance.DatabaseConfig.DatabaseTableName}` (`steamId`,`lastDisplayName`) VALUES (@steamId,@lastDisplayName) ON DUPLICATE KEY UPDATE lastDisplayName = @lastDisplayName;",
                new MySqlParameter("@lastDisplayName", lastDisplayName), new MySqlParameter("@steamId", steamId));*/

            /* Reader example
             * var readerResult = (List<Row>) ExecuteQuery(EQueryType.Reader,
                $"SELECT * FROM (SELECT t.steamId, t.points, t.lastDisplayName, @`rownum` := @`rownum` + 1 AS currentRank FROM `{Main.Instance.Configuration.Instance.DatabaseConfig.DatabaseTableName}` t JOIN (SELECT @`rownum` := 0) r ORDER BY t.points DESC) x WHERE x.steamId = @steamId;",
                new MySqlParameter("@steamId", steamId));

                return readerResult?.Select(k => new PlayerRank
                {
                    Points = k.Values["points"].ToString(), CurrentRank = k.Values["currentRank"].ToString(),
                    LastDisplayName = k.Values["lastDisplayName"].ToString()
                }).First();*/

            /* Scalar example
             * var output = 0;
             * var result = ExecuteQuery(EQueryType.Scalar,
                $"SELECT `currentRank` FROM (SELECT t.steamId, t.points, @`rownum` := @`rownum` + 1 AS currentRank FROM `{FeexRanks.Instance.Configuration.Instance.FeexRanksDatabaseConfig.DatabaseTableName}` t JOIN (SELECT @`rownum` := 0) r ORDER BY t.points DESC) x WHERE x.steamId = @steamId;",
                new MySqlParameter("@steamId", steamId));

                if (result != null) int.TryParse(result.ToString(), out output);

                return output;*/
        }
        #endregion

        #region CheckTables
        internal void CreateCheckSchema()
        {
            /*var check = ExecuteQuery(EQueryType.Scalar,
                $"SHOW TABLES LIKE '{Main.Instance.Configuration.Instance.DatabaseConfig.DatabaseTableName}';");

            if (check == null)
                ExecuteQuery(EQueryType.NonQuery,
                    $"CREATE TABLE `{Main.Instance.Configuration.Instance.DatabaseConfig.DatabaseTableName}` (`steamId` VARCHAR(32) NOT NULL, `points` INT(32) NOT NULL DEFAULT '0', `lastDisplayName` varchar(32) NOT NULL, `lastUpdated` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP, PRIMARY KEY (`steamId`));");*/
        }
        #endregion

        #region MySQL
        private object ExecuteQuery(EQueryType queryType, string query, params MySqlParameter[] parameters)
        {
            object result = null;
            MySqlDataReader reader = null;

            using (var connection = CreateConnection())
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = query;

                    foreach (var parameter in parameters)
                        command.Parameters.Add(parameter);

                    connection.Open();
                    switch (queryType)
                    {
                        case EQueryType.Reader:
                            var readerResult = new List<Row>();

                            reader = command.ExecuteReader();
                            while (reader.Read())
                                try
                                {
                                    var values = new Dictionary<string, object>();

                                    for (var i = 0; i < reader.FieldCount; i++)
                                    {
                                        var columnName = reader.GetName(i);
                                        values.Add(columnName, reader[columnName]);
                                    }

                                    readerResult.Add(new Row { Values = values });
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogError(
                                        $"The following query threw an error during reader execution:\nQuery: \"{query}\"\nError: {ex.Message}");
                                }

                            result = readerResult;
                            break;
                        case EQueryType.Scalar:
                            result = command.ExecuteScalar();
                            break;
                        case EQueryType.NonQuery:
                            result = command.ExecuteNonQuery();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
                finally
                {
                    reader?.Close();
                    connection.Close();
                }
            }

            return result;
        }

        private MySqlConnection CreateConnection()
        {
            MySqlConnection connection = null;
            try
            {
                if (Main.Instance.Configuration.Instance.DatabaseConfig.DatabasePort == 0)
                    Main.Instance.Configuration.Instance.DatabaseConfig.DatabasePort = 3306;
                connection = new MySqlConnection(
                    $"SERVER={Main.Instance.Configuration.Instance.DatabaseConfig.DatabaseAddress};DATABASE={Main.Instance.Configuration.Instance.DatabaseConfig.DatabaseName};UID={Main.Instance.Configuration.Instance.DatabaseConfig.DatabaseUsername};PASSWORD={Main.Instance.Configuration.Instance.DatabaseConfig.DatabasePassword};PORT={Main.Instance.Configuration.Instance.DatabaseConfig.DatabasePort};");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return connection;
        }

        internal DatabaseManager()
        {
            new CP1250();
            CreateCheckSchema();
        }
#endregion
    }

    #region Essentials
    public class DatabaseConfig
    {
        public string DatabaseAddress;
        public string DatabaseUsername;
        public string DatabasePassword;
        public string DatabaseName;
        public string DatabaseTableName;
        public int DatabasePort;
    }

    public enum EQueryType
    {
        Scalar,
        Reader,
        NonQuery
    }

    public sealed class Row
    {
        public Dictionary<string, object> Values;
    }
    #endregion
}
