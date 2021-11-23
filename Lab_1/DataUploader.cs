using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lab_PAB_INF3.Lab_1
{
    class DataUploader
    {
        private readonly string _connString = Settings._connString;
        private readonly DataBase _database;

        string basicQuery = Settings.basicQuery;

        public DataUploader()
        {
            _database = new DataBase(_connString);
        }

        /*
         * Metoda nr 1 (obowiązkowa z treści zadania)
         * Wysyła do bazy danych dane wiersz po wierszu, otwierając połączenie w metodzie i mierzy czas wykonania
         */
        public void SendDataLineByLine(ICollection<LineDTO> data)
        {
            ResearchTime research = new ResearchTime();
            List<double> meas = new List<double>();
            Logger.ConsoleLog(0, "zaczynam wykonywać próbę metodą nr 1..");
            research.Start = DateTime.Now;
            foreach (var x in data)
            {
                DateTime dataStart = DateTime.Now;
                string query = _database.BuildSendDataQuery(basicQuery, new Dictionary<string, string>() { { "@zipcode", x.ZipCode }, { "@address", x.Address }, { "@city", x.City }, { "@province", x.Province }, { "@district", x.District } });
                _database.InsertData(query);
                DateTime dataEnd = DateTime.Now;
                meas.Add(TimeSpan.FromTicks(dataEnd.Ticks - dataStart.Ticks).TotalSeconds);
            }
            research.End = DateTime.Now;
            research.Meas = meas;
            ShowResearchResult(research);
        }
        /*
         * Metoda nr 2 (obowiązkowa z treści zadania)
         * Pakuje w jedno zapytanie całą treść pliku i wysyła to do serwera bazy danych
         */
        public void SendAllData(ICollection<LineDTO> data)
        {
            ResearchTime research = new ResearchTime();
            Logger.ConsoleLog(0, "zaczynam wykonywać próbę metodą nr 2..");
            research.Start = DateTime.Now;
            string query = "";
            foreach (var x in data)
            {
                query += @$"
                     INSERT INTO [dbo].[Kody]
                               ([zipcode]
                               ,[address]
                               ,[city]
                               ,[province]
                               ,[district])
                         VALUES
                               ('{CharsHelper.SpecialChars(x.ZipCode)}'
                               ,'{CharsHelper.SpecialChars(x.Address)}'
                               ,'{CharsHelper.SpecialChars(x.City)}'
                               ,'{CharsHelper.SpecialChars(x.Province)}'
                               ,'{CharsHelper.SpecialChars(x.District)}'
                    );
                ";
            }
            DateTime dataStart = DateTime.Now;
            using (var conn = _database.Connect())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            DateTime dataEnd = DateTime.Now;
            research.End = DateTime.Now;
            research.Meas = new List<double>() { TimeSpan.FromTicks(dataEnd.Ticks - dataStart.Ticks).TotalSeconds };
            ShowResearchResult(research, false);    
        }
        /*
         * Metoda nr 3 (dodatkowa)
         * "Paczkuje" dane i wysyła je w rozmiarze n wierszy
         */
        public void SendPackageData(ICollection<LineDTO> data, int packageSize=1000)
        {
            ResearchTime research = new ResearchTime();
            List<double> meas = new List<double>();
            Logger.ConsoleLog(0, "zaczynam wykonywać próbę metodą nr 3..");
            Logger.ConsoleLog(0, $"wielkość pakietu: {packageSize.ToString()}");
            research.Start = DateTime.Now;
            string query = "";
            int sendData = 0, sizeData = data.Count;
            foreach (var x in data)
            {
                DateTime dataStart = DateTime.Now;
                query += _database.BuildSendDataQuery(basicQuery, new Dictionary<string, string>() { {"@zipcode", x.ZipCode }, { "@address", x.Address }, { "@city", x.City }, { "@province", x.Province }, { "@district", x.District } });
                sendData++;
                if((sendData % packageSize) == 0)
                {
                    _database.InsertData(query);
                    query = "";
                    DateTime dataEnd = DateTime.Now;
                    meas.Add(TimeSpan.FromTicks(dataEnd.Ticks - dataStart.Ticks).TotalSeconds);
                }
                else if(sizeData-sendData < packageSize)
                {
                    _database.InsertData(query);
                    query = "";
                    DateTime dataEnd = DateTime.Now;
                    meas.Add(TimeSpan.FromTicks(dataEnd.Ticks - dataStart.Ticks).TotalSeconds);
                }
            }
            research.End = DateTime.Now;
            research.Meas = meas;
            ShowResearchResult(research);
        }

        /*
         * Metoda nr 4
         * Wykorzystanie współbierzności poprzez stworzeniu kilku wątków pochodnych
         * Tworzy tyle wątków ile wynosi dzielenie ilości wierzy przez n (czyli jeden wątek wyśle maksymalnie n wierszy do bazy danych)
         * Dodatkowo można określić ile na jeden "strzał" danych wysyła wątek
         */
        public void SendDataMultiThread(ICollection<LineDTO> data, int maxOneSend, int threadSendPackage)
        {
            IList<LineDTO> listOfData = data.ToList();
            IList<Thread> threads = new List<Thread>();
            int threadCount = 0;
            if (data.Count % maxOneSend == 0)
                threadCount = data.Count / maxOneSend;
            else
                threadCount = (data.Count / maxOneSend) + 1;

            int currentRow = 0;
            for(int i = 1; i <= threadCount; i++)
            {
                IList<LineDTO> lineDTOs = new List<LineDTO>();
                while(currentRow <= i * maxOneSend && currentRow < data.Count)
                {
                    lineDTOs.Add(listOfData[currentRow]);
                    currentRow++;
                }
                threads.Add(StartThreads(lineDTOs, threadSendPackage));
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }

        private Thread StartThreads(ICollection<LineDTO> data, int packageSize)
        {
            Thread thread = new Thread(() =>
            {
                SendPackageData(data, packageSize);
            });
            thread.Start();
            return thread;
        }

        private void ShowResearchResult(ResearchTime research, bool expanded = true)
        {
            if(expanded)
            {
                Logger.ConsoleLog(0, " ------------------------ | WYNIK | ------------------------");
                Logger.ConsoleLog(0, $"start: {research.Start.ToString("HH:mm:ss.ffff")}");
                Logger.ConsoleLog(0, $"max: {research.Meas.Max()}");
                Logger.ConsoleLog(0, $"avg: {research.Meas.Average()}");
                Logger.ConsoleLog(0, $"min: {research.Meas.Min()}");
                Logger.ConsoleLog(0, $"koniec: {research.End.ToString("HH:mm:ss.ffff")}");
                Logger.ConsoleLog(0, $"łączny czas trwania: {TimeSpan.FromTicks(research.End.Ticks - research.Start.Ticks).TotalSeconds} sekund");
                Logger.ConsoleLog(0, " -----------------------------------------------------------");
            }
            else
            {
                Logger.ConsoleLog(0, " ------------------------ | WYNIK | ------------------------");
                Logger.ConsoleLog(0, $"start: {research.Start.ToString("HH:mm:ss.ffff")}");
                Logger.ConsoleLog(0, $"czas trwania zapytania: {research.Meas.First().ToString()} sekund");
                Logger.ConsoleLog(0, $"koniec: {research.End.ToString("HH:mm:ss.ffff")}");
                Logger.ConsoleLog(0, $"łączny czas trwania: {TimeSpan.FromTicks(research.End.Ticks - research.Start.Ticks).TotalSeconds} sekund");
                Logger.ConsoleLog(0, " -----------------------------------------------------------");
            }
        }
    }
}
