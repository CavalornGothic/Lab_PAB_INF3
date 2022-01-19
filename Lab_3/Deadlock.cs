using System;
using System.Data.SqlClient;
using System.Threading;

namespace Lab_PAB_INF3.Lab_3
{
    public class Deadlock
    {
        private readonly string _connString = Settings._connString;
        private readonly DataBase _database;

        public Deadlock()
        {
            _database = new DataBase(_connString);
        }
        public void SendQuery()
        {
            try
            {
                using (var con = _database.Connect())
                {
                    var insertQuery = @"
                        SET DEADLOCK_PRIORITY HIGH
                        UPDATE dbo.jobAds SET author = 'jasjfasjf' WHERE id IN (4, 10,7,6,12);
		                UPDATE dbo.jobAdsTechElem SET name = 'asfasfasf' WHERE idJobAds IN (3,4,5,6,7)
                        ";
                    con.Open();
                    using (SqlTransaction tran = con.BeginTransaction())
                    {
                        SqlCommand cmd = new SqlCommand(insertQuery, con);
                        cmd.Transaction = tran;
                        cmd.ExecuteNonQuery();
                        tran.Commit();
                    }
                    con.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void SendQuery2()
        {
            try
            {
                using (var con = _database.Connect())
                {
                    var insertQuery2 = @"
                        SET DEADLOCK_PRIORITY LOW
		                UPDATE dbo.jobAdsTechElem SET name = 'sds123124dg' WHERE idJobAds IN (3,4,5,6,7)
		                UPDATE dbo.jobAds SET author = 'jasjsdsdafa2121122112sjf' WHERE id IN (4, 10,7,6,12);
                        ";
                    con.Open();
                    using (SqlTransaction tran = con.BeginTransaction())
                    {
                        SqlCommand cmd = new SqlCommand(insertQuery2, con);
                        cmd.Transaction = tran;
                        cmd.ExecuteNonQuery();
                        tran.Commit();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
