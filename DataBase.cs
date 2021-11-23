using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Lab_PAB_INF3
{
    class DataBase
    {
        private readonly string _connString;

        public DataBase(string connString)
        {
            this._connString = connString;
        }

        public SqlConnection Connect()
        {
            try
            {
                return new SqlConnection(_connString);
            }
            catch (Exception ex)
            {
                Logger.ConsoleLog(2, $"podczas próby połączenia wystąpił błąd: {ex.Message}");   
                return null;
            }
        }

        public int InsertData(string query)
        {
            int affected = 0;
            using (var conn = this.Connect())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                affected = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return affected;
        }

        public string BuildSendDataQuery(string query, IDictionary<string, string> parameters)
        {
            string copyQuery = query;
            foreach(var param in parameters)
            {
                copyQuery.Replace(param.Key, CharsHelper.SpecialChars(param.Value));
            }
            return copyQuery;
        }

    }
}
