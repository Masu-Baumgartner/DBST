using Dapper;

using MySql.Data.MySqlClient;

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MB.DBST
{
    public class MySQLActions
    {
        public static string ConnectionString { get; set; }

        public static async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = new MySqlConnection(ConnectionString))
            {
                var data = await connection.QueryAsync<T>(sql, parameters);

                return data.ToList();
            }
        }

        public static async Task SaveData<T>(string sql, T parameters)
        {
            using (IDbConnection connection = new MySqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }

        public static async Task RemoveData<T>(string sql, T parameters)
        {
            using (IDbConnection connection = new MySqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }

        public static async Task<int> SaveDataAndReturnKey<T>(string sql, T parameters)
        {
            using (IDbConnection connection = new MySqlConnection(ConnectionString))
            {
                var res = await connection.QueryAsync<int>(sql, parameters);
                return res.Single();
            }
        }
    }
}
