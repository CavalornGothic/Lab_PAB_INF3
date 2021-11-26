using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Lab_PAB_INF3.Lab_2
{
    public class DataLoaderLab2
    {

        public ICollection<JobAdsDTO> LoadCSVJobAds(string fileName)
        {
            ICollection<JobAdsDTO> data = null;
            try
            {
                data = new List<JobAdsDTO>();
                foreach (var line in File.ReadAllLines(fileName))
                {
                    data.Add(new JobAdsDTO { CreateDate = line.Split(";")[0], Status = line.Split(";")[1], CloseDate = line.Split(";")[2], Title = line.Split(";")[3], Author = line.Split(";")[4], Description = line.Split(";")[5] });
                }
            }
            catch (Exception ex)
            {
                Logger.ConsoleLog(2, $"podczas próby odczytania danych z pliku wystąpił błąd: {ex.Message}");
            }
            return data;
        }

        public ICollection<JobAdsTechelemDTO> LoadCSVJobAdsTechElem(string fileName)
        {
            ICollection<JobAdsTechelemDTO> data = null;
            try
            {
                data = new List<JobAdsTechelemDTO>();
                foreach (var line in File.ReadAllLines(fileName))
                {
                    data.Add(new JobAdsTechelemDTO { idJobAds = line.Split(";")[0], name = line.Split(";")[1], Exp = String.IsNullOrEmpty(line.Split(";")[2]) ? null : Convert.ToInt32(line.Split(";")[2])});
                }
            }
            catch (Exception ex)
            {
                Logger.ConsoleLog(2, $"podczas próby odczytania danych z pliku wystąpił błąd: {ex.Message}");
            }
            return data;
        }

        public ICollection<JobAdsTechelemDTO> LoadAllJobAdsTechElemFromDB()
        {
            ICollection<JobAdsTechelemDTO> data = null;
            try
            {
                data = new List<JobAdsTechelemDTO>();
                string query = "SELECT  [idJobAds], [name], [exp] FROM[PABLab].[dbo].[jobAdsTechElem]";
                using(var con = new DataBase(Settings._connString).Connect())
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(query, con);
                    var rows = command.ExecuteReader();
                    while (rows.Read())
                    {
                        data.Add(new JobAdsTechelemDTO { idJobAds = rows[0].ToString(), name = rows[1].ToString(), Exp = rows.IsDBNull(2) ? null : Convert.ToInt32(rows[2]) });
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.ConsoleLog(2, $"podczas próby odczytania danych z bazy danych wystąpił błąd: {ex.Message}");
            }
            return data;
        }

    }
}
