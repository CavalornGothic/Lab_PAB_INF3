using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Lab_PAB_INF3.Lab_1
{
    class DataUploader
    {
        private readonly string _connString = @"Server=as-it\Arek;Database=PABLab;User Id=sa;Password=Qaz123elo;";
        /*
         * Metoda nr 1 (obowiązkowa z treści zadania)
         * Wysyła do bazy danych dane wiersz po wierszu, otwierając połączenie w metodzie i mierzy czas wykonania
         */
        public void SendDataLineByLine(ICollection<LineDTO> data)
        {
            ResearchTime research = new ResearchTime();
            List<double> meas = new List<double>();
            DataBase dataBase = new DataBase(_connString);
            Logger.ConsoleLog(0, "zaczynam wykonywać próbę metodą nr 1..");
            research.Start = DateTime.Now;
            foreach(var x in data)
            {
                DateTime dataStart = DateTime.Now;
                using (var conn = dataBase.Connect())
                {
                    conn.Open();
                    string query = @$"
                    INSERT INTO [dbo].[Kody]
                               ([zipcode]
                               ,[address]
                               ,[city]
                               ,[province]
                               ,[district])
                         VALUES
                               (@zipcode
                               ,@address
                               ,@city
                               ,@province
                               ,@district)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("zipcode", x.ZipCode);
                    cmd.Parameters.AddWithValue("address", x.Address);
                    cmd.Parameters.AddWithValue("city", x.City);
                    cmd.Parameters.AddWithValue("province", x.Province);
                    cmd.Parameters.AddWithValue("district", x.District);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                DateTime dataEnd = DateTime.Now;
                meas.Add(TimeSpan.FromTicks(dataEnd.Ticks - dataStart.Ticks).TotalSeconds);
            }
            research.End = DateTime.Now;
            research.Meas = meas;
            Logger.ConsoleLog(0, "skończono próbę metodą nr 1");
            Logger.ConsoleLog(0, " ------------------------ | WYNIK | ------------------------");
            Logger.ConsoleLog(0, $"start: {research.Start.ToString("HH:mm:ss.ffff")}");
            Logger.ConsoleLog(0, $"max: {research.Meas.Max()}");
            Logger.ConsoleLog(0, $"avg: {research.Meas.Average()}");
            Logger.ConsoleLog(0, $"min: {research.Meas.Min()}");
            Logger.ConsoleLog(0, $"koniec: {research.End.ToString("HH:mm:ss.ffff")}");
            Logger.ConsoleLog(0, $"łączny czas trwania: {TimeSpan.FromTicks(research.End.Ticks - research.Start.Ticks).TotalSeconds} sekund");
            Logger.ConsoleLog(0, " -----------------------------------------------------------");
        }
        /*
         * Metoda nr 2 (obowiązkowa z treści zadania)
         * Pakuje w jedno zapytanie całą treść pliku i wysyła to do serwera bazy danych
         */
        public void SendAllData(ICollection<LineDTO> data)
        {
            ResearchTime research = new ResearchTime();
            DataBase dataBase = new DataBase(_connString);
            Logger.ConsoleLog(0, "zaczynam wykonywać próbę metodą nr 2..");
            research.Start = DateTime.Now;
            string query = "";
            foreach(var x in data)
            {
                query += @$"
                     INSERT INTO [dbo].[Kody]
                               ([zipcode]
                               ,[address]
                               ,[city]
                               ,[province]
                               ,[district])
                         VALUES
                               ('{x.ZipCode}'
                               ,'{x.Address}'
                               ,'{x.City}'
                               ,'{x.Province}'
                               ,'{x.District}'
                    );
                ";
            }
            DateTime dataStart = DateTime.Now;
            using (var conn = dataBase.Connect())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            DateTime dataEnd = DateTime.Now;
            research.End = DateTime.Now;
            Logger.ConsoleLog(0, "skończoną próbę metodą nr 2");
            Logger.ConsoleLog(0, " ------------------------ | WYNIK | ------------------------");
            Logger.ConsoleLog(0, $"start: {research.Start.ToString("HH:mm:ss.ffff")}");
            Logger.ConsoleLog(0, $"czas trwania zapytania: {TimeSpan.FromTicks(dataStart.Ticks - dataEnd.Ticks).TotalSeconds} sekund");
            Logger.ConsoleLog(0, $"koniec: {research.End.ToString("HH:mm:ss.ffff")}");
            Logger.ConsoleLog(0, $"łączny czas trwania: {TimeSpan.FromTicks(research.End.Ticks - research.Start.Ticks).TotalSeconds} sekund");
            Logger.ConsoleLog(0, " -----------------------------------------------------------");
        }
    }
}
