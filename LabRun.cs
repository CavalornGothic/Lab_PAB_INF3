using System;
using System.Threading.Tasks;
using Lab_PAB_INF3.Lab_1;
using Lab_PAB_INF3.Lab_2;

namespace Lab_PAB_INF3
{
    class LabRun
    {
        static async Task Main(string[] args)
        {
            bool execute = true;
            while(execute)
            {
                Console.Clear();
                string wybor = null;
                Logger.ConsoleLog(0, "---------- | MENU | ----------");
                Logger.ConsoleLog(0, " [0] - Laboratorium nr 1 ");
                Logger.ConsoleLog(0, " [1] - Laboratorium nr 2 ");
                Logger.ConsoleLog(0, " [exit] - Wyjście ");
                wybor = Console.ReadLine();
                switch(wybor)
                {
                    case "0":
                        {
                            bool execute2 = true;
                            while(execute2)
                            {
                                string wybor2 = null;
                                string filePath = @"D:\kody.csv";
                                Logger.ConsoleLog(0, "---------- | MENU | ----------");
                                Logger.ConsoleLog(0, " [1] - Metoda nr 1 ");
                                Logger.ConsoleLog(0, " [2] - Metoda nr 2 ");
                                Logger.ConsoleLog(0, " [3] - Metoda nr 3 ");
                                Logger.ConsoleLog(0, " [4] - Metoda nr 4 (wielowątkowość) ");
                                Logger.ConsoleLog(0, " [back] - Powrót ");
                                wybor2 = Console.ReadLine();
                                DataUploader uploader = new DataUploader();
                                switch (wybor2)
                                {
                                    case "1":
                                        {
                                            Logger.ConsoleLog(0, "Metoda nr 1..");
                                            uploader.SendDataLineByLine(await new DataCSVLoader(filePath).GetAllLinesAsync());
                                            break;
                                        }
                                    case "2":
                                        {
                                            Logger.ConsoleLog(0, "Metoda nr 2..");
                                            uploader.SendAllData(await new DataCSVLoader(filePath).GetAllLinesAsync());
                                            break;
                                        }
                                    case "3":
                                        {
                                            Logger.ConsoleLog(0, "Metoda nr 3..");
                                            uploader.SendPackageData(await new DataCSVLoader(filePath).GetAllLinesAsync(), 125);
                                            break;
                                        }
                                    case "4":
                                        {
                                            Logger.ConsoleLog(0, "Metoda nr 4..");
                                            uploader.SendDataMultiThread(await new DataCSVLoader(filePath).GetAllLinesAsync(), 2500, 50);
                                            break;
                                        }
                                    case "back":
                                        {
                                            execute2 = false;
                                            break;
                                        }
                                    default: continue;
                                }
                            }
                            break;
                        }
                    case "1":
                        {
                            bool execute2 = true;
                            while (execute2)
                            {
                                Logger.ConsoleLog(0, "---------- | MENU | ----------");
                                Logger.ConsoleLog(0, " [0] - Wrzuć do bazy poprawne dane z plików CSV ");
                                Logger.ConsoleLog(0, " [1] - Wrzuć do bazy błędne dane z plików CSV ");
                                Logger.ConsoleLog(0, " [2] - Wrzuć do bazy poprawne dane z plików CSV [TRANSAKCJA]");
                                Logger.ConsoleLog(0, " [3] - Wrzuć do bazy błędne dane z plików CSV [TRANSAKCJA]");
                                Logger.ConsoleLog(0, " [4] - Wczytaj dane, które mogą być nullem");
                                Logger.ConsoleLog(0, " [back] - Powrót ");
                                string wybor2 = null;
                                wybor2 = Console.ReadLine();
                                switch (wybor2)
                                {
                                    case "0":
                                        {
                                            string correctJADataPath = @"D:\correctDataJobAds.csv";
                                            string correctJATechDataPath = @"D:\correctDataJobAdsTechElem.csv";
                                            DataUploaderLab2 uploader = new DataUploaderLab2();
                                            bool ok = uploader.SendJobsCSVData(correctJADataPath, correctJATechDataPath);
                                            if (ok)
                                                Logger.ConsoleLog(0, "udało się wczytać poprawnie wszystkie dane z pliku CSV.");
                                            break;
                                        }
                                    case "1":
                                        {
                                            string incorrectJADataPath = @"D:\incorrectDataJobAds.csv";
                                            string incorrectJATechDataPath = @"D:\incorrectDataJobAdsTechElem.csv";
                                            DataUploaderLab2 uploader = new DataUploaderLab2();
                                            bool ok = uploader.SendJobsCSVData(incorrectJADataPath, incorrectJATechDataPath);
                                            break;
                                        }
                                    case "2":
                                        {
                                            string correctJADataPath = @"D:\correctDataJobAds.csv";
                                            string correctJATechDataPath = @"D:\correctDataJobAdsTechElem.csv";
                                            DataUploaderLab2 uploader = new DataUploaderLab2();
                                            bool ok = uploader.SendJobsCSVDataAsTran(correctJADataPath, correctJATechDataPath);
                                            if (ok)
                                                Logger.ConsoleLog(0, "udało się wczytać poprawnie wszystkie dane z pliku CSV.");
                                            break;
                                        }
                                    case "3":
                                        {
                                            string incorrectJADataPath = @"D:\incorrectDataJobAds.csv";
                                            string incorrectJATechDataPath = @"D:\incorrectDataJobAdsTechElem.csv";
                                            DataUploaderLab2 uploader = new DataUploaderLab2();
                                            bool ok = uploader.SendJobsCSVDataAsTran(incorrectJADataPath, incorrectJATechDataPath);
                                            break;
                                        }
                                    case "4":
                                        {
                                            var coll = new DataLoaderLab2().LoadAllJobAdsTechElemFromDB();
                                            Logger.ConsoleLog(0, $"kolekcja zawiera {coll.Count} pól");
                                            break;
                                        }
                                    case "back":
                                        {
                                            execute2 = false;
                                            break;
                                        }
                                    default : break;
                                }
                            }
                            break;
                        }
                    case "exit":
                        {
                            execute = false;
                            break; 
                        }
                    default: continue;
                }
            }
        }
    }
}
