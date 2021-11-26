using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_PAB_INF3.Lab_2
{
    public class DataUploaderLab2
    {
        private readonly string _connString = Settings._connString;
        private readonly DataBase _database;

        private string queryDataJobAds = @"
            INSERT INTO [dbo].[jobAds]
                   ([createdate]
                   ,[status]
                   ,[closedate]
                   ,[title]
                   ,[author]
                   ,[description])
             VALUES
                   (@createdate
                   ,@status
                   ,@closedata
                   ,@title
                   ,@author
                   ,@description)
        ";

        private string queryDataJobsAdsTechElem = @"
            INSERT INTO [dbo].[jobAdsTechElem]
                ([idJobAds]
                ,[name]
                ,[exp])
            VALUES
                (@idJobAds
                ,@name
                ,@exp)
        ";

        public DataUploaderLab2()
        {
            _database = new DataBase(_connString);
        }

        public bool SendJobsCSVData(string JAData, string JATechData)
        {
            try
            {
                var dataJobAds = new DataLoaderLab2().LoadCSVJobAds(JAData);
                var dataJobsAdsTechElem = new DataLoaderLab2().LoadCSVJobAdsTechElem(JATechData);
                int count = 0, count2 = 0;
                foreach (var line in dataJobAds)
                {
                    Dictionary<string, string> mapParams = new Dictionary<string, string>()
                    {
                        {"@createdate", line.CreateDate},
                        {"@status", line.Status},
                        {"@closedata", line.CloseDate},
                        {"@title", line.Title},
                        {"@author", line.Author},
                        {"@description",line.Description}
                    };
                    count += _database.InsertDataWithParams(queryDataJobAds, mapParams);
                }
                Logger.ConsoleLog(0, $"wczytano {count} ofert pracy do bazy danych.");
                foreach (var line in dataJobsAdsTechElem)
                {
                    Dictionary<string, string> mapParams = new Dictionary<string, string>()
                    {
                        {"@idJobAds", line.idJobAds},
                        {"@name", line.name },
                        {"@exp", line.Exp.ToString()}
                    };
                    count2 += _database.InsertDataWithParams(queryDataJobsAdsTechElem, mapParams);
                }
                Logger.ConsoleLog(0, $"wczytano {count} elementów ofert pracy do bazy danych.");
                if (count > 0 && count2 > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Logger.ConsoleLog(2, $"wystąpił błąd podczas próby wczytania danych do bazy, powód:");
                Logger.ConsoleLog(2, ex.Message);
                return false;
            }
        }

        public bool SendJobsCSVDataAsTran(string JAData, string JATechData)
        {
            try
            {
                var dataJobAds = new DataLoaderLab2().LoadCSVJobAds(JAData);
                var dataJobsAdsTechElem = new DataLoaderLab2().LoadCSVJobAdsTechElem(JATechData);
                string insertQuery = ""; 
                foreach (var line in dataJobAds)
                {
                    insertQuery += queryDataJobAds.Replace("@createdate", $"'{line.CreateDate}'").Replace("@status", $"{line.Status}").Replace("@closedata", $"'{line.CloseDate}'").Replace("@title", $"'{line.Title}'").Replace("@author", $"'{line.Author}'").Replace("@description", $"'{line.Description}'") + ";";
                }
                foreach (var line in dataJobsAdsTechElem)
                {
                    insertQuery += queryDataJobsAdsTechElem.Replace("@idJobAds", $"{line.idJobAds}").Replace("@name", $"'{line.name}'").Replace("@exp", $"{line.Exp}") + ";";
                }

                int affected = 0;
                using(var con = _database.Connect())
                {
                    con.Open();
                    using(SqlTransaction tran = con.BeginTransaction())
                    {
                        SqlCommand cmd = new SqlCommand(insertQuery, con);
                        cmd.Transaction = tran;
                        affected = cmd.ExecuteNonQuery();
                        tran.Commit();
                    }
                    con.Close();
                }
                Logger.ConsoleLog(0, $"wczytano {affected} wierszy do bazy danych");
                if (affected > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Logger.ConsoleLog(2, $"wystąpił błąd podczas próby wczytania danych do bazy, powód:");
                Logger.ConsoleLog(2, ex.Message);
                return false;
            }
        }
    }
}
