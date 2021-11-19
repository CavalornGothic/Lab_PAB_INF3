using System;
using System.Threading.Tasks;
using Lab_PAB_INF3.Lab_1;

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
                                            uploader.SendPackageData(await new DataCSVLoader(filePath).GetAllLinesAsync());
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
