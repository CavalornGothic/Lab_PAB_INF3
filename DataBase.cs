using System;
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

    }
}
