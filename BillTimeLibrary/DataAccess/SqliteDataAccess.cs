using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;

namespace BillTimeLibrary.DataAccess
{
    public static class SqliteDataAccess
    {
        //LoadData<PersonModel>("Select * from Person", null) = List<PersonModel>
        public static List<T> LoadData<T>(string sqlStatement, Dictionary<string, object> parameters,
            string connectionName = "Default")
        {
            DynamicParameters p = parameters.ToDynamicParameters();

            //Option 2
//            parameters.ToList().ForEach(x => p.Add(x.Key, x.Value));

            using (IDbConnection connection =
                new SQLiteConnection(DataAccessHelpers.LoadConnectionString(connectionName)))
            {
                var rows = connection.Query<T>(sqlStatement, p);

                return rows.ToList();
            }

            #region Option 1: Foreach

            //Foreach Option 1
//            foreach (var param in parameters)
//            {
//                //@FirstName, "SumWin"
//                p.Add(param.Key, param.Value);
//            }

            #endregion
        }

        public static void SaveData(string sqlStatement, Dictionary<string, object> parameters,
            string connectionName = "Default")
        {
            DynamicParameters p = parameters.ToDynamicParameters();

            using (IDbConnection connection =
                new SQLiteConnection(DataAccessHelpers.LoadConnectionString(connectionName)))
            {
                connection.Execute(sqlStatement, p);
            }
        }

        private static DynamicParameters ToDynamicParameters(this Dictionary<string, object> p)
        {
            DynamicParameters output = new DynamicParameters();

            p.ToList().ForEach(x => p.Add(x.Key, x.Value));

            return output;
        }
    }
}